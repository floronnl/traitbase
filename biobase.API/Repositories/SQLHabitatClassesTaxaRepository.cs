using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLHabitatClassesTaxaRepository : IHabitatClassesTaxaRepository
    {
        private readonly BiobaseDbContext _dbContext;
        private readonly ILogger<SQLHabitatClassesTaxaRepository> _logger;

        public SQLHabitatClassesTaxaRepository(BiobaseDbContext dbContext, ILogger<SQLHabitatClassesTaxaRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<HabitatClassesTaxa>> GetHabitatTaxaAsync(
            string? habitat_classification, string? habitat_code, 
            string? taxon_category, string? rl, string? taxon_class)
        {
            var query = _dbContext.habitat_classes_taxa.AsQueryable();

            if (!string.IsNullOrEmpty(habitat_classification))
            {
                query = query.Where(x => x.habitat_classification != null && x.habitat_classification.Equals(habitat_classification, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(habitat_code))
            {
                query = query.Where(x => x.habitat_code != null && x.habitat_code.Equals(habitat_code, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(taxon_category))
            {
                query = query.Where(x => x.taxon_category != null && x.taxon_category.Equals(taxon_category, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(rl))
            {
                query = query.Where(x => x.rl != null && x.rl.Equals(rl, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(taxon_class))
            {
                query = query.Where(x => x.soortgroep != null && x.soortgroep.Equals(taxon_class, StringComparison.OrdinalIgnoreCase));
            }
                
            return await query.ToListAsync();
        }
        public async Task<List<HabitatCodes>> GetHabitatCodesAsync(string? habitat_classification = null)
        {
            var query = _dbContext.habitat_classes.AsQueryable(); // Target the correct table

            if (!string.IsNullOrWhiteSpace(habitat_classification))
            {
                query = query.Where(x => x.habitat_classification == habitat_classification);
            }

            return await query.ToListAsync();
        }
    }
}
