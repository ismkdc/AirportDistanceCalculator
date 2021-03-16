using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AirportDistanceCalculator.Helpers;
using AirportDistanceCalculator.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace AirportDistanceCalculator.Services
{
    public class AirportDistanceService : IAirportDistanceService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public AirportDistanceService(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<double> Calculate(AirportCodes airportCodes)
        {
            var cacheKey = $"{airportCodes.From}-{airportCodes.To}";
            var cacheKeyReverse = $"{airportCodes.To}-{airportCodes.From}";

            //check value in cache
            if (!_memoryCache.TryGetValue<double>(cacheKey, out double result))
            {
                var httpClient = _clientFactory.CreateClient();

                var fromReqTask = httpClient.GetAsync(_configuration.GetValue<string>("AirportApi") + $"/{airportCodes.From}");
                var toReqTask = httpClient.GetAsync(_configuration.GetValue<string>("AirportApi") + $"/{airportCodes.To}");

                await Task.WhenAll(fromReqTask, toReqTask);

                var fromReq = fromReqTask.Result;
                var toReq = toReqTask.Result;

                if (!toReq.IsSuccessStatusCode || !fromReq.IsSuccessStatusCode)
                {
                    return -1;
                }

                //parse results

                var fromAirport = JsonSerializer.Deserialize<AirportResponse>(
                    await fromReq.Content.ReadAsStringAsync()
                    );

                var toAirport = JsonSerializer.Deserialize<AirportResponse>(
                    await toReq.Content.ReadAsStringAsync()
                    );

                result = fromAirport.GetDistance(toAirport);

                _memoryCache.Set<double>(cacheKey, result);
                _memoryCache.Set<double>(cacheKeyReverse, result);
            }


            return result;
        }
    }
}
