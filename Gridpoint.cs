using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App
{
    public class Gridpoint
    {
        public string OfficeId { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
    }

    public class GridpointResponse
    {
        public GridpointProperties Properties { get; set; }
    }

    public class GridpointProperties
    {
        public string GridId { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }
    }
}
