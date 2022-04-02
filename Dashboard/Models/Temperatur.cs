using System;
using System.Collections.Generic;

namespace Dashboard.Models
{
    public partial class Temperatur
    {
        public DateTime Dato { get; set; }
        public TimeSpan Tidspunkt { get; set; }
        public decimal Grader { get; set; }
    }
}
