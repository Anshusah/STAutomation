﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class OnfidoChecks
    {
        [Key]
        [StringLength(450)]
        public string id { get; set; }
        public string ChecksId { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime? updated_at { get; set; }
        public string status { get; set; }
        public string redirect_uri { get; set; }
        public string result { get; set; }
        public bool sandbox { get; set; }
        public string tags { get; set; }
        public string results_uri { get; set; }
        public string form_uri { get; set; }
        public bool paused { get; set; }
        public string version { get; set; }
        public string report_ids { get; set; }
        public string href { get; set; }
        public string applicant_id { get; set; }
        public bool applicant_provides_data { get; set; }


        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }
    }
}
