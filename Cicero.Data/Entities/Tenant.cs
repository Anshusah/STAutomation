using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class Tenant
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Identifier { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string AddressPrimary { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string AddressSecondary { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string PostCode { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string City { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "varchar(100)")]
        public bool Status { get; set; }


        public virtual ICollection<TenantUser> TenantUsers { get; set; }
        public virtual ICollection<ApplicationRole> TenantRoles { get; set; }
        public virtual ICollection<Article> TenantArticles { get; set; }
        public virtual ICollection<Case> TenantCases { get; set; }
        public virtual ICollection<State> TenantStates { get; set; }
        public virtual ICollection<Queue> TenantQueues { get; set; }
        public virtual ICollection<ActivityLog> TenantActivityLogs { get; set; }
        public virtual ICollection<Message> TenantMessages { get; set; }
        public virtual ICollection<Media> TenantMedias { get; set; }
        public virtual ICollection<CaseForm> TenantCaseClaims { get; set; }
    }
}
