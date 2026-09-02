using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLEuTaxaRepository : IEuTaxaRepository
    {
        private readonly BiobaseDbContext _dbContext;

        public SQLEuTaxaRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EuTaxa>> GetEuTaxaAsync(
            string? directive = null
            )
        {
            var query = _dbContext.eu_bhd_species.AsQueryable();

            if (!string.IsNullOrWhiteSpace(directive))
            {
                query = query.Where(x => x.directive == directive);
            }

            return await query.ToListAsync();
        }
    }
}
