using AutoMapper.Configuration;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models.Core;
using Cicero.Service.Services;
using Cicero.Service.Services.API;
using Cicero.Service.Services.SimpleTransfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cicero.Data.Enumerations;
using static Cicero.Service.Extensions.Extensions;

namespace SimpleTransferAPI.Controllers.Api
{
    [Route("st/api")]
    [ApiController]
    public class RatesApisController: ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ICountryPayoutService _countryPayoutService;
        private readonly IExchangeRateServices exchangeRateServices;
        private readonly IRateSupplierService rateSupplierService;
        private readonly IBranchMapperService branchMapperService;
        private readonly IRateSupplierFeeConfigService rateSupplierFeeConfigService;
        private readonly IBankMapperService bankMapperService;
        private readonly ICommonService commonService;

        public RatesApisController(ICountryService countryService, ICountryPayoutService countryPayoutService, IExchangeRateServices exchangeRateServices,
            IRateSupplierService rateSupplierService, IBranchMapperService branchMapperService, IRateSupplierFeeConfigService rateSupplierFeeConfigService, IBankMapperService bankMapperService, ICommonService commonService)
        {
            _countryService = countryService;
            _countryPayoutService = countryPayoutService;
            this.exchangeRateServices = exchangeRateServices;
            this.rateSupplierService = rateSupplierService;
            this.branchMapperService = branchMapperService;
            this.rateSupplierFeeConfigService = rateSupplierFeeConfigService;
            this.bankMapperService = bankMapperService;
            this.commonService = commonService;
        }


