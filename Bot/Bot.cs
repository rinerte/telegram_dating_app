using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Bot
{
    internal class Bot
    {
        private static readonly string Key;

        private static TelegramBotClient client;
        static Bot()
        {
            try
            {
                var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                            .Build();
                if (String.IsNullOrEmpty(config["Key"])) throw new Exception();
                Key = config["Key"];
            }
            catch
            {
                Key = Environment.GetEnvironmentVariable("BOT_TOKEN");
            }
        }

        public static async Task<TelegramBotClient> Get()
        {
            if (client != null)
            {
                return client;
            }
            client = new TelegramBotClient(Key);

            return client;
        }
    }
}
