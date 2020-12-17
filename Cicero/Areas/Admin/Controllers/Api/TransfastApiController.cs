using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Newtonsoft.Json;
using static Cicero.Service.SimpleTransferEnums;
using static Cicero.Data.Enumerations;
using Cicero.Service.Services;
using Cicero.Service.Helpers;
using Cicero.Service.Configuration;
using Cicero.Service.Models;
using Cicero.Service.Services.API;
using Cicero.Service.Services.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer.SecureTrading;
using AutoMapper;
using System.Text.RegularExpressions;
using LexisNexis;
using Cicero.Service.Models.API.LexisNexis;

namespace Cicero.Areas.Admin.Controllers.Api
{
    [Route("api/transfast")]
    [ApiController]
    public class TransfastApiController : BaseController
    {
        private readonly int InternalServerError = (int)System.Net.HttpStatusCode.InternalServerError;
        private readonly IUserService userService;
        private readonly IConfiguration _config;
        private readonly IMapperService _mapperService;
        private readonly string apiUrl;
        private readonly string baseUrl;
        private readonly List<KeyValuePair<string, string>> externalHeaders;
        private readonly List<KeyValuePair<string, string>> values;
        private readonly Util util;
        private readonly IBankBranchServcie _bankBranchService;
        private readonly SimpleTransferApplicationDbContext _db;
        private readonly IExchangeRateServices exchangeRateServices;
        private readonly ICityService cityService;
        private readonly ITransactionMgmtService transactionMgmtService;
        private readonly ICountryService countryService;
        private readonly ICustomerService customerService;
        private readonly ISourceOfFundSetupService sourceOfFundSetupService;
        private readonly IPaymentPurposeSetupService paymentPurposeSetupService;
        private readonly IMapper mapper;
        private readonly IWorkflowService workflowService;
        private readonly Utils utils;
        private readonly IFormService formService;
        private readonly ICaseService caseService;
        private readonly ILexisNexisService lexisNexisService;
        private readonly string baseApiUrl;

        public TransfastApiController(IUserService userService, IConfiguration config, IMapperService mapperService, IBankBranchServcie bankBranchService,
            SimpleTransferApplicationDbContext db, IExchangeRateServices exchangeRateServices, ICityService cityService, ITransactionMgmtService transactionMgmtService,
            ICountryService countryService, ICustomerService customerService, ISourceOfFundSetupService sourceOfFundSetupService, IPaymentPurposeSetupService paymentPurposeSetupService, IMapper mapper, IWorkflowService workflowService, Utils utils, IFormService formService, ICaseService caseService, ILexisNexisService lexisNexisService) : base(userService)
        {
            this.userService = userService;
            _config = config;
            _mapperService = mapperService;
            _bankBranchService = bankBranchService;
            _db = db;
            util = new Util();
            apiUrl = _config.GetSection("Transfast").Value;
            baseUrl = _config.GetSection("BaseApiUrl").Value;
            values = _config.GetSection("ExternalHeaders:Transfast").GetChildren().ToDictionary(x => x.Key, x => x.Value).ToList();
            var systemId = values.Where(x => x.Key == "SystemId").Select(x => x.Value).FirstOrDefault();
            var userName = util.EncryptTo(values.Where(x => x.Key == "Username").Select(x => x.Value).FirstOrDefault(), values);
            var password = util.EncryptTo(values.Where(x => x.Key == "Password").Select(x => x.Value).FirstOrDefault(), values);
            var branchId = util.EncryptTo(values.Where(x => x.Key == "BranchId").Select(x => x.Value).FirstOrDefault(), values);
            var authorization = "<Authentication><Id>" + systemId + "</Id><UserName>" + userName + "</UserName><Password>" + password + "</Password><BranchId>" + branchId + "</BranchId></Authentication>";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(authorization);
            var base64Auth = Convert.ToBase64String(plainTextBytes);
            externalHeaders = new List<KeyValuePair<string, string>>();
            externalHeaders.Add(new KeyValuePair<string, string>("Authorization", base64Auth));
            this.exchangeRateServices = exchangeRateServices;
            this.cityService = cityService;
            this.transactionMgmtService = transactionMgmtService;
            this.countryService = countryService;
            this.customerService = customerService;
            this.sourceOfFundSetupService = sourceOfFundSetupService;
            this.paymentPurposeSetupService = paymentPurposeSetupService;
            this.mapper = mapper;
            this.workflowService = workflowService;
            this.utils = utils;
            this.formService = formService;
            this.caseService = caseService;
            this.lexisNexisService = lexisNexisService;
            baseApiUrl = _config.GetSection("BaseApiUrl").Value;
        }

