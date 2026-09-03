using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface IEuHabitatTaxaRepository
    {
        Task<List<EuHabitatTaxa>> GetEuHabitatTaxaAsync(
        string? habitatCode = null, 
        string? taxaGroup = null);
    }
}
