using AutoMapper;
using biobase.API.Models.DTO;
using biobase.API.Repositories;
using biobase.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace biobase.API.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/habitatCodes")]
    [ApiController]
    public class HabitatCodesController : ControllerBase
    {
        private readonly IHabitatCodesRepository _habitatCodesRepository;
        private readonly IEuHabitatCodesRepository _euHabitatCodesRepository;
        private readonly IMapper _mapper;
        private readonly ICsvExportService _csvExportService;
        private readonly ILogger<HabitatCodesController> _logger;

        public HabitatCodesController(
            IHabitatCodesRepository repository, 
            IEuHabitatCodesRepository euHabitatCodesRepository,
            IMapper mapper, 
            ICsvExportService csvExportService, 
            ILogger<HabitatCodesController> logger
            ) {
            _habitatCodesRepository = repository;
            _euHabitatCodesRepository = euHabitatCodesRepository;
            _mapper = mapper;
            _csvExportService = csvExportService;
            _logger = logger;
        }
        /// <summary>
        /// Retrieves data on habitat codes
        /// </summary>
        /// <param name="habitatClassification">
        /// Optional filter for habitat classification: 'beheertype', 'cultuurdoeltype', 'habitattype', or 'natuurdoeltype'.
        /// </param>
        /// <param name="format">
        /// Specify format in which to return the data, either "csv" or "json". Default is "csv".
        /// </param>
        /// <returns>A downloadable response containing the habitat codes.</returns>
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
                var habitat_codes = await _habitatCodesRepository.GetHabitatsAsync(habitatClassification);
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

        [HttpGet("euHabitatCodes")]
        [SwaggerOperation(
    Tags = new[] { "1.2 EU Habitat codes" },
    Summary = "Get EU habitat codes data",
    Description = "Retrieve EU habitat code definitions, descriptions and Annex I priority status.  \nNo API key is required to access this endpoint. The response format can be CSV or JSON.")]
        [SwaggerResponse(200, "The data was successfully retrieved.")]
        [SwaggerResponse(500, "An error occurred while processing your request.")]
        public async Task<IActionResult> GetEuHabitatCodes([FromQuery] string format = "csv")
        {
            try
            {
                var euHabitatCodes = await _euHabitatCodesRepository.GetAllAsync();
                var dto = _mapper.Map<List<EuHabitatCodeDto>>(euHabitatCodes);

                if (format.ToLower() == "json")
                {
                    return Ok(dto);
                }
                else if (format.ToLower() == "csv")
                {
                    var csvData = await _csvExportService.ExportToCsvAsync(dto);
                    return File(csvData, "text/csv", $"traitbase_export_euhabitatcodes_{DateTime.Now:yyyy-MM-dd-HHmm}.csv");
                }
                else
                {
                    return BadRequest("Unsupported format. Please use 'csv' or 'json'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting EU habitat codes.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

        