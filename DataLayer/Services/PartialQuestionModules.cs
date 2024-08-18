using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {
        public async Task<QuestionModule> GetQuestionModule(int questionModuleId)
        {
            return await _context.QuestionModules.FirstOrDefaultAsync(q => q.Id == questionModuleId);
        }
        public async Task<List<QuestionModule>> GetQuestionModules()
        {
            return await _context.QuestionModules.ToListAsync();
        }
        public async Task CreateQuestionModule(QuestionModule questionModule)
        {
            await _context.QuestionModules.AddAsync(questionModule);
            await _context.SaveChangesAsync();
        }
    }
}
