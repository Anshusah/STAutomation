using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.BankSetting
{
    public class BankSettingViewModel
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string CountryCode { get; set; }
        public bool Status { get; set; }
        public string UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}
