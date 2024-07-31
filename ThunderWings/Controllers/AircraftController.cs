using Microsoft.AspNetCore.Mvc;
using ThunderWings.Interfaces;
using ThunderWings.Model;
using ThunderWings.Services;

namespace ThunderWings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IAircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Aircraft>>> GetMilitaryJets()
        {
            try
            {
                var aircraftList = await _aircraftService.GetAircraftAsync();
                return Ok(aircraftList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<Aircraft>>> GetFilteredAircraft(
        [FromQuery] string? name = null,
        [FromQuery] string? country = null,
        [FromQuery] string? role = null,
        [FromQuery] string? manufacturer = null,
        [FromQuery] int? topSpeed = null,
        [FromQuery] long? price = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            try
            {
                var paginatedList = await _aircraftService.GetFilteredAircraftAsync(name, country, role, manufacturer, topSpeed, price, page, pageSize);
                var totalItems = await _aircraftService.GetAircraftAsync();
                var filteredCount = totalItems.Count(a => (string.IsNullOrEmpty(country) || a.Country.Equals(country, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(role) || a.Role.Equals(role, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(manufacturer) || a.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase)) &&
                (!topSpeed.HasValue || a.TopSpeed == topSpeed.Value) &&
                (!price.HasValue || a.Price == price.Value));

                Response.Headers.Add("X-Total-Count", filteredCount.ToString());
                Response.Headers.Add("X-Total-Pages", ((int)Math.Ceiling(filteredCount / (double)pageSize)).ToString());

                return Ok(paginatedList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}