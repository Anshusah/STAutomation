using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class RateSupplier:CreatedUpdatedTimeUser
    {
        [Key]

        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SystemId { get; set; }
        public int RatePriority { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreatedUpdatedTimeUser
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
