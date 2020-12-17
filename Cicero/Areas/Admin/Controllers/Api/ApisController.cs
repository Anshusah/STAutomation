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
using Cicero.Data.Entities.JazzCash;
using Cicero.Data;
using Cicero.Service.Services.JazzCash;
using Cicero.Service.Services.API;
using System.Text;

namespace SimpleTransferAPI.Controllers
{

    [Route("st/api")]
    [ApiController]
    public class ApisController : ControllerBase
    {
        private readonly int InternalServerError = (int)System.Net.HttpStatusCode.InternalServerError;
        private readonly IConfiguration config;
        private readonly ICountryService _countryService;
        private readonly IGenderSetupService genderSetupService;
        private readonly IRelationshipSetupService relationshipSetupService;
        private readonly IPaymentPurposeSetupService paymentPurposeSetupService;
        private readonly IMaritalStatusSetupService maritalStatusSetupService;
        private readonly ITransactionLimitConfigService transactionLimitConfigService;
        private readonly ICommonService commonService;
        private readonly IPayerService payerService;
        private readonly IMapper mapper;
        private readonly ISimpleTransferService simpleTransferService;
        private readonly IJazzCashTransactionMgmtService jazzCashTransactionMgmtService;
        private readonly ICustomerService customerService;
        private readonly ITransactionMgmtService transactionMgmtService;
        private readonly Utils utils;
        private readonly IPaymentRequestService paymentRequestService;
        private readonly ISmsService smsService;
        private readonly IListService listService;

        public ApisController(IConfiguration config, ICountryService countryService, IGenderSetupService genderSetupService,
            IRelationshipSetupService relationshipSetupService, IPaymentPurposeSetupService paymentPurposeSetupService, IMaritalStatusSetupService maritalStatusSetupService,
           ISmsService smsService, IListService listService, ITransactionLimitConfigService transactionLimitConfigService, ICommonService commonService, IPayerService payerService, IMapper mapper, ISimpleTransferService simpleTransferService, IJazzCashTransactionMgmtService jazzCashTransactionMgmtService, ICustomerService customerService, ITransactionMgmtService transactionMgmtService, Utils utils, IPaymentRequestService paymentRequestService)
        {
            this.config = config;
            _countryService = countryService;
            this.maritalStatusSetupService = maritalStatusSetupService;
            this.genderSetupService = genderSetupService;
            this.relationshipSetupService = relationshipSetupService;
            this.transactionLimitConfigService = transactionLimitConfigService;
            this.commonService = commonService;
            this.payerService = payerService;
            this.mapper = mapper;
            this.simpleTransferService = simpleTransferService;
            this.jazzCashTransactionMgmtService = jazzCashTransactionMgmtService;
            this.customerService = customerService;
            this.transactionMgmtService = transactionMgmtService;
            this.utils = utils;
            this.paymentRequestService = paymentRequestService;
            this.listService = listService;
            this.paymentPurposeSetupService = paymentPurposeSetupService;
            this.smsService = smsService;
        }

