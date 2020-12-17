using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Onfido
{
    public class OnfidoApplicantViewModel
    {
        public string id { get; set; }

        public string ApplicantId { get; set; }

        public string CustomerId { get; set; }

        public string created_at { get; set; }

        public string updated_at { get; set; }

        public bool sandbox { get; set; }

        [Required]
        public string first_name { get; set; }

        [Required]
        public string last_name { get; set; }

        public string email { get; set; }

        public string dob { get; set; }

        public DateTime? delete_at { get; set; }

        public string href { get; set; }

        public string flat_number { get; set; }

        public string building_number { get; set; }

        public string building_name { get; set; }

        public string street { get; set; }

        public string sub_street { get; set; }

        public string town { get; set; }

        public string state { get; set; }

        public string postcode { get; set; }

        public string country { get; set; }

        public string line1 { get; set; }

        public string line2 { get; set; }

        public string line3 { get; set; }

        public string id_numbers { get; set; }

        public bool Status { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }
    }
}
