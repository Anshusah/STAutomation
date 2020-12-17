using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Cicero.Data.Entities
{
   public class MailMergeObject
    {
        [Key]
        public int Id { get; set;}
        public int FormId { get; set;}
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int TemplateId { get; set; }
        public DateTime CreatedDate { get; set;}
        public int TenantId { get; set; }
      

    }
}
