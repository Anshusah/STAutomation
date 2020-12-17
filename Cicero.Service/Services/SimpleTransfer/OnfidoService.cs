using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Models.SimpleTransfer.Onfido;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IOnfidoService
    {
        Task<OnfidoApplicantViewModel> CreateOrUpdate(OnfidoApplicantViewModel model);
        OnfidoApplicantDocumentViewModel CreateOrUpdate(OnfidoApplicantDocumentViewModel model);
        OnfidoApplicantLivePhotoViewModel CreateOrUpdate(OnfidoApplicantLivePhotoViewModel model);
        Task<OnfidoChecksViewModel> CreateOrUpdate(OnfidoChecksViewModel model);

        Task<List<OnfidoDocument>> GetDocumentIdByApplicantId(string applicantId);
        Task<List<OnfidoDocument>> GetPhotoIdByApplicantId(string applicantId);
        Task<string> GetCheckIdByApplicantId(string applicantId);

        Task<OnfidoResults> CheckOnfidoVerifyResult(string email);
        Task<OnfidoResults> CheckOnfidoVerifyResultByUserId(string userId);
        Task<bool> SaveCheckOnfidoVerifyResult(string applicantId, string status);

        Task<bool> SaveReportData(OnfidoReport data, string applicantId);

        Task<string> GetReportIdsByCheckId(string checkId);
    }

    public class OnfidoService : IOnfidoService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<OnfidoService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public OnfidoService(SimpleTransferApplicationDbContext _db, ILogger<OnfidoService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public async Task<OnfidoResults> CheckOnfidoVerifyResult(string email)
        {
            var customer = await db.Customer.Where(x => x.Email == email).FirstOrDefaultAsync();
            var data = new OnfidoResults();
            if(customer != null)
            {
                data.IsOnfidoVerify = customer.IsOnfidoVerify;
                data.OnfidoCheckResult = customer.OnfidoChecksResult;
            }

            return data;
        }

        public async Task<OnfidoResults> CheckOnfidoVerifyResultByUserId(string userId)
        {
            var customer = await db.Customer.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            var data = new OnfidoResults();
            if (customer != null)
            {
                data.IsOnfidoVerify = customer.IsOnfidoVerify;
                data.OnfidoCheckResult = customer.OnfidoChecksResult;
            }

            return data;
        }

        public async Task<OnfidoApplicantViewModel> CreateOrUpdate(OnfidoApplicantViewModel model)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                OnfidoApplicant applicant = Mapper.Map<OnfidoApplicant>(model);
                applicant.updated_at = DateTime.Now;

                if (model.id == null)
                {
                    db.OnfidoApplicant.Add(applicant);
                    await db.SaveChangesAsync();
                    return Mapper.Map<OnfidoApplicantViewModel>(applicant);
                }
                else
                {
                    db.OnfidoApplicant.Attach(applicant).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }


                return model;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public OnfidoApplicantDocumentViewModel CreateOrUpdate(OnfidoApplicantDocumentViewModel model)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                OnfidoApplicantDocument applicantDocument = Mapper.Map<OnfidoApplicantDocument>(model);
                applicantDocument.updated_at = DateTime.Now;

                if (model.id == null)
                {
                    db.OnfidoApplicantDocument.Add(applicantDocument);
                    db.SaveChanges();
                    return Mapper.Map<OnfidoApplicantDocumentViewModel>(applicantDocument);
                }
                else
                {
                    db.OnfidoApplicantDocument.Local.Clear();
                    db.OnfidoApplicantDocument.Attach(applicantDocument).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return model;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public OnfidoApplicantLivePhotoViewModel CreateOrUpdate(OnfidoApplicantLivePhotoViewModel model)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                OnfidoApplicantLivePhoto applicantLivePhoto = Mapper.Map<OnfidoApplicantLivePhoto>(model);
                applicantLivePhoto.updated_at = DateTime.Now;

                if (model.id == null)
                {
                    db.OnfidoApplicantLivePhoto.Add(applicantLivePhoto);
                    db.SaveChanges();
                    return Mapper.Map<OnfidoApplicantLivePhotoViewModel>(applicantLivePhoto);
                }
                else
                {
                    db.OnfidoApplicantLivePhoto.Local.Clear();
                    db.OnfidoApplicantLivePhoto.Attach(applicantLivePhoto).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return model;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<OnfidoChecksViewModel> CreateOrUpdate(OnfidoChecksViewModel model)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                OnfidoChecks onfidoChecks = Mapper.Map<OnfidoChecks>(model);
                onfidoChecks.report_ids = string.Join(',' , model.report_ids);
                onfidoChecks.updated_at = DateTime.Now;

                if (model.id == null)
                {
                    db.OnfidoCheck.Add(onfidoChecks);
                    await db.SaveChangesAsync();
                    return Mapper.Map<OnfidoChecksViewModel>(onfidoChecks);
                }
                else
                {
                    db.OnfidoCheck.Attach(onfidoChecks).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }


                return model;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<string> GetCheckIdByApplicantId(string applicantId)
        {
            var checkId = await db.OnfidoCheck.Where(x => x.applicant_id == applicantId).Select(x => x.ChecksId).FirstOrDefaultAsync();
            return checkId;
        }

        public async Task<List<OnfidoDocument>> GetDocumentIdByApplicantId(string applicantId)
        {
            var data = await db.OnfidoApplicantDocument.Where(x => x.applicant_id == applicantId).Select(x=> new OnfidoDocument { Id = x.DocumentId, IdValue = x.id }).ToListAsync();
            return data;
        }

        public async Task<List<OnfidoDocument>> GetPhotoIdByApplicantId(string applicantId)
        {
            var data = await db.OnfidoApplicantLivePhoto.Where(x => x.applicant_id == applicantId).Select(x => new OnfidoDocument { Id = x.PhotoId, IdValue = x.id }).ToListAsync();
            return data;
        }

        public async Task<string> GetReportIdsByCheckId(string checkId)
        {
            var reportIds = await db.OnfidoCheck.Where(x => x.ChecksId == checkId).Select(x => x.report_ids).FirstOrDefaultAsync();
            return reportIds;
        }

        public async Task<bool> SaveCheckOnfidoVerifyResult(string applicantId, string status)
        {
            try
            {
                var customerId = await db.OnfidoApplicant.Where(x => x.ApplicantId == applicantId).Select(x => x.CustomerId).FirstOrDefaultAsync();
                var customer = await db.Customer.Where(x => x.Id == customerId).FirstOrDefaultAsync();
                customer.OnfidoChecksResult = status;
                db.Customer.Attach(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SaveReportData(OnfidoReport data, string applicantId)
        {
            try
            {
                var customerId = await db.OnfidoApplicant.Where(x => x.ApplicantId == applicantId).Select(x => x.CustomerId).FirstOrDefaultAsync();
                var customer = await db.Customer.Where(x => x.Id == customerId).FirstOrDefaultAsync();
                customer.Gender = data.properties.gender;
                var idType = await db.IdentificationType.Where(x => x.IdentificationTypeName == data.properties.document_type).Select(x => x.Id).FirstOrDefaultAsync();
                customer.IdType = idType;
                customer.IdNumber = data.properties.document_numbers.Select(x => x.value).FirstOrDefault();
                customer.IdExpiryDate = Convert.ToDateTime(data.properties.date_of_expiry);
                customer.DOB = Convert.ToDateTime(data.properties.date_of_birth);

                db.Customer.Attach(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }


    public class OnfidoDocument
    {
        public string IdValue { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
    }
}
