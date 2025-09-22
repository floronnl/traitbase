using biobase.API.Services;

namespace biobase.API.Middleware
{
    /// <summary>
    /// Middleware to enforce API key validation for most API requests.
    /// The Taxa endpoints are excluded from this validation and are publicly accessible.
    /// TraitsCategories endpoints require a specific API key.
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private readonly IConfiguration _configuration; // For retrieving API keys from appsettings
        private const string ApiKeyHeaderName = "X-API-KEY";

        /// <summary>
        /// Constructor for the ApiKeyMiddleware class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger instance for logging API key validation events.</param>
        /// <param name="configuration">The configuration to get API keys from appsettings.</param>
        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration; // Inject configuration
        }

        /// <summary>
        /// Invokes the middleware to check if the API key is provided in the request headers.
        /// Taxa endpoints are public, and TraitsCategories endpoints require a specific API key.
        /// </summary>
        /// <param name="context">The HTTP context of the current request.</param>
        /// <param name="userService">The user service to validate the API key against.</param>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            // Bypass API key validation for Taxa endpoints (public)
            if (
                context.Request.Path.StartsWithSegments("/api/taxa", StringComparison.OrdinalIgnoreCase) || 
                context.Request.Path.StartsWithSegments("/api/traits/pivot", StringComparison.OrdinalIgnoreCase) ||
                context.Request.Path.StartsWithSegments("/api/habitatclassestaxa/habitatcodes", StringComparison.OrdinalIgnoreCase)
                )
            {
                await _next(context); // Skip API key validation for Taxa and Pivot table endpoints
                return;
            }


            // TraitsCategories: check for specific API key
        //    if (context.Request.Path.StartsWithSegments("/api/TraitsCategories", StringComparison.OrdinalIgnoreCase))
        //    {
        //        var traitsCategoriesApiKey = _configuration["ApiKeys:TraitsCategoriesControllerApiKey"]; // Get specific key from config
        //
        //        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey) || providedApiKey != traitsCategoriesApiKey)
        //        {
        //            _logger.LogWarning("Unauthorized access to TraitsCategories");
        //            context.Response.StatusCode = 403; // Forbidden
        //            await context.Response.WriteAsync("Unauthorized access to TraitsCategories");
        //            return;
        //        }
        //
        //        await _next(context); // Allow access for valid API key
        //        return;
        //    }

            // General API key validation for other endpoints
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                _logger.LogWarning("API Key was not provided.");
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            var user = await userService.GetUserByApiKeyAsync(potentialApiKey);
            if (user == null)
            {
                _logger.LogWarning("Invalid API Key");
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Invalid API key");
                return;
            }

            await _next(context); // Proceed with the request for valid API key
        }
    }
}
