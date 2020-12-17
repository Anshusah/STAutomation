using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Cicero.Service.SimpleTransferEnums;

namespace Cicero.Service.Models.SimpleTransfer.AutoSchedulerSetting
{
    public class AutoSchedulerSettingViewModel
    {
        [Key]

        public int Id { get; set; }
        public SchedulerList Name { get; set; }
        public string Hour { get; set; }
        public string Minutes { get; set; }
        public string Interval { get; set; }
        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public bool Status { get; set; }
    }
}
