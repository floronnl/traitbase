using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface ITaxaRepository
    {
        Task<List<TaxaGroups>> GetTaxaGroupsAsync();
        Task<List<Taxa>> GetTaxaAsync(
            string? soortgroep = null, int? soortnummer = null, string? rl = null);
    }
}
