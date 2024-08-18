using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task WriteUserAnswers(UserAnswer answer);
        public Task<List<UserAnswer>> GetUserAnswers(int userId);

        public Task DeleteUserAnswers(int userId, int formId);
    }
}
