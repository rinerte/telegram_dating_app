using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task<QuestionModule> GetQuestionModule(int questionModuleId);
        public Task<List<QuestionModule>> GetQuestionModules();
        public Task CreateQuestionModule(QuestionModule questionModule);
    }
}
