using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface IHabitatClassesTaxaRepository
    {
        Task<List<HabitatClassesTaxa>> GetHabitatTaxaAsync(
            string? habitat_classification, string? habitat_code, 
            string? taxon_category, string? rl, string? soortgroep);
    }
}
