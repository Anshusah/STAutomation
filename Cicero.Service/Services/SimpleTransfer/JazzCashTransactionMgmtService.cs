using AutoMapper;
using Cicero.Data;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using Cicero.Data.Entities.SimpleTransfer;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IJazzCashTransactionMgmtService
    {
        Task<JazzCashTransactionMgmtViewModel> GetJazzCashTransactionMgmtByIdAsync(int id);
        Task<JazzCashTransactionMgmtViewModel> GetJazzCashTransactionMgmtByPaymentRequestIdAsync(int paymentRequestId);
        Task<JazzCashTransactionHistoryViewModel> GetJazzCashTransactionHistoryByReferenceNoAsync(string referenceNo);
        Task<JazzCashTransactionMgmtViewModel> CreateOrUpdate(JazzCashTransactionMgmtViewModel model);
        Task<JazzCashTransactionHistoryViewModel> CreateOrUpdate(JazzCashTransactionHistoryViewModel model);
        Task<bool> UpdateTransactionStatus(string referenceNo, string status);
        Task<JazzCashTransactionMgmtViewModel> CancelTransaction(string referenceNo);
        Task<List<PaymentRequestModel>> GetPaymentRequestByAccountNumber(string accountNumber);
        Task<PaymentRequestModel> GetPaymentRequestByRequestId(string requestId);
        Task<List<PaymentRequestModel>> GetPaymentRequestByDates(DateTime start, DateTime end);
    }

    public class JazzCashTransactionMgmtService : IJazzCashTransactionMgmtService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IJazzCashTransactionMgmtService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly ApplicationDbContext applicationDb;

        public JazzCashTransactionMgmtService(SimpleTransferApplicationDbContext _db, ILogger<IJazzCashTransactionMgmtService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils, ApplicationDbContext applicationDbContext)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
            applicationDb = applicationDbContext;
        }

        public async Task<JazzCashTransactionMgmtViewModel> GetJazzCashTransactionMgmtByIdAsync(int id)
        {
            var JazzCashTransactionMgmtList = await (from c in db.JazzCashTransaction
                                                     where c.JazzCashTransactionId == id
                                                     select c).FirstOrDefaultAsync();

            return Mapper.Map<JazzCashTransactionMgmtViewModel>(JazzCashTransactionMgmtList);
        }

        public async Task<JazzCashTransactionMgmtViewModel> GetJazzCashTransactionMgmtByPaymentRequestIdAsync(int paymentRequestId)
        {
            var JazzCashTransactionMgmtList = await (from c in db.JazzCashTransaction
                                                     where c.PaymentRequestId == paymentRequestId
                                                     select c).FirstOrDefaultAsync();

            return Mapper.Map<JazzCashTransactionMgmtViewModel>(JazzCashTransactionMgmtList);
        }

        public async Task<JazzCashTransactionHistoryViewModel> GetJazzCashTransactionHistoryByReferenceNoAsync(string referenceNo)
        {
            var JazzCashTransactionHistoryList = await (from h in db.JazzCashTransactionHistory
                                                        where h.TransactionRefNo == referenceNo
                                                        select h).FirstOrDefaultAsync();

            return Mapper.Map<JazzCashTransactionHistoryViewModel>(JazzCashTransactionHistoryList);
        }

        public async Task<bool> UpdateTransactionStatus(string referenceNo, string status)
        {
            try
            {
                var jazzcashTransaction = await db.JazzCashTransaction.Where(x => x.TransactionRefNo == referenceNo).FirstOrDefaultAsync();
                jazzcashTransaction.SupplierTxnStatus = status;
                db.JazzCashTransaction.Attach(jazzcashTransaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<JazzCashTransactionMgmtViewModel> CancelTransaction(string referenceNo)
        {
            try
            {
                var jazzCashTransaction = await db.JazzCashTransaction.Where(x => x.TransactionRefNo == referenceNo).FirstOrDefaultAsync();
                jazzCashTransaction.SupplierTxnStatus = "C";
                jazzCashTransaction.Status = (int)SimpleTransferTransactionStatus.Cancel;
                db.JazzCashTransaction.Attach(jazzCashTransaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Mapper.Map<JazzCashTransactionMgmtViewModel>(jazzCashTransaction);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<JazzCashTransactionMgmtViewModel> CreateOrUpdate(JazzCashTransactionMgmtViewModel model)
        {
            try
            {
                JazzCashTransaction transactionMgmt = new JazzCashTransaction();
                transactionMgmt = Mapper.Map<JazzCashTransaction>(model);
                transactionMgmt.TransactionRefNo = await db.PaymentRequest.Where(x => x.Id == model.PaymentRequestId).Select(x => x.PaymentReferenceNumber).FirstOrDefaultAsync();
                transactionMgmt.CreatedBy = model.CreatedBy;
                transactionMgmt.UpdatedBy = commonService.getLoggedInUserId();
                transactionMgmt.CreatedDate = model.CreatedDate;
                transactionMgmt.UpdatedDate = DateTime.Now;

                if (model.JazzCashTransactionId == 0)
                {
                    model.CreatedBy = model.UpdatedBy;
                    model.CreatedDate = model.UpdatedDate;
                    db.JazzCashTransaction.Add(transactionMgmt);
                    await db.SaveChangesAsync();

                    return Mapper.Map<JazzCashTransactionMgmtViewModel>(transactionMgmt);
                }
                else
                {
                    var TransactionMgmtData = Mapper.Map<JazzCashTransaction>(transactionMgmt);

                    db.JazzCashTransaction.Attach(transactionMgmt).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }


                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<JazzCashTransactionHistoryViewModel> CreateOrUpdate(JazzCashTransactionHistoryViewModel model)
        {
            try
            {
                JazzCashTransactionHistory transactionHistory = new JazzCashTransactionHistory();
                transactionHistory = Mapper.Map<JazzCashTransactionHistory>(model);
                transactionHistory.CreatedBy = model.CreatedBy;
                transactionHistory.UpdatedBy = commonService.getLoggedInUserId();
                transactionHistory.CreatedDate = Convert.ToDateTime(model.CreatedDate);
                transactionHistory.UpdatedDate = DateTime.Now;

                if (model.JazzCashTransactionHistoryId == 0)
                {
                    model.CreatedBy = model.UpdatedBy;
                    model.CreatedDate = model.UpdatedDate;
                    db.JazzCashTransactionHistory.Add(transactionHistory);
                    await db.SaveChangesAsync();

                    return Mapper.Map<JazzCashTransactionHistoryViewModel>(transactionHistory);
                }
                else
                {
                    var TransactionHistoryData = Mapper.Map<JazzCashTransactionHistory>(transactionHistory);

                    db.JazzCashTransactionHistory.Attach(TransactionHistoryData).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }


                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<PaymentRequestModel>> GetPaymentRequestByAccountNumber(string accountNumber)
        {
            try
            {
                var data = await db.PaymentRequest.Where(x => x.JazzCashAccountNumber == accountNumber).ToListAsync();
                return Mapper.Map<List<PaymentRequestModel>>(data);
            }
            catch (Exception ex)
            {
                return new List<PaymentRequestModel>();
            }
        }

        public async Task<PaymentRequestModel> GetPaymentRequestByRequestId(string requestId)
        {
            try
            {
                var data = await db.PaymentRequest.Where(x => x.RequestId == requestId).FirstOrDefaultAsync();
                return Mapper.Map<PaymentRequestModel>(data);
            }
            catch (Exception ex)
            {
                return new PaymentRequestModel();
            }
        }

        public async Task<List<PaymentRequestModel>> GetPaymentRequestByDates(DateTime start, DateTime end)
        {
            try
            {
                var data = await db.PaymentRequest.Where(x => x.CreatedDate >= start && x.CreatedDate <= end).ToListAsync();
                return Mapper.Map<List<PaymentRequestModel>>(data);
            }
            catch (Exception ex)
            {
                return new List<PaymentRequestModel>();
            }
        }
    }
}