        [HttpGet]
        [Route("getrates")]
        public IActionResult GetRates(string csb, string type, string bank, string number1, string city = "")
        {
            object response;
            var data = new { rate = "", calculate = (decimal)0.0, ratelabel = string.Empty, branch = "", transferFees = string.Empty };
            try
            {
                if (type == null)
                {
                    response = new { Success = true, StatusCode = 200, Message = "", DataList = "", Data = data };
                    return Ok(response);
                }

                var exchangeRateList = exchangeRateServices.GetExchangerateList(csb).Result;
                var topRateSupllier = rateSupplierService.GetTopPriorityRateSupplier().Result;
                string exchangeRate = "0";
                bool valid = false;
                if (exchangeRateList.Count > 0)
                {
                    valid = true;
                }
                IGrouping<decimal?, ExchangeRates> exchangeRateData = null;
                if ((int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", "")) == 1)
                {
                    exchangeRateData = exchangeRateList.Where(x => x.ModeOfPayment == 1).OrderBy(x => x.ExchangeRate).GroupBy(x => x.ExchangeRate).FirstOrDefault();
                }
                else
                {
                    if ((int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", "")) == 2)
                    {
                        var bankCodes = bank;//bankMapperService.GetBankMapperDataByBankCode(bank == null ? "" : bank).Result;
                        if (bankCodes != null)
                        {
                            exchangeRateData = exchangeRateList.Where(x => x.ModeOfPayment == 2 && (/*x.BankCode == bankCodes.NecMoneyBankCode || */x.BankCode == bankCodes/*.TransfastBankCode*/)).OrderBy(x => x.ExchangeRate).GroupBy(x => x.ExchangeRate).FirstOrDefault();
                        }
                    }
                }

                var prioritySupllierExchangeRate = new ExchangeRates();
                if (exchangeRateData != null)
                {
                    prioritySupllierExchangeRate = exchangeRateData.Where(x => x.Source == topRateSupllier).FirstOrDefault();
                }

                if (prioritySupllierExchangeRate != null)
                {
                    exchangeRate = Math.Round(Convert.ToDecimal(prioritySupllierExchangeRate.ExchangeRate), 2).ToString() + " " + prioritySupllierExchangeRate.ToCurrencyCode;
                }
                else
                {
                    var anyExchangeRate = new ExchangeRates();
                    if (exchangeRateData != null)
                    {
                        anyExchangeRate = exchangeRateData.FirstOrDefault();
                    }

                    if (anyExchangeRate != null)
                    {
                        exchangeRate = Math.Round(Convert.ToDecimal(anyExchangeRate.ExchangeRate), 2).ToString() + " " + anyExchangeRate.ToCurrencyCode;
                    }
                }

                var branchList = GetBranch(csb, city, bank);

                data = new { rate = exchangeRate, calculate = (decimal)0.0, ratelabel = string.Empty, branch = branchList.ToJson(), transferFees = string.Empty };
                var exchangeRateValues = exchangeRate.Split(" ");
                var exchangeRateValue = Convert.ToDecimal(exchangeRateValues[0]);

                var checkIfFirst = false;

                if ((int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", "")) == 2 && bank == null)
                {
                    checkIfFirst = true;
                }

                if (checkIfFirst)
                {
                    response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = data };
                    return Ok(response);
                }

                if (valid && exchangeRateValue > 0)
                {
                    decimal cal = 0;
                    var num1 = Convert.ToDecimal(number1);
                    if (num1 > 0)
                    {
                        cal = CalculateValues(number1, exchangeRateValue.ToString());
                    }

                    var currencyCode = _countryService.GetCountryCurrencyCode(csb).Result;
                    var exchangeRateLabel = GetExchangeRateValue(number1, cal, csb);
                    var transferFeesData = rateSupplierFeeConfigService.GetRateSupplierFeeBySendAmountAsync(number1 == null ? "0" : number1, csb, (int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", ""))).Result;
                    var transferFees = string.Empty;
                    if (transferFeesData != null)
                    {
                        transferFees = transferFeesData.FeeAmount.ToString();
                    }

                    var transferFeesValue = string.Empty;
                    if (transferFees != string.Empty)
                    {
                        transferFeesValue = transferFees + " " + "GBP";
                    }
                    data = new { rate = exchangeRate, calculate = Math.Round(cal, 2), ratelabel = exchangeRateLabel, branch = branchList.ToJson(), transferFees = transferFeesValue };
                    response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = data };

                }
                else
                {
                    response = new { Success = false, StatusCode = 404, Message = "Rate for this country has not been configured. Please contact customer care service.", DataList = "", Data = data };
                }

            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Internal Server Error. Please contact customer care service.", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        private decimal GetExchangeRate(string csb, string type, string bank)
        {
            try
            {
                var exchangeRateList = exchangeRateServices.GetExchangerateList(csb).Result;
                var topRateSupllier = rateSupplierService.GetTopPriorityRateSupplier().Result;
                decimal exchangeRate = 0;
                bool valid = false;
                if (exchangeRateList.Count > 0)
                {
                    valid = true;
                }
                IGrouping<decimal?, ExchangeRates> exchangeRateData = null;
                if ((int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", "")) == 1)
                {
                    exchangeRateData = exchangeRateList.Where(x => x.ModeOfPayment == 1).OrderBy(x => x.ExchangeRate).GroupBy(x => x.ExchangeRate).FirstOrDefault();
                }
                else
                {
                    if ((int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", "")) == 2)
                    {
                        var bankCodes = bank == null ? "" : bank;//bankMapperService.GetBankMapperDataByBankCode(bank == null ? "" : bank).Result;
                        if (bankCodes != null)
                        {
                            exchangeRateData = exchangeRateList.Where(x => x.ModeOfPayment == 2 && (/*x.BankCode == bankCodes.NecMoneyBankCode || */x.BankCode == bankCodes/*.TransfastBankCode*/)).OrderBy(x => x.ExchangeRate).GroupBy(x => x.ExchangeRate).FirstOrDefault();
                        }
                    }
                }

                var prioritySupllierExchangeRate = new ExchangeRates();
                if (exchangeRateData != null)
                {
                    prioritySupllierExchangeRate = exchangeRateData.Where(x => x.Source == topRateSupllier).FirstOrDefault();
                }

                if (prioritySupllierExchangeRate != null)
                {
                    exchangeRate = Math.Round(Convert.ToDecimal(prioritySupllierExchangeRate.ExchangeRate), 2);
                }
                else
                {
                    var anyExchangeRate = new ExchangeRates();
                    if (exchangeRateData != null)
                    {
                        anyExchangeRate = exchangeRateData.FirstOrDefault();
                    }

                    if (anyExchangeRate != null)
                    {
                        exchangeRate = Math.Round(Convert.ToDecimal(anyExchangeRate.ExchangeRate), 2);
                    }
                }

                if (valid && exchangeRate > 0)
                {
                    return exchangeRate;

                }
                else
                {
                    return exchangeRate;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpGet]
        [Route("gettype")]
        public IActionResult GetType(string csb, string userId)
        {
            object response;
            var typeList = new List<string> { "Bank Transfer", "Cash Pickup" };
            try
            {
                var exchangeRateList = exchangeRateServices.GetExchangerateList(csb).Result;
                var topRateSupllier = rateSupplierService.GetTopPriorityRateSupplier().Result;
                string exchangeRate = "0";
                bool valid = false;
                if (exchangeRateList.Count > 0)
                {
                    valid = true;
                }
                IGrouping<decimal?, ExchangeRates> exchangeRateData = null;
                var exchangeRateValueList = new List<decimal>();

                if (typeList.Contains("Bank Transfer"))
                {
                    exchangeRateData = exchangeRateList.Where(x => x.ModeOfPayment == 1).OrderBy(x => x.ExchangeRate).GroupBy(x => x.ExchangeRate).FirstOrDefault();
                    exchangeRate = GetExchangeRate(exchangeRateData, topRateSupllier);
                    var exchangeRateValues = exchangeRate.Split(" ");

                    var isDecimal = Decimal.TryParse(exchangeRateValues[0], out _);
                    var exchangeRateValue = isDecimal ? Convert.ToDecimal(exchangeRateValues[0]) : 0;
                    exchangeRateValueList.Add(exchangeRateValue);
                }
                if (typeList.Contains("Cash Pickup"))
                {
                    exchangeRateData = exchangeRateList.Where(x => x.ModeOfPayment == 2).OrderBy(x => x.ExchangeRate).GroupBy(x => x.ExchangeRate).FirstOrDefault();
                    exchangeRate = GetExchangeRate(exchangeRateData, topRateSupllier);
                    var exchangeRateValues = exchangeRate.Split(" ");

                    var isDecimal = Decimal.TryParse(exchangeRateValues[0], out _);
                    var exchangeRateValue = isDecimal ? Convert.ToDecimal(exchangeRateValues[0]) : 0;
                    exchangeRateValueList.Add(exchangeRateValue);
                }

                var paymentMethod = GetPaymentMethod(csb, userId);
                

                List<SelectListItemWithIcon> currencyto = _countryService.GetCountryToCurrenciesList(csb);
                if (valid && exchangeRateValueList.Where(x => x != 0).Count() > 0)
                {
                    var type = _countryPayoutService.GetCountryPayoutMethodByCountryAsync(csb).Result;
                    var bankTransferValue = EnumModel<PayoutMode>.GetDescription((int)PayoutMode.BankTransfer);
                    var typeValue = (type.Contains(bankTransferValue)) ? bankTransferValue : type.FirstOrDefault();
                    var data = new { rate = exchangeRate, type = typeValue, currencyto = currencyto.ToJson(), referenceNo = RandomString(10) };

                    var message = "Get Successfully";
                    if (type.Count == 0)
                    {
                        message = "Payment Method is not configured";
                    }
                    response = new { Success = true, StatusCode = 200, Message = message, DataList = "", Data = data, Target = paymentMethod.ToJson() };

                }
                else
                {
                    var data = new { rate = exchangeRate, type = "", currencyto = currencyto.ToJson() };
                    response = new { Success = false, StatusCode = 404, Message = "Rate for this country has not been configured. Please contact customer care service.", DataList = "", Data = data };
                }

            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Internal Server Error. Please contact customer care service.", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        private string GetExchangeRate(IGrouping<decimal?, ExchangeRates> exchangeRateData, string topRateSupllier)
        {
            string exchangeRate = "0";
            var prioritySupllierExchangeRate = new ExchangeRates();
            if (exchangeRateData != null)
            {
                prioritySupllierExchangeRate = exchangeRateData.Where(x => x.Source == topRateSupllier).FirstOrDefault();
            }

            if (prioritySupllierExchangeRate != null)
            {
                exchangeRate = Math.Round(Convert.ToDecimal(prioritySupllierExchangeRate.ExchangeRate), 2).ToString() + " " + prioritySupllierExchangeRate.ToCurrencyCode;
            }
            else
            {
                var anyExchangeRate = new ExchangeRates();
                if (exchangeRateData != null)
                {
                    anyExchangeRate = exchangeRateData.FirstOrDefault();
                }

                if (anyExchangeRate != null)
                {
                    exchangeRate = Math.Round(Convert.ToDecimal(anyExchangeRate.ExchangeRate), 2).ToString() + " " + anyExchangeRate.ToCurrencyCode;
                }
            }
            return exchangeRate;
        }


        [HttpGet]
        [Route("calculate")]
        public IActionResult Calculate(string number1, string type, string csb, string bank)
        {
            object response;
            try
            {
                var num1 = Convert.ToDecimal(number1);
                if (num1 < 0)
                {
                    object datas = new { calculate = 0, rate = "", transferFees = "" };
                    response = new { Success = true, StatusCode = 200, Message = "", DataList = "", Data = datas };
                    return Ok(response);
                }
                var number2 = GetExchangeRate(csb, type, bank);
                decimal cal = CalculateValues(number1, number2.ToString());
                var currencyCode = _countryService.GetCountryCurrencyCode(csb).Result;
                var exchangeRate = GetExchangeRateValue(number1, cal, csb);

                var transferFeesData = rateSupplierFeeConfigService.GetRateSupplierFeeBySendAmountAsync(number1 == null ? "0" : number1, csb, (int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", ""))).Result;
                var transferFees = string.Empty;
                if (transferFeesData != null)
                {
                    transferFees = transferFeesData.FeeAmount.ToString();
                }

                var transferFeesValue = string.Empty;
                if (transferFees != string.Empty)
                {
                    transferFeesValue = transferFees + " " + "GBP";
                }

                object data = new { calculate = Math.Round(cal, 2), rate = exchangeRate, transferFees = transferFeesValue };
                response = new { Success = true, StatusCode = 200, Message = "Calculated Successfully", DataList = "", Data = data };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 404, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        private decimal CalculateValues(string number1, string number2)
        {
            var num1 = Convert.ToDecimal(number1);
            var num2 = Convert.ToDecimal(number2);
            decimal cal = num1 * num2;

            return cal;
        }

        private string GetExchangeRateValue(string number1, decimal cal, string csb)
        {
            var currencyCode = _countryService.GetCountryCurrencyCode(csb).Result;
            var exchangeRate = number1 + " GBP=" + Math.Round(cal, 2) + " " + currencyCode;

            return exchangeRate;
        }

        [HttpGet]
        [Route("divide")]
        public IActionResult Divide(string number1, string csb, string type, string bank)
        {
            object response;
            try
            {
                var num1 = Convert.ToDecimal(number1);
                if (num1 < 0)
                {
                    object datas = new { calculate = 0, rate = "", transferFees = "" };
                    response = new { Success = true, StatusCode = 200, Message = "", DataList = "", Data = datas };
                    return Ok(response);
                }
                var num2 = GetExchangeRate(csb, type, bank);
                decimal cal = num1 / num2;
                var currencyCode = _countryService.GetCountryCurrencyCode(csb).Result;
                var exchangeRate = Math.Round(cal, 2) + " GBP= " + number1 + " " + currencyCode;

                var transferFeesData = rateSupplierFeeConfigService.GetRateSupplierFeeBySendAmountAsync(Math.Round(cal, 2).ToString(), csb, (int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", ""))).Result;
                var transferFees = string.Empty;
                if (transferFeesData != null)
                {
                    transferFees = transferFeesData.FeeAmount.ToString();
                }

                var transferFeesValue = string.Empty;
                if (transferFees != string.Empty)
                {
                    transferFeesValue = transferFees + " " + "GBP";
                }

                object data = new { calculate = Math.Round(cal, 2), rate = exchangeRate, transferFees = transferFeesValue };
                response = new { Success = true, StatusCode = 200, Message = "Calculated Successfully", DataList = "", Data = data };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 404, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getratesforbank")]
        public IActionResult GetRateForBank(string citycode)
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
        [Route("getbranchs")]
        public List<SelectListItem> GetBranch(string csb, string cityCode, string bankCode)
        {
            try
            {
                var branchList = branchMapperService.GetBranchList(csb, cityCode, bankCode).Result;
                branchList.Insert(0, new SelectListItemWithIcon { Text = "Select Branch", Value = "", Icon = "" });
                return branchList;
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }

        [HttpGet]
        [Route("getpaymentmethod")]
        public object GetPaymentMethod(string csb, string userId)
        {
            object response;
            try
            {
                var paymentMethods = _countryPayoutService.GetCountryPayoutMethodByCountryAsync(csb).Result;
                List<object> paymentMethodList = new List<object>();

                foreach (var item in paymentMethods)
                {
                    paymentMethodList.Add(new { value = item });
                }

                if (userId == null || userId == "")
                {
                    paymentMethodList.Add(new { value = "Login And Send Money" });
                }
                else
                {
                    paymentMethodList.Add(new { value = "Send Money" });
                }
                return paymentMethodList;
            }
            catch (Exception ex)
            {
                response = null;
                return null;
            }
        }

        private string RandomString(int size, bool lowerCase = false)
        {
            Random _random = new Random();
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
