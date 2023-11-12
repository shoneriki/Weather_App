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
        public async Task<Gridpoint> GetGridpoint(double latitude, double longitude)
        {
            RestClient client = new RestClient(BaseUrl);
            RestRequest request = new RestRequest($"/points/{latitude},{longitude}");
            request.AddHeader("User-Agent", "Weather_App (ikirenohs@gmail.com)");

            var response = await client.GetAsync<GridpointResponse>(request);
            if (response != null && response.Properties != null)
            {
                return new Gridpoint
                {
                    OfficeId = response.Properties.GridId,
                    GridX = response.Properties.GridX,
                    GridY = response.Properties.GridY
                };
            }
            else
            {
                throw new Exception("Failed to retrieve gridpoint data.");
            }
        }

        public async Task<Forecast> FetchHourlyForecast(string officeId, int gridX, int gridY)
        {
            RestClient client = new RestClient(BaseUrl);

            RestRequest request = new RestRequest($"/gridpoints/{officeId}/{gridX},{gridY}/forecast/hourly");
            request.AddHeader("User-Agent", "Weather_App (ikirenohs@gmail.com)");

            // GetAsync<Forecast> returns a Forecast object directly
            var forecast = await client.GetAsync<Forecast>(request);

            if (forecast != null)
            {
                return forecast;
            }
            else
            {
                throw new Exception("Error retrieving weather data.");
            }
        }


        public async Task<Forecast> FetchForecastForCity(string cityName)
        {
            // Step 1: We get the latitude and longitude of the city
            var latLngCoords = GetCoordinates(cityName);
            if (!latLngCoords.HasValue)
            {
                throw new Exception("Unable to get coordinates for the city.");
            }

            // Step 2: we use the latitude and longitude to get the office and gridpoints
            var gridpoint = await GetGridpoint(latLngCoords.Value.Lat, latLngCoords.Value.Lng);
            if (gridpoint == null)
            {
                throw new Exception("Unable to get gridpoint information from coordinates.");
            }

            // Step 3: Fetch the weather data using the office and gridpoints
            var forecast = await FetchHourlyForecast(gridpoint.OfficeId, gridpoint.GridX, gridpoint.GridY);
            return forecast;
        }


    }
}