        [HttpGet]
        [Route("getgender")]
        public IActionResult GetGender()
        {
            object response;
            try
            {
                var genderList = genderSetupService.GetGenderAsync(commonService.GetTenantIdByIdentifier("").ToString());
                genderList.Insert(0, new SelectListItem { Text = "Select Gender", Value = "", });
                object gender = new { gender = genderList.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = gender };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getrelationship")]
        public IActionResult GetRelationship()
        {
            object response;
            try
            {
                var relationshipList = relationshipSetupService.GetRelationshipAsync(commonService.GetTenantIdByIdentifier("").ToString());
                relationshipList.Insert(0, new SelectListItem { Text = "Select Relationship", Value = "" });
                object relationship = new { relationship = relationshipList.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = relationship };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getpaymentpurpose")]
        public IActionResult GetPaymentPurpose()
        {
            object response;
            try
            {
                var paymentPurposeList = paymentPurposeSetupService.GetPaymentPurposeAsync(commonService.GetTenantIdByIdentifier("").ToString());
                paymentPurposeList.Insert(0, new SelectListItem { Text = "Select Payment Purpose", Value = "" });
                object paymentPurpose = new { paymentPurpose = paymentPurposeList.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = paymentPurpose };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getmaritalstatus")]
        public IActionResult GetMaritalStatus()
        {
            object response;
            try
            {
                var maritalStatusList = maritalStatusSetupService.GetMaritalStatusAsync(commonService.GetTenantIdByIdentifier("").ToString());
                maritalStatusList.Insert(0, new SelectListItem { Text = "Select Marital Status", Value = "" });
                object maritalStatus = new { maritalStatus = maritalStatusList.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = maritalStatus };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("calculatetotalpay")]
        public IActionResult GetCalculateTotalPay(string number1, string transferFees, string paymentMethodType, string csb)
        {
            object response;
            try
            {
                var num1 = Convert.ToDecimal(number1);

                var limitDatas = transactionLimitConfigService.GetTransactionLimitConfigByCountryCodeAsync(csb).Result;
                if (limitDatas != null)
                {
                    if (num1 > limitDatas.LimitAmountPerTxn)
                    {
                        object totalPayData = new { totalpay = 0, paymentMethod = "", currencyCode = "", paymentMethodIcon = "" };
                        response = new { Success = false, StatusCode = 500, Message = "You have exceeded the maximum transaction amount.", DataList = "", Data = totalPayData };
                        return Ok(response);
                    }
                }

                var nums = Regex.Split(transferFees, @"\D+");

                decimal num2 = 0;
                if (nums.Length > 0)
                {
                    num2 = Convert.ToDecimal(nums[0]);
                }
                var currencyCode = _countryService.GetCountryCurrencyCode(csb).Result;
                var cal = num1 + num2;
                var totalToPay = string.Empty;
                if (cal > 0)
                {
                    totalToPay = Math.Round(cal, 2) + " " + "GBP";
                }
                var paymentMethod = string.Empty;
                paymentMethod = paymentMethodType == ((int)PaymentMethod.CardPayment).ToString() ? PaymentMethod.CardPayment.ToDescription().ToString() : PaymentMethod.BankTransfer.ToDescription().ToString();

                var paymentMethodIcon = string.Empty;
                paymentMethodIcon = paymentMethodType == ((int)PaymentMethod.CardPayment).ToString() ? "ri-visa-line" : "ri-bank-line";
                var paymentMeth = paymentMethodType == ((int)PaymentMethod.CardPayment).ToString() ? ((int)PaymentMethod.CardPayment).ToString() : ((int)PaymentMethod.BankTransfer).ToString();
                List<object> paymentMethodList = new List<object>();
                paymentMethodList.Add(new { value = paymentMeth });
                object totalPay = new { totalpay = totalToPay, paymentMethod = paymentMethod.ToJson(), currencyCode = currencyCode.ToJson(), paymentMethodIcon = paymentMethodIcon.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = totalPay, Target = paymentMethodList.ToJson() };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("sendotp")]
        public IActionResult SendOtp(string number,string countryCode)
        {
            object response;
            try
            {
                bool sms = smsService.SendOtp(number,countryCode);
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = sms.ToJson() };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getsourceoffund")]
        public IActionResult GetSourceOfFund()
        {
            object response;
            try
            {
                var sourceOfFundList = listService.GetActiveSourceOfFundList(commonService.GetTenantIdByIdentifier("").ToString());
                sourceOfFundList.Insert(0, new SelectListItem { Text = "Select Source Of Fund", Value = "" });
                object sourceOfFund = new { sourceOfFund = sourceOfFundList.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = sourceOfFund };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        #region JazzCash Apis
        [HttpPost]
        [Route("paymentrequest")]
        public IActionResult PaymentRequest(PaymentRequestModel data)
        {
            object response;
            try
            {
                Request.Headers.TryGetValue("token", out var traceValue);
                data.Token = traceValue;
                var token = data.Token;
                var validateToken = simpleTransferService.ValidateToken(token);
                if (!validateToken)
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Invalid User Token has been supplied.", DataList = "", Data = "" };
                    return Ok(response);
                }

                if (ModelState.IsValid)
                {
                    var mapData = mapper.Map<PaymentRequest>(data);
                    var callbackUrl = config.GetSection("BaseApiUrl").Value + "jazzcash/payer/onlinepayment?id=";

                    var result = payerService.Create(mapData, callbackUrl).Result;
                    if (result == null)
                    {
                        response = new { Success = false, StatusCode = InternalServerError, Message = "Request Fail", DataList = "", Data = "" };
                        return Ok(response);
                    }

                    var dataValue = new {PayerEmail = data.Email, PayeeAccountNumber = data.JazzCashAccountNumber, Amount = data.RequestAmount, PaymentReferenceNumber = result.PaymentReferenceNumber, CreatedDate = result.CreatedDate, DueDate = data.DueDate, Status = Enum.GetName(typeof(SimpleTransferTransactionStatus), result.Status) };
                    response = new { Success = true, StatusCode = 200, Message = "Request Successful", DataList = "", Data = dataValue };
                    return Ok(response);
                }
                else
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Fields Missing", DataList = "", Data = "" };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);

                var message = (ex.Message.Contains("Invalid User Token has been supplied.")) ? ex.Message : error.ErrorDescription;

                response = new { Success = false, StatusCode = InternalServerError, Message = message, DataList = "", Data = "" };
                return Ok(response);
            }
        }

        [Route("jazzcash/createpaymentrequesttransaction")]
        [HttpPost]
        public async Task<IActionResult> CreatePaymentRequestTransactionJazzCash(JazzCashCreateTransaction data)
        {
            object response;
            try
            {
                Request.Headers.TryGetValue("token", out var token);
                var validateToken = simpleTransferService.ValidateToken(token);
                if (!validateToken)
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Invalid User Token has been supplied.", DataList = "", Data = "" };
                    return Ok(response);
                }

                if (ModelState.IsValid)
                {
                    var paymentRequestData = await payerService.GetPaymentRequestData(data.PaymentReferenceNumber);
                    var receiveCountryIsoCode = await _countryService.GetCountryCurrencyCode(data.PayeeCountryCode);
                    var result = await CreateTransac(data.PayerId, data.PayeeFirstName, data.PayeeLastName, data.PayeeMobileNumber, data.PayeeCountryCode, data.PaymentModeId, receiveCountryIsoCode, data.SentAmount, (int)TransactionType.Remittance, paymentRequestData.Id);
                    if (!result)
                    {
                        response = new { Success = false, StatusCode = InternalServerError, Message = "Transaction Create Fail", DataList = "", Data = "" };
                        return Ok(response);
                    }

                    var dataValue = new { PayerEmail = paymentRequestData.Email, PayeeAccountNumber = paymentRequestData.JazzCashAccountNumber, Amount = data.SentAmount, PaymentReferenceNumber = data.PaymentReferenceNumber, PaymentMode = data.PaymentModeId, CreatedDate = paymentRequestData.CreatedDate, DueDate = paymentRequestData.DueDate, Status = Enum.GetName(typeof(SimpleTransferTransactionStatus), (int)SimpleTransferTransactionStatus.Authorised) };
                    response = new { Success = true, StatusCode = 200, Message = "Transaction Create Success", DataList = "", Data = dataValue };
                    return Ok(response);
                }
                else
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Fields Missing", DataList = "", Data = "" };
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                var statusCode = StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);

                var message = (ex.Message.Contains("Invalid User Token has been supplied.")) ? ex.Message : error.ErrorDescription;

                response = new { Success = false, StatusCode = InternalServerError, Message = message, DataList = "", Data = "" };
                return Ok(response);
            }
            finally
            {
                //log request and response
            }
        }

        private async Task<bool> CreateTransac(string userId, string beneFirstName, string beneLastName, string beneMobilePhone, string beneCountry, int paymentModeId, string receiveCountryIsoCode, decimal sentAmount, int transactionType, int paymentRequestId)
        {
            try
            {
                var sender = await customerService.GetCustomerById(userId);

                var transactionData = new JazzCashTransactionMgmtViewModel();
                transactionData.PaymentRequestId = paymentRequestId;
                transactionData.UserId = new Guid(userId);
                transactionData.SupplierId = 1;
                transactionData.SendAmount = sentAmount;
                transactionData.PayoutAmount = sentAmount * (new Random().Next(2, 4));
                transactionData.PaymentMethodId = paymentModeId;
                //transactionData.StateId = Convert.ToInt32(transaction.Receiver.StateId);
                transactionData.TransferFee = Math.Round(Convert.ToDecimal(new Random().Next(1, 200)));
                transactionData.ExchangeRate = Math.Round(Convert.ToDecimal(new Random().Next(1, 200)));
             //   transactionData.TransactionRefNo = RandomString(10);
                transactionData.SupplierTxnRefNo = RandomString(10);
                transactionData.SupplierTxnStatus = "I";
                transactionData.Status = (int)SimpleTransferTransactionStatus.Authorised;
                transactionData.SendCountryId = await transactionMgmtService.GetCountryId(sender.Country);
                transactionData.PayoutCountryId = await transactionMgmtService.GetCountryId(beneCountry);


                var result = await jazzCashTransactionMgmtService.CreateOrUpdate(transactionData);

                var transactionHistory = new JazzCashTransactionHistoryViewModel();
                transactionHistory.TransactionDate = DateTime.Now;
                transactionHistory.TransactionRefNo = result.TransactionRefNo;
                transactionHistory.SupplierTxnRefNo = transactionData.SupplierTxnRefNo;
                transactionHistory.SupplierTxnStatus = "I";
                transactionHistory.Status = (int)SimpleTransferTransactionStatus.Authorised;
                var transactionHistoryResult = await jazzCashTransactionMgmtService.CreateOrUpdate(transactionHistory);

                return true;
            }
            catch (Exception ex)
            {
                return false;
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

        [HttpPut]
        [Route("jazzcash/canceltransaction")]
        public async Task<IActionResult> CancelTransaction(JazzCashCancelTransaction data)
        {
            object response;
            try
            {
                Request.Headers.TryGetValue("token", out var token);
                var validateToken = simpleTransferService.ValidateToken(token);
                if (!validateToken)
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Invalid User Token has been supplied.", DataList = "", Data = "" };
                    return Ok(response);
                }

                if (ModelState.IsValid)
                {
                    var result = await jazzCashTransactionMgmtService.CancelTransaction(data.ReferenceNo);
                    if (result == null)
                    {
                        response = new { Success = false, StatusCode = InternalServerError, Message = "Transaction Cancel Fail", DataList = "", Data = "" };
                        return Ok(response);
                    }
                    var transactionHistory = await jazzCashTransactionMgmtService.GetJazzCashTransactionHistoryByReferenceNoAsync(data.ReferenceNo);
                    if (transactionHistory != null)
                    {
                        transactionHistory.JazzCashTransactionHistoryId = 0;
                        transactionHistory.SupplierTxnRefNo = "C";
                        transactionHistory.Status = (int)SimpleTransferTransactionStatus.Cancel;
                        var thResult = await jazzCashTransactionMgmtService.CreateOrUpdate(transactionHistory);
                    }

                    var paymentRequestData = await payerService.GetPaymentRequestData(data.ReferenceNo);
                    var dataValue = new { PayerEmail = paymentRequestData.Email, PayeeAccountNumber = paymentRequestData.JazzCashAccountNumber, Amount = paymentRequestData.RequestAmount, PaymentReferenceNumber = data.ReferenceNo, PaymentMode = result.PaymentMethodId, CreatedDate = paymentRequestData.CreatedDate, DueDate = paymentRequestData.DueDate, Status = Enum.GetName(typeof(SimpleTransferTransactionStatus), (int)SimpleTransferTransactionStatus.Cancel) };
                    response = new { Success = true, StatusCode = 200, Message = "Transaction Cancel Success", DataList = "", Data = dataValue };
                    return Ok(response);
                }
                else
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Fields Missing", DataList = "", Data = "" };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                var statusCode = StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);

                var message = (ex.Message.Contains("Invalid User Token has been supplied.")) ? ex.Message : error.ErrorDescription;

                response = new { Success = false, StatusCode = InternalServerError, Message = message, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [HttpGet]
        [Route("jazzcash/gettransactionstatusbyaccountnumber")]
        public async Task<IActionResult> GetTransactionStatusByAccountNumber(string accountNumber)
        {
            object response;
            try
            {
                Request.Headers.TryGetValue("token", out var token);
                var validateToken = simpleTransferService.ValidateToken(token);
                if (!validateToken)
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Invalid User Token has been supplied.", DataList = "", Data = "" };
                    return Ok(response);
                }

                var datas = await jazzCashTransactionMgmtService.GetPaymentRequestByAccountNumber(accountNumber);
                var transactionDatas = new List<JazzCashTransactionStatus>();
                foreach (var item in datas)
                {
                    var transaction = await jazzCashTransactionMgmtService.GetJazzCashTransactionMgmtByPaymentRequestIdAsync(item.Id);
                    if (transaction == null)
                    {
                        continue;
                    }
                    transactionDatas.Add(new JazzCashTransactionStatus
                    {
                        AccounNumber = item.JazzCashAccountNumber,
                        RequestId = item.RequestId,
                        Date = item.CreatedDate,
                        Status = Enum.GetName(typeof(SimpleTransferTransactionStatus), transaction.Status)
                    });
                }
                response = new { Success = true, StatusCode = 200, Message = "Get Data Success", DataList = transactionDatas, Data = "" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                var statusCode = StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);

                var message = (ex.Message.Contains("Invalid User Token has been supplied.")) ? ex.Message : error.ErrorDescription;

                response = new { Success = false, StatusCode = InternalServerError, Message = message, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [HttpGet]
        [Route("jazzcash/gettransactionstatusbyrequestid")]
        public async Task<IActionResult> GetTransactionStatusByRequestId(string requestId)
        {
            object response;
            try
            {
                Request.Headers.TryGetValue("token", out var token);
                var validateToken = simpleTransferService.ValidateToken(token);
                if (!validateToken)
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Invalid User Token has been supplied.", DataList = "", Data = "" };
                    return Ok(response);
                }

                var data = await jazzCashTransactionMgmtService.GetPaymentRequestByRequestId(requestId);
                var transactionData = new JazzCashTransactionStatus();
                var transaction = await jazzCashTransactionMgmtService.GetJazzCashTransactionMgmtByPaymentRequestIdAsync(data.Id);

                transactionData.AccounNumber = data.JazzCashAccountNumber;
                transactionData.RequestId = data.RequestId;
                transactionData.Date = data.CreatedDate;
                if (transaction != null)
                {
                    transactionData.Status =  Enum.GetName(typeof(SimpleTransferTransactionStatus), transaction.Status);
                }

                response = new { Success = true, StatusCode = 200, Message = "Get Data Success", DataList = "", Data = transactionData };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                var statusCode = StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);

                var message = (ex.Message.Contains("Invalid User Token has been supplied.")) ? ex.Message : error.ErrorDescription;

                response = new { Success = false, StatusCode = InternalServerError, Message = message, DataList = "", Data = "" };
                return Ok(response);
            }

        }

        [HttpGet]
        [Route("jazzcash/gettransactionstatusbydates")]
        public async Task<IActionResult> GetTransactionStatusByDates(DateTime start, DateTime end)
        {
            object response;
            try
            {
                Request.Headers.TryGetValue("token", out var token);
                var validateToken = simpleTransferService.ValidateToken(token);
                if (!validateToken)
                {
                    response = new { Success = false, StatusCode = InternalServerError, Message = "Invalid User Token has been supplied.", DataList = "", Data = "" };
                    return Ok(response);
                }

                var datas = await jazzCashTransactionMgmtService.GetPaymentRequestByDates(start, end);
                var transactionDatas = new List<JazzCashTransactionStatus>();
                foreach (var item in datas)
                {
                    var transaction = await jazzCashTransactionMgmtService.GetJazzCashTransactionMgmtByPaymentRequestIdAsync(item.Id);
                    if (transaction == null)
                    {
                        continue;
                    }
                    transactionDatas.Add(new JazzCashTransactionStatus
                    {
                        AccounNumber = item.JazzCashAccountNumber,
                        RequestId = item.RequestId,
                        Date = item.CreatedDate,
                        Status = Enum.GetName(typeof(SimpleTransferTransactionStatus), transaction.Status)
                    });
                }
                response = new { Success = true, StatusCode = 200, Message = "Get Data Success", DataList = transactionDatas, Data = "" };
                return Ok(response);
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                var statusCode = StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);

                var message = (ex.Message.Contains("Invalid User Token has been supplied.")) ? ex.Message : error.ErrorDescription;

                response = new { Success = false, StatusCode = InternalServerError, Message = message, DataList = "", Data = "" };
                return Ok(response);
            }

        }
        #endregion

        [HttpGet]
        [Route("changepaymentmethod")]
        public IActionResult ChangePaymentMethod(string paymentMethod)
        {
            object response;
            try
            {
                paymentMethod = (paymentMethod == "1") ? "2" : "1";
                object paymentMethodValue = new { paymentMethod = paymentMethod };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = paymentMethodValue };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("paymentrequestdata")]
        public IActionResult PaymentRequestData(string formId)
        {
            object response;
            try
            {
                int id = utils.DecryptId(formId);
                var data = paymentRequestService.GetPaymentRequestData(id).Result;
                object paymentRequestData = new { requestAmount = data.RequestAmount + " GBP", payeeName = data.PayeeName, requestId = data.RequestId, fees = 0.00 + " GBP", recipientGets = data.RequestAmount + " GBP", totalPayment =  (data.RequestAmount + 0) + " GBP", reason = Cicero.Service.Extensions.Extensions.EnumModel<PurposeOfRequest>.GetDescription(data.STPaymentRequestDetails.PurposeOfRequest), invoice = data.STPaymentRequestDetails.Invoice, dueDate = data.STPaymentRequestDetails.DueDate.ToString("yyyy/MM/dd"), totalAmount = data.RequestAmount, payeeId = data.PayeeId, payeeCountry = data.PayeeCountry};
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = paymentRequestData };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }
    }
}
