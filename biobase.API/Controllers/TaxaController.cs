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
    [Route("api/[controller]")]
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
        /// Retrieves a list of all taxa, with an optional filter by taxon class or species ID.
        /// </summary>
        /// <param name="taxon_class">
        /// Optional filter for taxa group. Use Dutch abbreviations like 'V' for vascular plants or 'R' for reptiles. If not provided, all taxa groups will be returned.
        /// </param>
        /// <param name="species_id">
        /// Optional filter to retrieve data for one species by providing the species ID. Species ID correspond to the ones in Verspreidingsatlas.
        /// </param>
        /// <param name="rl">
        /// Optional filter for Red List status. Use the Dutch abbreviations, for example 'BE' or 'KW'
        /// </param>
        /// <param name="format">
        /// The format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the taxa data.</returns>
        [HttpGet]
        [SwaggerOperation(
            Tags = new[] { "2. Taxa" },
            Summary = "Get all taxa", Description = "Retrieve a list of all taxa, optionally filtered by red list status or taxa group. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The list of taxa was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetTaxaAsync(
            [FromQuery] string? taxon_class, 
            [FromQuery] int? species_id, 
            [FromQuery] string? rl,
            [FromQuery] string format = "csv")
        {
            try
            {
                var taxaDomain = await _taxaRepository.GetTaxaAsync(taxon_class, species_id, rl);
                var taxaDto = _mapper.Map<List<TaxaDto>>(taxaDomain);

                if (format.ToLower() == "json")
                {
                    return Ok(taxaDto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(taxaDto);
                    return File(csvData, "text/csv", $"biobase_export_taxa_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
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
