using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface IHabitatCodesRepository
    {
        Task<List<HabitatCodes>> GetHabitatsAsync(
       string? habitat_classification = null);
    }
}
