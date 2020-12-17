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
    public class CountryApisController : ControllerBase
    {

        private readonly ICountryService _countryService;
        private readonly IBranchMapperService branchMapperService;

        public CountryApisController(ICountryService countryService, IBranchMapperService branchMapperService)
        {
            _countryService = countryService;
            this.branchMapperService = branchMapperService;
        }

        [HttpGet]
        [Route("getallcountries")]
        public IActionResult GetAllCountries()
        {
            object response;
            try
            {
                var countryList = _countryService.GetCountryList().Result;
                countryList.Insert(0, new SelectListItemWithIcon { Text = "Select Country", Value = "", Icon = "" });
                object country = new { country = countryList.ToJson() };

                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = country };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getcountries")]
        public IActionResult GetCountries(string countryCode)
        {
            object response;
            try
            {
                var countryList = _countryService.GetCountryListExceptUk(countryCode).Result;                
                countryList.Insert(0, new SelectListItemWithIcon { Text = "Select Country", Value = "", Icon = "" });
                object country = new { country = countryList.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = country };
                
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getcities")]
        public IActionResult GetCities(string csb)
        {
            object response;
            try
            {
                var cityList = branchMapperService.GetCities(csb).Result;
                cityList.Insert(0, new SelectListItemWithIcon { Text = "Select City", Value = "", Icon = "" });

                object city = new { city = cityList.ToJson() };

                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = city };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getbanks")]
        public IActionResult GetBanks(string citycode)
        {
            object response;
            try
            {
                var bankList = branchMapperService.GetBankListByCityCode(citycode).Result;
                bankList.Insert(0, new SelectListItemWithIcon { Text = "Select Bank", Value = "", Icon = "" });

                object bank = new { bank = bankList.ToJson() };

                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = bank };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getbranches")]
        public IActionResult GetBranches(string csb, string bankCode, string cityCode, string beneBankCode, string type)
        {
            object response;
            try
            {
                var branchList = branchMapperService.GetBranchListForBene(csb, cityCode, bankCode, beneBankCode, type).Result.Select(x => new SelectListItem()
                {
                    Text = x.Text,
                    Value = x.Value
                }).ToList();
                branchList.Insert(0, new SelectListItem { Text = "Select Branch", Value = "" });
                object branches = new { branches = branchList.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = branches };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getcurrenciesfrom")]
        public IActionResult GetCountryCurrenciesFrom()
        {
            object response;
            try
            {
                var countryList = _countryService.GetCountryFromCurrenciesList();
                object country = new { country = countryList.ToJson() };

                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = country };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getcurrenciesto")]
        public IActionResult GetCountryCurrenciesTo(string countryCode)
        {
            object response;
            try
            {
                var countryList = _countryService.GetCountryToCurrenciesList(countryCode);
                object country = new { country = countryList.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = country };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

    }
}
