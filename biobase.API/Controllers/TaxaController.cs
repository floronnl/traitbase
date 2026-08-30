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
        private readonly ITaxaGroupRepository _taxaGroupRepository;
        private readonly ITaxaRepository _taxaRepository;
        private readonly IHabitatTaxaRepository _habitatTaxaRepository;
        private readonly IMapper _mapper;
        private readonly ICsvExportService _csvExportService;
        private readonly ILogger<TaxaController> _logger;

        public TaxaController(
            ITaxaGroupRepository taxaGroupRepository,
            ITaxaRepository taxaRepository, 
            IHabitatTaxaRepository habitatTaxaRepository,
            IMapper mapper, 
            ICsvExportService csvExportService, 
            ILogger<TaxaController> logger
            ) {
            _taxaGroupRepository = taxaGroupRepository;
            _taxaRepository = taxaRepository;
            _habitatTaxaRepository = habitatTaxaRepository;
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
                var taxaGroupsDomain = await _taxaGroupRepository.GetAllAsync();
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
        /// Retrieves a list of all taxa, with an optional filter by taxa group, threat status, or habitat directive.
        /// </summary>
        /// <param name="taxaGroup">
        /// Optional filter for taxa group. Use Dutch abbreviations like 'V' for vascular plants or 'R' for reptiles. If not provided, all taxa groups will be returned.
        /// </param>
        /// <param name="taxonId">
        /// Optional filter to retrieve data for a single taxon by providing its ID. Taxon ID correspond to the ones in Verspreidingsatlas.
        /// </param>
        /// <param name="threatStatus">
        /// Optional filter for threat status. Use the Dutch abbreviations, for example 'BE' or 'KW'
        /// </param>
        /// <param name="format">
        /// The format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the taxa data.</returns>
        [HttpGet("taxa")]
        [SwaggerOperation(
            Tags = new[] { "2.2 Taxa" },
            Summary = "Get all taxa", Description = "Retrieve a list of all taxa, optionally filtered by red list status or taxa group.  \nNo API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The list of taxa was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetTaxaAsync(
            [FromQuery] string? taxaGroup,
            [FromQuery] int? taxonId,
            [FromQuery] string? threatStatus,
            [FromQuery] string format = "csv")
        {
            try
            {
                var taxaDomain = await _taxaRepository.GetTaxaAsync(taxaGroup, taxonId, threatStatus);
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
        /// <param name="taxaGroup">
        /// Filter for taxa group. Use the Dutch names (e.g. 'Vaatplanten' or 'Libellen').
        /// </param>
        /// <param name="format">
        /// Specify format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the habitat data.</returns>
        [HttpGet("habitatTaxa")]
        [SwaggerOperation(
            Tags = new[] { "2.3 Associated taxa per habitat class" },
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

                var habitatDomain = await _repository.GetHabitatTaxaAsync(habitatClassification, habitatCode, taxonCategory, threatStatus, taxaGroup);
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
