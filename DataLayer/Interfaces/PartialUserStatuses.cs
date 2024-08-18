using DataLayer.Enums;
using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task ChangeUserStatus(int userId, int formId, UserStatuses status);

        public Task<List<UserStatus>> GetUsersStatuses(UserStatuses status);

        public Task<UserStatus> GetUsersStatus(int userId, int formId);

    }
}