        [HttpGet]
        [Route("gettodayrates")]
        public async Task<IActionResult> GetTodayRates(string sourceCurrencyIsoCode, string receiveCountryIsoCode, string feeProduct = "")
        {
            try
            {
                string url = "/rates/countryrates?sourcecurrencyisocode=" + sourceCurrencyIsoCode + "&receivecountryisocode=" + receiveCountryIsoCode + "&feeProduct=" + feeProduct;
                var transfastRate = await WebApiService.InstanceForExternal.GetAsync<object>(url.CompleteUrl(apiUrl), true, externalHeaders);
                var transfastRates = transfastRate as JObject;
                var transfastRateList = transfastRates["Rates"].ToObject<List<TransfastRateModel>>();

                var datas = (from tr in transfastRateList
                             select new RateViewModel
                             {
                                 SourceCountry = "UK",
                                 DestinationCountry = tr.ReceiveCountryIsoCode,
                                 Currency = tr.ReceiveCurrencyIsoCode,
                                 Rate = Convert.ToDecimal(tr.StartRate),
                                 Bank = tr.PayerName,
                                 ModeOfPayment = GetModeOfPayment(tr.ModeOfPaymentId),
                                 PayerId = tr.PayerId
                             }).ToList();

                //var transfastExchangeRates = new List<decimal>();
                //foreach (var item in transfastRateList)
                //{
                //    transfastExchangeRates.Add(Convert.ToDecimal(item.StartRate));
                //}

                //var exchangeRate = transfastExchangeRates.OrderBy(x => x).FirstOrDefault();
                //var jo = new JObject();
                //jo.Add("Rate", exchangeRate);

                if (datas.Count > 0)
                    return Ok(datas);
                else
                    return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }

        private int GetModeOfPayment(string modeOfPayment)
        {
            if (modeOfPayment == "C")
            {
                return 1;
            }
            else if (modeOfPayment == "2")
            {
                return 2;
            }
            else if (modeOfPayment == "G")
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        [HttpGet]
        [Route("getbanks")]
        public async Task<IActionResult> GetBanks(string countryIsoCode)
        {
            try
            {
                string url = "/catalogs/banks?countryIsoCode=" + countryIsoCode;
                var transfastBanks = await WebApiService.InstanceForExternal.GetAsync<object>(url.CompleteUrl(apiUrl), true, externalHeaders);
                var transfastRates = transfastBanks as JObject;
                if (transfastRates != null)
                {
                    var transfastBankList = transfastRates["Banks"].ToObject<List<TransfastBankModel>>();

                    var datas = (from tr in transfastBankList
                                 select new TransfastBankModel
                                 {
                                     Id = tr.Id,
                                     Name = tr.Name
                                 }).ToList();
                    await _bankBranchService.CreateOrUpdateBank(countryIsoCode, RateSupplierEnum.Transfast, datas, null);

                    if (datas.Count > 0)
                        return Ok(datas);
                    else
                        return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
                }
                return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }
        [HttpGet]
        [Route("getbankbranches")]
        public async Task<IActionResult> GetBankBranches(string countryIsoCode)
        {
            try
            {
                var banks = _db.SupplierBank.Where(x => x.SupplierId == (int)RateSupplierEnum.Transfast && x.CountryCode.ToUpper() == countryIsoCode.ToUpper()).ToList();
                foreach (var bank in banks)
                {
                    var cities = await cityService.GetAllCity(countryIsoCode);
                    foreach (var city in cities)
                    {
                        string url = "/catalogs/BankBranch?BankId=" + bank.BankCode.ToUpper() + "&CityId=" + city.CityCode;
                        var BankBranches = await WebApiService.InstanceForExternal.GetAsync<object>(url.CompleteUrl(apiUrl), true, externalHeaders);
                        var transfastBankBranches = BankBranches as JArray;
                        List<TransfastBankBranchModel> transfastBankBranchList = transfastBankBranches.ToObject<List<TransfastBankBranchModel>>();
                        transfastBankBranchList.ForEach(x => x.CityCode = city.CityCode);
                        transfastBankBranchList.ForEach(x => x.BankBranchID = x.BankBranchID.Trim());
                        transfastBankBranchList.ForEach(x => x.BankBranchName = x.BankBranchName.Trim());
                        List<TransfastBankBranchModel> mapped = _mapperService.Map<List<TransfastBankBranchModel>>(_db.SupplierBankBranch
                            .Where(x => x.BankCode.Trim() == bank.BankCode.Trim() && x.CountryCode.ToUpper() == countryIsoCode.ToUpper()).ToList());
                        var newList = transfastBankBranchList.Where(x => !mapped.Any(y => y.BankBranchID.Trim() == x.BankBranchID.Trim()
                        && y.BankID.Trim() == x.BankID.Trim())).ToList();
                        foreach (var branch in newList)
                        {
                            //var exists = _db.SupplierBankBranch.Where(x => x.BranchCode.Trim() == branch.BankBranchID.Trim() && x.BankCode.Trim() == branch.BankID.Trim()).FirstOrDefault();
                            if (branch.BankBranchID.Trim() != "" && branch.BankBranchName.Trim() != "")
                            {
                                var data = new TransfastBankBranchModel()
                                {
                                    BankBranchID = branch.BankBranchID.Trim(),
                                    BankBranchName = branch.BankBranchName.Trim(),
                                    BankID = branch.BankID.Trim(),
                                    CityCode = city.CityCode.Trim()
                                };
                                _bankBranchService.CreateBankBranch(countryIsoCode, RateSupplierEnum.Transfast, data, null);
                            }
                        }

                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }
        [HttpGet]
        [Route("getcities")]
        public async Task<IActionResult> GetCities(string countryIsoCode)
        {
            try
            {
                string url = "/catalogs/cities?countryisocode=" + countryIsoCode;
                var cities = await WebApiService.InstanceForExternal.GetAsync<object>(url.CompleteUrl(apiUrl), true, externalHeaders);
                var transfastCities = cities as JArray;
                var jObject = JsonConvert.DeserializeObject<JObject>(cities.ToString());
                var jsondata = jObject["Cities"];
                var transfastCitiesList = jsondata.ToObject<List<TransfastCityModel>>();

                var data = (from tr in transfastCitiesList
                            select new SupplierCity
                            {
                                CityCode = tr.Id,
                                CountryCode = tr.CountryIsoCode,
                                StateId = tr.StateId,
                                StateName = tr.StateName,
                                CityName = tr.Name,
                                SupplierId = (int)RateSupplierEnum.Transfast,
                                CreatedBy = "System",
                                Status = true
                            }).ToList();
                await _bankBranchService.CreateOrUpdateCity(data, countryIsoCode);
                return Ok();
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }

        [HttpGet]
        [Route("settodayrate")]
        public IActionResult SetTodayRate(string sourceCurrencyIsoCode, string receiveCountryIsoCode, string feeProduct = "")
        {
            try
            {
                var transfastResult = GetTodayRates(sourceCurrencyIsoCode, receiveCountryIsoCode).Result as OkObjectResult;
                var transfastRateList = transfastResult.Value as List<RateViewModel>;

                var necExchangeRate = exchangeRateServices.ExchangeRates(transfastRateList, SchedulerList.Transfast.ToString());

                var necExchangeRateHistory = exchangeRateServices.ExchangeRatesHistory(transfastRateList, SchedulerList.Transfast.ToString());

                var result = exchangeRateServices.CreateOrUpdate(necExchangeRate).Result;
                result = exchangeRateServices.CreateOrUpdate(necExchangeRateHistory).Result;
                if (result)
                    return Ok(result);
                else
                    return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }

        //[HttpPost]
        [Route("createtransaction")]
        //public async Task<IActionResult> CreateTransaction(TransactionModel datas)
        [HttpGet]
        public async Task<IActionResult> CreateTransaction(string userId, string beneFirstName, string beneLastName, string beneMobilePhone, string beneCountry, string paymentModeId, string payoutModeId, decimal sentAmount, string referenceNo, string caseId, string bankId, string bankBranchId, string beneBankId, string beneBankBranchId, string purposeOfPayment, string sourceOfFund)
        {
            object response;
            try
            {
                string beneId = string.Empty;
                int id = utils.DecryptId(caseId);
                //var receiveCountryIsoCode = await countryService.GetCountryCurrencyCode(beneCountry);
                var result = await CreateTransac(userId, beneFirstName, beneLastName, beneMobilePhone, beneCountry, paymentModeId, payoutModeId, sentAmount, (int)TransactionType.Remittance, referenceNo, id, bankId, bankBranchId, beneId, purposeOfPayment, sourceOfFund);

                var dataList = await transactionMgmtService.GetCaseData(id);
                if (dataList.Count > 0)
                {
                    var toStateId = await transactionMgmtService.GetStateId("Credited");
                    var res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), dataList.LastOrDefault(), toStateId, id, baseApiUrl);
                    await UpdateCaseData(id, res, "transfer");

                    toStateId = await transactionMgmtService.GetStateId("Not Credited");
                    if (res == toStateId)
                    {
                        var results = await transactionMgmtService.BTTransaction(referenceNo);
                        var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.BTTrxNew);
                    }
                }
                if (result == "")
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Unsuccessfull", DataList = "", Data = "" };
                    return Ok(response);
                }
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = "" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = InternalServerError, Message = "Unsuccessfull", DataList = "", Data = "" };
                return Ok(response);
            }
            finally
            {
                //log request and response
            }
        }

        private async Task<bool> UpdateCaseData(int caseId, int res, string form)
        {
            try
            {
                var cvm = caseService.GetCaseById(caseId);
                cvm.StateId = res;
                cvm.UpdatedAt = DateTime.Now;
                var caseresult = await formService.SaveCaseAsync(cvm, form);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [Route("createpaymentrequesttransaction")]
        [HttpGet]
        public async Task<IActionResult> CreatePaymentRequestTransaction(string payerId, string payeeId, string paymentModeId, decimal sentAmount)
        {
            object response;
            try
            {
                var sender = await customerService.GetCustomerById(payerId);
                var receiver = await customerService.GetCustomerById(payeeId);
                var receiveCountryIsoCode = await countryService.GetCountryCurrencyCode(receiver.Country);
                var result = true; //create transaction for internal
                if (!result)
                {
                    response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
                    return Ok(response);
                }
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = "" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = InternalServerError, Message = "Unsuccessfull", DataList = "", Data = "" };
                return Ok(response);
            }
            finally
            {
                //log request and response
            }
        }

        private async Task<string> CreateTransac(string userId, string beneFirstName, string beneLastName, string beneMobilePhone, string beneCountry, string paymentMethodId, string payoutModeId, decimal sentAmount, int transactionType, string referenceNo, int caseId, string bankId, string bankBranchId, string beneId, string purposeOfPayment, string sourceOfFund)
        {
            try
            {
                string url = "/transaction/invoice";
                var datas = new TransactionModel();
                datas.Sender = new SenderModel();
                datas.Receiver = new ReceiverModel();
                datas.TransactionInfo = new TransactionInfoModel();
                datas.Compliance = new ComplianceModel();

                var sender = await customerService.GetCustomerById(userId);

                datas.Sender.Name = sender.FirstName + " " + sender.LastName;
                datas.Sender.Address = sender.AddressLine;
                datas.Sender.PhoneMobile = sender.MobileNumber;
                datas.Sender.CountryIsoCode = sender.Country;
                datas.Sender.TypeOfId = "PA";
                datas.Sender.IdNumber = "12345";
                datas.Sender.DateOfBirth = new DateTime(2002, 10, 18);
                datas.Sender.NationalityIsoCode = beneCountry;
                datas.Sender.IdExpiryDate = new DateTime(2022, 10, 18);

                datas.Receiver.FirstName = beneFirstName;
                datas.Receiver.LastName = beneLastName;
                datas.Receiver.MobilePhone = beneMobilePhone;
                datas.Receiver.CountryIsoCode = beneCountry;

                var payoutMode = (int)Enum.Parse(typeof(PayoutMode), payoutModeId.Replace(" ", ""));
                datas.TransactionInfo.PaymentModeId = "2";//payoutMode.ToString();
                if (beneCountry == "GB")
                {
                    datas.TransactionInfo.PaymentModeId = "C";
                    datas.TransactionInfo.PayerId = "AT01";
                    datas.TransactionInfo.PayingBranchId = "AT010035";
                    datas.TransactionInfo.BankBranchId = "AACCGB21";
                    datas.TransactionInfo.Account = "GB33BUKB20201555555555";
                    datas.Receiver.CityId = 19725;
                    datas.TransactionInfo.BankId = "UK1";
                }
                datas.TransactionInfo.ReceiveCurrencyIsoCode = await countryService.GetCountryCurrencyCode(beneCountry);
                datas.TransactionInfo.SourceCurrencyIsoCode = await countryService.GetCountryCurrencyCode(sender.Country);
                datas.TransactionInfo.SentAmount = sentAmount;
                datas.TransactionInfo.ReferenceNumber = referenceNo;//RandomString(10);
                datas.TransactionInfo.PurposeOfRemittanceId = "1";

                datas.Compliance.ReceiverDateOfBirth = new DateTime(2002, 10, 18);
                datas.Compliance.SenderDateOfBirth = new DateTime(2002, 10, 18);

                var transaction = await WebApiService.InstanceForExternal.PostAsyncTransfast<TransactionResponseModel>(url.CompleteUrl(apiUrl), true, externalHeaders, datas);
                var transactionData = new TransactionMgmtViewModel();
                transactionData.CaseId = caseId;
                transactionData.UserId = new Guid(userId);
                transactionData.SupplierId = 1;
                transactionData.SendAmount = transaction.TransactionInfo.SentAmount;
                transactionData.PayoutAmount = transaction.TransactionInfo.ReceiveAmount;
                transactionData.PaymentMethodId = Convert.ToInt32(paymentMethodId);
                transactionData.PayoutModeId = Convert.ToInt32(Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastApiPayoutMode>(transaction.TransactionInfo.PaymentModeId));
                //transactionData.StateId = Convert.ToInt32(transaction.Receiver.StateId);
                transactionData.TransferFee = transaction.TransactionInfo.ServiceFee;
                transactionData.ExchangeRate = transaction.TransactionInfo.Rate;
                transactionData.TransactionRefNo = transaction.TransactionInfo.ReferenceNumber;
                transactionData.SupplierTxnRefNo = transaction.TfPin;
                transactionData.SupplierTxnStatus = ((int)Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastTransactionStatus>(transaction.StatusId)).ToString();
                transactionData.Status = (int)SimpleTransferTransactionStatus.New;
                transactionData.SendCountryId = await transactionMgmtService.GetCountryId(transaction.Sender.CountryIsoCode);
                transactionData.PayoutCountryId = await transactionMgmtService.GetCountryId(transaction.Receiver.CountryIsoCode);
                transactionData.TrasactionType = transactionType;

                var result = await transactionMgmtService.CreateOrUpdate(transactionData);

                var transactionHistory = new TransactionHistoryViewModel();
                transactionHistory.TransactionDate = DateTime.Now;
                transactionHistory.TransactionRefNo = transaction.TransactionInfo.ReferenceNumber;
                transactionHistory.SupplierTxnRefNo = transaction.TfPin;
                transactionHistory.SupplierTxnStatus = ((int)Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastTransactionStatus>(transaction.StatusId)).ToString();
                transactionHistory.Status = (int)SimpleTransferTransactionStatus.New;
                var transactionHistoryResult = await transactionMgmtService.CreateOrUpdate(transactionHistory);

                return transaction.TransactionInfo.ReferenceNumber;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [HttpPut]
        [Route("releasetransaction")]
        public async Task<IActionResult> ReleaseTransaction(ReleaseCancelTransaction data)
        {
            object response;
            try
            {
                string url = "/transaction/release";
                var tfPinData = new { TfPin = data.TfPin };
                var transaction = await WebApiService.InstanceForExternal.PutAsyncTransfast<string>(url.CompleteUrl(apiUrl), true, externalHeaders, tfPinData);
                var result = await transactionMgmtService.ReleaseTransaction(data.TfPin);
                var transactionData = await transactionMgmtService.GetTransactionByTfPinAsync(data.TfPin);
                var transactionHistory = await transactionMgmtService.GetTransactionHistoryByTfPinAsync(data.TfPin);
                transactionHistory.TransactionHistoryId = 0;
                transactionHistory.SupplierTxnStatus = ((int)Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastTransactionStatus>("T")).ToString();
                transactionHistory.Status = (int)SimpleTransferTransactionStatus.Authorised;
                var thResult = await transactionMgmtService.CreateOrUpdate(transactionHistory);

                var form = transactionData.TrasactionType == 1 ? "transfer" : "jazzcash";
                var res = await transactionMgmtService.GetStateId("Authorized");
                await UpdateCaseData(data.CaseId, res, form);

                response = new { Success = true, StatusCode = 200, Message = "Release Successful", DataList = "", Data = "" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [HttpPut]
        [Route("canceltransaction")]
        public async Task<IActionResult> CancelTransaction(ReleaseCancelTransaction data)
        {
            object response;
            try
            {
                string url = "/transaction/cancel";
                var cancelData = new { TfPin = data.TfPin, ReasonId = data.ReasonId };
                var transaction = await WebApiService.InstanceForExternal.PutAsyncTransfast<string>(url.CompleteUrl(apiUrl), true, externalHeaders, cancelData);
                var result = await transactionMgmtService.CancelTransaction(data.TfPin);
                var transactionHistory = await transactionMgmtService.GetTransactionHistoryByTfPinAsync(data.TfPin);
                transactionHistory.TransactionHistoryId = 0;
                transactionHistory.SupplierTxnStatus = ((int)Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastTransactionStatus>("C")).ToString();
                transactionHistory.Status = (int)SimpleTransferTransactionStatus.Cancel;
                var thResult = await transactionMgmtService.CreateOrUpdate(transactionHistory);
                response = new { Success = true, StatusCode = 200, Message = "Cancel Successful", DataList = "", Data = "" };
                return Ok(response);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        #region Credit Transaction 
        //[HttpPut]
        //[Route("credittransaction")]
        //public async Task<IActionResult> CreditTransaction(CreditTransaction data)
        //{
        //    object response;
        //    try
        //    {
        //        var transaction = await transactionMgmtService.GetTransactionByReferenceNoAsync(data.ReferenceNo);
        //        var result = await transactionMgmtService.CreditTransaction(data.ReferenceNo);

        //        var thResult = UpdateTransactionHistory(data.ReferenceNo, (int)SimpleTransferTransactionStatus.Credited);

        //        if (result && thResult != null)
        //        {
        //            if (data.Type == "Bank")
        //            {
        //                await FCBeneficiary(transaction.UserId.ToString(), transaction.BeneficiaryId, data.ReferenceNo);
        //                await UpdateTransactionHistory(data.ReferenceNo, (int)SimpleTransferTransactionStatus.FraudCheck);
        //            }
        //            await KycCheck(transaction.UserId.ToString(), data.ReferenceNo);
        //        }
        //        response = new { Success = true, StatusCode = 200, Message = "Credited Successful", DataList = "", Data = "" };
        //        return Ok(response);

        //    }
        //    catch (Exception ex)
        //    {
        //        ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
        //        response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
        //        return Ok(response);
        //    }

        //}
        #endregion

        #region Transaction Apis
        [Route("credittransaction")]
        public async Task<IActionResult> CreditTransaction(string referenceNo, string sentAmount)
        {
            object response;
            try
            {
                var userId = await transactionMgmtService.GetUserId(referenceNo);
                //secure trading api 
                string url = baseUrl + "admin/securetrading/pushpayment?userId=" + userId + "&amount=" + sentAmount;
                var secureTradingResult = await WebApiService.InstanceForExternal.GetAsync<object>(url, false, "");
                var secureTradingValues = JObject.Parse(secureTradingResult.ToJson())["data"];

                var mapped = mapper.Map<SecureTradingPaymentDetail>(secureTradingValues);
                var requestReference = mapped.requestreference;

                //Transaction Status to Credited if secure trading success
                if (mapped.settlestatus == "0")
                {
                    var transaction = await transactionMgmtService.GetTransactionByReferenceNoAsync(referenceNo);
                    var result = await transactionMgmtService.CreditTransaction(referenceNo);

                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.TRXLive);
                    var saveRequestRefernceNo = await transactionMgmtService.SaveSecureTradingReference(referenceNo, requestReference);

                    response = new { Success = true, StatusCode = 200, Message = "Credited Successful", DataList = "", Data = "" };
                    return Ok(response);
                }
                else
                {
                    var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.PaymentFailure);
                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.PaymentFailure);
                    response = new { Success = false, StatusCode = 500, Message = "Payment Failure", DataList = "", Data = "" };
                    return Ok(response);
                }



            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [Route("fraudcheck")]
        public async Task<IActionResult> TransactionFraudCheck(string referenceNo, string beneficiaryId, string type)
        {
            object response;
            try
            {
                var userId = await transactionMgmtService.GetUserId(referenceNo);
                var checkFirstTransaction = await transactionMgmtService.CheckFirstTransaction(userId);
                if (checkFirstTransaction)
                {
                    var kycCheck = await KycCheck(userId, referenceNo);
                    if (kycCheck)
                    {
                        var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.FraudCheck);

                        var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.KYC);

                        thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.KYC);

                        response = new { Success = true, StatusCode = 200, Message = "KYC Check Successful", DataList = "", Data = "" };
                        return Ok(response);
                    }
                    else
                    {
                        if (type == "2")
                        {
                            response = new { Success = true, StatusCode = 200, Message = "Fraud Check Successful", DataList = "", Data = "" };
                            return Ok(response);
                        }
                        var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.KYCCheckFailed);

                        var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.KYCCheckFailed);

                        response = new { Success = false, StatusCode = 500, Message = "KYC Check Failure", DataList = "", Data = "" };
                        return Ok(response);
                    }
                }
                var transaction = await transactionMgmtService.GetTransactionByReferenceNoAsync(referenceNo);
                var secureTradingData = await transactionMgmtService.GetSecureTradingData(transaction.SecureTradingReferenceNo);
                var fcCardPayment = await FCCardPayment(secureTradingData.CardDetailId, referenceNo);
                var fcBene = await FCBeneficiary(userId, 0, referenceNo);
                if (fcCardPayment || fcBene)
                {
                    var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.FraudCheck);

                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.FraudCheck);

                    response = new { Success = true, StatusCode = 200, Message = "Fraud Check Successful", DataList = "", Data = "" };
                    return Ok(response);
                }
                else
                {
                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.FraudCheckFailed);
                    response = new { Success = false, StatusCode = 500, Message = "Fraud Check Failure", DataList = "", Data = "" };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [Route("kyccheck")]
        public async Task<IActionResult> TransactionKYCCheck(string referenceNo)
        {
            object response;
            try
            {
                var userId = await transactionMgmtService.GetUserId(referenceNo);
                var kycCheck = await KycCheck(userId, referenceNo);
                if (kycCheck)
                {
                    //var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.FraudCheck);

                    var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.KYC);

                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.KYC);

                    response = new { Success = true, StatusCode = 200, Message = "KYC Check Successful", DataList = "", Data = "" };
                    return Ok(response);
                }
                else
                {
                    var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.KYCCheckFailed);

                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.KYCCheckFailed);

                    response = new { Success = false, StatusCode = 500, Message = "KYC Check Failure", DataList = "", Data = "" };
                    throw new Exception();
                    //   return Ok(response);
                }

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [Route("rulescheck")]
        public async Task<IActionResult> TransactionRulesCheck(string referenceNo, string countryCode, decimal sendAmount)
        {
            object response;
            try
            {
                var rulesCheck = await RulesCheck(countryCode, sendAmount, referenceNo);
                if (rulesCheck)
                {
                    var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.RulesCheck);

                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.RulesCheck);

                    response = new { Success = true, StatusCode = 200, Message = "Rules Check Successful", DataList = "", Data = "" };
                    return Ok(response);
                }
                else
                {
                    var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.RulesCheckFailed);

                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.RulesCheckFailed);

                    response = new { Success = false, StatusCode = 500, Message = "Rules Check Failure", DataList = "", Data = "" };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [Route("sanctioncheck")]
        public async Task<IActionResult> TransactionSanctionCheck(string referenceNo)
        {
            object response;
            try
            {
                var userId = await transactionMgmtService.GetUserId(referenceNo);
                var sanctionCheck = await SanctionCheckBeneficiary(userId, referenceNo);
                if (sanctionCheck)
                {
                    sanctionCheck = await SanctionCheckCustomer(userId, referenceNo);
                }
                if (sanctionCheck)
                {
                    var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.Sanction);

                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.Sanction);

                    response = new { Success = true, StatusCode = 200, Message = "Sanction Check Successful", DataList = "", Data = "" };
                    return Ok(response);
                }
                else
                {
                    var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.SanctionCheckFailed);

                    var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.SanctionCheckFailed);

                    response = new { Success = false, StatusCode = 500, Message = "Sanction Check Failure", DataList = "", Data = "" };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [Route("releasetransaction")]
        public async Task<IActionResult> ReleaseTransaction(string referenceNo)
        {
            object response;
            try
            {
                var tfPin = await transactionMgmtService.GetTfPinByReferenceNo(referenceNo);
                string url = "/transaction/release";
                var tfPinData = new { TfPin = tfPin };
                var transaction = await WebApiService.InstanceForExternal.PutAsyncTransfast<string>(url.CompleteUrl(apiUrl), true, externalHeaders, tfPinData);
                var result = await transactionMgmtService.ReleaseTransaction(tfPin);
                var transactionHistory = await transactionMgmtService.GetTransactionHistoryByTfPinAsync(tfPin);
                transactionHistory.TransactionHistoryId = 0;
                transactionHistory.SupplierTxnStatus = ((int)Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastTransactionStatus>("T")).ToString();
                transactionHistory.Status = (int)SimpleTransferTransactionStatus.Authorised;
                var thResult = await transactionMgmtService.CreateOrUpdate(transactionHistory);
                response = new { Success = true, StatusCode = 200, Message = "Release Successful", DataList = "", Data = "" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [Route("confirmpaymentreceived")]
        public async Task<IActionResult> ConfirmPaymentReceived(string referenceNo)
        {
            object response;
            try
            {
                var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.TRXKY);

                var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.TRXKY);

                response = new { Success = true, StatusCode = 200, Message = "Payment Received Successful", DataList = "", Data = "" };
                return Ok(response);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [Route("initiatetransaction")]
        public async Task<IActionResult> InitiateTransaction(string referenceNo)
        {
            object response;
            try
            {
                var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.TRXLive);

                var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.TRXLive);

                response = new { Success = true, StatusCode = 200, Message = "Transaction Initiated Successful", DataList = "", Data = "" };
                return Ok(response);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [Route("declinetransaction")]
        [Route("declinetransaction.html")]
        public async Task<IActionResult> DeclineTransaction(int caseId, string referenceNo)
        {
            try
            {
                var transaction = await transactionMgmtService.GetTransactionByReferenceNoAsync(referenceNo);
                var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.Cancel);
                var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.Cancel);

                var form = transaction.TrasactionType == 1 ? "transfer" : "jazzcash";
                var res = await transactionMgmtService.GetStateId("Cancel");
                await UpdateCaseData(caseId, res, form);

                return Ok("success");
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, "Internal Server Error");
            }

        }

        [Route("kycfaileddeclinetransaction")]
        [Route("kycfaileddeclinetransaction.html")]
        public async Task<IActionResult> KYCFailedDeclineTransaction(int caseId, string referenceNo)
        {
            try
            {
                var transaction = await transactionMgmtService.GetTransactionByReferenceNoAsync(referenceNo);
                var result = await transactionMgmtService.UpdateTransactionStatusByReferenceNo(referenceNo, (int)SimpleTransferTransactionStatus.KYCCheckFailedDeclined);
                var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.KYCCheckFailedDeclined);

                var form = transaction.TrasactionType == 1 ? "transfer" : "jazzcash";
                var res = await transactionMgmtService.GetStateId("KYC Check Failed Declined");
                await UpdateCaseData(caseId, res, form);

                return Ok("success");
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, "Internal Server Error");
            }

        }

