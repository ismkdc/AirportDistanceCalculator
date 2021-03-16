using System.Text.Json.Serialization;

namespace AirportDistanceCalculator.Models
{
    public class AirportResponse
    {
        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        public double LatitudeDouble => double.Parse(Latitude);
        public double LongitudeDouble => double.Parse(Longitude);
    }
}
