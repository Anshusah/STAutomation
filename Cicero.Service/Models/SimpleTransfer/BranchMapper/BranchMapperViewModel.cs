using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.BranchMapper
{
    public class BranchMapperViewModel
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public string BranchName { get; set; }

        public string BranchCode { get; set; }

        public string BankCode { get; set; }

        public string CountryCode { get; set; }

        public string CityCode { get; set; }

        public bool Status { get; set; }

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class BranchMapperDataModel
    {
        public string CountryCode { get; set; }
        public int SupplierId { get; set; }
        public string BankCode { get; set; }
        public List<string> Cities { get; set; }
        public List<string> BranchCode { get; set; }
        public List<string> BranchName { get; set; }
    }
}
