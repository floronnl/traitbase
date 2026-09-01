using AutoMapper;
using biobase.API.Models.DTO;
using biobase.API.Repositories;
using biobase.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace biobase.API.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/habitatClasses")]
    [ApiController]
    public class HabitatClassesController : ControllerBase
    {
        private readonly IHabitatClassesRepository _habitatClassesRepository;
        private readonly IEuHabitatClassesRepository _euHabitatClassesRepository;
        private readonly IMapper _mapper;
        private readonly ICsvExportService _csvExportService;
        private readonly ILogger<HabitatClassesController> _logger;

        public HabitatClassesController(
            IHabitatClassesRepository repository, 
            IEuHabitatClassesRepository euHabitatClassesRepository,
            IMapper mapper, 
            ICsvExportService csvExportService, 
            ILogger<HabitatClassesController> logger
            ) {
            _habitatClassesRepository = repository;
            _euHabitatClassesRepository = euHabitatClassesRepository;
            _mapper = mapper;
            _csvExportService = csvExportService;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves data on habitat classes
        /// </summary>
        /// <param name="habitatClassification">
        /// Optional filter for habitat classification: 'beheertype', 'cultuurdoeltype', 'habitattype', or 'natuurdoeltype'.
        /// </param>
        /// <param name="format">
        /// Specify format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable response containing the habitat classes.</returns>
        [HttpGet("getHabitatClasses")]
        [SwaggerOperation(
            Tags = new[] { "1.1 Habitat classes" },
            Summary = "Get habitat classes data", Description = "Retrieve a list of all habitat classes with an optional filter for a specific classification.  \nNo API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The data was successfully retrieved.")]
        [SwaggerResponse(401, "API Key is missing or invalid.")]
        [SwaggerResponse(404, "Query unsuccesfull. Please double check the filters-input.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetHabitatClasses(
            [FromQuery] string? habitatClassification,
            [FromQuery] string format = "csv")
        {
            try
            {
                var habitat_classes = await _habitatClassesRepository.GetHabitatsAsync(habitatClassification);
                var habitatDto = _mapper.Map<List<HabitatClassesDto>>(habitat_classes);

                if (format.ToLower() == "json")
                {
                    return Ok(habitatDto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(habitatDto);
                    return File(csvData, "text/csv", $"traitbase_export_habitatclasses_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting habitat classes.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getEuHabitatClasses")]
        [SwaggerOperation(
    Tags = new[] { "1.2 EU Habitat classes" },
    Summary = "Get EU habitat classes data",
    Description = "Retrieve EU habitat class definitions, descriptions and Annex I priority status.  \nNo API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The data was successfully retrieved.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetEuHabitatClasses(
            [FromQuery] string format = "csv"
            )
        {
            try
            {
                var euHabitatClasses = await _euHabitatClassesRepository.GetAllAsync();
                var dto = _mapper.Map<List<EuHabitatClassesDto>>(euHabitatClasses);

                if (format.ToLower() == "json")
                {
                    return Ok(dto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(dto);
                    return File(csvData, "text/csv", $"traitbase_export_EUhabitatclasses_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting EU habitat classes.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

        