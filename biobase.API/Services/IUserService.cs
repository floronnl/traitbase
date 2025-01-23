using biobase.API.Models.Domain;

namespace biobase.API.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByApiKeyAsync(string apiKey);
    }
}
