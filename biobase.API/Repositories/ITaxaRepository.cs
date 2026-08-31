using biobase.API.Models.Domain;

namespace biobase.API.Repositories
{
    public interface ITaxaRepository
    {
        Task<List<Taxa>> GetTaxaAsync(
            string? taxon_class = null, 
            int? species_id = null, 
            string? rl = null
            );
    }
}
