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
            //DataTable otds = facLabControler.GetInfoApi();
            DataTable otds = facLabControler.GetOrderApi();
            if (otds.Rows.Count > 0)
            {
                int carc = 1;
                foreach (DataRow isegm in otds.Rows)
                {
                    //string tipo = isegm["trc_licnum"].ToString();
                    
                    string order = isegm["ord_hdrnumber"].ToString();
                    string licencia = isegm["trc_licnum"].ToString();


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
                                        value = order
                                    }
                                },
                                equipmentIdentifiers = new List<Equipment>
                                {
                                    new Equipment
                                    {
                                        type = "LICENSE_PLATE",
                                        value = licencia
                                    }
                                },
                               
                                    shipmentStops = new List<Stops>()
                                
                            };
                            DataTable obtinfoq = facLabControler.GetInfoApi(order);
                            if (obtinfoq.Rows.Count > 0)
                            {
                                foreach (DataRow isegmqa in obtinfoq.Rows)
                                {
                                    contacts.shipmentStops.Add(new Stops
                                    {
                                        stopNumber = isegmqa["stp_mfh_sequence"].ToString(),
                                        appointmentWindow = new Appointment
                                        {
                                            startDateTime = isegmqa["startDateTime"].ToString(),
                                            endDateTime = isegmqa["endDateTime"].ToString(),
                                            localTimeZoneIdentifier = "Mexico/Ciudad de Mexico"
                                        },
                                        location = new Location
                                        {
                                            address = new Address
                                            {
                                                postalCode = isegmqa["cmp_zip"].ToString(),
                                                addressLines = new List<string>
                                                {
                                                    isegmqa["addressLines"].ToString()
                                                },
                                                city = isegmqa["cty_nmstct"].ToString(),
                                                state = isegmqa["cmp_state"].ToString(),
                                                country = isegmqa["cmp_country"].ToString()
                                                //AQUI VA LA LISTA DE ADDRESSLINE
                                            },
                                            contact = new Ucontact
                                            {
                                                companyName = isegmqa["cmp_id"].ToString()
                                            }
                                        },
                                        stopName = isegmqa["cmp_id"].ToString() + " - " + isegmqa["stp_number"].ToString()

                                    });

                                }

                            }


                            string contactsJson = JsonConvert.SerializeObject(contacts, Formatting.Indented);
                            System.IO.File.WriteAllText(@"C:\Administración\Proyecto API\JsonGenerados\" + datestring +"-"+carc +  "-JsonAPI.json", contactsJson);
                    //File.WriteAllText(_path, contactsJson);
                    carc++;



                }
            }
            
            return contacts;
        }
        #endregion
    }
}
