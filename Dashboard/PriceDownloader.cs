using System.Text;

namespace Dashboard

{
    internal class PriceDownloader
    {
        public static async Task<string> GetTodaysPricesJsonAsync()
        {

            HttpClient client = new();
            string payload = @"{  ""operationName"": ""Dataset"",  ""variables"": {},  ""query"": ""  query Dataset { elspotprices (order_by:{HourUTC:desc},limit:500,offset:0)  { HourUTC,HourDK,PriceArea,SpotPriceDKK,SpotPriceEUR } }""   }";

            var response = await client.PostAsync("https://data-api.energidataservice.dk/v1/graphql", new StringContent(payload, Encoding.UTF8, "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
