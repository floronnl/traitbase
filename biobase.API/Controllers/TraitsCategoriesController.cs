using AutoMapper;
using biobase.API.Models.DTO;
using biobase.API.Repositories;
using biobase.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace biobase.API.Controllers
{
    /// <summary>
    /// Controller to handle API requests for TraitsCategories. Provides endpoints to retrieve and export traits category data as CSV or JSON files.
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class TraitsCategoriesController : ControllerBase
    {
        private readonly ITraitsCategoriesRepository _traitsCategoriesRepository;
        private readonly IMapper _mapper;
        private readonly ICsvExportService _csvExportService;
        private readonly ILogger<TraitsCategoriesController> _logger;

        public TraitsCategoriesController(ITraitsCategoriesRepository traitsCategoriesRepository, IMapper mapper, ICsvExportService csvExportService, ILogger<TraitsCategoriesController> logger)
        {
            _traitsCategoriesRepository = traitsCategoriesRepository;
            _mapper = mapper;
            _csvExportService = csvExportService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of traits categories for a specific taxon group.
        /// The result is returned as a downloadable CSV file or JSON.
        /// </summary>
        /// <param name="taxon_class">
        /// Optional filter for trait categories belonging to a specific taxa class (e.g. 'V' for vascular plants or 'R' for reptiles).
        /// </param>
        /// <param name="format">
        /// The format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable CSV file or JSON response containing the traitsCategories data.</returns>
        [HttpGet]
        [SwaggerOperation(
            Tags = new[] { "3. Traits categories" }, 
            Summary = "Traits categories for a taxon class", Description = "Retrieve a list of all traits categories for a specific taxon class. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The list of traits categories was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? taxon_class,
            [FromQuery] string format = "csv")
        {
            try
            {
                var traitsCategoriesDomain = await _traitsCategoriesRepository.GetAllTraitsCategoriesAsync(taxon_class);
                var traitsCategoriesDto = _mapper.Map<List<TraitsCategoriesDto>>(traitsCategoriesDomain);

                if (format.ToLower() == "json")
                {
                    return Ok(traitsCategoriesDto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(traitsCategoriesDto);
                    return File(csvData, "text/csv", $"{DateTime.Now:yyyyMMdd}_traitsCategories_{taxon_class}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all traitsCategories.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
