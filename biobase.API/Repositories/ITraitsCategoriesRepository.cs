using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface ITraitsCategoriesRepository
    {
        Task<List<TraitsCategories>> GetAllTraitsCategoriesAsync(string? soortgroep = null);
    }
}
