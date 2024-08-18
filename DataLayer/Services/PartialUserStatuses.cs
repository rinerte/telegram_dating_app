using Microsoft.EntityFrameworkCore;
using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {

        public async Task ChangeUserStatus(int userId, int formId, UserStatuses status)
        {
            var exist = await _context.UserStatuses.FirstOrDefaultAsync(x => x.UserId == userId && x.FormId==formId);

            if(exist == null)
            {
                await _context.UserStatuses.AddAsync(new UserStatus()
                {
                    UserId = userId,
                    FormId = formId,
                    Status = status,
                    StatusAssignedAt = DateTime.Now.ToUniversalTime(),
            });
            } else
            {
                exist.Status = status;
                exist.StatusAssignedAt = DateTime.Now.ToUniversalTime();
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserStatus>> GetUsersStatuses(UserStatuses status)
        {
            return await _context.UserStatuses.Where(u => u.Status == status).ToListAsync();
        }

        public async Task<UserStatus> GetUsersStatus(int userId, int formId)
        {
            return await _context.UserStatuses.FirstOrDefaultAsync(s => s.UserId == userId && s.FormId == formId);
        }
    }
}
