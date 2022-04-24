using System.Text.Json;

namespace Dashboard
{
    internal class PriceParcer
    {
        private readonly PriceModel model;

        public PriceParcer(string json)
        {
            PriceModel? priceModel = JsonSerializer.Deserialize<PriceModel>(json);
            if (priceModel == null)
            {
                throw new NullReferenceException("Json parses to null model");
            }
            this.model = priceModel;
        }

        public double? GetPrice(DateTime hour)
        {
            foreach (var price in model.data.elspotprices)
            {
                if (price.HourDK == hour && price.PriceArea == "DK1")
                {
                    return Math.Round(price.SpotPriceEUR / 1000, 2);
                }
            }
            return null;
        }

    }


    public class PriceModel
    {
        public data? data { get; set; }
    }

    public class data
    {
        public elspotprices[]? elspotprices { get; set; }
    }

    public class elspotprices
    {
        public DateTimeOffset HourUTC { get; set; }


        public DateTimeOffset HourDK { get; set; }


        public string? PriceArea { get; set; }

       // public double SpotPriceDKK { get; set; }


        public double SpotPriceEUR { get; set; }
    }
}
