using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task<UsersStamp> GetStampByMatchId(int matchId);
        public Task DeleteStampAndMatches(int userId, int formId);
        public Task CreateStamp(List<UserAnswer> answers, int formId);
        public Task<UsersStamp> GetStamp(int userId, int formId);
        public Task<List<UsersStamp>> FindMatching(string pattern);
    }
}
