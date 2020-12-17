using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class TenantUser
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant TenantForUser { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser UserForTenant { get; set; }
    }
}
