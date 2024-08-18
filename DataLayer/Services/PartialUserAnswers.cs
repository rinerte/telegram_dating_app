using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {
        public async Task WriteUserAnswers(UserAnswer answer)
        {
           await _context.UserAnswers.AddAsync(answer);
           await _context.SaveChangesAsync();
        }
        public async Task<List<UserAnswer>> GetUserAnswers(int userId)
        {
            return await _context.UserAnswers.Where(a=> a.UserId == userId).Include(i=>i.FormModuleConnection).ToListAsync();
        }

        public async Task DeleteUserAnswers(int userId, int formId)
        {
            var allAnswers = await GetUserAnswers(userId);
            var allConnections = await GetFormModuleConnections(formId);
            List<UserAnswer> answersToDelete = new();

            foreach (var answer in allAnswers)
            {
                if( allConnections.Any(el => el.Id==answer.FormModuleConnectionId)) answersToDelete.Add(answer);
            }

            _context.UserAnswers.RemoveRange(answersToDelete);
            await _context.SaveChangesAsync();            
        }
    }
}
