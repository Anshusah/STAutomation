using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class CreateCheck
    {
        public string applicant_id { get; set; }
        public List<string> report_names { get; set; }
    }
}
