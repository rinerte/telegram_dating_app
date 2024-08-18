
using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task<List<AnswerBlock>> GetAllAnswerBlocks();
    }
}
