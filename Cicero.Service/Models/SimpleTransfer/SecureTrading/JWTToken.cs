using Cicero.Service.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Cicero.Service.Extensions.Extensions;

namespace Cicero.Service.Models.SimpleTransfer
{
    public class Payload
    {
        public string accounttypedescription { get; set; }
        public string baseamount { get; set; }
        public string currencyiso3a { get; set; }
        public string sitereference { get; set; }
    }
}
