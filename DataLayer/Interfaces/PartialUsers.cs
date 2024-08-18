using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        /// <summary>
        /// Creates NEW user in BD.
        /// returns TRUE on success.
        /// returns FALSE in other cases.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> CreateNewUser(User user);

        public Task<User?> GetUser(long userId);
        public Task<User?> GetUser(int userId);
    }
}
