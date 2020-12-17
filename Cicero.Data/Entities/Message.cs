using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int ParentId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Subject { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Attachment { get; set; }

        public bool IsNotice { get; set; }

        public bool ClientNotified { get; set; }

        //[ForeignKey("MessageUsers")]
        //public string To { get; set; }

        [ForeignKey("Case")]
        public int? ClaimId { get; set; }

        [ForeignKey("Sender")]
        public string From { get; set; }

        [ForeignKey("TenantId")]
        public int TenantId { get; set; }

        public bool SenderDelete { get; set; }

        public bool ReceiverDelete { get; set; }

        public virtual Case Case { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<MessageUser> MessageUsers { get; set; }

        public virtual ApplicationUser Sender { get; set; }

    }
}
