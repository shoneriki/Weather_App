using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App
{
    public class Period
    {
        public string Name { get; set; } // e.g., "This Afternoon", "Tonight"
        public DateTime StartTime { get; set; } // Start time of the period
        public DateTime EndTime { get; set; } // End time of the period
        public int Temperature { get; set; } // Temperature in degrees
        public string TemperatureUnit { get; set; } // e.g., "F" for Fahrenheit
        public string ShortForecast { get; set; } // Short description of the weather
    }
}
