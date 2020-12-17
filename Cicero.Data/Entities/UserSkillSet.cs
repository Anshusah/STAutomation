using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cicero.Data.Entities
{
    public class UserSkillSet
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public int SkillSetId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("SkillSetId")]
        public virtual SkillSet SkillSet { get; set; }
    }
}
