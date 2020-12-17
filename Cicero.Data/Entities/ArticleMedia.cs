using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class ArticleMedia
    {
        [Key]
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public int MediaId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        [ForeignKey("MediaId")]
        public virtual Media Media { get; set; }
    }
}
