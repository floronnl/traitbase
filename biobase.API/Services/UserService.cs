using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Services
{
    public class UserService : IUserService
    {
        private readonly BiobaseDbContext _context;

        public UserService(BiobaseDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByApiKeyAsync(string apiKey)
        {
            return await _context.authentication.SingleOrDefaultAsync(u => u.apikey == apiKey);
        }
    }
}
