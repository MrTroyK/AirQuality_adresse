using System;
using System.Net;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_quality_input_address
{
    class MainClass
    {
        static void Main(string[] args)
        {
            //string ip;
            string pos;
            string air_quality, meteo, lat, lon;

            Console.WriteLine("Tapez une adresse : ");
            string adresse = Console.ReadLine();
            adresse = adresse.Replace(" ", "+");
            pos = GetCoor(adresse);
            dynamic g = JsonConvert.DeserializeObject<dynamic>(pos);
            var loc = g[0];
            lat = loc.lat;
            lon = loc.lon;
            air_quality = GetAirQuality(lat, lon);
            meteo = GetMeteo(lat, lon);
            dynamic quality = JsonConvert.DeserializeObject<dynamic>(air_quality);
            dynamic m = JsonConvert.DeserializeObject<dynamic>(meteo);

            Console.WriteLine($"GPS     : {loc.lat},{loc.lon}\n");
            Console.WriteLine($"{quality.country_description}{quality.dominant_polluant_description}");
            Console.WriteLine($"Temps max : {m.data.weather[0].maxtempC} Temps min : {m.data.weather[0].mintempC}");
            Console.ReadLine();

        }

        static string GetCoor(string adresse)
        {
            WebClient webclient;
            string res;
            string url;

            webclient = new WebClient();
            url = string.Format("https://nominatim.openstreetmap.org/search.php?q={0}&format=json", adresse);
            res = webclient.DownloadString(url);
            res = res.Trim();
            return res;
        }

        static string GetMeteo(string lat, string lon)
        {
            WebClient webclient;
            string res;
            string url;

            webclient = new WebClient();
            url = string.Format($"https://api.worldweatheronline.com/premium/v1/weather.ashx?key=a5d6b49f48274d58b5d102212172311&q={lat},{lon}&num_of_days=1&tp=3&format=json", lat, lon);
            res = webclient.DownloadString(url);
            res = res.Trim();
            return res;
        }


        static string GetAirQuality(string lat, string lon)
        {
            WebClient webclient;
            string res;
            string url;

            webclient = new WebClient();
            url = $"https://api.breezometer.com/baqi/?lat={lat}&lon={lon}&key=3e3ca9627cd24faf8626cead119876ed";
            res = webclient.DownloadString(url);
            res = res.Trim();
            return res;
        }
    }
}