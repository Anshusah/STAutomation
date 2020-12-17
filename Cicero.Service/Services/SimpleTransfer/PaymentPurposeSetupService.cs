using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Cicero.Service.Models.General;
using static Cicero.Service.Extensions.Extensions;
using PaymentPurpose = Cicero.Data.Entities.SimpleTransfer.PaymentPurpose;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IPaymentPurposeSetupService
    {
        DTResponseModel GetPaymentPurposeSetupListByFilter(DTPostModel model);
        Task<PaymentPurposeSetupViewModel> GetPaymentPurposeSetupByIdAsync(int id);
        Task<PaymentPurposeSetupViewModel> CreateOrUpdate(PaymentPurposeSetupViewModel model);
        List<SelectListItem> GetPaymentPurposeAsync(string tenantId);
        bool CreateOrUpdateTransfastRemittancePurpose(List<TransfastBankModel> model, string countryCode);
        List<SelectListItem> GetTransfastRemittancePurposeAsync(int transfastId);

        Task<bool> ActivePaymentPurposeSetupById(int id);
        Task<bool> InActivePaymentPurposeSetupById(int id);
        Task<bool> DeletePaymentPurposeSetupById(int id);
        bool CheckDuplicate(PaymentPurposeSetupViewModel model);
    }
    public class PaymentPurposeSetupService : IPaymentPurposeSetupService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IPaymentPurposeSetupService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public PaymentPurposeSetupService(SimpleTransferApplicationDbContext _db, ILogger<IPaymentPurposeSetupService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetPaymentPurposeSetupListByFilter(DTPostModel model)
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

            var PaymentPurposeSetupList = from c in db.PaymentPurpose
                                    select new
                                    {
                                        id = c.Id,
                                        name = c.PurposeName,
                                        tenantid=c.TenantId,
                                        created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                        updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                        status = (c.Status) ? "Active" : "Inactive",
                                        action = "<a href='/admin/PaymentPurposeSetup/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit PaymentPurpose Payout' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit PaymentPurpose</span></a>"
                                    };
            totalResultsCount = PaymentPurposeSetupList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                PaymentPurposeSetupList = PaymentPurposeSetupList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            PaymentPurposeSetupList = PaymentPurposeSetupList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = PaymentPurposeSetupList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<PaymentPurposeSetupViewModel> CreateOrUpdate(PaymentPurposeSetupViewModel model)
        {
            model.CreatedBy = model.UpdatedBy = commonService.getLoggedInUserId();
            model.TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            PaymentPurpose paymentPurposeSetup = new PaymentPurpose
            {
                Id = model.Id,
                PurposeName=model.PurposeName,
                TenantId=model.TenantId,
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedDate = Convert.ToDateTime(model.CreatedDate),
                UpdatedDate = DateTime.Now,
                Status = model.Status,
                TransfastId=model.TransfastId
            };

            if (model.Id == 0)
            {
                paymentPurposeSetup.CreatedBy = paymentPurposeSetup.UpdatedBy;
                paymentPurposeSetup.CreatedDate = DateTime.Now;
                db.PaymentPurpose.Add(paymentPurposeSetup);
                await db.SaveChangesAsync();

                return Mapper.Map<PaymentPurposeSetupViewModel>(paymentPurposeSetup);
            }
            else
            {
                var paymentPurposeSetupData = Mapper.Map<PaymentPurpose>(paymentPurposeSetup);

                db.PaymentPurpose.Attach(paymentPurposeSetupData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<PaymentPurposeSetupViewModel> GetPaymentPurposeSetupByIdAsync(int id)
        {
            var PaymentPurposeSetupList = await (from c in db.PaymentPurpose
                                         where c.Id == id
                                           select c).FirstOrDefaultAsync();

            return Mapper.Map<PaymentPurposeSetupViewModel>(PaymentPurposeSetupList);
        }

        public async Task<bool> ActivePaymentPurposeSetupById(int id)
        {
            var PaymentPurposeSetup = await db.PaymentPurpose.FindAsync(id);
            if (PaymentPurposeSetup != null)
            {
                PaymentPurposeSetup.Status = true;
                var result = db.PaymentPurpose.Update(PaymentPurposeSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "PaymentPurpose changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/PaymentPurposeSetup/" + utils.EncryptId(PaymentPurposeSetup.Id) + "/edit.html'>" + PaymentPurposeSetup.PurposeName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("PaymentPurposeService - ActivePaymentPurposeById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActivePaymentPurposeSetupById(int id)
        {
            var PaymentPurposeSetup = await db.PaymentPurpose.FindAsync(id);
            if (PaymentPurposeSetup != null)
            {
                PaymentPurposeSetup.Status = false;
                var result = db.PaymentPurpose.Update(PaymentPurposeSetup);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "PaymentPurpose changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/PaymentPurposeSetup/" + utils.EncryptId(PaymentPurposeSetup.Id) + "/edit.html'>" + PaymentPurposeSetup.PurposeName + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("PaymentPurposeService - ActivePaymentPurposeById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeletePaymentPurposeSetupById(int id)
        {
            var PaymentPurposeSetup = await db.PaymentPurpose.FindAsync(id);
            if (PaymentPurposeSetup != null)
            {
                db.PaymentPurpose.Remove(PaymentPurposeSetup);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Beneficiary PaymentPurpose deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/PaymentPurposeSetup/" + utils.EncryptId(PaymentPurposeSetup.Id) + "/edit.html'>" + PaymentPurposeSetup.PurposeName + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("PaymentPurposeService - ActivePaymentPurposeById - " + id + " - : ");
            return false;
        }

        public List<SelectListItem> GetPaymentPurposeAsync(string tenantId)
        {
            return db.PaymentPurpose.Where(x => x.TenantId == Convert.ToInt32(tenantId) && x.Status)
                .Select(x=>new SelectListItem() { Text=x.PurposeName, Value=x.Id.ToString()}).ToList();
        }
        public bool CheckDuplicate(PaymentPurposeSetupViewModel model)
        {
            if (db.PaymentPurpose.Where(x => x.PurposeName.ToUpper() == model.PurposeName.ToUpper() && x.Id != model.Id).Any())
                return false;
            else
                return true;
        }
        public bool CreateOrUpdateTransfastRemittancePurpose(List<TransfastBankModel> model,string countryCode)
        {
            var data = Mapper.Map<List<TransfastRemittancePurpose>>(model);
            data.ForEach(x => x.CreatedDate = DateTime.Now);
            data.ForEach(x => x.CountryCode = countryCode.ToUpper() );
            data.ForEach(x => x.Status = true);
            data.ForEach(x => x.TenantId = Convert.ToInt32(commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession())));
            db.TransfastRemittancePurpose.AddRange(data);
            db.SaveChanges();
            return true;
        }
        public List<SelectListItem> GetTransfastRemittancePurposeAsync(int transfastId)
        {
            var paymentPurpose = db.PaymentPurpose.Where(x => x.Status && x.TenantId == commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession())).Select(x => new SelectListItem() { Text = x.PurposeName, Value = x.TransfastId.ToString() }).ToList();
            var transfastPaymentPurpose = db.TransfastRemittancePurpose.Where(x => x.Status && x.TenantId == commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession()))
                .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            var newList = transfastPaymentPurpose.Where(x => !paymentPurpose.Any(y => y.Value == x.Value && y.Value != transfastId.ToString())).ToList();
            return newList;
        }
    }
}
