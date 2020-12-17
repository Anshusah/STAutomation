using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Cicero.Service.Models
{
    public class ElementStateViewModel
    {
        [Key]
        public long Id { get; set; }
        public string Type { get; set; }
        public bool isDefaultEnd { get; set; } = false;
        public string Name { get; set; }
        public string ElementId { get; set; }
        public int TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ForEventType { get; set; }
        public int FormId { get; set; }
    }
}
