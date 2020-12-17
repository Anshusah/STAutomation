using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{

    public class ApplicationUser : IdentityUser<string>
    {
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string DisplayName { get; set; }

        [StringLength(450)]
        public string UserId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Address { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsSuperAdmin { get; set; } = false;

        public virtual ICollection<TenantUser> TenantUsers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<MessageUser> UserMessages { get; set; }
        public virtual ICollection<UserMedia> UserMedias { get; set; }
    }

}
