using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SereializarJson.Models
{
    public class Contact
    {
        //public string Name { get; set; }
        //public Address Address { get; set; }
        //public List<Phone> Phone { get; set; }
        //public DateTime DateOfBirth { get; set; }
        public Carrier carrierIdentifier { get; set; }
        public List<Shipment> shipmentIdentifiers { get; set; }
        public List<Equipment> equipmentIdentifiers { get; set; }
        public List<Stops> shipmentStops { get; set; }
        //public Appointment appointmentWindow { get; set; }
    }
}
