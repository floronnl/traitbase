using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface IEuHabitatClassesRepository
    {
        Task<List<EuHabitatClasses>> GetAllAsync();
    }
}
