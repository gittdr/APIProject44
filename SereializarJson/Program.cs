using Newtonsoft.Json;
using SereializarJson.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SereializarJson
{
    public class Persona
    {
        public int Codigo;
        public string Nombre;
        public string Proyecto;
    }
    public class MyDate
    {
        public int year;
        public int month;
        public int day;
    }

    public class Lad
    {
        public string firstName;
        public string lastName;
        public MyDate dateOfBirth;
    }
    public class Program
    {
        static storedProcedure sql = new storedProcedure("miConexion");
        public static FacLabControler facLabControler = new FacLabControler();
        public string tipo = "";

        private static string _path = @"C:\Administración\Proyecto API\Json\Contacts.json";
        private static object contacts;

        static void Main(string[] args)
        {

            var contacts = GetContacts();
            //SerializeJsonFile(contacts);
            

        }
        #region "Writing JSON"
        //public static void SerializeJsonFile(object contacts)
        //{
        //    string contactsJson = JsonConvert.SerializeObject(contacts, Formatting.Indented);
        //    File.WriteAllText(_path, contactsJson);
        //}
        public static object GetContacts()
        {
            string datestring = DateTime.Now.ToString("yyyyMMddHHmmss");
            DataTable otds = facLabControler.GetInfoApi();
            if (otds.Rows.Count > 0)
            {
                foreach (DataRow isegm in otds.Rows)
                {
                    string tipo = isegm["trc_licnum"].ToString();
                    Contact contacts = new Contact
                    {


                        carrierIdentifier = new Carrier
                        {
                            type = tipo,
                            value = "DKCARRIE"
                        },
                        shipmentIdentifiers = new List<Shipment>
                    {
                        new Shipment
                        {
                            type = "ORDER",
                            value = "1122334455"
                        }
                    },
                        equipmentIdentifiers = new List<Equipment>
                    {
                        new Equipment
                        {
                            type = "LICENSE_PLATE",
                            value = "ABC123"
                        }
                    },
                        shipmentStops = new List<Stops>
                    {
                        new Stops
                        {
                            stopNumber = 1,
                            appointmentWindow = new Appointment
                            {
                                startDateTime = "2020-12-02T07:30:00",
                                endDateTime = "2020-12-02T08:00:00",
                                localTimeZoneIdentifier = "Europe/Brussels"
                            },
                            location = new Location
                            {
                                address = new Address
                                {
                                    postalCode = "9400",
                                    addressLines = new List<string>
                                    {
                                        "Strømmen 6"
                                    },
                                    city = "Nørresundby",
                                    state = "",
                                    country = "DK"
                                    //AQUI VA LA LISTA DE ADDRESSLINE
                                },
                                contact = new Ucontact
                                {
                                    companyName = "project44"
                                }
                            },
                            stopName = "project44 Europe HQ"
                        },//AQUI TERMINA EL PRIMER STOP 
                        new Stops
                        {
                            stopNumber = 2,
                            appointmentWindow = new Appointment
                            {
                                startDateTime = "2020-12-03T20:00:00",
                                endDateTime = "2020-12-03T22:00:00",
                                localTimeZoneIdentifier = "Europe/Brussels"
                            },
                            location = new Location
                            {
                                address = new Address
                                {
                                    postalCode = "24983",
                                    addressLines = new List<string>
                                    {
                                        "SKANDINAVIENBOGEN 6"
                                    },
                                    city = "HANDEWITT",
                                    state = "",
                                    country = "DE"
                                    //AQUI VA LA LISTA DE ADDRESSLINE
                                },
                                contact = new Ucontact
                                {
                                    companyName = "Example Customer"
                                }
                            },
                            stopName = "Example Stop Name"
                        }
                    }//AQUI TERMINA EL SEGUNDO STOP

                       
                    };
                    string contactsJson = JsonConvert.SerializeObject(contacts, Formatting.Indented);
                    System.IO.File.WriteAllText(@"C:\Administración\Proyecto API\Json\" + datestring + "-JsonAPI.json", contactsJson);
                    //File.WriteAllText(_path, contactsJson);

                }
            }
            
            return contacts;
        }
        #endregion
    }
}
