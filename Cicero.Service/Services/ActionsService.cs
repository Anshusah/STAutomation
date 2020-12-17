using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Cicero.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering; 

namespace Cicero.Service.Services
{
    public interface IActionsService
    {
        DTResponseModel GetActionsList(DTPostModel model, string isNew, DateTime datetime);
        DTResponseModel GetStateList(DTPostModel model);
        
         
        Task<bool> ActiveActionsById(int id);
        Task<bool> InactiveActionsById(int id);
        Task<ActionsViewModel> CreateOrUpdateActionsAsync(ActionsViewModel avm);
    }

    public class ActionsService : IActionsService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ActionsService> _Log;
        private readonly IHttpContextAccessor _IHttpContextAccessor = null;
        private readonly IHostingEnvironment _HostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;
        private readonly Utils _utils;
        private readonly ICommonService _commonService;

        public ActionsService(ApplicationDbContext db, ILogger<ActionsService> Log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper mapper, ICommonService commonService, IActivityLogService activityLogService, Utils utils)
        {
            _db = db;
            _Log = Log;
            _IHttpContextAccessor = httpContextAccessor;
            _HostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _commonService = commonService;
            _activityLogService = activityLogService;
            _utils = utils;
        }

        public DTResponseModel GetActionsList(DTPostModel model, string isNew, DateTime datetime)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
            bool sortDir = true;

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
                    sortDir = model.order[0].dir.ToLower() == "asc";
                }
            }

            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            var Actions = _db.Actions
                           .Where(a => (a.TenantId == tenantid || tenantid == 0) && a.CreatedAt >= datetime)
                           .GroupJoin(_db.Users, que => que.CreatedBy, creat => creat.Id, (x, z) => new { que = x, creat = z })
                           .SelectMany(y => y.creat.DefaultIfEmpty(), (x, z) => new { que = x.que, creat = z })
                           .GroupJoin(_db.Users, d => d.que.UpdatedBy, upda => upda.Id, (d, e) => new { que = d, upda = e })
                           .SelectMany(f => f.upda.DefaultIfEmpty(), (g, h) => new { que = g.que, upda = h })
                           .Select(u => new
                           {
                               id = u.que.que.Id,
                               name = u.que.que.Name,
                               created_by = u.que.creat.FirstName + " " + u.que.creat.LastName,
                               updated_by = u.upda.FirstName + " " + u.upda.LastName,
                               //role = _db.Roles.Where(f => f.Id == u.que.que.RoleId).FirstOrDefault().Name,
                               updated_at = Utils.GetDefaultDateFormat(u.que.que.UpdatedAt),
                               created_at = u.que.que.CreatedAt,
                               //order = u.que.que.Order,
                               //order = 1,
                               action = "<a href='/admin" + _utils.GetTenantForUrl(false) + "/manage/Actions/" + _utils.EncryptId(u.que.que.Id) + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Action' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Action</span></a>"
                           });

            totalResultsCount = Actions.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                Actions = Actions.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = Actions.Count();
            Actions = Actions.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = Actions.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public DTResponseModel GetStateList(DTPostModel model)
        {

            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
            bool sortDir = true;

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
                    sortDir = model.order[0].dir.ToLower() == "asc";
                }
            }

            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            string loggeduser = _commonService.getLoggedInUserId();

            string roleid = _commonService.GetRoleIdByUserId(loggeduser);

            var state = _db.State
                            .Where(a => a.TenantId == tenantid || tenantid == 0)
                            .GroupJoin(_db.Users, sta => sta.CreatedBy, creat => creat.Id, (x, z) => new { sta = x, creat = z })
                            .SelectMany(y => y.creat.DefaultIfEmpty(), (x, z) => new { sta = x.sta, creat = z })
                            .GroupJoin(_db.Users, d => d.sta.UpdatedBy, upda => upda.Id, (d, e) => new { sta = d, upda = e })
                            .SelectMany(f => f.upda.DefaultIfEmpty(), (g, h) => new { sta = g.sta, upda = h })
                            .Select(u => new
                            {
                                id = u.sta.sta.Id,
                                name = u.sta.sta.SystemName,
                                created_by = u.sta.creat.FirstName + " " + u.sta.creat.LastName,
                                updated_by = u.upda.FirstName + " " + u.upda.LastName,
                                created_at = Utils.GetDefaultDateFormat(u.sta.sta.CreatedAt),
                                updated_at = Utils.GetDefaultDateFormat(u.sta.sta.UpdatedAt),
                                //role = u.sta.sta.StateForForm.SelectMany(x => x.StatePermission).Where(y => y.RoleId == roleid).FirstOrDefault().RoleForState.DisplayName,
                                action = "<a href='/admin" + _utils.GetTenantForUrl(false) + "/manage/state/" + _utils.EncryptId(u.sta.sta.Id) + "/edit.html' title='Edit State' data-toggle='tooltip'><i class='fa fa-edit'></i></a>"
                            });

            totalResultsCount = state.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                state = state.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = state.Count();
            state = state.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = state.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<bool> ActiveActionsById(int id)
        {
            var Actions = await _db.Actions.FindAsync(id);
            if (Actions != null)
            {
                Actions.Status = true;
                _db.Actions.Update(Actions);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "Actions changed to Active <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/Actions/" + _utils.EncryptId(Actions.Id) + "/edit.html'>" + Actions.Name + "</a>. Changed By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            _Log.LogError("ActionsService - ActiveActionsById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveActionsById(int id)
        {
            var Actions = await _db.Actions.FindAsync(id);
            if (Actions != null)
            {
                Actions.Status = false;
                _db.Actions.Update(Actions);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "Actions changed to InActive <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage_Actions/Actions/" + _utils.EncryptId(Actions.Id) + "/edit.html'>" + Actions.Name + "</a>. Changed By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            _Log.LogError("ActionsService - InactiveActionsById - " + id + " - : ");
            return false;
        }
        private async Task<int> CreateActionsAsync(Actions action)
        {
            var loggedUser = _commonService.getLoggedInUserId();
            var fullName = _commonService.GetUserFullName().Result;

            action.Id = 0;
            action.CreatedBy = loggedUser;
            action.CreatedAt = DateTime.Now;
            await _db.Actions.AddAsync(action);

            await _db.SaveChangesAsync();

            await _activityLogService.CreateLog(loggedUser, "New Actions created <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/queue/" + _utils.EncryptId(action.Id) + "/edit.html'>" + action.Name + "</a>. Created By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

            return action.Id;
        }

        private async Task UpdateActionsAsync(Actions action )
        {
            var loggedUser = _commonService.getLoggedInUserId();
            var fullName = _commonService.GetUserFullName().Result;

            //var temp = _db.QueueForForm.Where(c => c.QueueId == queue.Id && queue.QueueForForm.Count() > 0 && c.CaseFormId == queue.QueueForForm.FirstOrDefault().CaseFormId);

            //if (temp.Count() > 0)
            //{

            //    var qffid = temp.FirstOrDefault().Id;

            //    _db.QueuePermission.RemoveRange(_db.QueuePermission.Where(x => x.QueueForFormId == qffid).ToList());

            //    _db.QueueForForm.RemoveRange(_db.QueueForForm.Where(x => x.QueueId == queue.Id && x.CaseFormId == queue.QueueForForm.FirstOrDefault().CaseFormId).ToList());
            //}

            //if (relationremove == true)
            //{
            //    action.QueueForForm = null;
            //}

            action.UpdatedBy = loggedUser;
            _db.Actions.Attach(action).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            await _activityLogService.CreateLog(loggedUser, "Queue updated <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/queue/" + _utils.EncryptId(action.Id) + "/edit.html'>" + action.Name + "</a>. Updated By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

        }

        public async Task<ActionsViewModel> CreateOrUpdateActionsAsync(ActionsViewModel avm)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    avm.UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now);

                    var actions = _mapper.Map<Actions>(avm);

                    if (avm.Id == 0)
                    {

                        avm.Id = await CreateActionsAsync(actions);
                    }
                    else
                    {

                        await UpdateActionsAsync(actions );

                    }

                    transaction.Commit();
                    return avm;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return avm;
                }
            }
        }
 
    }
}
