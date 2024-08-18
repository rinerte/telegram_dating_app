using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {

        public async Task<List<AnswerVariant>> GetAnswerVariantsOfBlock(int asnwerBlockId)
        {
            return await _context.AnswerVariants.Where(av=>av.AnswerBlockId == asnwerBlockId).ToListAsync();
        }
        public async Task CreateAnswerVariants(List<AnswerVariant> answerVariants)
        {
            await _context.AnswerVariants.AddRangeAsync(answerVariants);
            await _context.SaveChangesAsync();
        }

        public async Task<AnswerVariant> GetAnswerVariant(int answerVariantId)
        {
            return await _context.AnswerVariants.FirstOrDefaultAsync(a => a.Id == answerVariantId);
        }
    }
}
