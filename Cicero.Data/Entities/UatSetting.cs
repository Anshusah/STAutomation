using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class UatSetting
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
    }
}
