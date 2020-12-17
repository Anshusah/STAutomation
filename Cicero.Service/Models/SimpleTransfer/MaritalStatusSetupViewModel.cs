using Cicero.Service.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Cicero.Service.Extensions.Extensions;

namespace Cicero.Service.Models.SimpleTransfer
{
    public class MaritalStatusSetupViewModel
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        [Required(ErrorMessage = "Please Enter Marital Status.")]
        [Display(Name = "Marital Status")]
        [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters and at least have {2} characters. ", MinimumLength = 3)]

        public string MaritalStatusName { get; set; }
        public bool Status { get; set; }
        public string UpdatedDate { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; } 
    }
}
