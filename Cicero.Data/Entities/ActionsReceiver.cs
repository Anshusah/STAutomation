
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cicero.Data.Entities
{
    public class ActionsReceiver
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public int ActionsId { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("ActionsId")]
        public virtual Actions Actions { get; set; }

      
    }
}
