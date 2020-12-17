using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class ChartDefinitionModel
    {
        public string DimensionOne { get; set; }
        public int Quantity { get; set; }
        public int Settled { get; set; }
    }
}
