using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {
        public async Task DeleteMatches(int stampId)
        {
            var matches =await _context.Matches.Where(m=>m.UsersStampId == stampId).ToListAsync();
            _context.Matches.RemoveRange(matches);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Match>> GetMatches(int usersStampId)
        {
            return await _context.Matches.Where(m=>m.UsersStampId==usersStampId).Include(u=>u.UsersStamp).ToListAsync();
        }
        public async Task<List<Match>> GetUnsentMatches()
        {
            return await _context.Matches.Where(m => !m.MessageSent).ToListAsync();
        }
        public async Task CreateMatch(Match match)
        {
            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();
        }

        public async Task SetMatchStatusSent(int matchId)
        {
            Match match = await _context.Matches.FirstOrDefaultAsync(m=>m.Id==matchId);
            match.MessageSent = true;
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }
    }
}
