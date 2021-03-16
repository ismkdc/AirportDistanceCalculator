using System.Threading.Tasks;
using AirportDistanceCalculator.Models;

namespace AirportDistanceCalculator.Services
{
    public interface IAirportDistanceService
    {
        Task<double> Calculate(AirportCodes airportCodes);
    }
}
