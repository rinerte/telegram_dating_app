using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task DeleteMatches(int stampId);
        public Task<List<Match>> GetMatches(int usersStampId);
        public Task CreateMatch(Match match);
        public Task<List<Match>> GetUnsentMatches();
        public Task SetMatchStatusSent(int matchId);
    }
}
