//using biobase.API.Models.Domain;
using biobase.API.Models.Domain;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace biobase.API.Repositories
{
    public interface ITraitsRepository
    {
        Task<IDictionary<string, object>> GetTraitsSingleAsync(int soortnummer);

        Task<List<IDictionary<string, object>>> GetTraitsPivotAsync();
    }
}
