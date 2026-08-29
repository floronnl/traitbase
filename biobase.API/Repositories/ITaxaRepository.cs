using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface ITaxaRepository
    {
        Task<List<Taxa>> GetTaxaAsync();
    }
}
