using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities
{
    public class CountryList
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Code { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }
    }
}
