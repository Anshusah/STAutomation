﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class OnfidoApplicantDocumentViewModel
    {
        public string id { get; set; }

        public string DocumentId { get; set; }

        public string created_at { get; set; }

        public string updated_at { get; set; }

        public string file_name { get; set; }

        public string file_type { get; set; }

        public string type { get; set; }

        public string side { get; set; }

        public string Url { get; set; }

        public string issuing_country { get; set; }

        public string applicant_id { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}
