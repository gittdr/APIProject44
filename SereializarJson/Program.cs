using Newtonsoft.Json;
using SereializarJson.Models;
using System;
using System.Collections.Generic;
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
    class Program
    {
        private static string _path = @"C:\Administración\Proyecto API\Json\Contacts.json";

        static void Main(string[] args)
        {

            var contacts = GetContacts();
            SerializeJsonFile(contacts);
            ////string jsonFactura = "";
            ////jsonFactura = "{\r\n\r\n  \"carrierIdentifier\": {";
            ////jsonFactura += "\r\n\r\n\r\n  \"nombre\": Pancho";
            ////jsonFactura += ",\r\n\r\n\r\n  \"idSucursal\": 20" + "\r\n\r\n},";
            ////jsonFactura += "\r\n\r\n}";

            //var obj = new Lad
            //{
            //    firstName = "Markoff",
            //    lastName = "Chaney",
            //    dateOfBirth = new MyDate
            //    {
            //        year = 1901,
            //        month = 4,
            //        day = 30
            //    }
            //};
            //var json = new JavaScriptSerializer().Serialize(obj);
            ////Console.WriteLine(json);

            //System.IO.File.WriteAllText(@"C:\Administración\Proyecto API\Json\JSONTralixGenerador.txt", json);   
        }
        #region "Writing JSON"
        public static void SerializeJsonFile(object contacts)
        {
            string contactsJson = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            File.WriteAllText(_path, contactsJson);
        }
        public static object GetContacts()
        {
            Contact contacts = new Contact
            {


                carrierIdentifier = new Carrier
                {
                    type = "P44_EU",
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

                ////HASTA AQUI TERMINA EL PRIMER CONTACTO
                //new Contact
                //{
                //    Name = "David Beckham",
                //    DateOfBirth = new DateTime(1988, 06, 17),
                //    Address = new Address
                //    {
                //        Street = "Candiles drive",
                //        Number = 23,
                //        City = new City
                //        {
                //            Name = "New York",
                //            Country = new Country
                //            {
                //                Code = "USA",
                //                Name = "United States"
                //            }
                //        }
                //    },
                //    Phone = new List<Phone>
                //    {
                //        new Phone
                //        {
                //            Name = "Personal",
                //            Number = "3221212"
                //        },
                //        new Phone
                //        {
                //            Name = "Work",
                //            Number = "32312121"
                //        }
                //    }
                //}, // AQUI TERMINA EL SEGUNDO CONTACTO
                //new Contact
                //{
                //    Name = "Pancho Torres",
                //    DateOfBirth = new DateTime(1976, 02, 17),
                //    Address = new Address
                //    {
                //        Street = "Amnsterdant drive",
                //        Number = 23,
                //        City = new City
                //        {
                //            Name = "Puebla",
                //            Country = new Country
                //            {
                //                Code = "MX",
                //                Name = "México"
                //            }
                //        }
                //    },
                //    Phone = new List<Phone>
                //    {
                //        new Phone
                //        {
                //            Name = "Personal",
                //            Number = "3432112"
                //        },
                //        new Phone
                //        {
                //            Name = "Work",
                //            Number = "43421212"
                //        }
                //    }
                //}//AQUI TERMINA EL TERCER CONTACTO
            };
            return contacts;
        }
        #endregion
    }
}
