using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class Emails
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Emailstring { get; set; }

        public int EmailGroupId { get; set; }

        [ForeignKey("EmailGroupId")]
        public virtual EmailGroup EmailGroups { get; set; }
    }
}
