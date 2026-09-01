using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface IHabitatClassesRepository
    {
        Task<List<HabitatClasses>> GetHabitatsAsync(
       string? habitat_classification = null);
    }
}
