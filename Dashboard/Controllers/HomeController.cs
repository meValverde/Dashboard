using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;





namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        private static readonly HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        static string Baseurl = "https://localhost:7195";

        DashboardValues model = new DashboardValues();




        public async Task<ActionResult> Index()
        {
            await producer.CreateProducer();
            model.ElectricityPrices = await ReadKafka.Consumer();
            
            using (client)
            {
                model.Energy = await GetProducedEnergy();
                model.Forecast = await GetForecast();
            }

            model.Temperature = GetRoomTemperature();

            return View(model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public static async Task<List<ForecastValues>> GetForecast()
        {

            HttpResponseMessage Res = await client.GetAsync("api/values");
            var response = await client.PostAsync("https://localhost:7195/forecast/kolding", null);
            var responseString = await response.Content.ReadAsStringAsync();
            List<ForecastValues>? forecastResponse = new();
            forecastResponse = JsonConvert.DeserializeObject<List<ForecastValues>>(responseString);
            return forecastResponse;

        }


        public static async Task<List<EnergyValues>> GetProducedEnergy()
        {
            List<EnergyValues>? energy = new();

            client.BaseAddress = new Uri(Baseurl);

            HttpResponseMessage Res = await client.GetAsync("api/values");


            if (Res.IsSuccessStatusCode)
            {

                var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                energy = JsonConvert.DeserializeObject<List<EnergyValues>>(EmpResponse);
            }

            return energy;

        }


        public static List<Temperatur> GetRoomTemperature() 
        {

            DateTime current = DateTime.Now;
            List<Temperatur> temperatura = new List<Temperatur>();
            using (var ctx = new indeklimaContext())
            {
                var temp = ctx.Temperaturs.Where(s => s.Dato == current).OrderByDescending(x => x.Tidspunkt).Take(1).First();
               
                
                temperatura.Add(new Temperatur
                {
                    Dato = temp.Dato,
                    Tidspunkt = temp.Tidspunkt,
                    Grader = temp.Grader

                });

                return temperatura;


            }
        }
    }
    
}