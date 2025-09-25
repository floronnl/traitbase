using AutoMapper;
using biobase.API.Models.DTO;
using biobase.API.Repositories;
using biobase.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace biobase.API.Controllers
{
    /// <summary>
    /// Controller to handle API requests for Taxa. Provides endpoints to retrieve and export taxa data as CSV or JSON files.
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/taxa")]
    [ApiController]
    public class TaxaController : ControllerBase
    {
        private readonly ITaxaRepository _taxaRepository;
        private readonly IMapper _mapper;
        private readonly ICsvExportService _csvExportService;
        private readonly ILogger<TaxaController> _logger;

        public TaxaController(ITaxaRepository taxaRepository, IMapper mapper, ICsvExportService csvExportService, ILogger<TaxaController> logger)
        {
            _taxaRepository = taxaRepository;
            _mapper = mapper;
            _csvExportService = csvExportService;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves a list of all taxa groups
        /// </summary>
        /// <param name="format">
        /// The format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the taxa data.</returns>
        [HttpGet("taxaGroup")]
        [SwaggerOperation(
            Tags = new[] { "2.1 Taxa groups" },
            Summary = "Get all taxa groups", Description = "Retrieve a list of all taxa groups.\nNo API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The list of taxa groups was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetTaxaGroupsAsync(
            [FromQuery] string format = "csv")
        {
            try
            {
                var taxaGroupsDomain = await _taxaRepository.GetTaxaGroupsAsync();
                var taxaGroupsDto = _mapper.Map<List<TaxaGroupsDto>>(taxaGroupsDomain);

                if (format.ToLower() == "json")
                {
                    return Ok(taxaGroupsDto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(taxaGroupsDto);
                    return File(csvData, "text/csv", $"traitbase_export_taxagroups_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all taxa groups.");
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

            /// <summary>
            /// Retrieves a list of all taxa, with an optional filter by taxon class or species ID.
            /// </summary>
            /// <param name="taxaGroup">
            /// Optional filter for taxa group. Use Dutch abbreviations like 'V' for vascular plants or 'R' for reptiles. If not provided, all taxa groups will be returned.
            /// </param>
            /// <param name="taxonId">
            /// Optional filter to retrieve data for one species by providing the taxon ID. Taxon ID correspond to the ones in Verspreidingsatlas.
            /// </param>
            /// <param name="threatStatus">
            /// Optional filter for threat status. Use the Dutch abbreviations, for example 'BE' or 'KW'
            /// </param>
            /// <param name="format">
            /// The format in which to return the data, either "csv" or "json". Default is "csv".
            /// </param>
            /// <returns>A downloadable CSV file or JSON response containing the taxa data.</returns>
            [HttpGet]
        [SwaggerOperation(
            Tags = new[] { "2.2 Taxa" },
            Summary = "Get all taxa", Description = "Retrieve a list of all taxa, optionally filtered by red list status or taxa group.  \nNo API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The list of taxa was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetTaxaAsync(
            [FromQuery] string? taxonGroup, 
            [FromQuery] int? taxonId, 
            [FromQuery] string? threatStatus,
            [FromQuery] string format = "csv")
        {
            try
            {
                var taxaDomain = await _taxaRepository.GetTaxaAsync(taxonGroup, taxonId, threatStatus);
                var taxaDto = _mapper.Map<List<TaxaDto>>(taxaDomain);

                if (format.ToLower() == "json")
                {
                    return Ok(taxaDto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(taxaDto);
                    return File(csvData, "text/csv", $"traitbase_export_taxa_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
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
