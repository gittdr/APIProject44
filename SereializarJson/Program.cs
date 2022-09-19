using Newtonsoft.Json;
using RestSharp;
using SereializarJson.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Authentication;
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

        
        private static object contacts;

        static void Main(string[] args)
        {
            //ObtenerToken();
            var contacts = GetContacts();
        }
       
        public static void ObtenerToken()
        {
            var client = new RestClient("https://na12.api.project44.com/");
            client.Authenticator = new HttpBasicAuthenticator("admin.user@tdr.p44.com", "welcomeP44!");
            var request = new RestRequest("api/v4/oauth2/client-applications", Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{ \"grant_type\":\"client_credentials\" }", ParameterType.RequestBody);
            var responseJson = client.Execute(request).Content;
            var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["clientApplications"].ToString();
            Console.WriteLine(token);
            if (token.Length == 0)
            {
                throw new AuthenticationException("API authentication failed.");
            }
        }
        #region "Writing JSON"

        public static object GetContacts()
        {
            
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
                                    type = "N/A",
                                    value = "N/A"
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
                                        //stopNumber = isegmqa["stp_mfh_sequence"].ToString(),
                                        stopNumber = isegmqa["stp_number"].ToString(),
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

                            Random rd = new Random();

                            int rand_num = rd.Next(100, 200000);
                            string datestring = DateTime.Now.ToString("yyyyMMddHHmmss");
                            string contactsJson = JsonConvert.SerializeObject(contacts, Formatting.Indented);
                            System.IO.File.WriteAllText(@"C:\Administración\Proyecto API\JsonGenerados\Orden-" + order+".json", contactsJson);
                            //System.IO.File.WriteAllText(@"C:\Administración\Proyecto API\JsonGenerados\" + rand_num + "-" + carc + "-JsonAPI.json", contactsJson);

                    carc++;

                    //try
                    //{
                    //    var client = new RestClient("https://na12.api.project44.com/api/v4/oauth2/client-applications");
                    //    //AQUI VAN LAS CREDENCIALES
                    //    var user = "admin.user@tdr.p44.com";
                    //    var password = "welcomeP44!";
                    //    client.Authenticator = new HttpBasicAuthenticator(user, password);
                    //    var request = new RestRequest(Method.GET);

                    //    request.AddHeader("cache-control", "no-cache");

                    //    request.AddHeader("content-length", "834");
                    //    request.AddHeader("accept-encoding", "gzip, deflate");
                    //    request.AddHeader("Host", "canal1.xsa.com.mx:9050");
                    //    //request.AddHeader("Postman-Token", "b6b7d8eb-29f2-420f-8d70-7775701ec765,a4b60b83-429b-4188-98d4-7983acc6742e");
                    //    request.AddHeader("Cache-Control", "no-cache");
                    //    request.AddHeader("Accept", "*/*");
                    //    request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");
                    //    request.AddParameter("application/json", contactsJson, ParameterType.RequestBody);
                    //    //request.AddParameter("application/json", contactsJson, ParameterType.RequestBody);
                    //    IRestResponse response = client.Execute(request);
                    //    //string respuesta = response.StatusCode.ToString();
                    //    //var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["access_token"].ToString();
                    //    //if (token.Length == 0)
                    //    //{
                    //    //    throw new AuthenticationException("API authentication failed.");
                    //    //}
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}



                }
            }
            
            return contacts;
        }
        #endregion
    }
}
