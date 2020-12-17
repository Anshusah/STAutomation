using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{

    public class ApplicationRole : IdentityRole<string>
    {
        [Column(TypeName = "int")]
        public int Type { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string DisplayName { get; set; }

        public short Status { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        public int TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenants { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        public string OrganizationName { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        //public virtual ICollection<Queue> RoleQueues { get; set; }

        public virtual ICollection<StatePermission> RoleState { get; set; }
    }

}
