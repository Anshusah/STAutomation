using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities.JazzCash;
using Cicero.Service.Services.JazzCash;
using JazzCash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NecMoneyServiceReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAFK;
using SimpleTransferAPI.Filters;
using SimpleTransferAPI.Model;
using SimpleTransferAPI.Service;
using Utilities;

namespace SimpleTransferAPI.Controllers
{
    [Route("api/jazzcash")]
    [ApiController]
    public class JazzCashApiController : ControllerBase
    {
        private TransactionAPISoapClient _transactionApiSoapClient = new TransactionAPISoapClient(TransactionAPISoapClient.EndpointConfiguration.TransactionAPISoap);
        private readonly IConfiguration _config;
        private readonly string _exchangeCompanyId;
        private readonly string _password;
        private readonly string _mode;
        private readonly IMapperService _mapperService;
        private readonly Login login;
        private readonly IPayerService payerService;
        private readonly IMapper mapper;

        public JazzCashApiController(IConfiguration config, IMapperService mapperService, IPayerService payerService, IMapper mapper)
        {
            _exchangeCompanyId = config.GetSection("ExternalHeaders:JazzCash-Moneta:ExchangeCompanyId").Value;
            _password = config.GetSection("ExternalHeaders:JazzCash-Moneta:Password").Value;
            _mode = config.GetSection("ExternalHeaders:JazzCash-Moneta:Mode").Value;
            this.payerService = payerService;
            this.mapper = mapper;
        }

        //For Jazzcash
        [HttpPost]
        [Route("paymentrequest")]
        public IActionResult PaymentRequest(PaymentRequestModel data)
        {
            try
            {
                var token = data.Token;
                //var validateToken = validateToken(token);
                //if (validateToken)
                var mapData = mapper.Map<PaymentRequest>(data);
                var callbackUrl = "http://localhost:30000/jazzcash/payer/onlinepayment?id=";

                var result = payerService.Create(mapData, callbackUrl).Result;
                if (result == null) return Ok("failed");

                return Ok("success");
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
        }


        //For User
        [HttpGet]
        [Route("getbank")]
        public IActionResult GetBanks()
        {
            try
            {
                var exchangeCompanyId = _exchangeCompanyId;
                var hash1 = Hash(_password);
                var hash2 = hash1 + _mode;
                var hashValue = Hash(hash2);
                var getBank = _transactionApiSoapClient.ListBanksAsync(exchangeCompanyId, hashValue, Convert.ToInt32(_mode), "").Result;
                var data = getBank.Body.ListBanksResult;

                return Ok(data);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
        }

        [HttpGet]
        [Route("balanceinquiry")]
        public IActionResult BalanceInquiry()
        {
            try
            {
                var exchangeCompanyId = _exchangeCompanyId;
                var hash1 = Hash(_password);
                var hash2 = hash1 + _exchangeCompanyId;
                var hashValue = Hash(hash2);
                var balanceInquiry = _transactionApiSoapClient.BalanceInquiryAsync(exchangeCompanyId, hashValue, _mode).Result;
                var data = balanceInquiry.Body.BalanceInquiryResult;

                return Ok(data);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
        }

        [HttpGet]
        [Route("fetchaccounttitle")]
        public IActionResult FetchAccountTitle(string accountNumber, string bankCode, string agentId, string amount)
        {
            try
            {
                var exchangeCompanyId = _exchangeCompanyId;
                var hash1 = Hash(_password);
                var hash2 = hash1 + accountNumber;
                var hashValue = Hash(hash2);
                var fetchAccountTitle = _transactionApiSoapClient.FetchAccountTitleAsync(exchangeCompanyId, hashValue, accountNumber, bankCode, agentId, amount).Result;
                var data = fetchAccountTitle.Body.FetchAccountTitleResult;

                return Ok(data);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
        }

        [HttpGet]
        [Route("expresstransaction")]
        public IActionResult ExpressTransaction(string batchId, string txnType, string recordUniqueId, string recordCheckSum)
        {
            try
            {
                _transactionApiSoapClient.Endpoint.Binding.SendTimeout = new TimeSpan(0, 10, 0);
                _transactionApiSoapClient.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 10, 0);
                _transactionApiSoapClient.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 10, 0);
                _transactionApiSoapClient.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                var exchangeCompanyId = _exchangeCompanyId;
                var hash1 = Hash(_password);
                var hash2 = hash1 + recordUniqueId;
                var hashValue = Hash(hash2);
                string txnRecord = "#" + recordUniqueId + "##MUHAMMAD##MUHAMMAD##1000##ABCD##00581630000049##4222-4559-5##abc karachiH.No 123 abc karachi##03001234567##abc@xyz.com##Beneficiary purpose##0991246666666666##H.No123 abc karachi P.E.C.H.S##03001234567##abc@xyz.com##brother##ASSADD##Hill ParkBranch##P.E.C.H.S, Shara-e-Faisal Branch P.E.C.H.S, Shara-e-Faisal Branch P.E.C.H.S#";
                var expressTransaction = _transactionApiSoapClient.ExpressTransactionAsync(exchangeCompanyId, hashValue, batchId, txnType, recordUniqueId, txnRecord, recordCheckSum, 0).Result;
                var data = expressTransaction.Body.ExpressTransactionResult;
                var failureReason = expressTransaction.Body.FailureReason;
                if (data)
                {
                    return Ok("Success");
                }
                return Ok(failureReason);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
        }

        [HttpGet]
        [Route("statusinquiry")]
        public IActionResult StatusInquiry(string recordUniqueId, string txnType, string agentId)
        {
            try
            {
                var exchangeCompanyId = _exchangeCompanyId;
                var hash1 = Hash(_password);
                var hash2 = hash1 + recordUniqueId;
                var hashValue = Hash(hash2);
                var statusInquiry = _transactionApiSoapClient.StatusInquiryAsync(exchangeCompanyId, hashValue, recordUniqueId, txnType, agentId).Result;
                var data = statusInquiry.Body.StatusInquiryResult;

                return Ok(data);

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
        }

        private string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}