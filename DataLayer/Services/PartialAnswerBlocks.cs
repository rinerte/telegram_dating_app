using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {

        public async Task<List<AnswerBlock>> GetAllAnswerBlocks()
        {
            return await _context.AnswerBlocks.ToListAsync();
        }
    }
}
