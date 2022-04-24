using Confluent.Kafka;
using System.Net;

namespace Dashboard
{
    public class producer 
    { 
         public static async Task CreateProducer()
         {
            
              string message = await CreateMessage();

              string Text = System.IO.File.ReadAllText(@"C:\Users\abak1\wslip.txt").Replace("\r\n", "**newline**").Replace("\n", "").Replace("**newline**", "\r\n");
              string Ip = ":9092";


                 var config = new ProducerConfig
                 {
                  BootstrapServers = Text + Ip,
                  ClientId = Dns.GetHostName(),

                 };

                 using (var producer = new ProducerBuilder<Null, string>(config).Build())
                 {
                 await producer.ProduceAsync("quickstart-events", new Message<Null, string> { Value = message});
                 }

         }

        public static async Task<string> CreateMessage()
        {
            string? json = Cache.Get();
            if (json == null)
            {
                json = await PriceDownloader.GetTodaysPricesJsonAsync();
                Cache.Put(json);
            }

            DateTime now = DateTime.Now;
            TimeOnly time = new(now.Hour, 00);
            DateTime thisHour = DateOnly.FromDateTime(now).ToDateTime(time);

            PriceParcer parser = new(json);
            var price = parser.GetPrice(thisHour);
            return price.ToString();
        }


    }

    
}
