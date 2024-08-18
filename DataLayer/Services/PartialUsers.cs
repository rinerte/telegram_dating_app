using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {
        
        public async Task<bool> CreateNewUser(User user)
        {
            try
            {
                var exist = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                if (exist == null)
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    return true;
                } else
                {
                    if(exist.Login != user.Login)
                    {
                        exist.Login = user.Login;
                        _context.Users.Update(exist);
                        await _context.SaveChangesAsync();
                    }
                }
                return false;
            } catch
            {                
                return false;
            }
            
            
        }
        public async Task<User?> GetUser(long userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public async Task<User?> GetUser(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
