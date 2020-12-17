using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class MessageUser
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("MessageForUser")]
        public int MessageId { get; set; }

        [ForeignKey("UserForMessage")]
        public string UserId { get; set; }

        public virtual ApplicationUser UserForMessage { get; set; }

        public virtual Message MessageForUser { get; set; }
    }
}
