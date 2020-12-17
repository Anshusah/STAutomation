using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Cicero.Service.SimpleTransferEnums;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class AutoSchedulerSetting : CreatedUpdatedTimeUser
    {
        [Key]

        public int Id { get; set; }
        public SchedulerList Name { get; set; }
        public string Hour { get; set; }
        public string Minutes { get; set; }
        public string Interval { get; set; }
        public bool IsActive { get; set; }
    }    
}
