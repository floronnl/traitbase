using biobase.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace biobase.API.Repositories
{
    public class SQLHabitatCodesRepository : IHabitatCodesRepository
    {
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
