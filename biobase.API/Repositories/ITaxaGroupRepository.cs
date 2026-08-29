using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface ITaxaGroupRepository
    {
        Task<List<TaxaGroups>> GetAllAsync();
    }
}
