using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class CaseMedia
    {
        [Key]
        public int Id { get; set; }

        public int CaseId { get; set; }

        public int MediaId { get; set; }

        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; }

        [ForeignKey("MediaId")]
        public virtual Media Media { get; set; }
    }
}
