using Microsoft.EntityFrameworkCore;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {

        public async Task<List<FormModuleConnection>> GetFormModuleConnections(int formId)
        {
            var result = await _context.FormModuleConnections.Where(c => c.FormId == formId).Include(i=>i.QuestionModule).ToListAsync();
            result.Sort((a, b) => a.NumberInSequence - b.NumberInSequence);
            return result;
        }

        public async Task<FormModuleConnection> GetFormModuleConnection(int formId, int moduleId)
        {
            return  await _context.FormModuleConnections.FirstOrDefaultAsync(fmc => fmc.FormId == formId && fmc.QuestionModuleId == moduleId);           
        }

        public async Task CreateFormModuleConnection(FormModuleConnection formModuleConnection)
        {
            await _context.FormModuleConnections.AddAsync(formModuleConnection);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteFormModuleConnection(int formModuleConnectionId)
        {
            var fmc = await _context.FormModuleConnections.FirstOrDefaultAsync(f=>f.Id== formModuleConnectionId);
            _context.FormModuleConnections.Remove(fmc);
            await _context.SaveChangesAsync();
        }
        public async Task SortFormModuleConnection(int formId)
        {
            List<FormModuleConnection> connections = await _context.FormModuleConnections.Where(c=>c.FormId == formId).ToListAsync();
            connections.Sort((x, y) => x.NumberInSequence - y.NumberInSequence);
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].NumberInSequence = i + 1;
            }
            _context.FormModuleConnections.UpdateRange(connections);
            await _context.SaveChangesAsync();
        }
        public async Task IncrementNumberInSequenceFormModuleConnection(FormModuleConnection connection)
        {
            List<FormModuleConnection> list = await _context.FormModuleConnections.Where(c => c.FormId == connection.FormId).ToListAsync();
            list.Sort((x, y) => x.NumberInSequence - y.NumberInSequence);

            int index = connection.NumberInSequence;

            if (index < 2) return;
            else
            {
                list[index - 1].NumberInSequence--;
                list[index - 2].NumberInSequence++;
            }
            _context.FormModuleConnections.UpdateRange(list);
            await _context.SaveChangesAsync();
        }
        public async Task DecrementNumberInSequenceFormModuleConnection(FormModuleConnection connection)
        {
            List<FormModuleConnection> list = await _context.FormModuleConnections.Where(c => c.FormId == connection.FormId).ToListAsync();
            list.Sort((x, y) => x.NumberInSequence - y.NumberInSequence);

            int index = connection.NumberInSequence;

            if (index == list.Count) return;
            else
            {
                list[index - 1].NumberInSequence++;
                list[index].NumberInSequence--;
            }
            _context.FormModuleConnections.UpdateRange(list);
            await _context.SaveChangesAsync();
        }
    }
}
