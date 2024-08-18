using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
        public Task<List<FormModuleConnection>> GetFormModuleConnections(int formId);
        public Task<FormModuleConnection> GetFormModuleConnection(int formId, int moduleId);

        public Task CreateFormModuleConnection(FormModuleConnection formModuleConnection);
        public Task DeleteFormModuleConnection(int formModuleConnectionId);
        public Task SortFormModuleConnection(int formId);
        public Task IncrementNumberInSequenceFormModuleConnection(FormModuleConnection connection);
        public Task DecrementNumberInSequenceFormModuleConnection(FormModuleConnection connection);

    }
}
