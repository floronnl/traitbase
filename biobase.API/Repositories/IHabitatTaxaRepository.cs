using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface IHabitatTaxaRepository
    {
        Task<List<HabitatTaxa>> GetHabitatTaxaAsync(
            string? habitat_classification, string? habitat_code, 
            string? taxon_category, string? rl, string? soortgroep);
    };
}
