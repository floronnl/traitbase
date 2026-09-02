using biobase.API.Data;
using biobase.API.Repositories;

namespace biobase.API.Repositories
{
    public class SQLEuHabitatTaxaRepository : IEuHabitatTaxaRepository
    {
        private readonly BiobaseDbContext _dbContext;
        public SQLEuHabitatTaxaRepository(BiobaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}