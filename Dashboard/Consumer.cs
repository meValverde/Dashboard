using Confluent.Kafka;
using System.Diagnostics;

namespace Dashboard
{
    public class ReadKafka
    {

        public static async Task <string> Consumer()
        {

            
            string Text = System.IO.File.ReadAllText(@"C:\Users\abak1\wslip.txt").Replace("\r\n", "**newline**").Replace("\n", "").Replace("**newline**", "\r\n");
            string Ip = ":9092";
            string message;
           
            var config = new ConsumerConfig
            {
                BootstrapServers = Text + Ip,
                GroupId = "Gruppe",
                AutoOffsetReset = AutoOffsetReset.Latest,

            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe("quickstart-events");
                Console.WriteLine("Ready to receive messages");


                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                while (true)
                {
                    var consumeResult = consumer.Consume();
                    message = consumeResult.Message.Value;

                    Console.WriteLine($"Besked modtaget:{message}");

                    if (stopwatch.ElapsedMilliseconds >= 1 * 1000)
                    {
                        break;
                    }
                    break;
                   
                }

                consumer.Close();
            }

            return message;

           
        }

    }
}
