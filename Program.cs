using RestSharp;
using System;
using System.Threading.Tasks;

namespace Weather_App
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            WeatherApiCall apiCall = new WeatherApiCall();

            var latLngCoords = apiCall.GetCoordinates("Cleveland, OH");

            if (latLngCoords.HasValue)
            {
                //double latitude = latLngCoords.Value.Lat;
                //double longitude = latLngCoords.Value.Lng;

                int latitude = 83;
                int longitude = 65;
                string office = "CLE";

                Forecast forecast = await apiCall.FetchHourlyForecast(office, latitude, longitude);

                if (forecast != null && forecast.Properties != null)
                {
                    foreach (var period in forecast.Properties.Periods)
                    {
                        Console.WriteLine($"Time: {period.StartTime} => Temp: {period.Temperature} {period.TemperatureUnit}");
                    }
                }
                else
                {
                    Console.WriteLine("I apologize, forecast data is not available it seems.");
                }
            }
            else
            {
                Console.WriteLine("I'm sorry, we failed to get coordinates.");
            }
        }
    }
}
