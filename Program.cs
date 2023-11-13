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

            string cityName = "Cleveland, OH";
            await apiCall.ListConditionsAndAdvice(cityName);
        }


    }
}
