using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.BankMapper
{
    public class BankMapperViewModel
    {
        public int Id { get; set; }

        public string CountryCode { get; set; }

        public string TransfastBankCode { get; set; }

        public string NecMoneyBankCode { get; set; }

        public bool Status { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; }
    }

    public class BankListViewModel
    {
        public List<SelectListItem> NecMoneyBankList { get; set; }
        public List<SelectListItem> TransfastBankList { get; set; }
        public List<bool> Status { get; set; }
    }
}
