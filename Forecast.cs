using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App
{
    public class Forecast
    {
        // Corresponds to the "properties" object in the JSON
        public ForecastProperties Properties { get; set; }
    }

    public class ForecastProperties
    {
        // Corresponds to the "periods" array in the JSON
        public List<ForecastPeriod> Periods { get; set; }
    }

    public class ForecastPeriod
    {
        // Properties corresponding to each period's details in the JSON
        public int Number { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Temperature { get; set; }
        public string TemperatureUnit
        {
            get; set;


        }
    }
}
