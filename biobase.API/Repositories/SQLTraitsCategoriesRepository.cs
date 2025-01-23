using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLTraitsCategoriesRepository : ITraitsCategoriesRepository
    {
        private readonly BiobaseDbContext _dbContext;

        public SQLTraitsCategoriesRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET ALL TRAITS CATEGORIES
        public async Task<List<TraitsCategories>> GetAllTraitsCategoriesAsync(string taxon_class)
        {
            var traitsCategoryQuery = _dbContext.traits_categories.AsQueryable();

            // Apply filtering if 'rubriek' is provided
            if (!string.IsNullOrWhiteSpace(taxon_class))
            {
                traitsCategoryQuery = traitsCategoryQuery.Where(x => 
                x.soortgroep.Equals(taxon_class, StringComparison.OrdinalIgnoreCase));
            }
            return await traitsCategoryQuery.ToListAsync();
        }
    }
}
