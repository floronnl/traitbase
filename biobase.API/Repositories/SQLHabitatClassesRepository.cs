using biobase.API.Data;
using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLHabitatClassesRepository : IHabitatClassesRepository
    {
        private readonly BiobaseDbContext _dbContext;

        public SQLHabitatClassesRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HabitatClasses>> GetHabitatsAsync(
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
