using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{

    public class Setting
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FieldKey { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string FieldValue { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string FieldDisplay { get; set; }

        public int FieldVisiblity { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FieldType { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string FieldOptions { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string FieldGridSize { get; set; }

        public int? TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

    }

}
