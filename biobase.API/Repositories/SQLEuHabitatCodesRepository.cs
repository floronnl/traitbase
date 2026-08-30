using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLEuHabitatCodesRepository : IEuHabitatCodesRepository
    {
        private readonly BiobaseDbContext _dbContext;
        private readonly ILogger<SQLEuHabitatCodesRepository> _logger;

        public SQLEuHabitatCodesRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EuHabitatCodes>> GetAllAsync()
        {
            return await _dbContext.eu_habitat_classes.ToListAsync();
        }
    }
}
