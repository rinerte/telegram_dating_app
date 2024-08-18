using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {        
        public Task SetUsersDelay(int userId, TimeSpan delay, DateTime? lastMessageSent);
        public Task<UserDelay> GetUsersDelay(int userId);
    }
}
