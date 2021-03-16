using AirportDistanceCalculator.Models;
using GeoCoordinatePortable;

namespace AirportDistanceCalculator.Helpers
{
    public static class AirportResponseExtension
    {
        public static double GetDistance(this AirportResponse from, AirportResponse to)
        {
            var fromCoord = new GeoCoordinate(from.LatitudeDouble, from.LongitudeDouble);
            var toCord = new GeoCoordinate(to.LatitudeDouble, to.LongitudeDouble);

            return fromCoord.GetDistanceTo(toCord) / 1000;
        }
    }
}
