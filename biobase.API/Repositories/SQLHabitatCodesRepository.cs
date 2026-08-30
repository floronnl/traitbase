using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLHabitatCodesRepository : IHabitatCodesRepository
    {
        private readonly BiobaseDbContext _dbContext;

        public SQLHabitatCodesRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HabitatCodes>> GetHabitatsAsync(
            string? habitat_classification = null
            ) {
            var query = _dbContext.habitat_classes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(habitat_classification))
            {
                query = query.Where(x => x.habitat_classification == habitat_classification);
            }

            return await query.ToListAsync();
        }
    }
}
