using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public class IEuHabitatCodesRepository
    {
        Task<List<EuHabitatCodes>> GetAllAsync();
    }
}
