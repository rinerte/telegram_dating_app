using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Telegram.Bot;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Bot
{
    public static class ListMethods
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _connectionString;
        private IProfileService _db;
        private TelegramBotClient client;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;

            try
            {
                var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                            .Build();
                if (String.IsNullOrEmpty(config["connectionstring"])) throw new Exception();
                _connectionString = config["connectionstring"];
            }
            catch
            {
                _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            client =  await Bot.Get();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                try
                {
                    try
                    {
                        CreateContext();
                        _logger.LogInformation("Context created: {time}", DateTimeOffset.Now);

                        List<UserStatus> users = await _db.GetUsersStatuses(DataLayer.Enums.UserStatuses.AnswersChanged);
                        if (users != null && users.Count() > 0) await ProcessAnswerChangedUsers(users);

                        users = await _db.GetUsersStatuses(DataLayer.Enums.UserStatuses.Processing);
                        if (users != null && users.Count() > 0) await ProcessFindingMatchUsers(users);

                        var pendingMessages = await _db.GetUnsentMatches();
                        if (pendingMessages != null)
                            await SendMessages(pendingMessages);

                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation("Exception: {e}", e.Message);
                    }
                    finally
                    {
                        _logger.LogInformation("Context disposed: {time}", DateTimeOffset.Now);
                        Dispose();
                    }
                } catch (Exception ex)
                {
                    _logger.LogInformation("Critical ERROR: {ex} {time}",ex.Message, DateTimeOffset.Now);
                }
                

                _logger.LogInformation("Worker paused at: {time}", DateTimeOffset.Now);
                await Task.Delay(60000, stoppingToken);
            }
        }
        
        private async Task SendMessages(List<Match> pendingMessages)
        {
             
             List<Message> messages = new List<Message>();

            var userGroups = pendingMessages.GroupBy(m => m.UsersStamp.UserId);
                foreach(var pendingMessagesUserGroup in userGroups)
                {
                    foreach (Match match in pendingMessagesUserGroup)
                    {
                        bool messageAdded = false;
                        var user = await _db.GetUser(match.UsersStamp.UserId);
                        var delay = await _db.GetUsersDelay(user.Id);

                        bool freeToSend = false;
                        if( delay.LastMessageSent == null)
                        {
                            freeToSend = true;
                        } else
                        {
                            var difference = TimeSpan.FromTicks(DateTime.Now.Ticks).Subtract(TimeSpan.FromTicks((long)delay.LastMessageSent?.Ticks));
                            freeToSend = (TimeSpan.Compare(delay.Delay, difference) < 0);
                        }
                        
                        if (freeToSend)
                        {
                            var foundUser = await _db.GetUser(match.FoundUserId);
                            if (foundUser.Login != null)
                            {
                                messages.Add(new()
                                {
                                    UserId = user.UserId,
                                    MatchId = match.Id,
                                    MessageText = await Messages.FoundUserMessage(match.Id, foundUser, _db)
                                });
                            messageAdded = true;
                            }
                        }
                        if (messageAdded) break;
                    }
                }

            foreach (Message message in messages)
            {
                
                try
                {
                    var user = await _db.GetUser((long)message.UserId);
                    try
                    {
                        await client.SendTextMessageAsync(message.UserId, message.MessageText);
                    } catch (Exception ex)
                    {
                        if(ex.Message.Contains("Forbidden: bot was blocked by the user"))
                        {
                            _logger.LogInformation("User {0} blocked the bot\n MatchID = {1}", message.UserId, message.MatchId);
                        }
                        await _db.SetMatchStatusSent(message.MatchId);
                        continue;
                    }
                    
                    await _db.SetUsersDelay(user.Id, TimeSpan.FromHours(1), DateTime.Now);
                    await _db.SetMatchStatusSent(message.MatchId);

                    await Task.Delay(100);

                } catch (Exception ex)
                {
                    _logger.LogInformation("Error sending message {0} Time:: {1}",ex.Message, DateTimeOffset.Now);
                    _logger.LogError(ex.InnerException.ToString() +"\n" + ex.StackTrace);
                    await _db.SetMatchStatusSent(message.MatchId);
                    if (ex.Message.ToLower().Contains("bot was blocked by the user"))
                    {
                        // здесь может быть удаление всей информации о пользователе,
                        // который заблокировал бота.
                        // но её нет.
                    }
                }
            }
        }
       
        private async Task ProcessFindingMatchUsers(List<UserStatus> users)
        {
            foreach (var user in users)
            {
                UsersStamp stamp = await _db.GetStamp(user.UserId, user.FormId);
                
                List<UsersStamp> matching = await _db.FindMatching(stamp.Pattern);
                if(matching != null && matching.Count>0)
                {
                    if (matching.Count > 2)
                    {
                        matching.Shuffle();
                    }                    

                    foreach(UsersStamp matchingUsersStamp in matching)
                    {
                        if(matchingUsersStamp.UserId != user.UserId)
                        {
                            var matches = await _db.GetMatches(stamp.Id);
                            var index = matches.FindIndex(m => m.FoundUserId == matchingUsersStamp.UserId);
                            if (matches == null || matches.Count() == 0 || index < 0)
                            {
                                await _db.CreateMatch(new()
                                {
                                    FoundUserId = matchingUsersStamp.UserId,
                                    MessageSent = false,
                                    UsersStampId = stamp.Id
                                });
                            }
                        }
                    }
                }
            }
        }

        private async Task ProcessAnswerChangedUsers(List<UserStatus> users)
        {
            foreach (var user in users)
            {
                try
                {
                    await _db.SetUsersDelay(user.UserId, TimeSpan.FromMilliseconds(0), null);
                    await _db.DeleteStampAndMatches(user.UserId, user.FormId);

                    var answerList = await _db.GetUserAnswers(user.UserId);
                    var groupedAnswers = answerList.GroupBy(p => p.FormModuleConnection.FormId);

                    foreach(var group in groupedAnswers)
                    {
                        int formId = group.First().FormModuleConnection.FormId;

                        if (formId == user.FormId)
                        {
                            await _db.CreateStamp(group.OrderBy(o => o.FormModuleConnection.NumberInSequence).ToList(), formId);
                            await _db.ChangeUserStatus(user.UserId, formId, DataLayer.Enums.UserStatuses.Processing);
                        }
                    }
                } catch
                {

                }
            }
        }
        public void CreateContext()
        {
            DbContextOptionsBuilder<ProfileContext> cbuilder = new DbContextOptionsBuilder<ProfileContext>();
            cbuilder.UseNpgsql(_connectionString);
            ProfileContext context = new ProfileContext(cbuilder.Options);
            _db = new ProfileService(context);
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}