using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {

        public async Task<Form> GetForm(int formId)
        {
            return await _context.Forms.FirstOrDefaultAsync(f => f.Id == formId);
        }

        public async Task<List<Form>> GetForms()
        {
            return await _context.Forms.ToListAsync();
        }

        public async Task ToggleFormActive(int formId)
        {
            Form form = await _context.Forms.FirstOrDefaultAsync(f => f.Id == formId);
            form.Active = !form.Active;
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteForm(int formId)
        {
            Form form = await _context.Forms.FirstOrDefaultAsync(f => f.Id == formId);
            _context.Forms.Remove(form);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateForm(Form form)
        {
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
        }

        public async Task CreateForm(Form form)
        {
            await _context.Forms.AddAsync(form);
            await _context.SaveChangesAsync();
        }
    }
}
