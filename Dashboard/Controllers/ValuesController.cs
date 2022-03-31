using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using Newtonsoft.Json;
using System.Net;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;
using ServiceReference1;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        List<string> files = GetDirectoryFiles();


        [HttpGet]

        public string GetTotalinHour()
        {
            var energyValues = new List<EnergyValues>();
            DateTime oldDate = DateTime.Now.AddYears(-1).AddHours(-1);
            string date = oldDate.ToString("yyMMddHH");
            var match = files.FirstOrDefault(stringToCheck => stringToCheck.Contains(date));

            if (match != null)
            {
                List<string> totalHour = GetEnergyProduction(match.ToString());
                energyValues.Add(new EnergyValues
                {
                    Date = oldDate.ToString("dd/MM/yyyy HH"),
                    TotalEnergy = SumarizeProduction(totalHour).ToString()
                });
            }

           else
                energyValues.Add(new EnergyValues
                {
                    Date = oldDate.ToString("dd/MM/yyyy HH"),
                    TotalEnergy = "0"
                });

            var convert = JsonConvert.SerializeObject(energyValues);


            return convert;
        }

        [HttpPost("/forecast/{location}")]
        public string Post(string location)
        {
            ForecastServiceClient client = new();
            var result = client.GetForecastAsync(location, "Jeger1studerende").Result.Body.GetForecastResult;

            var hourlyForecast = new List<ForecastValues>();
            var dayforecast = new List<string>();

            for (int x = 0; x < 24; x++)
            {
                hourlyForecast.Add(new ForecastValues
                {
                    temp = result.location.values[x].temp.ToString(),
                    precip = result.location.values[x].precip.ToString(),
                    conditions = result.location.values[x].conditions.ToString(),
                    cloudcover = result.location.values[x].cloudcover.ToString()
                });


            }

            var convert = JsonConvert.SerializeObject(hourlyForecast);


            return convert;
        }



        public static List<string> GetDirectoryFiles()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://inverter.westeurope.cloudapp.azure.com/");
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential("studerende", "kmdp4gslmg46jhs");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();
                List<string> files = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                return files;
            }
            catch (Exception)
            {
                throw;
            }



        }

        public static List<string> GetEnergyProduction(string file)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://inverter.westeurope.cloudapp.azure.com/" + file);
                request.Credentials = new NetworkCredential("studerende", "kmdp4gslmg46jhs");

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);

                var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    HasHeaderRecord = false,
                    Comment = '#',
                    AllowComments = true,
                    Delimiter = ";",
                    MissingFieldFound = null

                };


                using var CsvReader = new CsvReader(reader, csvConfig);
                List<string> energy = new List<string>();


                while (CsvReader.Read())
                {
                    var producedEnergy = CsvReader.GetField(37);
                    if (producedEnergy != null)
                    {
                        energy.Add(producedEnergy.ToString());
                    }

                }
                return energy;

            }

            catch (Exception)
            {
                throw;
            }

        }

        public static int SumarizeProduction(List<string> list)
        {
            int result = int.Parse(list[list.Count - 1]) - int.Parse(list[1]);

            return result;
        }
    }
}
