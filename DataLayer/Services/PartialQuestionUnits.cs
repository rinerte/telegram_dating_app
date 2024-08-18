using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {

        public async Task<QuestionUnit> GetQuestionUnit(int questionUnitId)
        {
            return await _context.QuestionUnits.FirstOrDefaultAsync(q => q.Id == questionUnitId);
        }
        public async Task<List<QuestionUnit>> GetQuestionUnits()
        {
            return await _context.QuestionUnits.ToListAsync();
        }
        public async Task DeleteQuestionUnit(int questionUnitId)
        {
            var q = await _context.QuestionUnits.FirstOrDefaultAsync(q => q.Id == questionUnitId);
            _context.QuestionUnits.Remove(q);
            await _context.SaveChangesAsync();
        }
        public async Task CreateQuestionUnit(string questionText)
        {
            QuestionUnit questionUnit = new QuestionUnit() { TextQuestion = questionText };
            await _context.QuestionUnits.AddAsync(questionUnit);
           await _context.SaveChangesAsync();
        }
    }
}
