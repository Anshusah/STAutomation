using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class Applicant
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public bool sandbox { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string dob { get; set; }
        public object delete_at { get; set; }
        public string href { get; set; }
        public Address address { get; set; }
        public List<object> id_numbers { get; set; }

        public Error error { get; set; }
    }
}
