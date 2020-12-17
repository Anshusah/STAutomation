using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class NotificationHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        
        public Guid NotifiedToId { get; set; }
        public DateTime NotifiedDate { get; set; }
        [ForeignKey("NotifiedToId")]
        public virtual ApplicationUser User { get; set; }
    }

}
