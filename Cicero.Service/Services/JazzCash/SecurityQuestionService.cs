using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.JazzCash;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Cicero.Data.Entities.JazzCash;
using Microsoft.EntityFrameworkCore;

namespace Cicero.Service.Services.JazzCash
{
    public interface ISecurityQuestionService
    {
        DTResponseModel GetSecurityQuestionListByFilter(DTPostModel model);
        Task<SecurityQuestionViewModel> CreateOrUpdate(SecurityQuestionViewModel model);
        Task<SecurityQuestionViewModel> GetSecurityQuestionByIdAsync(int id);

        Task<bool> ActiveSecurityQuestionById(int id);
        Task<bool> InActiveSecurityQuestionById(int id);
        Task<bool> DeleteSecurityQuestionById(int id);
    }

    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<SecurityQuestionService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly UserManager<ApplicationUser> userManager;

        public SecurityQuestionService(SimpleTransferApplicationDbContext _db, ILogger<SecurityQuestionService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils, UserManager<ApplicationUser> _UserManager)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
            userManager = _UserManager;
        }

        public DTResponseModel GetSecurityQuestionListByFilter(DTPostModel model)
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

            var questionList = (from c in db.SecurityQuestion
                               select new
                               {
                                   id = c.Id,
                                   question = c.Question,
                                   created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                   updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                   status = (c.Status) ? "Active" : "Inactive",
                                   action = "<a href='/admin/securityquestion/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Security Question' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Security Question</span></a>"
                               });

            totalResultsCount = questionList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                questionList = questionList.Where(o => o.question.ToLower().Contains(searchBy.ToLower()));

            }
            questionList = questionList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = questionList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<SecurityQuestionViewModel> CreateOrUpdate(SecurityQuestionViewModel model)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                var securityQuestion = new SecurityQuestion();
                securityQuestion = Mapper.Map<SecurityQuestion>(model);
                securityQuestion.CreatedBy = model.CreatedBy;
                securityQuestion.UpdatedBy = commonService.getLoggedInUserId();
                securityQuestion.CreatedDate = Convert.ToDateTime(model.CreatedDate);
                securityQuestion.UpdatedDate = DateTime.Now;

                if (model.Id == 0)
                {
                    securityQuestion.CreatedBy = securityQuestion.UpdatedBy;
                    securityQuestion.CreatedDate = securityQuestion.UpdatedDate;
                    db.SecurityQuestion.Add(securityQuestion);
                    await db.SaveChangesAsync();
                    return Mapper.Map<SecurityQuestionViewModel>(securityQuestion);
                }
                else
                {
                    db.SecurityQuestion.Attach(securityQuestion).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SecurityQuestionViewModel> GetSecurityQuestionByIdAsync(int id)
        {
            var securityQuestion = await (from sq in db.SecurityQuestion
                                         where sq.Id == id
                                         select sq).FirstOrDefaultAsync();

            return Mapper.Map<SecurityQuestionViewModel>(securityQuestion);
        }

        public async Task<bool> ActiveSecurityQuestionById(int id)
        {
            var securityQuestion = await db.SecurityQuestion.FindAsync(id);
            if (securityQuestion != null)
            {
                securityQuestion.Status = true;
                var result = db.SecurityQuestion.Update(securityQuestion);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Security Question changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/SecurityQuestion/" + utils.EncryptId(securityQuestion.Id) + "/edit.html'>" + securityQuestion.Question + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/securityquestion/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("SecurityQuestion - ActiveSecurityQuestionById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveSecurityQuestionById(int id)
        {
            var securityQuestion = await db.SecurityQuestion.FindAsync(id);
            if (securityQuestion != null)
            {
                securityQuestion.Status = false;
                var result = db.SecurityQuestion.Update(securityQuestion);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Security Question changed to InActive <a href ='/admin" + utils.GetTenantForUrl(false) + "/SecurityQuestion/" + utils.EncryptId(securityQuestion.Id) + "/edit.html'>" + securityQuestion.Question + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/securityquestion/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("SecurityQuestion - InActiveSecurityQuestionById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteSecurityQuestionById(int id)
        {
            var securityQuestion = await db.SecurityQuestion.FindAsync(id);
            if (securityQuestion != null)
            {
                db.SecurityQuestion.Remove(securityQuestion);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Security Question deleted <a href ='/admin" + utils.GetTenantForUrl(false) + "/SecurityQuestion/" + utils.EncryptId(securityQuestion.Id) + "/edit.html'>" + securityQuestion.Question + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/securityquestion/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("SecurityQuestion - DeleteSecurityQuestionById - " + id + " - : ");
            return false;
        }
    }
}