        #endregion

        private async Task<TransactionHistoryViewModel> UpdateTransactionHistory(string referenceNo, int status)
        {
            try
            {
                var transactionHistory = await transactionMgmtService.GetTransactionHistoryByReferenceNoAsync(referenceNo);
                transactionHistory.TransactionHistoryId = 0;
                transactionHistory.Status = status;
                var thResult = await transactionMgmtService.CreateOrUpdate(transactionHistory);
                return thResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #region Transaction Apis Functions

        private async Task<bool> FCCardPayment(int cardDetailId, string referenceNo)
        {
            try
            {
                var fcCardPayment = await transactionMgmtService.FCCardPayment(cardDetailId);
                return fcCardPayment;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> FCBeneficiary(string senderId, int beneId, string referenceNo)
        {
            try
            {
                var fcBene = await transactionMgmtService.FCBeneficiary(senderId, beneId);
                return fcBene;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> KycCheck(string senderId, string referenceNo)
        {
            try
            {
                var kycCheck = await transactionMgmtService.KycCheck(senderId);
                if (!kycCheck)
                {
                    var manualKycCheck = await transactionMgmtService.ManualKycCheck(senderId);
                    return manualKycCheck;
                }
                //if (kycCheck)
                //{
                //    await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.KYC);
                //}
                //else
                //{
                //    await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.ComplianceHold);
                //}
                return kycCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> RulesCheck(string countryCode, decimal sendAmount, string referenceNo)
        {
            try
            {
                var rulesCheck = false;
                var data = await transactionMgmtService.GetTransactionLimitConfigByCountryCodeAsync(countryCode);
                if (data != null && sendAmount <= data.LimitAmountPerTxn)
                {
                    rulesCheck = true;
                }

                return rulesCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> SanctionCheckCustomer(string userId, string referenceNo)
        {
            try
            {
                var sanctionCheck = false;
                sanctionCheck = await transactionMgmtService.SanctionCheckCustomer(userId);

                return sanctionCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> SanctionCheckBeneficiary(string beneId, string referenceNo)
        {
            try
            {
                var sanctionCheck = false;
                sanctionCheck = await transactionMgmtService.SanctionCheckBeneficiary(beneId);

                return sanctionCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        [HttpGet]
        [Route("transaction/bytfpin")]
        public async Task<IActionResult> GetTransactionByTfPin(string tfPin)
        {
            object response;
            try
            {
                string url = "/transaction/bytfpin?TfPin=" + tfPin;
                var transaction = await WebApiService.InstanceForExternal.GetAsync<GetTransactionModel>(url.CompleteUrl(apiUrl), true, externalHeaders);
                response = new { Success = true, StatusCode = 200, Message = "Get Successful", DataList = "", Data = transaction };
                return Ok(response);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("transaction/byreferenceno")]
        public async Task<IActionResult> GetTransactionByReferenceNo(string referenceNumber)
        {
            object response;
            try
            {
                string url = "/transaction/ByReferenceNo?ReferenceNumber=" + referenceNumber;
                var transaction = await WebApiService.InstanceForExternal.GetAsync<GetTransactionModel>(url.CompleteUrl(apiUrl), true, externalHeaders);
                response = new { Success = true, StatusCode = 200, Message = "Get Successful", DataList = "", Data = transaction };
                return Ok(response);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("transaction/status")]
        public async Task<IActionResult> TransactionStatus()
        {
            object response;
            try
            {
                var tfPinList = await transactionMgmtService.GetTfPins();
                var startDate = DateTime.Today.AddDays(-2);
                var endDate = DateTime.Now;
                string url = "/transaction/bytimeinterval?startDate=" + startDate + "&endDate=" + endDate;
                var transaction = await WebApiService.InstanceForExternal.GetAsync<TransactionList>(url.CompleteUrl(apiUrl), true, externalHeaders);
                foreach (var tfPin in tfPinList)
                {
                    var status = transaction.Results.Where(x => x.TfPin == tfPin).Select(x => x.InvoiceStatusId).FirstOrDefault();
                    var result = await transactionMgmtService.UpdateTransactionStatus(tfPin, status);
                    var transactionHistory = await transactionMgmtService.GetTransactionHistoryByTfPinAsync(tfPin);
                    transactionHistory.TransactionHistoryId = 0;
                    transactionHistory.SupplierTxnRefNo = status;
                    var thResult = await transactionMgmtService.CreateOrUpdate(transactionHistory);
                }

                response = new { Success = true, StatusCode = 200, Message = "Successful", DataList = "", Data = "" };
                return Ok(response);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                response = new { Success = false, StatusCode = InternalServerError, Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }
        }

        //private int RandomNumber(int min, int max)
        //{
        //    Random _random = new Random();  
        //    return _random.Next(min, max);
        //}

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
        [HttpGet]
        [Route("getsourceoffund")]
        public async Task<IActionResult> GetSourceOfFunds()
        {
            try
            {
                string url = "/catalogs/sourceoffunds";
                var transfastSourceOfFund = await WebApiService.InstanceForExternal.GetAsync<object>(url.CompleteUrl(apiUrl), true, externalHeaders);
                var transfastSourceOfFunds = transfastSourceOfFund as JObject;
                if (transfastSourceOfFunds != null)
                {
                    var transfastSOFList = transfastSourceOfFunds["SourcesOfFunds"].ToObject<List<TransfastBankModel>>();

                    var datas = (from tr in transfastSOFList
                                 select new TransfastBankModel
                                 {
                                     Id = tr.Id,
                                     Name = tr.Name
                                 }).ToList();
                    sourceOfFundSetupService.CreateOrUpdateTransfastSOF(datas);

                    if (datas.Count > 0)
                        return Ok(datas);
                    else
                        return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
                }
                return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }

        [Route("admin/customer/savecard")]
        [HttpGet]
        public async Task<IActionResult> SaveCardDetail(string userId, string cardType, string cardNumber, string expMonth, string csv, string expYear, string isSave, string beneFirstName, string beneLastName, string beneMobilePhone, string beneCountry, string paymentModeId, string payoutModeId, decimal sentAmount, string caseId, string referenceNo, string bankId, string bankBranchId, string beneBankId, string beneBankBranchId, string purposeOfPayment, string sourceOfFund)
        {
            object response;
            try
            {
                string beneId = string.Empty;
                if (isSave == "1")
                {
                    var customerDetail = new CustomerCardDetail()
                    {
                        CustomerId = userId,
                        UserId = userId,
                        Type = cardType,
                        Number = cardNumber,
                        ExpDate = expMonth + "/" + expYear,
                        Csv = csv,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    await customerService.SaveCustomerCardDetails(customerDetail);
                }

                int id = utils.DecryptId(caseId);
                var result = await CreateTransac(userId, beneFirstName, beneLastName, beneMobilePhone, beneCountry, paymentModeId, payoutModeId, sentAmount, (int)TransactionType.Remittance, referenceNo, id, bankId, bankBranchId, beneId, purposeOfPayment, sourceOfFund);

                //Transaction Status to Credited if secure trading success
                var dataList = await transactionMgmtService.GetCaseData(id);
                if (dataList.Count > 0)
                {
                    var toStateId = await transactionMgmtService.GetStateId("Credited");
                    var res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), dataList.LastOrDefault(), toStateId, id, baseApiUrl);
                    await UpdateCaseData(id, res, "transfer");

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("Fraud Check Completed");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "transfer");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("KYC Check Completed");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "transfer");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("Rules Check Completed");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "transfer");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("Sanction Check Completed");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "transfer");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("Authorized");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "transfer");
                    }
                }
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = "" };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getremittancepurposes")]
        public async Task<IActionResult> GetRemittancePurposes(string CountryIsoCode)
        {
            try
            {
                string url = "/catalogs/remittancepurposes?CountryIsoCode=" + CountryIsoCode;
                var transfastRemittancePurpose = await WebApiService.InstanceForExternal.GetAsync<object>(url.CompleteUrl(apiUrl), true, externalHeaders);
                var transfastRemittancePurposes = transfastRemittancePurpose as JObject;
                if (transfastRemittancePurposes != null)
                {
                    var transfastRemittancePurposeList = transfastRemittancePurposes["RemittancePurposes"].ToObject<List<TransfastBankModel>>();

                    var datas = (from tr in transfastRemittancePurposeList
                                 select new TransfastBankModel
                                 {
                                     Id = tr.Id,
                                     Name = tr.Name
                                 }).ToList();
                    paymentPurposeSetupService.CreateOrUpdateTransfastRemittancePurpose(datas, CountryIsoCode);

                    if (datas.Count > 0)
                        return Ok(datas);
                    else
                        return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
                }
                return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }

        private string GetTransfastStatus(string transfastStatus)
        {
            var status = string.Empty;
            switch (transfastStatus)
            {
                case "R":
                    status = TransfastTransactionStatus.PendingRelease.ToString();
                    break;
                case "W":
                    status = TransfastTransactionStatus.Web.ToString();
                    break;
                case "X":
                    status = TransfastTransactionStatus.Prestore.ToString();
                    break;
                case "I":
                    status = TransfastTransactionStatus.InProcess.ToString();
                    break;
                case "T":
                    status = TransfastTransactionStatus.Transmit.ToString();
                    break;
                case "H":
                    status = TransfastTransactionStatus.Hold.ToString();
                    break;
                case "C":
                    status = TransfastTransactionStatus.Cancel.ToString();
                    break;
                case "P":
                    status = TransfastTransactionStatus.Paid.ToString();
                    break;
                case "S":
                    status = TransfastTransactionStatus.Escrow.ToString();
                    break;
            }
            return status;
        }

        #region Payment Request
        [Route("admin/customer/pr/savecard")]
        [HttpGet]
        public async Task<IActionResult> PaymentRequestSaveCardDetail(string userId, string cardType, string cardNumber, string expMonth, string csv, string expYear, string isSave, string formId, string paymentModeId, string sentAmount, string referenceNo, string caseId, string purposeOfPayment, string sourceOfFund)
        {
            object response;
            try
            {
                int id = utils.DecryptId(caseId);
                if (isSave == "1")
                {
                    var customerDetail = new CustomerCardDetail()
                    {
                        CustomerId = userId,
                        UserId = userId,
                        Type = cardType,
                        Number = cardNumber,
                        ExpDate = expMonth + "/" + expYear,
                        Csv = csv,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    await customerService.SaveCustomerCardDetails(customerDetail);
                }

                var ruvm = new Service.Models.SimpleTransfer.User.RegisterUserViewModel();

                var sentAmountValue = Convert.ToDecimal(Regex.Match(sentAmount, @"\d+").Value);
                string payoutModeId = "2";
                var paymentRequestId = utils.DecryptId(formId);
                var payeeId = await transactionMgmtService.GetPayeeId(paymentRequestId);
                var paymentRequestData = await transactionMgmtService.GetPaymentRequestData(paymentRequestId);
                var payee = await customerService.GetCustomerById(payeeId);

                string bankId = string.Empty;
                string bankBranchId = string.Empty;
                if (paymentRequestData != null)
                {
                    bankId = paymentRequestData.STPaymentRequestDetails.Bank;
                    bankBranchId = paymentRequestData.STPaymentRequestDetails.Branch;
                }
                referenceNo = paymentRequestData.RequestId;
                //payee.Country = "BD";

                ruvm.Id = payeeId;
                ruvm.FirstName = payee.FirstName;
                ruvm.LastName = payee.LastName;
                ruvm.Address = payee.AddressLine;

                var ress = await SanctionPep(ruvm);

                var result = await CreateTransac(userId, payee.FirstName, payee.LastName, payee.MobileNumber, payee.Country, paymentModeId, payoutModeId, sentAmountValue, (int)TransactionType.PaymentRequest, referenceNo, id, bankId, bankBranchId, payeeId.ToString(), purposeOfPayment, sourceOfFund);

                var prResult = await transactionMgmtService.UpdateTransactionStatusByPaymentRequestId(paymentRequestId, (int)PaymentRequestStatus.PaymentInProgress);

                var dataList = await transactionMgmtService.GetCaseData(id);
                if (dataList.Count > 0)
                {
                    var toStateId = await transactionMgmtService.GetStateId("PR_Credited");
                    var res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), dataList.LastOrDefault(), toStateId, id, baseApiUrl);
                    await UpdateCaseData(id, res, "jazzcash");

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("PR_FraudCheckCompleted");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "jazzcash");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("PR_KYCCheckCompleted");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "jazzcash");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("PR_RulesCheckCompleted");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "jazzcash");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("PR_SanctionCheckCompleted");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "jazzcash");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await transactionMgmtService.GetStateId("PR_Authorized");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, id, baseApiUrl);
                        await UpdateCaseData(id, res, "jazzcash");
                    }
                }
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = "" };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }


        [Route("pr/createtransaction")]
        [HttpGet]
        public async Task<IActionResult> CreateTransactionPaymentRequest(string userId, string formId, string paymentModeId, string sentAmount, string referenceNo, string caseId, string purposeOfPayment, string sourceOfFund)
        {
            object response;
            try
            {
                int id = utils.DecryptId(caseId);
                var sentAmountValue = Convert.ToDecimal(Regex.Match(sentAmount, @"\d+").Value);
                string payoutModeId = "2";
                var paymentRequestId = utils.DecryptId(formId);
                var payeeId = await transactionMgmtService.GetPayeeId(paymentRequestId);
                var paymentRequestData = await transactionMgmtService.GetPaymentRequestData(paymentRequestId);
                var payee = await customerService.GetCustomerById(payeeId);

                string bankId = string.Empty;
                string bankBranchId = string.Empty;
                if (paymentRequestData != null)
                {
                    bankId = paymentRequestData.STPaymentRequestDetails.Bank;
                    bankBranchId = paymentRequestData.STPaymentRequestDetails.Branch;
                }

                //payee.Country = "BD";
                var result = await CreateTransac(userId, payee.FirstName, payee.LastName, payee.MobileNumber, payee.Country, paymentModeId, payoutModeId, sentAmountValue, (int)TransactionType.PaymentRequest, referenceNo, id, bankId, bankBranchId, payeeId.ToString(), purposeOfPayment, sourceOfFund);
                if (result == "")
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Unsuccessfull", DataList = "", Data = "" };
                    return Ok(response);
                }

                var prResult = await transactionMgmtService.UpdateTransactionStatusByPaymentRequestId(paymentRequestId, (int)PaymentRequestStatus.PaymentInProgress);

                var dataList = await transactionMgmtService.GetCaseData(id);
                if (dataList.Count > 0)
                {
                    var toStateId = await transactionMgmtService.GetStateId("PR_Credited");
                    var res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), dataList.LastOrDefault(), toStateId, id, baseApiUrl);
                    await UpdateCaseData(id, res, "jazzcash");

                    toStateId = await transactionMgmtService.GetStateId("PR_NotCredited");
                    if (res == toStateId)
                    {
                        var results = await transactionMgmtService.BTTransaction(referenceNo);
                        var thResult = await UpdateTransactionHistory(referenceNo, (int)SimpleTransferTransactionStatus.BTTrxNew);
                    }
                }
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = "" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = InternalServerError, Message = "Unsuccessfull", DataList = "", Data = "" };
                return Ok(response);
            }
            finally
            {
                //log request and response
            }
        }

        [Route("generatereferenceno")]
        [HttpGet]
        public IActionResult GenerateReferenceNo()
        {
            object response;
            try
            {
                var referenceNo = RandomString(10);
                var data = new { referenceNo = referenceNo };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = data };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = InternalServerError, Message = "Unsuccessfull", DataList = "", Data = "" };
                return Ok(response);
            }
            finally
            {
                //log request and response
            }
        }
        #endregion


