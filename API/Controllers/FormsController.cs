using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace ProfileAPI.Controllers
{
   
    public class FormsController : Controller
    {
        private ILogger _logger;
        private readonly IProfileService _db;
        public FormsController(IProfileService db, ILogger<UpdateController> logger)
        {
            _db = db;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Index accessed: {DateTime.Now.ToLongTimeString()}");

            try
            {
                List<Form> _forms = await _db.GetForms();
                return Json(_forms);
            } catch (Exception ex)
            {
                _logger.LogError($"No Access to database: {ex.Message+ex.Source}");
            }

            return Json("HELLO");
        }

        [HttpGet]
        public async Task<IActionResult> Forms(long userId) // site/forms/forms?userId=10
        {
            
            _logger.LogInformation($"GetForms accessed by: {userId} : {DateTime.Now.ToLongTimeString()}");

            var _user = await _db.GetUser(userId);
            if (_user == null) return NotFound();

            List<Form> _forms = await _db.GetForms();
            List<FrontForm> frontForms = new List<FrontForm>();

            foreach(var form in _forms)
            {
                if(form.Active)
                {
                    frontForms.Add(new FrontForm()
                    {
                        Id = form.Id,
                        Name = form.Name,
                        Percentage = await CalculatePercentage(form, _user)
                    });
                }                
            }
            
            return Json(frontForms);
        }

        [HttpGet]
        public async Task<IActionResult> Form(long userId, int formId) // site/forms/form?userId=10&formId=10
        {
           
            _logger.LogInformation($"Form {formId} accessed by: {userId} : {DateTime.Now.ToLongTimeString()}");

            var _user = await _db.GetUser(userId);
            if (_user == null) return NotFound();

            var form = await _db.GetForm(formId);
            if (form == null) return NotFound();

            var formQuestions = await _db.GetFormModuleConnections(formId);
            if (formQuestions==null || formQuestions.Count == 0) return NotFound();

            var result = new FrontFormWithQuestions();
            result.Name = form.Name;
            result.QuestionModules = new();

            foreach(var question in formQuestions)
            {
                var qModule = await _db.GetQuestionModule(question.QuestionModuleId);

                var frontQuestionModule = new FrontQuestionModule()
                {
                    QuestionModuleId = qModule.Id,
                    Questions = new()
                };

                if (qModule.SecondQuestionId != null)
                {
                    var qUnit = await _db.GetQuestionUnit(qModule.FirstQuestionId);
                    var qAnswers = await _db.GetAnswerVariantsOfBlock(qModule.FirstQuestionAnswerBlockId);

                    var firstQuestion = new FrontQuestion()
                    {
                        Guid = Guid.NewGuid(),
                        Multiselectable = qModule.FirstMultiselectable,
                        Answers = new(),
                        Title = qUnit.TextQuestion
                    };
                    
                    foreach(var answer in qAnswers)
                    {
                        firstQuestion.Answers.Add(new()
                        {
                            AnswerId = answer.Id,
                            Caption = answer.Caption,
                            Selected = false
                        });
                    }

                     qUnit = await _db.GetQuestionUnit((int)qModule.SecondQuestionId);
                     qAnswers = await _db.GetAnswerVariantsOfBlock((int)qModule.SecondQuestionAnswerBlockId);

                    var secondQuestion = new FrontQuestion()
                    {
                        Guid = Guid.NewGuid(),
                        Multiselectable = qModule.SecondMultiselectable,
                        Answers = new(),
                        Title = qUnit.TextQuestion
                    };

                    foreach (var answer in qAnswers)
                    {
                        secondQuestion.Answers.Add(new()
                        {
                            AnswerId = answer.Id,
                            Caption = answer.Caption,
                            Selected = false
                        });
                    }

                    frontQuestionModule.Questions.Add(firstQuestion);
                    frontQuestionModule.Questions.Add(secondQuestion);
                } else
                {
                    var qUnit = await _db.GetQuestionUnit(qModule.FirstQuestionId);
                    var qAnswers = await _db.GetAnswerVariantsOfBlock(qModule.FirstQuestionAnswerBlockId);

                    var firstQuestion = new FrontQuestion()
                    {
                        Guid = Guid.NewGuid(),
                        Multiselectable = qModule.FirstMultiselectable,
                        Answers = new(),
                        Title = qUnit.TextQuestion
                    };

                    foreach (var answer in qAnswers)
                    {
                        firstQuestion.Answers.Add(new()
                        {
                            AnswerId = answer.Id,
                            Caption = answer.Caption,
                            Selected = false
                        });
                    }

                    frontQuestionModule.Questions.Add(firstQuestion);
                }
                
                result.QuestionModules.Add(frontQuestionModule);
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Form(long userId,[FromBody]FormResults results) //site/forms/results
        {
            _logger.LogInformation($"User : {userId} POSTED Answers : {DateTime.Now.ToLongTimeString()}");

            User user = await _db.GetUser(userId);
            if(user==null) return NotFound();

            for ( int i =0; i < results.Questions.Count(); i++)
            {
                var fmc = await _db.GetFormModuleConnection(results.FormId, results.Questions[i].ModuleId);
                if (fmc == null) return NotFound();

                try
                {
                    if (results.Questions[i].ModuleId == results.Questions[i + 1].ModuleId)
                    {
                        await CreateUserAnswer(user, fmc, results.Questions[i].Answers, results.Questions[i + 1].Answers);
                        i++;
                    } else
                    {
                        await CreateUserAnswer(user, fmc, results.Questions[i].Answers);
                    }
                } catch
                {
                    await CreateUserAnswer(user, fmc, results.Questions[i].Answers);
                }
                
            }

            await _db.ChangeUserStatus(user.Id, results.FormId, DataLayer.Enums.UserStatuses.AnswersChanged);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long userId, int formId)
        {
            _logger.LogInformation($"User : {userId} Deleted Answers for FORM_ID: {formId} : {DateTime.Now.ToLongTimeString()}");

            User user = await _db.GetUser(userId);
            if (user == null) return NotFound();

            await _db.DeleteUserAnswers(user.Id, formId);
            await _db.ChangeUserStatus(user.Id, formId, DataLayer.Enums.UserStatuses.AnswersChanged);
            return Ok();
        }

        public async Task CreateUserAnswer(User user, FormModuleConnection fmc, int[] first, int[] second = null)
        {
            UserAnswer answer = new UserAnswer()
            {
                User = user,
                FormModuleConnection = fmc,
                FirstQuestionAnswerVariantsJSON = JsonConvert.SerializeObject(first)                
            };
            if (second != null) answer.SecondQuestionAnswerVariantsJSON = JsonConvert.SerializeObject(second);

            
             await _db.WriteUserAnswers(answer);
            
        }
        public class FormResults
        {
            public string FormName { get; set; }
            public int FormId { get; set; }
            public FormResultsQuestion[] Questions { get; set; }
        }
        public class FormResultsQuestion
        {
            public int ModuleId { get; set; }
            public string Title { get; set; }
            public int[] Answers { get; set; }
        }
        private async Task<int> CalculatePercentage(Form form, User user)
        {
            var formQuestions = await _db.GetFormModuleConnections(form.Id);
            var userAnswers = await _db.GetUserAnswers(user.Id);

            int questionsTotal = formQuestions.Count();
            int questionsAnswered = 0;

            foreach(var question in formQuestions)
            {
                if (userAnswers.Find(x => x.FormModuleConnectionId == question.Id) != null) questionsAnswered++;
            }

            return (int)((double)questionsAnswered / (double)questionsTotal * 100.0);
        }
        public class FrontFormWithQuestions
        {
            public string Name { get; set; }
            public List<FrontQuestionModule> QuestionModules { get; set; }
        }
        public class FrontQuestionModule
        {
            public int QuestionModuleId { get; set; }
            public List<FrontQuestion> Questions {get;set;}
        }
        public class FrontQuestion
        {
            public Guid Guid { get; set; }
            public string Title { get; set; }
            public bool Multiselectable { get; set; }
            public List<FrontAnswer> Answers { get; set; }
        }
        public class FrontAnswer
        {
            public int AnswerId { get; set; }
            public string Caption { get; set; }
            public bool Selected { get; set; }
        }
        public class FrontForm
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Percentage { get; set; }
        }

    }
}
