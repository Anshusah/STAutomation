using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cicero.Data.Entities
{
    public class ElementState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Type { get; set; }

        public bool isDefaultEnd { get; set; }

        public string Name { get; set; }

        public string ElementId { get; set; }

        public int TenantId { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ForEventType { get; set; }

        public int FormId { get; set; }
    }
}