using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SereializarJson.Models
{
    public class Address
    {
        public string postalCode { get; set; }
        public List<string> addressLines { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        //public int Number { get; set; }
        //public City City { get; set; }
    }
}
