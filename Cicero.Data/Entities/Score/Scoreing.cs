using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class Scoreing
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }

       [ForeignKey("ProductId")]
       public virtual ProductConfig ProductConfig { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime StartDate { get; set; }

        [Column(TypeName ="varchar(500)")]
        public string PoolName { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Value { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string CreatedBy { get; set; }
        public string Parameter { get; set; }
        public string Expression { get; set; }
        public int CaseFormId { get; set; }

    }
}
