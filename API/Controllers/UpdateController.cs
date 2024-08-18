using Microsoft.AspNetCore.Mvc;
using DataLayer.Interfaces;
using Telegram.Bot.Types;

namespace ProfileAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdateController : ControllerBase
    {
        private ILogger _logger;
        private readonly IProfileService _db;
        public UpdateController(IProfileService db , ILogger<UpdateController> logger)
        {
            _db = db;
            _logger = logger;
        }


        // На этот контроллер будут приходить апдейты от телеграма
        // post site/update
        [HttpPost(Name = "PostUpdate")]
        public async Task<IActionResult> Post(Update update)
        {
            _logger.LogInformation($"new telegram update posted: {DateTime.Now.ToLongTimeString()}");

            if (update.Message != null)
            {
                var user = new DataLayer.Models.User()
                {
                    FirstName = update.Message.From.FirstName,
                    IsBot = update.Message.From.IsBot,
                    UserId = update.Message.From.Id,
                    Login = update.Message.From.Username
                };

                if(await _db.CreateNewUser(user))
                    _logger.LogInformation($"new user created: {DateTime.Now.ToLongTimeString()}");
            }
            
            return new OkResult();
        }
    }
}
