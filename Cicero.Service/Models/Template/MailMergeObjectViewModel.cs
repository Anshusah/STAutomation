using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
   public class MailMergeObjectViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FormId { get; set; }

        [Required]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public int TemplateId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TenantId { get; set; }
    }
}
