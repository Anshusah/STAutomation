using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Service.Models.JazzCash
{
    public class SecurityQuestionViewModel
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Question { get; set; }

        public bool Status { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; } 

        public string UpdatedDate { get; set; }
    }
}
