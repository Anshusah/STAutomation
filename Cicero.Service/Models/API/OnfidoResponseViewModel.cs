using Cicero.Service.Models.SimpleTransfer.Onfido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class GetApplicantsResponseViewModel
    {
        public List<Applicant> applicants { get; set; }
        public Error error { get; set; }
    }
    public class OnFidoErrorViewModel
    {
        public Error error { get; set; }
    }
    public class Document
    {
        public string id { get; set; }

        public string created_at { get; set; }

        public string updated_at { get; set; }

        public string file_name { get; set; }

        public string file_type { get; set; }

        public string type { get; set; }

        public string side { get; set; }

        public string issuing_country { get; set; }

        public string applicant_id { get; set; }

        public string href { get; set; }

        public string download_href { get; set; }

    }

    public class LivePhoto
    {
        public string id { get; set; }

        public string created_at { get; set; }

        public string updated_at { get; set; }

        public string file_name { get; set; }

        public string file_type { get; set; }

        public string applicant_id { get; set; }

        public string href { get; set; }

        public string download_href { get; set; }

    }

    public class Report
    {
        public string created_at { get; set; }
        public List<object> documents { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public ReportProperties properties { get; set; }
        public string result { get; set; }
        public string status { get; set; }
        public string sub_result { get; set; }
        public string variant { get; set; }
        public object breakdown { get; set; } 
        public string check_id { get; set; }
    }

    public class ReportDocuments
    {
        public object id { get; set; }
    }

    public class ReportProperties
    {
        //passport
        public string nationality { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string issuing_country { get; set; }
        public string gender { get; set; }
        public string document_type { get; set; }
        public List<DocumentNumbers> document_numbers { get; set; }
        public string date_of_expiry { get; set; }
        public string date_of_birth { get; set; }

        //facial
        public string score { get; set; }
    }

    public class DocumentNumbers
    {
        public string value { get; set; }
        public string type { get; set; }
    }
}
