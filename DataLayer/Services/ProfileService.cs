using DataLayer.Interfaces;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {
        ProfileContext _context;
        public ProfileService(ProfileContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            try
            {
                _context.Dispose();
            }
            catch { }
        }
    }
}
