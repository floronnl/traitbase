using AutoMapper;
using biobase.API.Models.DTO;
using biobase.API.Repositories;
using biobase.API.Services;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using Swashbuckle.AspNetCore.Annotations;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace biobase.API.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class HabitatClassesTaxaController : ControllerBase
    {
        private readonly IHabitatClassesTaxaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICsvExportService _csvExportService;
        private readonly ILogger<HabitatClassesTaxaController> _logger;

        public HabitatClassesTaxaController(IHabitatClassesTaxaRepository repository, IMapper mapper, ICsvExportService csvExportService, ILogger<HabitatClassesTaxaController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _csvExportService = csvExportService;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves data on habitat classes and associated taxa
        /// At least one of the filters below must be specified.
        /// </summary>
        /// <param name="habitatClassification">
        /// Optional filter for habitat classification: 'beheertype', 'cultuurdoeltype', 'habitattype' of 'natuurdoeltype'.
        /// </param>
        /// <param name="format">
        /// Specify format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the habitat data.</returns>
        [HttpGet("habitatCodes")]
        [SwaggerOperation(
            Tags = new[] { "1.1 Habitat codes" },
            Summary = "Get habitat codes data", Description = "Retrieve a list of all habitat codes with an optional filter for habitat classification.  \nNo API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The data was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(404, "Query unsuccesfull. Please double check the filters-input.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetHabitatCodes(
            [FromQuery] string? habitatClassification,
            [FromQuery] string format = "csv")
        {
            try
            {
                var habitat_codes = await _repository.GetHabitatCodesAsync(habitatClassification);
                var habitatDto = _mapper.Map<List<HabitatCodesDto>>(habitat_codes);

                if (format.ToLower() == "json")
                {
                    return Ok(habitatDto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(habitatDto);
                    return File(csvData, "text/csv", $"traitbase_export_habitatcodes_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting habitat codes.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Retrieves data on habitat classes and associated taxa
        /// At least one of the filters below must be specified.
        /// </summary>
        /// <param name="habitatClassification">
        /// Filter for habitat classes: 'beheertype', 'cultuurdoeltype', 'habitattype' of 'natuurdoeltype'.
        /// </param>
        /// <param name="habitatCode">
        /// Filter for habitat code (e.g. '3.46' or 'N15.01').
        /// </param>
        /// <param name="taxonCategory">
        /// Filter for taxon category (e.g. 'snlsoort' or 'typischesoort').
        /// </param>
        /// <param name="threatStatus">
        /// Filter for Red List status. Use the Dutch abbreviations (e.g. 'BE' or 'KW'). 
        /// </param>
        /// <param name="taxonGroup">
        /// Filter for taxon class. Use the Dutch names (e.g. 'Vaatplanten' or 'Libellen').
        /// </param>
        /// <param name="format">
        /// Specify format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the habitat data.</returns>
        [HttpGet]
        [SwaggerOperation(
            Tags = new[] { "1.2 Habitat classes - Taxa" },
            Summary = "Get Habitat-Species data", Description = "Retrieve a habitat class and the associated species (or vice versa).  \nA valid API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The data was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(404, "Query unsuccesfull. Please double check the filters-input.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string? habitatClassification, [FromQuery] string? habitatCode, 
            [FromQuery] string? taxonCategory, [FromQuery] string? threatStatus, [FromQuery] string? taxonGroup, 
            [FromQuery] string format = "csv")
        {
            try
            {
                // Ensure at least one filter is provided
                if (string.IsNullOrEmpty(habitatClassification) && string.IsNullOrEmpty(habitatCode) &&
                    string.IsNullOrEmpty(taxonCategory) && string.IsNullOrEmpty(threatStatus) &&
                    string.IsNullOrEmpty(taxonGroup))
                {
                    return BadRequest("At least one filter must be specified.");
                }

                var habitatDomain = await _repository.GetHabitatTaxaAsync(habitatClassification, habitatCode, taxonCategory, threatStatus, taxonGroup);
                var habitatDto = _mapper.Map<List<HabitatClassesTaxaDto>>(habitatDomain);

                if (format.ToLower() == "json")
                {
                    return Ok(habitatDto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(habitatDto);
                    return File(csvData, "text/csv", $"traitbase_export_habitattaxa_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all taxa.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
