using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLEuHabitatTaxaRepository : IEuHabitatTaxaRepository
    {
        private readonly BiobaseDbContext _dbContext;
        public SQLEuHabitatTaxaRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EuHabitatTaxa>> GetEuHabitatTaxaAsync(
            string? habitatCode = null, 
            string? taxaGroup = null)
        {
            var query = _dbContext.eu_habitat_taxa.AsQueryable();
            if (!string.IsNullOrEmpty(habitatCode))
            {
                query = query.Where(e => e.habitat_code == habitatCode);
            }
            if (!string.IsNullOrEmpty(taxaGroup))
            {
                query = query.Where(e => e.taxa_group == taxaGroup);
            }
            return await query.ToListAsync();
        }
    }
}