using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class CaseStateHistory
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int CaseId { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public int? PreviousStateId { get; set; }

        public int? CurrentStateId { get; set; }

        public string Reason { get; set; }

        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("PreviousStateId")]
        public virtual State PreviousState { get; set; }

        [ForeignKey("CurrentStateId")]
        public virtual State CurrentState { get; set; }
    }
}
