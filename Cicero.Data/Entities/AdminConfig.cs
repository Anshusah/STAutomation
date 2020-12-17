using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Data.Entities
{
    public class AdminConfig
    {
        [Key]
        public int Id { get; set; }
        public string KeyId { get; set; }
        public string Value { get; set; }
    }
}