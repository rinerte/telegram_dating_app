using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task<List<AnswerVariant>> GetAnswerVariantsOfBlock(int asnwerBlockId);
        public Task CreateAnswerVariants(List<AnswerVariant> answerVariants);
        public Task<AnswerVariant> GetAnswerVariant(int answerVariantId);
    }
}
