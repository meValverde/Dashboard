using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static readonly HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        string Baseurl = "https://localhost:7195";

        DashboardValues model = new DashboardValues();

        public async Task<ActionResult> Index()
        {
            

            using (client)
            {

                client.BaseAddress = new Uri(Baseurl);

                HttpResponseMessage Res = await client.GetAsync("api/values");

               


                if (Res.IsSuccessStatusCode)
                {

                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    model.Energy = JsonConvert.DeserializeObject<List<EnergyValues>>(EmpResponse);
               
                }

                var response = await client.PostAsync("https://localhost:7195/forecast/kolding",null);
                var responseString = await response.Content.ReadAsStringAsync();


                model.Forecast = JsonConvert.DeserializeObject<List<ForecastValues>>(responseString);

            }


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
    }
}