using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface IEuTaxaRepository
    {
        Task<List<EuTaxa>> GetEuTaxaAsync(
    string? directive = null);
    }
}
