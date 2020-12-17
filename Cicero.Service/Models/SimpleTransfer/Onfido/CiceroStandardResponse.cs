using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class CiceroStandardResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<Applicant> Datas { get; set; }
        public Applicant Data { get; set; }
    }
}
