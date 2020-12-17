using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Models;
using Cicero.Service.Extensions;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Data;
using Cicero.Service.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Cicero.Data.Enumerations;
using Cicero.Service.Models.SimpleTransfer;
using cicero.service.models.simpletransfer.transaction;
using Cicero.Service.Models.SimpleTransfer.Transaction;
using Microsoft.Extensions.Configuration;
using static Cicero.Service.Extensions.Extensions;
using Cicero.Service.Models.SimpleTransfer.TransactionLimitConfig;
using Cicero.Service.Models.PaymentRequest;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ITransactionMgmtService
    {
        Task<PaymentRequestViewModel> GetPaymentRequestData(int id);
        Task<bool> UpdateTransactionStatusByPaymentRequestId(int paymentRequestId, int status);
        Task<List<TransactionMgmtViewModel>> GetBTTrxNew();
        Task<TransactionDetailsViewModel> GetTransactionDetails(int transactionId);
        Task<string> GetPayeeId(int paymentRequestId);
        Task<string> GetTfPinByReferenceNo(string referenceNo);
        Task<TransactionLimitConfigViewModel> GetTransactionLimitConfigByCountryCodeAsync(string countryCode);
        Task<bool> SaveSecureTradingReference(string referenceNo, string requestReference);
        Task<SecureTradingPaymentDetail> GetSecureTradingData(string requestReference);
        Task<List<int>> GetCaseData(int caseId);
        Task<int> GetStateId(string stateName);
        string GetBaseUrl();
        DTResponseModel GetTransactionMgmtListByFilter(DTPostModel model, string type, string senderCountry = "", string receiverCountry = "");
        Task<TransactionQueueModel> GetTransactionMgmtListQueue();
        Task<List<TransactionTimeStampViewModel>> GetTransactionTimeStampByReferenceNo(string referenceNo);
        Task<TransactionMgmtViewModel> GetTransactionMgmtByIdAsync(int id);
        Task<TransactionHistoryViewModel> GetTransactionHistoryByTfPinAsync(string tfPin);
        Task<TransactionMgmtViewModel> GetTransactionByTfPinAsync(string tfPin);
        Task<TransactionHistoryViewModel> GetTransactionHistoryByReferenceNoAsync(string referenceNo);
        Task<TransactionMgmtViewModel> GetTransactionByReferenceNoAsync(string referenceNo);
        Task<TransactionMgmtViewModel> CreateOrUpdate(TransactionMgmtViewModel model);
        Task<TransactionHistoryViewModel> CreateOrUpdate(TransactionHistoryViewModel model);
        Task<List<string>> GetTfPins();
        Task<string> GetUserId(string referenceNo);
        Task<bool> CheckFirstTransaction(string userId);
        Task<bool> UpdateTransactionStatusByReferenceNo(string referenceNo, int status);
        Task<bool> UpdateTransactionStatus(string tfPin, string status);
        Task<bool> ReleaseTransaction(string tfPin);
        Task<bool> CancelTransaction(string tfPin);
        Task<bool> CreditTransaction(string referenceNo);
        Task<bool> BTTransaction(string referenceNo);
        Task<bool> FCCardPayment(int cardDetailId);
        Task<bool> FCBeneficiary(string senderId, int beneId);
        Task<bool> KycCheck(string senderId);
        Task<bool> ManualKycCheck(string senderId);
        Task<bool> SanctionCheckCustomer(string userId);
        Task<bool> SanctionCheckBeneficiary(string userId);
        bool SanctionCheckBoth(string userId, string beneficiaryId);
        List<SelectListItem> GetGenderAsync(string tenantId);
        List<SelectListItem> GetGenderByCode(string genderCode);

        Task<bool> ActiveTransactionMgmtById(int id);
        Task<bool> InActiveTransactionMgmtById(int id);
        Task<bool> DeleteTransactionMgmtById(int id);
        bool CheckDuplicate(TransactionMgmtViewModel model);

        Task<int> GetSupplierId(string type, string countryCode, string bankCode);
        Task<int> GetCountryId(string countryCode);
        Task<int> GetBankId(string bankCode);
        Task<int> GetBranchId(string branchCode);
    }
    public class TransactionMgmtService : ITransactionMgmtService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ApplicationDbContext adb;
        private readonly ILogger<ITransactionMgmtService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly ApplicationDbContext applicationDb;
        private readonly IConfiguration config;
        private readonly ICustomerService customerService;
        private readonly ICountryService countryService;

        public TransactionMgmtService(SimpleTransferApplicationDbContext _db, ApplicationDbContext _adb, ILogger<ITransactionMgmtService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils, ApplicationDbContext applicationDbContext, IConfiguration config, ICustomerService customerService, ICountryService countryService)
        {
            db = _db;
            adb = _adb;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
            applicationDb = applicationDbContext;
            this.config = config;
            this.customerService = customerService;
            this.countryService = countryService;
        }

        public DTResponseModel GetTransactionMgmtListByFilter(DTPostModel model, string type, string senderCountry = "", string receiverCountry = "")
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
            bool sortDir = false;

            int totalResultsCount = 0;
            int filteredResultsCount = 0;
            int draw = 0;

            if (model != null)
            {
                searchBy = (model.search != null) ? model.search.value : null;
                take = model.length;
                skip = model.start;
                draw = model.draw;

                if (model.order != null)
                {
                    sortBy = model.columns[model.order[0].column].data;
                    sortDir = model.order[0].dir.ToLower() == "desc";
                }
            }

            //var transactionMgmtList = from c in db.SecureTradingPaymentDetail.Where(x => x.requesttypedescription == "AUTH" && x.isRefund != true)
            //                          select new
            //                          {
            //                              TradeDate = Convert.ToDateTime(c.transactionstartedtimestamp).ToString("MM/dd/yyyy"),
            //                              TransactionId = c.Id,
            //                              TransactionBookingNo = c.requestreference,
            //                              TransactionRefNo = c.requestreference,
            //                              TransferFee = c.baseamount,
            //                              SendAmount = (Convert.ToDecimal(c.baseamount) / 100),
            //                              SendCountryId = c.merchantcountryiso2a,
            //                              SourceOfFund = "",
            //                              StateId = "",
            //                              PayoutAmount = c.baseamount,
            //                              PayoutCountryId = "BD",
            //                              SupplierId = "",
            //                              CreatedDate = c.transactionstartedtimestamp,
            //                              SendCurrency = c.currencyiso3a,
            //                              SenderName = "",
            //                              ReceiverCurrency = "BDT",
            //                              PaymentStatus = c.settlestatus,
            //                              AuthorisationStatus = "",
            //                              ProviderStatus = "",
            //                              Onfido = "",
            //                              TradeTime = Convert.ToDateTime(c.transactionstartedtimestamp).ToString("HH:mm"),
            //                              Provider = "Transfast",
            //                              SenderAccount = "0000010101010101",
            //                              action = "",
            //                              requestreference = c.transactionreference
            //                          };

            IQueryable<TransactionDataViewModel> transactionMgmtList = Enumerable.Empty<TransactionDataViewModel>().AsQueryable();

            switch (type)
            {
                case "normal":
                    transactionMgmtList = GetDatas();
                    break;

                case "paymentRequest":
                    transactionMgmtList = GetPaymentRequestDatas();
                    break;

                case "today":
                    transactionMgmtList = GetTodaysDatas();
                    break;

                case "unsettled":
                    transactionMgmtList = GetUnsettledDatas();
                    break;

                case "unauthorised":
                    transactionMgmtList = GetUnauthorisedDatas();
                    break;

                case "payment":
                    transactionMgmtList = GetPaymentHeldDatas();
                    break;

                case "compliance":
                    transactionMgmtList = GetComplianceDatas();
                    break;

                case "card":
                    transactionMgmtList = GetCanceledDatas(1);
                    break;

                case "bank":
                    transactionMgmtList = GetCanceledDatas(2);
                    break;

                case "filter":
                    var senderCountryId = db.CountryList.Where(x => x.Code == senderCountry).Select(x => x.Id).FirstOrDefault();
                    var receiverCountryId = db.CountryList.Where(x => x.Code == receiverCountry).Select(x => x.Id).FirstOrDefault();
                    transactionMgmtList = GetFilterDatas(senderCountryId, receiverCountryId);
                    break;
            }

            if (!String.IsNullOrEmpty(searchBy))
            {
                var searchByLower = searchBy.ToLower();
                transactionMgmtList = transactionMgmtList.Where(o => NullToString(o.tradeDate).ToLower().Contains(searchByLower) || NullToString(o.sendAmount).ToString().ToLower().Contains(searchByLower) || NullToString(o.payoutAmount).ToString().ToLower().Contains(searchByLower) || NullToString(o.sendCurrency).ToLower().Contains(searchByLower) || NullToString(o.receiverCurrency).ToLower().Contains(searchByLower) || NullToString(o.senderAccount).ToLower().Contains(searchByLower) || NullToString(o.tradeTime).ToLower().Contains(searchByLower) || NullToString(o.createdDate).ToString().ToLower().Contains(searchByLower) || NullToString(o.transferFee).ToString().ToLower().Contains(searchByLower));
            }

            totalResultsCount = transactionMgmtList.Count();
            transactionMgmtList = transactionMgmtList.OrderBy(sortBy, sortDir).Skip(skip).Take(take);

            var list = transactionMgmtList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        static string NullToString(object Value)
        {
            return Value == null ? "" : Value.ToString();
        }

        public IQueryable<TransactionDataViewModel> GetDatas()
        {
            var datas = (from c in db.Transaction
                         where c.TrasactionType == (int)TransactionType.Remittance
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Beneficiary.Where(x => x.Id == c.BeneficiaryId).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Id == c.PayoutCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),//Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public IQueryable<TransactionDataViewModel> GetPaymentRequestDatas()
        {
            var datas = (from c in db.Transaction
                         join a in db.STPaymentRequest on c.TransactionRefNo equals a.RequestId
                         where c.TrasactionType == (int)TransactionType.PaymentRequest && c.CreatedDate.Date >= DateTime.Now.Date
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString())
                             .Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Customer.Where(x => x.UserId == a.PayeeId.ToString())
                             .Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Code == a.PayeeCountry).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public IQueryable<TransactionDataViewModel> GetTodaysDatas()
        {
            var datas = (from c in db.Transaction.Where(x => x.TrasactionType == (int)TransactionType.Remittance && x.CreatedDate.Date >= DateTime.Now.Date)
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Beneficiary.Where(x => x.Id == c.BeneficiaryId).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Id == c.PayoutCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public IQueryable<TransactionDataViewModel> GetUnsettledDatas()
        {
            var datas = (from c in db.Transaction.Where(x => x.Status == (int)SimpleTransferTransactionStatus.New)
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Beneficiary.Where(x => x.Id == c.BeneficiaryId).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Id == c.PayoutCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public IQueryable<TransactionDataViewModel> GetUnauthorisedDatas()
        {
            var datas = (from c in db.Transaction.Where(x => x.Status == (int)SimpleTransferTransactionStatus.TRXLive)
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Beneficiary.Where(x => x.Id == c.BeneficiaryId).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Id == c.PayoutCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public IQueryable<TransactionDataViewModel> GetComplianceDatas()
        {
            var datas = (from c in db.Transaction
                         .Where(x => x.Status == (int)SimpleTransferTransactionStatus.ComplianceHold || x.Status == (int)SimpleTransferTransactionStatus.KYCCheckFailed || x.Status == (int)SimpleTransferTransactionStatus.SanctionCheckFailed)
                             //join cus in db.Customer on c.UserId.ToString() equals cus.UserId
                             //where !cus.IsOnfidoVerify
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Beneficiary.Where(x => x.Id == c.BeneficiaryId).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Id == c.PayoutCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public IQueryable<TransactionDataViewModel> GetPaymentHeldDatas()
        {
            var datas = (from c in db.Transaction
                         .Where(x => x.Status == (int)SimpleTransferTransactionStatus.PaymentFailure)
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Beneficiary.Where(x => x.Id == c.BeneficiaryId).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Id == c.PayoutCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public IQueryable<TransactionDataViewModel> GetCanceledDatas(int type)
        {
            var datas = (from c in db.Transaction
                         .Where(x => x.Status == (int)SimpleTransferTransactionStatus.Cancel && x.PaymentMethodId == type)
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Beneficiary.Where(x => x.Id == c.BeneficiaryId).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Id == c.PayoutCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public IQueryable<TransactionDataViewModel> GetFilterDatas(int senderCountryId, int receiverCountryId)
        {
            IQueryable<Transaction> filterDatas = Enumerable.Empty<Transaction>().AsQueryable();
            if (senderCountryId != 0 && receiverCountryId != 0)
            {
                filterDatas = (from c in db.Transaction
                               where c.SendCountryId == senderCountryId && c.PayoutCountryId == receiverCountryId
                               select c);
            }
            else if (receiverCountryId != 0)
            {
                filterDatas = (from c in db.Transaction
                               where c.PayoutCountryId == receiverCountryId
                               select c);
            }
            else
            {
                filterDatas = (from c in db.Transaction
                               where c.SendCountryId == senderCountryId
                               select c);
            }
            var datas = (from c in filterDatas
                         select new TransactionDataViewModel
                         {
                             tradeDate = Convert.ToDateTime(c.CreatedDate).ToString("MM/dd/yyyy"),
                             transactionId = c.TransactionId,
                             caseId = c.CaseId,
                             transactionRefNo = c.TransactionRefNo,
                             transferFee = c.TransferFee,
                             sendAmount = c.SendAmount,//(Convert.ToDecimal(c.SendAmount) / 100),
                             sourceOfFund = c.SourceOfFund,
                             stateId = "",
                             payoutAmount = c.PayoutAmount,
                             supplierId = c.SupplierId,
                             createdDate = c.CreatedDate,
                             updatedDate = c.UpdatedDate,
                             sendCurrency = db.CountryList.Where(x => x.Id == c.SendCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             senderName = db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverName = db.Beneficiary.Where(x => x.Id == c.BeneficiaryId).Select(x => (x.FirstName + " " + ((x.MiddleName == null) ? "" : x.MiddleName) + " " + x.LastName)).FirstOrDefault(),
                             receiverCurrency = db.CountryList.Where(x => x.Id == c.PayoutCountryId).Select(x => x.CurrencyCode).FirstOrDefault(),
                             paymentStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), c.Status),
                             authorisationStatus = "",
                             providerStatus = Enum.GetName(typeof(TransfastTransactionStatus), Convert.ToInt32(c.SupplierTxnStatus)),
                             onfido = (db.Customer.Where(x => x.UserId == c.UserId.ToString()).Select(x => x.IsOnfidoVerify).FirstOrDefault()) ? "Completed" : "Pending",
                             ofac = "Pending",
                             blackList = "Yes",
                             rules = "",
                             sanctionPep = SanctionCheckBoth(c.UserId.ToString(), c.BeneficiaryId.ToString()).ToString(),
                             tradeTime = Convert.ToDateTime(c.CreatedDate).ToString("HH:mm"),
                             provider = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                             senderAccount = "0000010101010101",
                             action = "",
                             requestReference = c.TransactionRefNo,
                             requestReferenceTransfast = c.SupplierTxnRefNo,
                             paymentMethod = EnumModel<PaymentMethod>.GetDescription(c.PaymentMethodId),
                             type = c.TrasactionType,
                             transactionType = EnumModel<TransactionType>.GetDescription(c.TrasactionType)
                         });

            return datas;
        }

        public async Task<TransactionMgmtViewModel> CreateOrUpdate(TransactionMgmtViewModel model)
        {
            try
            {
                Transaction transactionMgmt = new Transaction();
                transactionMgmt = Mapper.Map<Transaction>(model);
                transactionMgmt.CreatedBy = model.CreatedBy;
                transactionMgmt.UpdatedBy = commonService.getLoggedInUserId();
                transactionMgmt.CreatedDate = model.CreatedDate;
                transactionMgmt.UpdatedDate = DateTime.Now;

                if (model.TransactionId == 0)
                {
                    model.CreatedBy = model.UpdatedBy;
                    model.CreatedDate = model.UpdatedDate;
                    db.Transaction.Add(transactionMgmt);
                    await db.SaveChangesAsync();

                    return Mapper.Map<TransactionMgmtViewModel>(transactionMgmt);
                }
                else
                {
                    var TransactionMgmtData = Mapper.Map<Transaction>(transactionMgmt);

                    db.Transaction.Attach(transactionMgmt).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }


                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TransactionQueueModel> GetTransactionMgmtListQueue()
        {
            var data = new TransactionQueueModel();
            data.TodaysPaymentRequest = await (from t in db.Transaction where t.TrasactionType == (int)TransactionType.PaymentRequest && t.CreatedDate.Date >= DateTime.Now.Date select t).CountAsync();
            data.ExpiredPaymentRequest = await (from t in db.Transaction where t.Status == (int)SimpleTransferTransactionStatus.Expired select t).CountAsync();
            data.TodaysTRX = await (from t in db.Transaction where t.TrasactionType == (int)TransactionType.Remittance && t.CreatedDate.Date >= DateTime.Now.Date select t).CountAsync();
            data.UnsettledTRX = await (from t in db.Transaction where t.Status == (int)SimpleTransferTransactionStatus.New select t).CountAsync();
            data.PaymentHeldTRX = await (from t in db.Transaction where t.Status == (int)SimpleTransferTransactionStatus.PaymentFailure select t).CountAsync();
            data.ComplianceHeldTRX = await (from t in db.Transaction join cus in db.Customer on t.UserId.ToString() equals cus.UserId where !cus.IsOnfidoVerify select t).CountAsync();
            data.UnauthorisedTRX = await (from t in db.Transaction where t.Status == (int)SimpleTransferTransactionStatus.TRXLive select t).CountAsync();
            data.RejectedTRXForLimitBreach = await (from t in db.Transaction where t.Status == (int)SimpleTransferTransactionStatus.RulesCheckFailed select t).CountAsync();
            data.CancelledTRXPendingRefundCardPayment = await (from t in db.Transaction where t.Status == (int)SimpleTransferTransactionStatus.Cancel && t.PaymentMethodId == 1 select t).CountAsync();
            data.CancelledTRXPendingRefundBankTransfer = await (from t in db.Transaction where t.Status == (int)SimpleTransferTransactionStatus.Cancel && t.PaymentMethodId == 2 select t).CountAsync();

            return data;
        }

        public async Task<TransactionMgmtViewModel> GetTransactionMgmtByIdAsync(int id)
        {
            var TransactionMgmtList = await (from c in db.Transaction
                                             where c.TransactionId == id
                                             select c).FirstOrDefaultAsync();

            return Mapper.Map<TransactionMgmtViewModel>(TransactionMgmtList);
        }

        public async Task<TransactionHistoryViewModel> GetTransactionHistoryByTfPinAsync(string tfPin)
        {
            var TransactionHistoryList = await (from h in db.TransactionHistory
                                                where h.SupplierTxnRefNo == tfPin
                                                select h).FirstOrDefaultAsync();

            return Mapper.Map<TransactionHistoryViewModel>(TransactionHistoryList);
        }

        public async Task<TransactionMgmtViewModel> GetTransactionByTfPinAsync(string tfPin)
        {
            var TransactionHistoryList = await (from h in db.Transaction
                                                where h.SupplierTxnRefNo == tfPin
                                                select h).FirstOrDefaultAsync();

            return Mapper.Map<TransactionMgmtViewModel>(TransactionHistoryList);
        }

        public async Task<TransactionHistoryViewModel> GetTransactionHistoryByReferenceNoAsync(string referenceNo)
        {
            var TransactionHistoryList = await (from h in db.TransactionHistory
                                                where h.TransactionRefNo == referenceNo
                                                select h).FirstOrDefaultAsync();

            return Mapper.Map<TransactionHistoryViewModel>(TransactionHistoryList);
        }

        public async Task<TransactionMgmtViewModel> GetTransactionByReferenceNoAsync(string referenceNo)
        {
            var transactionList = await (from t in db.Transaction
                                         where t.TransactionRefNo == referenceNo
                                         select t).FirstOrDefaultAsync();

            return Mapper.Map<TransactionMgmtViewModel>(transactionList);
        }

        public async Task<bool> ActiveTransactionMgmtById(int id)
        {
            var TransactionMgmt = await db.Gender.FindAsync(id);
            if (TransactionMgmt != null)
            {
                TransactionMgmt.Status = true;
                var result = db.Gender.Update(TransactionMgmt);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Gender changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/TransactionMgmt/" + utils.EncryptId(TransactionMgmt.Id) + "/edit.html'>" + TransactionMgmt.Name + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("GenderService - ActiveGenderById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveTransactionMgmtById(int id)
        {
            var TransactionMgmt = await db.Gender.FindAsync(id);
            if (TransactionMgmt != null)
            {
                TransactionMgmt.Status = false;
                var result = db.Gender.Update(TransactionMgmt);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Gender changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/TransactionMgmt/" + utils.EncryptId(TransactionMgmt.Id) + "/edit.html'>" + TransactionMgmt.Name + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("GenderService - ActiveGenderById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteTransactionMgmtById(int id)
        {
            var TransactionMgmt = await db.Gender.FindAsync(id);
            if (TransactionMgmt != null)
            {
                db.Gender.Remove(TransactionMgmt);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary Gender deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/TransactionMgmt/" + utils.EncryptId(TransactionMgmt.Id) + "/edit.html'>" + TransactionMgmt.Name + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("GenderService - ActiveGenderById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetGenderAsync(string tenantId)
        {
            return db.Gender.Where(x => x.Status)
                .Select(x => new SelectListItem() { Text = x.Name, Value = x.Code.ToString() }).ToList();
        }
        public List<SelectListItem> GetGenderByCode(string genderCode)
        {
            var gender = db.Gender.Where(x => x.Status && x.Code.ToUpper() != genderCode.ToUpper()).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Code
            }).ToList();
            var genderSelect = db.Gender.Where(x => x.Status && x.Code.ToUpper() == genderCode.ToUpper()).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Code
            }).ToList();
            gender.AddRange(genderSelect);
            return gender;
        }
        public bool CheckDuplicate(TransactionMgmtViewModel model)
        {
            //if (db.Gender.Where(x => x.Code.ToUpper() == model.Code.ToUpper()&&x.Id!=model.Id).Any())
            //    return false;
            //else
            return true;
        }

        public async Task<int> GetSupplierId(string type, string countryCode, string bankCode)
        {
            int supplierId = 0;
            var exchangeRates = await db.ExchangeRates.Where(x => x.ToCountryCode == countryCode && x.BankCode != "" && x.ModeOfPayment == 1).OrderBy(x => x.ExchangeRate).GroupBy(x => x.ExchangeRate).FirstOrDefaultAsync();

            if ((int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", "")) == 1)
            {
                var source = exchangeRates.Select(x => x.Source).FirstOrDefault();
                supplierId = (int)Enum.Parse(typeof(RateSupplierEnum), source);
                return supplierId;
            }

            var bankMapperDatas = await db.SupplierBankMap.Where(x => x.NecMoneyBankCode == bankCode || x.TransfastBankCode == bankCode).FirstOrDefaultAsync();
            var exchangeRatesData = await db.ExchangeRates.Where(x => x.ToCountryCode == countryCode && (x.BankCode == bankMapperDatas.NecMoneyBankCode || x.BankCode == bankMapperDatas.TransfastBankCode)).OrderBy(x => x.ExchangeRate).FirstOrDefaultAsync();

            if (exchangeRatesData != null)
            {
                supplierId = (int)Enum.Parse(typeof(Cicero.Service.SimpleTransferEnums.RateSupplier), exchangeRatesData.Source);
            }
            return supplierId;
        }

        public async Task<int> GetCountryId(string countryCode)
        {
            var countryId = await db.CountryList.Where(x => x.IsActive && x.Code == countryCode)
               .Select(x => x.Id).FirstOrDefaultAsync();

            return countryId;
        }

        public async Task<int> GetBankId(string bankCode)
        {
            var bankId = await db.SupplierBank.Where(x => x.Status && x.BankCode == bankCode)
               .Select(x => x.Id).FirstOrDefaultAsync();

            return bankId;
        }

        public async Task<int> GetBranchId(string branchCode)
        {
            var branchId = await db.SupplierBankBranch.Where(x => x.Status && x.BranchCode == branchCode)
              .Select(x => x.Id).FirstOrDefaultAsync();

            return branchId;
        }

        public async Task<bool> UpdateTransactionStatus(string tfPin, string status)
        {
            try
            {
                var transaction = await db.Transaction.Where(x => x.SupplierTxnRefNo == tfPin).FirstOrDefaultAsync();
                transaction.SupplierTxnStatus = ((int)Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastTransactionStatus>(status)).ToString();
                db.Transaction.Attach(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckFirstTransaction(string userId)
        {
            try
            {
                var transactionList = await db.Transaction.Where(x => x.UserId == new Guid(userId)).ToListAsync();
                if (transactionList.Count == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTransactionStatusByReferenceNo(string referenceNo, int status)
        {
            try
            {
                var transaction = await db.Transaction.Where(x => x.TransactionRefNo == referenceNo).FirstOrDefaultAsync();
                transaction.Status = status;
                db.Transaction.Attach(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ReleaseTransaction(string tfPin)
        {
            try
            {
                var transaction = await db.Transaction.Where(x => x.SupplierTxnRefNo == tfPin).FirstOrDefaultAsync();
                transaction.SupplierTxnStatus = ((int)Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastTransactionStatus>("T")).ToString();
                transaction.Status = (int)SimpleTransferTransactionStatus.Authorised;
                db.Transaction.Attach(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CancelTransaction(string tfPin)
        {
            try
            {
                var transaction = await db.Transaction.Where(x => x.SupplierTxnRefNo == tfPin).FirstOrDefaultAsync();
                transaction.SupplierTxnStatus = ((int)Cicero.Service.Extensions.Extensions.GetValueFromDescription<TransfastTransactionStatus>("C")).ToString();
                transaction.Status = (int)SimpleTransferTransactionStatus.Cancel;
                db.Transaction.Attach(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreditTransaction(string referenceNo)
        {
            try
            {
                var transaction = await db.Transaction.Where(x => x.TransactionRefNo == referenceNo).FirstOrDefaultAsync();
                transaction.Status = (int)SimpleTransferTransactionStatus.TRXLive;
                db.Transaction.Attach(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> BTTransaction(string referenceNo)
        {
            try
            {
                var transaction = await db.Transaction.Where(x => x.TransactionRefNo == referenceNo).FirstOrDefaultAsync();
                transaction.Status = (int)SimpleTransferTransactionStatus.BTTrxNew;
                db.Transaction.Attach(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<TransactionHistoryViewModel> CreateOrUpdate(TransactionHistoryViewModel model)
        {
            try
            {
                TransactionHistory transactionHistory = new TransactionHistory();
                transactionHistory = Mapper.Map<TransactionHistory>(model);
                transactionHistory.CreatedBy = model.CreatedBy;
                transactionHistory.UpdatedBy = commonService.getLoggedInUserId();
                transactionHistory.TransactionDate = DateTime.Now;
                transactionHistory.CreatedDate = Convert.ToDateTime(model.CreatedDate);
                transactionHistory.UpdatedDate = DateTime.Now;

                if (model.TransactionHistoryId == 0)
                {
                    transactionHistory.CreatedBy = transactionHistory.UpdatedBy;
                    transactionHistory.TransactionDate = Convert.ToDateTime(transactionHistory.UpdatedDate);
                    transactionHistory.CreatedDate = Convert.ToDateTime(transactionHistory.UpdatedDate);
                    db.TransactionHistory.Add(transactionHistory);
                    await db.SaveChangesAsync();

                    return Mapper.Map<TransactionHistoryViewModel>(transactionHistory);
                }
                else
                {
                    var TransactionHistoryData = Mapper.Map<TransactionHistory>(transactionHistory);

                    db.TransactionHistory.Attach(TransactionHistoryData).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }


                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<string>> GetTfPins()
        {
            try
            {
                var statusList = new List<string> { "R", "C", "P" };
                var tfPins = await db.Transaction.Where(x => !statusList.Contains(x.SupplierTxnStatus)).Select(x => x.SupplierTxnRefNo).ToListAsync();
                return tfPins;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }

        }

        public async Task<List<TransactionTimeStampViewModel>> GetTransactionTimeStampByReferenceNo(string referenceNo)
        {
            var failureList = new List<string> { "2", "5", "8", "10", "11", "13", "15", "16", "17", "18" };
            var datas = await db.TransactionHistory.Where(x => x.TransactionRefNo == referenceNo).Select(x => new TransactionTimeStampViewModel
            {
                TransactionDate = x.TransactionDate,
                Date = x.TransactionDate.ToString("dd/MM/yyyy"),
                Time = x.TransactionDate.ToString("hh:mm"),
                Status = x.Status,
                Description = EnumModel<SimpleTransferTransactionManagementStatus>.GetDescription(x.Status),
                ClassName = (failureList.Contains(x.Status.ToString())) ? "failure" : "success"
            }).GroupBy(x => x.Status).Select(g => g.First()).OrderBy(x => x.TransactionDate).ToListAsync();

            return datas;
        }

        public string GetBaseUrl()
        {
            return config.GetSection("BaseApiUrl").Value;
        }

        //Fraud Check Card Payment
        public async Task<bool> FCCardPayment(int cardDetailId)
        {
            try
            {
                var sameCardPaymentCount = await (from st in db.SecureTradingPaymentDetail
                                                  where st.CardDetailId == cardDetailId && st.settlestatus == "0"
                                                  select st.requestreference).CountAsync();

                if (sameCardPaymentCount <= 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Fraud Check Beneficiary
        public async Task<bool> FCBeneficiary(string senderId, int beneId)
        {
            try
            {
                var sameBeneCount = await (from t in db.Transaction
                                           where t.UserId == new Guid(senderId) && t.BeneficiaryId == beneId
                                           select t.TransactionId).CountAsync();

                if (sameBeneCount <= 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> KycCheck(string senderId)
        {
            try
            {
                var data = await (from c in db.Customer
                                  where c.UserId == senderId
                                  select c).FirstOrDefaultAsync();


                var kycCheck = false;
                var verifiedDaysCount = (DateTime.Now.Date - Convert.ToDateTime(data.KycVerifiedDate).Date).TotalDays;
                if (data.IsOnfidoVerify && (verifiedDaysCount >= 0 && verifiedDaysCount <= 365))
                {
                    kycCheck = true;
                }

                return kycCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SanctionCheckCustomer(string userId)
        {
            try
            {
                var data = await (from ln in db.LexisNexis
                                  where ln.UserId == userId
                                  select ln).FirstOrDefaultAsync();

                var confirmMatchData = await (from ln in db.LexisNexis
                                              join spc in db.SanctionPepCustomer on ln.Id equals spc.LexisNexisId
                                              where ln.UserId == userId && spc.IsMatch == true
                                              select spc).FirstOrDefaultAsync();


                var sanctionCheck = false;
                if (confirmMatchData != null)
                {
                    if (!confirmMatchData.IsMatch)
                    {
                        sanctionCheck = true;
                        return sanctionCheck;
                    }
                    else
                    {
                        sanctionCheck = false;
                        return sanctionCheck;
                    }
                }

                if (data != null && data.SanctionMatch == false && data.PepMatch == false)
                {
                    sanctionCheck = true;
                }
                return sanctionCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SanctionCheckBeneficiary(string userId)
        {
            try
            {
                var data = await (from ln in db.LexisNexis
                                  join spc in db.SanctionPepBeneficiary on ln.Id equals spc.LexisNexisId
                                  where ln.UserId == userId && spc.IsMatch == true
                                  select ln.Id).ToListAsync();


                var sanctionCheck = false;
                if (data.Count == 0)
                {
                    sanctionCheck = true;
                }

                return sanctionCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SanctionCheckBoth(string userId, string beneficiaryId)
        {
            try
            {
                var data = (from ln in db.LexisNexis
                            where ln.UserId == userId
                            select ln).FirstOrDefault();

                var confirmMatchData = (from ln in db.LexisNexis
                                        join spc in db.SanctionPepCustomer on ln.Id equals spc.LexisNexisId
                                        where ln.UserId == userId && spc.IsMatch == true
                                        select spc).FirstOrDefault();


                var sanctionCheck = false;
                if (confirmMatchData != null)
                {
                    if (!confirmMatchData.IsMatch)
                    {
                        sanctionCheck = true;
                    }
                    else
                    {
                        sanctionCheck = false;
                        return sanctionCheck;
                    }
                }
                else
                {
                    if (data != null && data.SanctionMatch == false && data.PepMatch == false)
                    {
                        sanctionCheck = true;
                    }
                    else
                    {
                        sanctionCheck = false;
                        return sanctionCheck;
                    }
                }


                var datas = (from ln in db.LexisNexis
                             join spc in db.SanctionPepBeneficiary on ln.Id equals spc.LexisNexisId
                             where ln.UserId == beneficiaryId && spc.IsMatch == true
                             select ln.Id).ToList();
                if (datas.Count == 0)
                {
                    sanctionCheck = true;
                }

                return sanctionCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ManualKycCheck(string senderId)
        {
            try
            {
                var data = await (from c in db.Customer
                                  where c.UserId == senderId
                                  select c).FirstOrDefaultAsync();


                var kycCheck = false;
                var manualPassDaysCount = (DateTime.Now.Date - Convert.ToDateTime(data.KycManualPassDate).Date).TotalDays;
                if (data.KycFailedCount > 0)
                {
                    if (data.KycManualPass && (manualPassDaysCount >= 0 && manualPassDaysCount <= 365))
                    {
                        kycCheck = data.KycManualPass;
                    }
                }
                return kycCheck;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<int>> GetCaseData(int caseId)
        {
            try
            {
                var data = await adb.Case.Where(x => x.Id == caseId).FirstOrDefaultAsync();
                var dataList = new List<int>();
                if (data != null)
                {
                    dataList.Add(data.CaseFormId);
                    dataList.Add(data.StateId);
                }
                return dataList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetStateId(string stateName)
        {
            try
            {
                var stateId = await adb.State.Where(x => x.ActionName == stateName).Select(x => x.Id).FirstOrDefaultAsync();
                return stateId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> SaveSecureTradingReference(string referenceNo, string requestReference)
        {
            try
            {
                var transaction = await db.Transaction.Where(x => x.TransactionRefNo == referenceNo).FirstOrDefaultAsync();
                transaction.SecureTradingReferenceNo = requestReference;
                db.Transaction.Attach(transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<SecureTradingPaymentDetail> GetSecureTradingData(string requestReference)
        {
            var data = await db.SecureTradingPaymentDetail.Where(x => x.requestreference == requestReference).FirstOrDefaultAsync();
            return Mapper.Map<SecureTradingPaymentDetail>(data);
        }

        public async Task<TransactionLimitConfigViewModel> GetTransactionLimitConfigByCountryCodeAsync(string countryCode)
        {
            var transactionLimitConfigList = await (from c in db.TransactionLimitConfig
                                                    where c.CountryCode == countryCode
                                                    select c).FirstOrDefaultAsync();

            return Mapper.Map<TransactionLimitConfigViewModel>(transactionLimitConfigList);
        }

        public async Task<string> GetUserId(string referenceNo)
        {
            var userId = await db.Transaction.Where(x => x.TransactionRefNo == referenceNo).Select(x => x.UserId).FirstOrDefaultAsync();
            return userId.ToString();
        }

        public async Task<string> GetTfPinByReferenceNo(string referenceNo)
        {
            var tfPin = await db.Transaction.Where(x => x.TransactionRefNo == referenceNo).Select(x => x.SupplierTxnRefNo).FirstOrDefaultAsync();
            return tfPin;
        }

        public async Task<string> GetPayeeId(int paymentRequestId)
        {
            var payeeId = await db.STPaymentRequest.Where(x => x.Id == paymentRequestId).Select(x => x.PayeeId).FirstOrDefaultAsync();
            return payeeId;
        }

        public async Task<TransactionDetailsViewModel> GetTransactionDetails(int transactionId)
        {
            try
            {
                var datas = await db.Transaction.Where(x => x.TransactionId == transactionId).FirstOrDefaultAsync();
                var tdDatas = new TransactionDetailsViewModel();
                tdDatas.TransactionId = transactionId;
                tdDatas.TransactionNumber = datas.TransactionRefNo;
                tdDatas.TransactionPin = datas.SupplierTxnRefNo;
                tdDatas.TransactionStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), datas.Status);
                tdDatas.CustomerLexisNexisReports = new List<SelectListItem>();
                tdDatas.BeneficiaryLexisNexisReports = new List<SelectListItem>();

                var customerData = await (from ln in db.LexisNexis
                                          join spc in db.SanctionPepCustomer on ln.Id equals spc.LexisNexisId
                                          where ln.UserId == datas.UserId.ToString()
                                          select new
                                          {
                                              ProfileUrl = ln.ProfileUrl,
                                              Date = spc.CreatedDate,
                                              Reference = ln.Reference
                                          }).ToListAsync();

                if (customerData.Count > 0)
                {
                    foreach (var item in customerData)
                    {
                        tdDatas.CustomerLexisNexisReports.Add(new SelectListItem
                        {
                            Value = item.ProfileUrl,
                            Text = item.Date.ToString("yyyy-MM-dd") + "_" + item.Reference
                        });
                    }
                }

                var beneficiaryData = await (from ln in db.LexisNexis
                                             join spc in db.SanctionPepBeneficiary on ln.Id equals spc.LexisNexisId
                                             where ln.UserId == datas.UserId.ToString()
                                             select new
                                             {
                                                 ProfileUrl = ln.ProfileUrl,
                                                 Date = spc.CreatedDate,
                                                 Reference = ln.Reference
                                             }).ToListAsync();

                if (beneficiaryData.Count > 0)
                {
                    foreach (var item in beneficiaryData)
                    {
                        tdDatas.BeneficiaryLexisNexisReports.Add(new SelectListItem
                        {
                            Value = item.ProfileUrl,
                            Text = item.Date.ToString("yyyy-MM-dd") + "_" + item.Reference
                        });
                    }
                }

                tdDatas.Sender = new TransactionDetailsSender();

                var sender = await customerService.GetCustomerById(datas.UserId.ToString());
                tdDatas.Sender.FirstName = sender.FirstName;
                tdDatas.Sender.MiddleName = sender.MiddleName;
                tdDatas.Sender.LastName = sender.LastName;
                tdDatas.Sender.AddressLine1 = sender.AddressLine;
                tdDatas.Sender.AddressLine2 = sender.AddressLine;
                tdDatas.Sender.PostCode = sender.PostCode;
                tdDatas.Sender.MobileNumber = sender.MobileNumber;
                tdDatas.Sender.Country = await countryService.GetCountryName(sender.Country);
                tdDatas.Sender.DocumentExpirationDate = sender.IdExpiryDate;
                tdDatas.Sender.EmailAddress = sender.Email;
                tdDatas.Sender.DateOfBirth = sender.DOB;
                tdDatas.Sender.SendingReason = datas.PaymentPurpose == 0 ? "" : Enum.GetName(typeof(PaymentPurpose), datas.PaymentPurpose);
                tdDatas.Sender.FundsOrigin = datas.SourceOfFund == 0 ? "" : Enum.GetName(typeof(SourceOfFund), datas.SourceOfFund);

                var receiver = await customerService.GetCustomerById(datas.BeneficiaryId.ToString());
                tdDatas.Receiver = new TransactionDetailsReceiver();
                tdDatas.Receiver.FirstName = receiver.FirstName;
                tdDatas.Receiver.MiddleName = receiver.MiddleName;
                tdDatas.Receiver.LastName = receiver.LastName;
                tdDatas.Receiver.AddressLine1 = receiver.AddressLine;
                tdDatas.Receiver.AddressLine2 = receiver.AddressLine;
                tdDatas.Receiver.PostCode = receiver.PostCode;
                tdDatas.Receiver.MobileNumber = receiver.MobileNumber;
                tdDatas.Receiver.City = receiver.City;
                tdDatas.Receiver.Country = await countryService.GetCountryName(receiver.Country);
                tdDatas.Receiver.EmailAddress = receiver.Email;
                tdDatas.Receiver.DateOfBirth = receiver.DOB;

                tdDatas.PaymentInformation = new PaymentInformation();
                tdDatas.PaymentInformation.OriginCurrency = await countryService.GetCountryCurrencyCode(sender.Country);
                tdDatas.PaymentInformation.AmountSent = datas.SendAmount;
                tdDatas.PaymentInformation.ExchangeRate = datas.ExchangeRate;
                tdDatas.PaymentInformation.Fees = datas.TransferFee;
                tdDatas.PaymentInformation.TotalPaid = datas.SendAmount + datas.TransferFee;
                tdDatas.PaymentInformation.DestinationCurrency = await countryService.GetCountryCurrencyCode(receiver.Country);
                tdDatas.PaymentInformation.AmountToReceive = datas.PayoutAmount;
                tdDatas.PaymentInformation.TotalAmountToReceive = datas.PayoutAmount;

                tdDatas.informationForPayer = new InformationForPayer();
                tdDatas.informationForPayer.PaymentType = datas.PaymentMethodId == 0 ? "" : EnumModel<SimpleTransferTransactionManagementStatus>.GetDescription(datas.PaymentMethodId);

                tdDatas.Compliance = new TransactionDetailsCompliance();
                return tdDatas;
            }
            catch (Exception ex)
            {
                var datas = new TransactionDetailsViewModel();
                datas.Sender = new TransactionDetailsSender();
                datas.Receiver = new TransactionDetailsReceiver();
                datas.Compliance = new TransactionDetailsCompliance();
                datas.PaymentInformation = new PaymentInformation();
                datas.informationForPayer = new InformationForPayer();
                return datas;
            }
        }

        public async Task<List<TransactionMgmtViewModel>> GetBTTrxNew()
        {
            try
            {
                var TransactionMgmtList = await (from c in db.Transaction
                                                 where c.Status == (int)SimpleTransferTransactionStatus.BTTrxNew
                                                 select c).ToListAsync();

                return Mapper.Map<List<TransactionMgmtViewModel>>(TransactionMgmtList);
            }
            catch (Exception ex)
            {
                var datas = new List<TransactionMgmtViewModel>();
                return datas;
            }
        }

        public async Task<bool> UpdateTransactionStatusByPaymentRequestId(int paymentRequestId, int status)
        {
            try
            {
                var paymentRequest = await db.STPaymentRequest.Where(x => x.Id == paymentRequestId).FirstOrDefaultAsync();
                paymentRequest.Status = status;
                db.STPaymentRequest.Attach(paymentRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<PaymentRequestViewModel> GetPaymentRequestData(int id)
        {
            try
            {
                var data = await db.STPaymentRequest.Where(x => x.Id == id).FirstOrDefaultAsync();
                var paymentRequestData = Mapper.Map<PaymentRequestViewModel>(data);
                paymentRequestData.PayeeName = db.Customer.Where(x => x.UserId == paymentRequestData.PayeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                var name = new List<string>();
                if (data.PayerName != null)
                {
                    name = new List<string>(data.PayerName.Split(" "));
                    paymentRequestData.PayerFirstName = data.PayerName;
                }
                if (name.Count > 1)
                {
                    paymentRequestData.PayerFirstName = string.Join(" ", name.Take(name.Count - 1));
                    paymentRequestData.PayerLastName = name.LastOrDefault();
                }

                paymentRequestData.STPaymentRequestDetails = new STPaymentRequestDetailsViewModel();
                var paymentRequestDetailData = await db.STPaymentRequestDetails.Where(x => x.STPaymentRequestId == id.ToString()).FirstOrDefaultAsync();
                paymentRequestData.STPaymentRequestDetails = Mapper.Map<STPaymentRequestDetailsViewModel>(paymentRequestDetailData);
                return paymentRequestData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
