using Cicero.Data.Extensions;
using Cicero.Service.Models.General;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Cicero.Service.Extensions.Extensions;

namespace Cicero.Service.Models.SimpleTransfer
{
    public class RateSupplierFeeConfigViewModel
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public int SupplierId { get; set; }
        public int TenantId { get; set; }
        public int PayoutModeId { get; set; }
        [Required(ErrorMessage = "Please Enter Upper Limit Amount.")]
        [Display(Name = "Upper Limit Amount")]
        [GreaterThan("LowerLimitAmount")]

        public decimal UpperLimitAmount { get; set; }
        [Required(ErrorMessage = "Please Enter Lower Limit Amount.")]
        [Display(Name = "Lower Limit Amount")]
        [LessThan("UpperLimitAmount")]
        public decimal LowerLimitAmount { get; set; }
        [Required(ErrorMessage = "Please Enter Fee Amount.")]
        [Display(Name = "Fee Amount")]
        [Range(0,1000)]
        public decimal FeeAmount { get; set; }
        [Required(ErrorMessage = "Please Enter Fee Percentage.")]
        [Range(0,100)]
        [Display(Name = "Fee Percentage")]
        public decimal FeePercentage { get; set; }
        public string Remark { get; set; }
        public bool Status { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        public List<SelectListItem> SupplierList { get; set; }
        public List<SelectListItem> PayoutModeList { get; set; }
    }

  
}
