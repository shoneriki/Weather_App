using GMap.NET;
using GMap.NET.MapProviders;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Weather_App
{
    public class WeatherApiCall
    {
        private const string BaseUrl = "https://api.weather.gov";

        public PointLatLng? GetCoordinates(string location)
        {
            GeoCoderStatusCode statusCode;
            var pointLatLng = GMapProviders.OpenStreetMap.GetPoint(location, out statusCode);

            if (statusCode == GeoCoderStatusCode.OK && pointLatLng != null)
            {
                return pointLatLng;
            }

            return null;
        }
        public async Task<GridpointResponse> GetGridpoint(double latitude, double longitude)
        {
            RestClient client = new RestClient(BaseUrl);
            RestRequest request = new RestRequest($"/points/{latitude},{longitude}");
            request.AddHeader("User-Agent", "Weather_App (ikirenohs@gmail.com)");

            var response = await client.GetAsync<GridpointResponse>(request);
            if (response != null && response.Properties != null)
            {
                return response;
            }
            else
            {
                throw new Exception("Failed to retrieve gridpoint data.");
            }
        }
        public async Task<List<ForecastPeriod>> FetchHourlyForecastForToday(string officeId, int gridX, int gridY)
        {
            RestClient client = new RestClient(BaseUrl);
            RestRequest request = new RestRequest($"/gridpoints/{officeId}/{gridX},{gridY}/forecast/hourly");
            request.AddHeader("User-Agent", "Weather_App (ikirenohs@gmail.com)");

            var fullForecast = await client.GetAsync<Forecast>(request);
            if (fullForecast == null || fullForecast.Properties == null)
            {
                throw new Exception("Error retrieving weather data.");
            }

            // Filter for the current day
            DateTime now = DateTime.Now;
            DateTime startOfToday = new DateTime(now.Year, now.Month, now.Day);
            DateTime startOfTomorrow = startOfToday.AddDays(1);

            var todayForecasts = new List<ForecastPeriod>();

            // Loop through each forecast period and add to the new list if it's for today
            foreach (var period in fullForecast.Properties.Periods)
            {
                if (period.StartTime >= startOfToday && period.StartTime < startOfTomorrow)
                {
                    todayForecasts.Add(period);
                }
            }

            return todayForecasts;
        }

        public async Task<List<ForecastPeriod>> FetchForecastForCity(string cityName)
        {
            var latLngCoords = GetCoordinates(cityName);
            if (!latLngCoords.HasValue)
            {
                throw new Exception("Unable to get coordinates for the city.");
            }

            var gridpointResponse = await GetGridpoint(latLngCoords.Value.Lat, latLngCoords.Value.Lng);
            if (gridpointResponse == null || gridpointResponse.Properties == null)
            {
                throw new Exception("Unable to get gridpoint information from coordinates.");
            }

            var forecast = await FetchHourlyForecastForToday(gridpointResponse.Properties.GridId, gridpointResponse.Properties.GridX, gridpointResponse.Properties.GridY);
            return forecast;
        }

    }
}
