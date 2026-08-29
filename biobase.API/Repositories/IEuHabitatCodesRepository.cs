using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface IEuHabitatCodesRepository
    {
        Task<List<EuHabitatCodes>> GetAllAsync();
    }
}
