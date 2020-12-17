using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class UserMedia
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int MediaId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("MediaId")]
        public virtual Media Media { get; set; }
    }
}
