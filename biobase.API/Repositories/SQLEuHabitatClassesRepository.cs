using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLEuHabitatClassesRepository : IEuHabitatClassesRepository
    {
        private readonly BiobaseDbContext _dbContext;
        private readonly ILogger<SQLEuHabitatClassesRepository> _logger;

        public SQLEuHabitatClassesRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EuHabitatClasses>> GetAllAsync()
        {
            return await _dbContext.eu_habitat_classes.ToListAsync();
        }
    }
}
