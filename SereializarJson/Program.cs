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
    public class Mymodels
    {
        public RShipment shipment { get; set; }

        
        //public string value { get; set; }
    }
    public class RShipment
    {
        public Carrier carrierIdentifier { get; set; }
        public string id { get; set; }
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
            var request = new RestRequest("api/v4/tl/shipments/500166588871/statuses", Method.GET);
            request.AddHeader("content-type", "application/json");
            //request.AddBody();
            request.AddParameter("application/json", "{ \"grant_type\":\"client_credentials\" }", ParameterType.RequestBody);
            var responseJson = client.Execute(request).Content;
            //DataTable dt = (DataTable)JsonConvert.DeserializeObject(responseJson, (typeof(DataTable)));


            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Mymodels>(responseJson);
            var idd = data.shipment.id;
             
            //Console.WriteLine(token);
            //if (token.Length == 0)
            //{
            //    throw new AuthenticationException("API authentication failed.");
            //}
        }
        #region "Writing JSON"

        public static object GetContacts()
        {
            
          
      
            DataTable otds = facLabControler.GetOrderApi();
            if (otds.Rows.Count > 0)
            {
                int carc = 1;
                foreach (DataRow isegm in otds.Rows)
                {
                    
                    //string order = isegm["ord_hdrnumber"].ToString();
                    //string licencia = isegm["trc_licnum"].ToString();
                    //string tractor = isegm["lgh_tractor"].ToString();
                    string order = "1205836";
                    string licencia = "78AP5S";
                    string tractor = "1793";

                    //Valide que no estan proceddos en orderheader 
                    DataTable orproc = facLabControler.GetOrpApi(order);
                    if (orproc.Rows.Count > 0)
                    {
                        foreach (DataRow orprocz in orproc.Rows)
                        {
                            string rext = orprocz["ord_extrainfo2"].ToString();
                            if (rext == "")
                            {
                                DataTable otdsr = facLabControler.GetCmpidApi(order);
                                if (otdsr.Rows.Count == 0)
                                {
                                    Contact contacts = new Contact
                                    {


                                        carrierIdentifier = new Carrier
                                        {
                                            type = "P44_EU",
                                            value = "MXTDRTRA"
                                        },
                                        shipmentIdentifiers = new List<Shipment>
                                {
                                    new Shipment
                                    {
                                        type = "BILL_OF_LADING",
                                        value = order
                                    }
                                },
                                        equipmentIdentifiers = new List<Equipment>
                                {
                                    new Equipment
                                    {
                                        type = "LICENSE_PLATE",
                                        value = licencia
                                    },
                                    new Equipment
                                    {
                                        type = "VEHICLE_ID",
                                        value = tractor
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
                                                //stopNumber = isegmqa["stp_number"].ToString(),
                                                appointmentWindow = new Appointment
                                                {
                                                    startDateTime = isegmqa["startDateTime"].ToString(),
                                                    endDateTime = isegmqa["endDateTime"].ToString()//,
                                                                                                   //localTimeZoneIdentifier = "Mexico/Ciudad de Mexico"
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
                                                        //country = isegmqa["cmp_country"].ToString()
                                                        country = "MX"
                                                        //AQUI VA LA LISTA DE ADDRESSLINE
                                                    },
                                                    contact = new Ucontact
                                                    {
                                                        companyName = isegmqa["cmp_id"].ToString()
                                                    }
                                                },
                                                geoCoordinates = new GeoCoordinates
                                                {
                                                    latitude = isegmqa["latitude"].ToString(),
                                                    longitude = isegmqa["longitude"].ToString()//,
                                                                                               //localTimeZoneIdentifier = "Mexico/Ciudad de Mexico"
                                                },
                                                stopName = isegmqa["cmp_id"].ToString()

                                            });

                                        }

                                    }

                                    GJson(contacts, order);
                                    //Random rd = new Random();

                                    //int rand_num = rd.Next(100, 200000);
                                    //string datestring = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    //string contactsJson = JsonConvert.SerializeObject(contacts, Formatting.Indented);
                                    //System.IO.File.WriteAllText(@"C:\Administración\Proyecto API\JsonGenerados\Orden-" + order + ".json", contactsJson);
                                    //System.IO.File.WriteAllText(@"C:\Administración\Proyecto API\JsonGenerados\" + rand_num + "-" + carc + "-JsonAPI.json", contactsJson);

                                    carc++;
                                    
                                    //var client = new RestClient("https://na12.api.project44.com/api/v4/tl/shipments");
                                    //var username = "admin.user@tdr.p44.com";
                                    //var password = "welcomeP44!";
                                    //client.Authenticator = new HttpBasicAuthenticator(username, password);
                                    //var request = new RestRequest(Method.POST);

                                    //request.AddHeader("cache-control", "no-cache");

                                    //request.AddHeader("content-length", "834");
                                    //request.AddHeader("accept-encoding", "gzip, deflate");
                                    //request.AddHeader("Host", "na12.api.project44.com");
                                    ////request.AddHeader("Postman-Token", "b6b7d8eb-29f2-420f-8d70-7775701ec765,a4b60b83-429b-4188-98d4-7983acc6742e");
                                    //request.AddHeader("Cache-Control", "no-cache");
                                    //request.AddHeader("Accept", "*/*");
                                    //request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");

                                    //request.AddParameter("application/json", contactsJson, ParameterType.RequestBody);
                                    //IRestResponse response = client.Execute(request);

                                    //string Coderespuesta = response.StatusCode.ToString();
                                    ////PASO 13 - AQUI VALIDA LA RESPUESTA DE TRALIX Y SI ES OK AVANZA Y SUBE AL FTP E INSERTA EL REGISTRO A VISTA_CARTA_PORTE
                                    //if (Coderespuesta == "BadRequest")
                                    //{
                                    //    string rmensaje = response.Content;
                                    //}
                                    //else
                                    //{
                                    //    string respuesta = response.Content;
                                    //    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Mymodels>(respuesta);
                                    //    var idd = data.shipment.id;
                                    //    DataTable uporderq = facLabControler.UpdateOrderHeaderAPI(order, idd);
                                    //    //facLabControler.OrderHeader(rorderh, rfecha);
                                    //}



                                }
                            }
                        }
                            
                    }
                   




                }
            }
            
            return contacts;
        }

        public static void GJson(Contact contacts, string order)
        {
            Random rd = new Random();

            int rand_num = rd.Next(100, 200000);
            string datestring = DateTime.Now.ToString("yyyyMMddHHmmss");
            string contactsJson = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            System.IO.File.WriteAllText(@"C:\Administración\Proyecto API\JsonGenerados\Orden-" + order + ".json", contactsJson);
            Api(order, contactsJson);
        }
        public static void Api(string order,string contactsJson)
        {
            var client = new RestClient("https://na12.api.project44.com/api/v4/tl/shipments");
            var username = "admin.user@tdr.p44.com";
            var password = "welcomeP44!";
            client.Authenticator = new HttpBasicAuthenticator(username, password);
            var request = new RestRequest(Method.POST);

            request.AddHeader("cache-control", "no-cache");

            request.AddHeader("content-length", "834");
            request.AddHeader("accept-encoding", "gzip, deflate");
            request.AddHeader("Host", "na12.api.project44.com");
            //request.AddHeader("Postman-Token", "b6b7d8eb-29f2-420f-8d70-7775701ec765,a4b60b83-429b-4188-98d4-7983acc6742e");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");

            request.AddParameter("application/json", contactsJson, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            string Coderespuesta = response.StatusCode.ToString();
            //PASO 13 - AQUI VALIDA LA RESPUESTA DE TRALIX Y SI ES OK AVANZA Y SUBE AL FTP E INSERTA EL REGISTRO A VISTA_CARTA_PORTE
            if (Coderespuesta == "BadRequest")
            {
                string rmensaje = response.Content;
            }
            else
            {
                string respuesta = response.Content;
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Mymodels>(respuesta);
                var idd = data.shipment.id;
                DataTable uporderq = facLabControler.UpdateOrderHeaderAPI(order, idd);
                //facLabControler.OrderHeader(rorderh, rfecha);
            }
        }
        #endregion
    }
}