        public async Task<IActionResult> CheckBankTransferNewTransaction()
        {
            try
            {
                var transactions = await transactionMgmtService.GetBTTrxNew();
                foreach (var item in transactions)
                {
                    var result = canCancel(item.UpdatedDate);
                    if (result)
                    {
                        var res = await DeclineTransaction(item.CaseId, item.TransactionRefNo) as OkObjectResult;
                    }

                }
                return Ok(true);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        public static bool canCancel(DateTime start)
        {
            DateTime ending = start.AddHours(23).AddMinutes(59).AddSeconds(59);

            var n = DateTime.Compare(ending, DateTime.Now);
            if (n == -1)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        //Lexis Nexis
        [Route("sanctionpep")]
        public async Task<IActionResult> SanctionPep(Service.Models.SimpleTransfer.User.RegisterUserViewModel data)
        {
            try
            {
                var url = baseApiUrl + "api/lexisnexis/iduprocess";
                var datas = new Request();
                datas.Login = new LoginDetails();
                datas.IDU = new IDUDetails();
                datas.Person = new PersonDetails();
                datas.Services = new ServiceDetails();

                datas.IDU.Reference = RandomString(10);
                datas.IDU.Scorecard = "IDU default";

                datas.Person.forename = data.FirstName;
                datas.Person.surname = data.LastName;
                datas.Person.address1 = data.Address;

                datas.Services.sanction = true;

                var result = await Cicero.Service.Configuration.WebApiService.InstanceForExternal.PostAsyncTransfast<Result>(url, true, externalHeaders, datas);

                var lexisNexis = new LexixNexisViewModel();
                if (result != null)
                {
                    lexisNexis.UserId = data.Id;
                    lexisNexis.LexisNexisId = result.Summary.ID;
                    lexisNexis.Ikey = result.Summary.IKey;
                    lexisNexis.EquifaxUsername = result.Summary.equifaxUsername;
                    lexisNexis.Reference = result.Summary.Reference;
                    lexisNexis.ScoreCard = result.Summary.Scorecard;
                    lexisNexis.ResultText = result.Summary.ResultText;
                    lexisNexis.ProfileUrl = result.Summary.ProfileURL;
                    lexisNexis.Credits = result.Summary.Credits;
                    lexisNexis.UKLexIdField = result.Summary.UKLexId;

                    if (result.Sanction.Count() > 0)
                    {
                        var sanctionTypes = result.Sanction.Select(x => x.Type.ToLower()).ToList();
                        lexisNexis.SanctionMatch = sanctionTypes.Contains("sanction") ? true : false;
                        lexisNexis.PepMatch = sanctionTypes.Contains("pep") ? true : false;
                        var res = await lexisNexisService.CreateOrUpdate(lexisNexis);

                        var sanctionPepPersonData = new List<SanctionPepPersonViewModel>();
                        sanctionPepPersonData = result.Sanction.Select(x => new SanctionPepPersonViewModel
                        {
                            AddressesField = x.Addresses.Length > 0 ? x.Addresses.Select(y => y.Address1).FirstOrDefault() : "",
                            AliasesField = x.Aliases != null ? string.Join("-", x.Aliases) : "",
                            CountryField = x.Country,
                            DOBField = x.DOB != null ? string.Join("-", x.DOB) : "",
                            ExceptionsField = x.Exceptions != null ? string.Join("-", x.Exceptions) : "",
                            MatchScoreField = x.MatchScore,
                            NameField = x.Name,
                            PositionsField = x.Positions != null ? string.Join("-", x.Positions) : "",
                            RecencyField = x.Recency,
                            SourceField = x.Source,
                            TypeField = x.Type
                        }).ToList();

                        sanctionPepPersonData.ToList().ForEach(c => c.LexisNexisId = res.Id);
                        var ress = await lexisNexisService.Create(sanctionPepPersonData);

                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }
    }
}