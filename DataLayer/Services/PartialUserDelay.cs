using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {
        public async Task SetUsersDelay(int userId, TimeSpan delay, DateTime? lastMessageSent)
        {
            var _delay = await _context.UserDelays.FirstOrDefaultAsync(d=>d.UserId == userId);

            if(_delay == null)
            {
                _delay = new()
                {
                    UserId = userId,
                    Delay = delay,
                    LastMessageSent = lastMessageSent?.ToUniversalTime(),
                    Counter = 0
                };
                await _context.UserDelays.AddAsync(_delay);
                await _context.SaveChangesAsync();
            } else
            {
               // _delay = new();
                if (_delay.Counter >= 3)
                {
                    _delay.Delay = TimeSpan.FromHours(21);
                    _delay.LastMessageSent = lastMessageSent?.ToUniversalTime();
                    _delay.Counter = 0;
                } else
                {
                    _delay.Delay = delay;
                    _delay.LastMessageSent = lastMessageSent?.ToUniversalTime();
                    _delay.Counter += 1;
                }
                
                _context.UserDelays.Update(_delay);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<UserDelay> GetUsersDelay(int userId)
        {
            return await _context.UserDelays.FirstOrDefaultAsync(d=>d.UserId == userId);
        }
    }
}
