using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
   public class MailMergeFieldViewModel
    {
     
        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name is Required")]
        [Display(Name = "Field Name")]
        public string FieldName { get; set; }

        public string Alias { get; set; }

        [Required(ErrorMessage = "Source Table is Required")]
        public string DbSourceTable { get; set; }

        [Required(ErrorMessage = "Source Field is Required")]
        public string DbSourceField { get; set; }

        [Required(ErrorMessage = "Template Type is Required")]
        public int TemplateType { get; set; }

        public int TenantId { get; set; }

       
        public int FormId { get; set; }

        public bool isDeleted { get; set; }
    }
}
