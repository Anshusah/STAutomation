using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class OnfidoReport
    {
        public string created_at { get; set; }
        public List<object> documents { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public OnfidoReportProperties properties { get; set; }
        public string result { get; set; }
        public string status { get; set; }
        public string sub_result { get; set; }
        public string variant { get; set; }
        public object breakdown { get; set; }
        public string check_id { get; set; }
    }

    public class OnfidoReportDocuments
    {
        public string id { get; set; }
    }

    public class OnfidoReportProperties
    {
        //passport
        public string nationality { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string issuing_country { get; set; }
        public string gender { get; set; }
        public string document_type { get; set; }
        public List<OnfidoDocumentNumbers> document_numbers { get; set; }
        public string date_of_expiry { get; set; }
        public string date_of_birth { get; set; }

        //facial
        public string score { get; set; }
    }

    public class OnfidoDocumentNumbers
    {
        public string value { get; set; }
        public string type { get; set; }
    }
}
