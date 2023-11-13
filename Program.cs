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

            try
            {
                string cityName = "Cleveland, OH";
                var forecastPeriods = await apiCall.FetchForecastForCity(cityName);

                if (forecastPeriods != null && forecastPeriods.Count > 0)
                {
                    foreach (var period in forecastPeriods)
                    {
                        double tempCelsius = (period.Temperature - 32) * 5 / 9.0;
                        Console.WriteLine($"Time: {period.StartTime}, " +
                                          $"Temp: {period.Temperature}°F / {tempCelsius:0.0}°C, " +
                                          $"Forecast: {period.ShortForecast}");
                    }
                }
                else
                {
                    Console.WriteLine("Forecast data is not available.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


    }
}
