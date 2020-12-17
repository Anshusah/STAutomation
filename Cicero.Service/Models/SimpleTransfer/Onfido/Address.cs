using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class Address
    {
        public object flat_number { get; set; }
        public string building_number { get; set; }
        public object building_name { get; set; }
        public string street { get; set; }
        public object sub_street { get; set; }
        public string town { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public object line1 { get; set; }
        public object line2 { get; set; }
        public object line3 { get; set; }
    }
}
