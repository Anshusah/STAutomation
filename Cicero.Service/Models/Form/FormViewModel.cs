using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class FormViewModel
    {

        public int Id { get; set; }

        public string CaseGeneratedId { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public int TenantId { get; set; }

        [Display(Name = "Insurance Policy Type")]
        [Required(ErrorMessage = "Please Select a policy type.")]
        public int CaseFormId { get; set; }

        [Display(Name = "Case Type")]
        public string CaseFormName { get; set; }

        public object FormBuilder { get; set; }

    }
}