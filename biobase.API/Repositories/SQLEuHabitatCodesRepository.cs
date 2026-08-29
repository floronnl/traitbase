using biobase.API.Data;
using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public class SQLEuHabitatCodesRepository : IEuHabitatCodesRepository
    {
        private readonly BiobaseDbContext _dbContext;
        private readonly ILogger<SQLEuHabitatCodesRepository> _logger;

        public SQLEuHabitatCodesRepository(BiobaseDbContext dbContext, ILogger<SQLEuHabitatCodesRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<EuHabitatCodes>> GetAllAsync()
        {
            return await _dbContext.eu_habitat_classes.ToListAsync();
        }
    }
}
