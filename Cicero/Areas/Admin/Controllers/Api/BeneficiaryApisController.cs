using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Services.SimpleTransfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Models.Core;
using Cicero.Service.Services;
using System.Text.RegularExpressions;
using static Cicero.Data.Enumerations;
using Microsoft.AspNetCore.Http;
using Cicero.Data.Extensions;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer;

namespace SimpleTransferAPI.Controllers
{

    [Route("st/api")]
    [ApiController]
    public class BeneficiaryApisController : ControllerBase
    {
        private readonly ICountryService countryService;
        private readonly IBranchMapperService branchMapperService;
        private readonly IRelationshipSetupService relationshipSetupService;
        private readonly IBeneficiarySetupService beneficiarySetupService;
        private readonly IGenderSetupService genderSetupService;

        public BeneficiaryApisController(ICountryService countryService, IBranchMapperService branchMapperService,
            IRelationshipSetupService relationshipSetupService,IBeneficiarySetupService beneficiarySetupService, IGenderSetupService genderSetupService)
        {
            this.countryService = countryService;
            this.branchMapperService = branchMapperService;
            this.relationshipSetupService = relationshipSetupService;
            this.beneficiarySetupService = beneficiarySetupService;
            this.genderSetupService = genderSetupService;
        }

        [HttpGet]
        [Route("getbeneficiarybyuserid")]
        public IActionResult GetBeneficiaryByUserIdAsync(string userId, string type, string countryCode)
        {
            object response;
            try
            {
                var beneList = beneficiarySetupService.GetBeneficiaryListAsync(userId, countryCode);
                beneList.Insert(0, new SelectListItem { Text = "Select Beneficiary", Value = "" });
                object beneficiaries = new { beneficiary = beneList.ToJson() };
                var typeValue = TypeValue(type);
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = beneficiaries, Target = typeValue.ToJson() };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getbeneficiarydetailbyid")]
        public IActionResult GetBeneficiaryDetailById(int id)
        {
            object response;
            try
            {
                var beneDetail = beneficiarySetupService.GetBeneficiaryDetailByIdAsync(id);
                string countryCode = beneDetail["CountryId"].ToString();
                string beneRelationshipId = beneDetail["RelationshipToBeneId"].ToString();
                string gender = beneDetail["GenderId"].ToString();
                string cityCode = beneDetail["CityId"].ToString();
                string bankCode = beneDetail["BankId"].ToString();
                string bankBranchCode = beneDetail["BankBranchId"].ToString();
                string accountTypeId = beneDetail["AccountTypeId"].ToString();

                List<SelectListItem> accountType = new List<SelectListItem>();
                accountType.Add(new SelectListItem()
                {
                    Text = "Select Account Type",
                    Value = ""
                });
              
                accountType.Add(new SelectListItem()
                {
                    Text = "Current",
                    Value = "2",
                    Selected = accountTypeId == "2" ? true : false
                });

                accountType.Add(new SelectListItem()
                {
                    Text = "Saving",
                    Value = "1",
                    Selected = accountTypeId == "1" ? true : false
                });
                beneDetail["CountryId"] = countryService.GetCountryByCodeAsync(countryCode).ToJson();
                beneDetail["CityId"] = branchMapperService.GetCityByCode(countryCode, cityCode).ToJson();
                beneDetail["RelationshipToBeneId"] = relationshipSetupService.GetRelationshipById(beneRelationshipId).ToJson();
                beneDetail["GenderId"] = genderSetupService.GetGenderByCode(gender).ToJson();
                beneDetail["BankId"] = branchMapperService.GetBankByCityCode(bankCode, cityCode).ToJson();
                beneDetail["BankBranchId"] = branchMapperService.GetBranchListByBankCode(countryCode, cityCode, bankCode, bankBranchCode).ToJson();
                beneDetail["AccountTypeId"] = accountType.ToJson();

                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = beneDetail };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        public List<object> TypeValue(string type)
        {
            try
            {
                List<object> paymentMethodList = new List<object>();
                paymentMethodList.Add(new { value = type });
                return paymentMethodList;
            }
            catch (Exception ex)
            {
                return new List<object>();
            }
        }

    }
}
