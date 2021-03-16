using System.Threading.Tasks;
using AirportDistanceCalculator.Models;
using AirportDistanceCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirportDistanceCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirportController : ControllerBase
    {
        IAirportDistanceService _airportDistanceService;
        public AirportController(IAirportDistanceService airportDistanceService)
        {
            _airportDistanceService = airportDistanceService;
        }

        [HttpGet("Calculate")]
        public async Task<IActionResult> Calculate([FromQuery] AirportCodes airportCodes)
        {
            var result = await _airportDistanceService.Calculate(airportCodes);

            return result switch
            {
                -1 => NotFound(),
                _ => Ok(result),
            };
        }
    }
}
