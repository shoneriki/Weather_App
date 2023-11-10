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


    }
}
