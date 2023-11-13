using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Weather_App
    {
        public class WeatherDisplayService
        {
            private readonly WeatherApiCall _weatherApiCall;

            public WeatherDisplayService(WeatherApiCall weatherApiCall)
            {
                _weatherApiCall = weatherApiCall;
            }

            public async Task DisplayConditionsAndAdvice(string cityName)
            {
                try
                {
                    var forecastPeriods = await _weatherApiCall.FetchForecastForCity(cityName);

                    if (forecastPeriods == null || forecastPeriods.Count == 0)
                    {
                        Console.WriteLine("Forecast data is not available.");
                        return;
                    }

                    ForecastInfo forecastInfo = ProcessNightForecast(forecastPeriods);

                    // Displaying the range of temperatures and advice
                    Console.WriteLine($"Temperature for the rest of the day: {forecastInfo.TemperatureRangeFahrenheit}°F / {forecastInfo.TemperatureRangeCelsius}°C " +
                                      $"(Hours left: {forecastInfo.HoursLeftTonight}), {forecastInfo.Advice}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            private ForecastInfo ProcessNightForecast(List<ForecastPeriod> forecastPeriods)
            {
                int minTempF = forecastPeriods.Min(period => period.Temperature);
                int maxTempF = forecastPeriods.Max(period => period.Temperature);
                int hoursLeft = forecastPeriods.Count;

                string tempRangeF = $"{minTempF}-{maxTempF}";
                string tempRangeC = $"{ConvertFahrenheitToCelsius(minTempF)}-{ConvertFahrenheitToCelsius(maxTempF)}";

                ForecastInfo forecastInfo = new ForecastInfo
                {
                    TemperatureRangeFahrenheit = tempRangeF,
                    TemperatureRangeCelsius = tempRangeC,
                    HoursLeftTonight = hoursLeft,
                    Advice = GetOverallAdvice(forecastPeriods)
                };

                return forecastInfo;
            }

            private string GetOverallAdvice(List<ForecastPeriod> forecastPeriods)
            {
                bool needsUmbrella = forecastPeriods.Any(period => period.ShortForecast.Contains("Rain", StringComparison.OrdinalIgnoreCase));
                int minTempC = ConvertFahrenheitToCelsius(forecastPeriods.Min(period => period.Temperature));

                string advice = "";
                if (needsUmbrella) advice += "Carry an umbrella. ";
                if (minTempC < 10) advice += "Wear a heavy jacket. ";
                else if (minTempC < 20) advice += "Consider a light jacket.";

                return advice.Trim();
            }

            private int ConvertFahrenheitToCelsius(int temperatureF)
            {
                return (int)((temperatureF - 32) * 5 / 9.0);
            }
        }

        public class ForecastInfo
        {
            public string TemperatureRangeFahrenheit { get; set; }
            public string TemperatureRangeCelsius { get; set; }
            public int HoursLeftTonight { get; set; }
            public string Advice { get; set; }
        }
    }
