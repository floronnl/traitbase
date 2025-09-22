
using biobase.API.Repositories;
using biobase.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace biobase.API.Controllers
{
    /// <summary>
    /// Controller for handling operations related to traits.
    /// Provides functionality to retrieve traits and export them as CSV or JSON files.
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class TraitsController : ControllerBase
    {
        private readonly ITraitsRepository _traitsRepository;
        private readonly ICsvExportService _csvExportService;
        private readonly ILogger<TraitsController> _logger;

        public TraitsController(ITraitsRepository traitsRepository, ICsvExportService csvExportService, ILogger<TraitsController> logger)
        {
            _traitsRepository = traitsRepository;
            _csvExportService = csvExportService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all traits for one species.
        /// The result is returned as a downloadable CSV file or JSON.
        /// </summary>
        /// <param name="taxonId"> 
        /// Provide a single taxon ID (soortnummer). Taxon IDs correspond to the ones in Verspreidingsatlas.
        /// </param>
        /// <param name="format">
        /// The format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the traits data.</returns>
        [HttpGet("traitsSingleTaxon")]
        [SwaggerOperation(
            Tags = new[] { "4.1 Traits - single taxon" },
            Summary = "Get all traits for one species",
            Description = "Retrieve a list of all traits for one species.  \nA valid API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The list of species traits was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(404, "No data found for the specified taxon ID.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetTraitsSingleSpecies(
            [FromQuery, Required(ErrorMessage = "Taxon ID is a required parameter.")] int taxonId,
            [FromQuery] string format = "csv")
        {
            try
            {
                if (taxonId <= 0)
                {
                    return BadRequest("The field 'taxonId' is missing or invalid.");
                }

                // Fetch data dynamically from repository
                var traits = await _traitsRepository.GetTraitsSingleAsync(taxonId);

                if (format.ToLower() == "json")
                {
                    // Return JSON response
                    return Ok(traits);
                }
                else if (format.ToLower() == "csv")
                {
                    // Convert dictionary to a list of key-value pairs for CSV export
                    var csvData = await _csvExportService.ExportToCsvAsync(new List<IDictionary<string, object>> { traits });
                    // Return the CSV-file
                    return File(csvData, "text/csv", $"traitbase_export_traits_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the data.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Retrieves a list of all traits for vascular plants, mosses, lichens, and stoneworts. The response format can be CSV or JSON.
        /// The result is returned as a downloadable CSV file or JSON.
        /// </summary>
        /// <param name="format">
        /// The format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the traits data.</returns>
        [HttpGet("traitsPivot")]
        [SwaggerOperation(
            Tags = new[] { "4.2 Traits table for plants, mosses, lichens, and stoneworts" },
            Summary = "Get a pivot table with traits for all vascular plants, mosses, lichens, and stoneworts",
            Description = "Retrieve a table with all traits for vascular plants, mosses, lichens, and stoneworts.  \nNo API key is required to access this endpoint.The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The list of species traits was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]

        public async Task<IActionResult> GetTraitsPivot(
            [FromQuery] string format = "csv")
        {
            try
            {
                // Fetch data dynamically from repository
                var traitsPivot = await _traitsRepository.GetTraitsPivotAsync();

                if (format.ToLower() == "json")
                {
                    // Return JSON response
                    return Ok(traitsPivot);
                }
                else if (format.ToLower() == "csv")
                {
                    // Convert dictonary to a list of key-value pairs for CSV export
                    var csvData = await _csvExportService.ExportToCsvAsync(traitsPivot);
                    // Return the CSV-file
                    return File(csvData, "text/csv", $"traitbase_export_traits_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the data.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

