using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
   public class MailMergeField
    {
        [Key]
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string Alias { get; set; }
        public string DbSourceTable { get; set; }
        public string DbSourceField { get; set; }
        public int TemplateType { get; set; }
        public int TenantId { get; set; }
        public int FormId { get; set; }
        public bool isDeleted { get; set; }

    }
}
