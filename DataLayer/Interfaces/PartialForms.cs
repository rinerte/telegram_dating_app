using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task<Form> GetForm(int formId);
        public Task<List<Form>> GetForms();

        public Task ToggleFormActive(int formId);
        public Task DeleteForm(int formId);
        public Task UpdateForm(Form form);
        public Task CreateForm(Form form);
    }
}
