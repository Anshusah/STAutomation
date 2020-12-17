using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public partial class MessageViewModel
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public int ClaimId { get; set; }

        public string ClaimGeneratedId { get; set; }

        //[Required(ErrorMessage ="Select the intended reciever for the message.")]
        public List<string> To { get; set; }

        public string From { get; set; }

        public string NameSender { get; set; }

        public List<string> NameReceiver { get; set; }

        [Required(ErrorMessage = "Enter the subject for the message.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message cannot be blank.")]
        public string Content { get; set; }

        public string Attachment { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        public int TenantId { get; set; }

        public bool SenderDelete { get; set; }

        public bool ReceiverDelete { get; set; }

        public bool IsNotice { get; set; }

        public bool ClientNotified { get; set; }

        public string SenderImage { get; set; }

        public List<string> ReceiverImage { get; set; }
    }
}