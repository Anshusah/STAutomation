using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class Error
    {
        public string type { get; set; }
        public string message { get; set; }
        public Fields fields { get; set; }
    }

    public class Fields
    {
    }
}
