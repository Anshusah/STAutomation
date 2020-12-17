using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class StateToState
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey("FromState")]
        public int FromStateId { get; set; }

        [ForeignKey("ToState")]
        public int ToStateId { get; set; }

        public int CaseFormId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string StatePosX { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string StatePosY { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string LinePosX { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string LinePosY { get; set; }

        public bool Aero { get; set; }

    

        public virtual State FromState { get; set; }

        public virtual State ToState { get; set; }

        [ForeignKey("Actions")]
        public int? ActionsId { get; set; }

        public virtual Actions Actions { get; set; }
        
    }

}
