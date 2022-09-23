using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SereializarJson.Models
{
    public class Stops
    {
        public string stopNumber { get; set; }
        public Appointment appointmentWindow { get; set; }
        public Location location { get; set; }
        
        public GeoCoordinates geoCoordinates {get; set;}
        public string stopName { get; set; }
        //public DataTable oabtinfso { get; set; }
    }
}
