using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{

    public class RolePermission
    {
       
        public int Id { get; set; }
        [ForeignKey("ApplicationRole")]
        public string RoleId { get; set; }
        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public int PermissionGroupId { get; set; }
        public Permission Permission { get; set; }
        public ApplicationRole ApplicationRole { get; set; }

    }

}
