using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class CustomerCardDetail
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string ExpDate { get; set; }
        public string Csv { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TenantId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; }
    }
}
