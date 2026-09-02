using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLHabitatTaxaRepository : IHabitatTaxaRepository
    {
        private readonly BiobaseDbContext _dbContext;

        public SQLHabitatTaxaRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HabitatTaxa>> GetHabitatTaxaAsync(
            string? habitat_classification = null,
            string? habitat_code = null,
            string? taxon_category = null,
            string? rl = null,
            string? soortgroep = null
            ) {

            var query = _dbContext.habitat_taxa.AsQueryable();

            if (!string.IsNullOrWhiteSpace(habitat_classification))
            {
                query = query.Where(x => x.habitat_classification == habitat_classification);
            }
            if (!string.IsNullOrWhiteSpace(habitat_code))
            {
                query = query.Where(x => x.habitat_code == habitat_code);
            }
            if (!string.IsNullOrWhiteSpace(taxon_category))
            {
                query = query.Where(x => x.taxon_category == taxon_category);
            }
            if (!string.IsNullOrWhiteSpace(rl))
            {
                query = query.Where(x => x.rl == rl);
            }
            if (!string.IsNullOrWhiteSpace(soortgroep))
            {
                query = query.Where(x => x.soortgroep == soortgroep);
            }

            return await query.ToListAsync();
        }
    }
}