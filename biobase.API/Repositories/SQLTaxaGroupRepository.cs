using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLTaxaGroupRepository : ITaxaGroupRepository
    {
        private readonly BiobaseDbContext _dbContext;
        private readonly ILogger<SQLTaxaGroupRepository> _logger;

        public SQLTaxaGroupRepository(BiobaseDbContext dbContext, ILogger<SQLTaxaGroupRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<TaxaGroups>> GetAllAsync()
        {
            return await _dbContext.taxa_groups.ToListAsync();
        }
    }
}
