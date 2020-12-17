using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class QueueToState
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public int StateId { get; set; }

        public int QueueId { get; set; }

        public int CaseFormId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string PosX { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string PosY { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string LinePosX { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string LinePosY { get; set; }

        public bool IsQueue { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [ForeignKey("QueueId")]
        public virtual Queue Queue { get; set; }
    }
}
