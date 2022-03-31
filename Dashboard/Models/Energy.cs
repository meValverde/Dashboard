using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dashboard.Models
{

    public class EnergyValues
    {
        public string? Date { get; set; }
        public string? TotalEnergy { get; set; }
    }

    public class ForecastValues
    {
        public string? temp { get; set; }
        public string? cloudcover { get; set; }
        public string? precip { get; set; }
        public string? conditions { get; set; }
    }

    public class DashboardValues
    { 
        public List <EnergyValues>? Energy { get; set; }
        public List <ForecastValues>? Forecast { get; set; }
    }
}
