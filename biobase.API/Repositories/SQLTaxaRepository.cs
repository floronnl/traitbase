using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace biobase.API.Repositories
{
    public class SQLTaxaRepository : ITaxaRepository
    {
        private readonly BiobaseDbContext _dbContext;

        public SQLTaxaRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET ALL TAXA GROUPS
        public async Task<List<TaxaGroups>> GetTaxaGroupsAsync()
        {
            var taxaGroupsQuery = _dbContext.taxa_groups.AsQueryable();
            return await taxaGroupsQuery.ToListAsync();
        }

        // GET ALL TAXA
        public async Task<List<Taxa>> GetTaxaAsync(
            string? taxon_class = null, int? species_id = null, string? rl = null)
        {
            var taxaQuery = _dbContext.taxa.AsQueryable();

            // Apply filtering if 'soortgroep' is provided
            if (!string.IsNullOrWhiteSpace(taxon_class))
            {
                taxaQuery = taxaQuery.Where(x => x.groep.Contains(taxon_class, StringComparison.OrdinalIgnoreCase));
            }
            if (species_id.HasValue) 
            {
                taxaQuery = taxaQuery.Where(x => x.soortnummer == species_id.Value);
            }
            if (!string.IsNullOrWhiteSpace(rl))
            {
                taxaQuery = taxaQuery.Where(x => x.rl.Equals(rl, StringComparison.OrdinalIgnoreCase));
            }

            return await taxaQuery.ToListAsync();
        }
    }
}
