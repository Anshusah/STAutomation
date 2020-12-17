using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class Check
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string status { get; set; }
        public string redirect_uri { get; set; }
        public string result { get; set; }
        public bool sandbox { get; set; }
        public List<string> tags { get; set; }
        public string results_uri { get; set; }
        public string form_uri { get; set; }
        public bool paused { get; set; }
        public string version { get; set; }
        public List<string> report_ids { get; set; }
        public string href { get; set; }
        public string applicant_id { get; set; }
        public bool applicant_provides_data { get; set; }
    }
}
