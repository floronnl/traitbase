using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLTaxaGroupRepository : ITaxaGroupRepository
    {
        private readonly BiobaseDbContext _dbContext;

        public SQLTaxaGroupRepository(BiobaseDbContext dbContext)
        { 
            _dbContext = dbContext;
        }

        public async Task<List<TaxaGroups>> GetAllAsync()
        {
            return await _dbContext.taxa_groups.ToListAsync();
        }
    }
}
