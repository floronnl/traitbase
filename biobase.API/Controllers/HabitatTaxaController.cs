using AutoMapper;
using biobase.API.Models.DTO;
using biobase.API.Repositories;
using biobase.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace biobase.API.Controllers
{
    /// <summary>
    /// Controller to handle API requests for Habitat-Taxa. Provides endpoints to retrieve and export taxa data as CSV or JSON files.
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api")]
    [ApiController]
    public class HabitatTaxaController : ControllerBase
    {
        private readonly IHabitatTaxaRepository _habitatTaxaRepository;
        private readonly IEuHabitatTaxaRepository _euHabitatTaxaRepository;
        private readonly IMapper _mapper;
        private readonly ICsvExportService _csvExportService;
        private readonly ILogger<HabitatTaxaController> _logger;

        public HabitatTaxaController(
            IHabitatTaxaRepository habitatTaxaRepository,
            IEuHabitatTaxaRepository euHabitatTaxaRepository,
            IMapper mapper,
            ICsvExportService csvExportService,
            ILogger<HabitatTaxaController> logger
            )
        {
            _habitatTaxaRepository = habitatTaxaRepository;
            _euHabitatTaxaRepository = euHabitatTaxaRepository;
            _mapper = mapper;
            _csvExportService = csvExportService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all associated taxa per habitat, with an optional filter by habitat classification, habitat code, taxon category, threat status, or taxa group. Please not that at least one filter must be specified. If no filters are provided, a BadRequest response will be returned.
        /// </summary>
        /// <param name="habitatClassification">
        /// Optional filter to retrieve data for a specific habitat classification, for example 'basiskwaliteitnatuur' or 'natuurdoeltype'.
        /// </param>
        /// <param name="habitatCode">
        /// Optional filter to retrieve data for a specific habitat code, for example 'landelijk_rivierenlandschap' or '2.1'.
        /// </param>
        /// <param name="taxonCategory">
        /// Optional filter to retrieve data for a specific taxon category, for example 'basiskwaliteitnatuur' or 'doelsoort'.
        /// </param>
        /// <param name="taxaGroup">
        /// Optional filter for taxa group. Use Dutch names like 'Vaatplanten' or 'Reptielen'. If not provided, all taxa groups will be returned.
        /// </param>
        /// <param name="threatStatus">
        /// Optional filter for threat status. Use the Dutch abbreviations, for example 'BE' or 'KW'
        /// </param>
        /// <param name="format">
        /// The format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the filtered habitat-taxa data.</returns>
        [HttpGet("habitatTaxa")]
        [SwaggerOperation(
            Tags = new[] { "2.1 Associated taxa per habitat class" },
            Summary = "Get Habitat-Taxa data", Description = "Retrieve a habitat class and the associated taxon (or vice versa).  \nA valid API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The data was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(404, "Query unsuccesfull. Please double check the filters-input.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string? habitatClassification, [FromQuery] string? habitatCode,
            [FromQuery] string? taxonCategory, [FromQuery] string? threatStatus, [FromQuery] string? taxaGroup,
            [FromQuery] string format = "csv")
        {
            try
            {
                // Ensure at least one filter is provided
                if (string.IsNullOrEmpty(habitatClassification) && string.IsNullOrEmpty(habitatCode) &&
                    string.IsNullOrEmpty(taxonCategory) && string.IsNullOrEmpty(threatStatus) &&
                    string.IsNullOrEmpty(taxaGroup))
                {
                    return BadRequest("At least one filter must be specified.");
                }

                var habitatDomain = await _habitatTaxaRepository.GetHabitatTaxaAsync(habitatClassification, habitatCode, taxonCategory, threatStatus, taxaGroup);
                var habitatDto = _mapper.Map<List<HabitatTaxaDto>>(habitatDomain);

                if (format.ToLower() == "json")
                {
                    return Ok(habitatDto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(habitatDto);
                    return File(csvData, "text/csv", $"Traitbase_export_HabitatTaxa_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
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

        /// <summary>
        /// Retrieves a list of all associated taxa per EU habitat class, with an optional filter by directive (Bird or Habitat Directive).
        /// </summary>
        /// <param name="habitatCode">
        /// Optional filter to retrieve all taxa for a specific habitat code, for example 'H2330' or 'H1160'.
        /// </param>
        /// <param name="taxaGroup">
        /// Optional filter to retrieve data for a specific taxa group. Use Dutch names like 'Vogels' or 'Mossen'.
        /// </param>
        /// <param name="format">
        /// The format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the filtered EU habitat-taxa data.</returns>
        [HttpGet("euHabitatTaxa")]
        [SwaggerOperation(
            Tags = new[] { "2.2 Associated taxa per EU habitat class" },
            Summary = "Get EU Habitat-Taxa data", 
            Description = "Retrieve a habitat class and the associated taxon (or vice versa).\nNo API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The data was successfully retrieved.")]
        [SwaggerResponse(404, "Query unsuccesfull. Please double check the filters-input.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetEuHabitatTaxa(
            [FromQuery] string? habitatCode,
            [FromQuery] string? taxaGroup,
            [FromQuery] string format = "csv")
        {

            var euHabitatTaxaDomain = await _euHabitatTaxaRepository.GetEuHabitatTaxaAsync(habitatCode, taxaGroup);
            var euHabitatTaxaDto = _mapper.Map<List<EuHabitatTaxaDto>>(euHabitatTaxaDomain);

            if (format.ToLower() == "json")
            {
                return Ok(euHabitatTaxaDto);
            }
            else if (format.ToLower() == "csv")
            {
                var csvData = await _csvExportService.ExportToCsvAsync(euHabitatTaxaDto);
                return File(csvData, "text/csv", $"Traitbase_export_EU_HabitatTaxa_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
            }
            else
            {
                return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
            }
        }
    }
}
