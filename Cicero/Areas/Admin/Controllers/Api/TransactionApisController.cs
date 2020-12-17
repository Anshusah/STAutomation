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
    public class TransacitonApisController : ControllerBase
    {
        private readonly IListService listService;
        private readonly ITransactionMgmtService transactionMgmtService;

        public TransacitonApisController(IListService listService, ITransactionMgmtService transactionMgmtService)
        {
            this.listService = listService;
            this.transactionMgmtService = transactionMgmtService;
        }

        [Route("admin/customer/savetransaction")]
        [HttpGet]
        public async Task<IActionResult> SaveTransactionDetail(string userId, decimal exchangeRate, string bankId, string branchId, int beneficiaryId, decimal sendAmount, decimal payoutAmount, string countryCode, decimal transferFee, string type)
        {
            object response;
            try
            {
                var sendCountry = listService.GetSenderCountryList().FirstOrDefault();
                var sendCountryId = 0;
                if (sendCountry != null)
                {
                    sendCountryId = await transactionMgmtService.GetCountryId(sendCountry.Value); ;
                }

                var supplierId = 0;
                var countryId = 0;
                var bankIdValue = 0;
                var branchIdValue = 0;
                supplierId = await transactionMgmtService.GetSupplierId(type, countryCode, bankId);
                countryId = await transactionMgmtService.GetCountryId(countryCode);
                bankIdValue = await transactionMgmtService.GetBankId(bankId);
                branchIdValue = await transactionMgmtService.GetBranchId(branchId);
                var transactionDetail = new TransactionMgmtViewModel()
                {
                    UserId = new Guid(userId),
                    SupplierId = supplierId,
                    BankId = bankIdValue,
                    BankBranchId = branchIdValue,
                    BeneficiaryId = beneficiaryId,
                    SendAmount = sendAmount,
                    PayoutAmount = payoutAmount,
                    SendCountryId = sendCountryId,
                    PayoutCountryId = countryId,
                    TransferFee = transferFee,
                    ExchangeRate = exchangeRate,
                    TransactionBookingNo = new Random().Next().ToString()
                };
                await transactionMgmtService.CreateOrUpdate(transactionDetail);
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = "" };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }
    }
}
