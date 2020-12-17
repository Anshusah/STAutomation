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
using System.Transactions;

namespace Cicero.Service.Services
{
    public interface IQueueService
    {
        string GetQueueNameByFormId(int caseformid, int stateId);
        DTResponseModel GetQueueList(DTPostModel model, string isNew, DateTime datetime);
        DTResponseModel GetStateList(DTPostModel model);
        QueueViewModel GetQueueByCaseTypeId(int tenantid, int id, int caseformid);
        StateViewModel GetStateByCaseTypeId(int tenantid, int id, int caseformid);
        QueueViewModel GetQueueById(int tenantid, int id);
        StateViewModel GetStateById(int tenantid, int id);
        List<int> GetStateIdsByQueueId(int queueid, int caseformid);
        int GetStateByName(int tenantid, string name);
        StateViewModel GetStatePermissionsById(int tenantid, int id);
        QueueViewModel GetQueueByQueueIdentifier(string id);
        List<StateViewModel> GetSelectStateList(int tenantid);
        List<QueueViewModel> GetSelectQueueList(int tenantid);
        Task<QueueViewModel> CreateOrUpdateQueueAsync(QueueViewModel qvm);
        Task<StateViewModel> CreateOrUpdateStateAsync(StateViewModel svm);
        Task<bool> DeleteQueueById(int id);
        Task<bool> ActiveQueueById(int id);
        Task<bool> InactiveQueueById(int id);
        Task<bool> DeleteStateById(int id);
        Task<bool> ActiveStateById(int id);
        Task<bool> InactiveStateById(int id);
        List<QueueViewModel> GetBackendQueuesByActiveTenantIdAndRole();

        bool IsDuplicateUrl(string url, int id);
        bool IsDuplicateQueueOrder(int id, int order, int caseformid);
        bool IsDuplicateStateOrder(int id, int order, int caseformid);

        string GetTenantQueueNameByIdentifier(string id);

        List<QueueListViewModel> GetQueueListForClaim();
        //QueueListViewModel GetCasesByRoleIdQueueId(int queueid);

        string GetQueueNameByStateId(int stateId);
        string GetStateNameById(int stateId, int caseformid);
        string GetStateNameByStateId(int stateId);

        List<SelectListItem> GetStateSelectList();
        List<SelectListItem> GetStateSelectListByFormId(int formid, string roleid);
        List<SelectListItem> GetStateSelectListByUserFormRoleId(int formid, string roleid);

        bool IsStateInRole(int id);
        int GetTenantQueueCount();
        List<StateViewModel> GetAllStates();
        bool RemoveQueueById(int id);
        List<Queue> GetAddedQueue(DateTime startDatetime);

        bool SaveStateWorkFlow(int caseformid, List<JsonStateViewModel> jsvm);
        bool SaveQueueWorkFlow(int caseformid, List<JsonStateViewModel> jsvm);
        int GetCaseFormWorkFlowCount(int caseFormId);
        bool RemoveWorkflowByCaseFormId(int caseFormId);
        bool UpdateStateForForm(int caseFormId, bool isDelete = false, string stateIds = "");
        int GetFirstState(int caseFormId, string side);
        List<ActionsViewModel> GetSelectActionList(int tenantid);
        List<SelectListItem> GetCaseMovement(int caseformid, int caseid);
        UserViewModel GetCaseOwner(int caseFormId, int stateId, string userId);
        ElementStateViewModel SaveOrUpdateStateObject(ElementStateViewModel elementStateViewModel);
        bool SetStateObjectStatus(int objectId, bool status);
        ElementStateViewModel GetStateObjectById(int objectId);
    }

    public class QueueService : IQueueService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<QueueService> _Log;
        private readonly IHttpContextAccessor _IHttpContextAccessor = null;
        private readonly IHostingEnvironment _HostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;
        private readonly Utils _utils;
        private readonly ICommonService _commonService;

        public QueueService(ApplicationDbContext db, ILogger<QueueService> Log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IMapper mapper, ICommonService commonService, IActivityLogService activityLogService, Utils utils)
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

        public DTResponseModel GetQueueList(DTPostModel model, string isNew, DateTime datetime)
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
            var queue = _db.Queue
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
                               action = "<a href='/admin" + _utils.GetTenantForUrl(false) + "/manage/queue/" + _utils.EncryptId(u.que.que.Id) + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Queue' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Queue</span></a>"
                           });

            totalResultsCount = queue.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                queue = queue.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = queue.Count();
            queue = queue.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = queue.ToList();
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

        public QueueViewModel GetQueueByCaseTypeId(int tenantid, int id, int caseformid)
        {

            try
            {

                //var queue = _mapper.Map<QueueViewModel>(_db.Queue
                //                                        .Include(y => y.QueueForForm.Where(a => a.CaseFormId == caseformid).FirstOrDefault()).ThenInclude(b => b.QueuePermissions)
                //                                        .Include(z => z.QueueForForm.Where(a => a.CaseFormId == caseformid).FirstOrDefault()).ThenInclude(c => c.CaseForm)
                //                                        .Where(x => x.Id == id && tenantid == x.TenantId).FirstOrDefault());

                var qp = _db.QueueForForm
                                  .Include(x => x.QueuePermissions).ToList();

                var queue = _mapper.Map<QueueViewModel>(_db.Queue
                                .GroupJoin(qp.Where(n => n.CaseFormId == caseformid), que => que.Id, qff => qff.QueueId, (x, z) => new { que = x, qff = z })
                                .SelectMany(y => y.qff.DefaultIfEmpty(), (x, z) => new { que = x.que, qff = z })
                                .GroupJoin(_db.CaseForm, que => que.que.CaseFormId, cf => cf.Id, (l, m) => new { que = l, cf = m })
                                .SelectMany(i => i.cf.DefaultIfEmpty(), (j, k) => new { que = j.que, cf = k })
                                .Where(x => x.que.que.Id == id && tenantid == x.que.que.TenantId)
                                .FirstOrDefault().que.que);

                //statelist inside viewmodel
                queue.StateList = _mapper.Map<List<StateViewModel>>(_db.State
                                                                    .Where(x => tenantid == x.TenantId)
                                                                    .ToList());

                return queue;
            }
            catch (Exception ex)
            {
                _Log.LogError("QueueService - GetQueueByCaseTypeId - " + ex);
            }
            return null;
        }

        public StateViewModel GetStateByCaseTypeId(int tenantid, int id, int caseformid)
        {
            try
            {
                var statepermission = _db.StateForForm
                                    .Include(x => x.StatePermissions).ToList();

                var state = _mapper.Map<StateViewModel>(_db.State
                                    .GroupJoin(statepermission.Where(n => n.CaseFormId == caseformid), sta => sta.Id, sff => sff.StateId, (x, z) => new { sta = x, sff = z })
                                    .SelectMany(y => y.sff.DefaultIfEmpty(), (x, z) => new { sta = x.sta, sff = z })
                                    .GroupJoin(_db.CaseForm, sta => sta.sta.Id, cf => cf.Id, (l, m) => new { sta = l, cf = m })
                                    .SelectMany(i => i.cf.DefaultIfEmpty(), (j, k) => new { sta = j.sta, cf = k })
                                    .Where(x => x.sta.sta.Id == id && tenantid == x.sta.sta.TenantId)
                                    .FirstOrDefault().sta.sta);

                //statelist inside viewmodel
                state.StateList = _mapper.Map<List<StateViewModel>>(_db.State
                                                                    .Where(x => x.Id != id && tenantid == x.TenantId)
                                                                    .ToList());
                state.StateForForm.RemoveAll(b => b.CaseFormId != caseformid);
                return state;
            }
            catch (Exception ex)
            {
                _Log.LogError("QueueService - GetStateByCaseTypeId - " + ex);
            }
            return null;
        }

        public QueueViewModel GetQueueById(int tenantid, int id)
        {

            try
            {

                var queue = _mapper.Map<QueueViewModel>(_db.Queue
                                                        .Include(y => y.QueueForForm).ThenInclude(b => b.QueuePermissions)
                                                        .Include(z => z.QueueForForm).ThenInclude(c => c.CaseForm)
                                                        .Where(x => x.Id == id && tenantid == x.TenantId).FirstOrDefault());

                //statelist inside viewmodel
                queue.StateList = _mapper.Map<List<StateViewModel>>(_db.State
                                                                    .Where(x => tenantid == x.TenantId)
                                                                    .ToList());
                //var queueToStateList = _db.QueueToState.Where(x => x.QueueId == id).ToList();
                //var statecheckList = new List<int>();
                //if (queueToStateList.Count() > 0)
                //{
                //    foreach (var item in queueToStateList)
                //    {
                //        statecheckList.Add(item.StateId);
                //    }
                //}
                //queue.StateSelectedList = statecheckList;

                return queue;
            }
            catch (Exception ex)
            {
                _Log.LogError("QueueService - GetQueueById - " + ex);
            }
            return null;
        }

        public QueueViewModel GetQueueByQueueIdentifier(string id)
        {

            try
            {
                int tenantId = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                var queue = _mapper.Map<QueueViewModel>(_db.Queue.Where(x => x.UrlIdentifier == id && x.TenantId == tenantId).FirstOrDefault());
                if (queue != null)
                {
                    return queue;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _Log.LogError("QueueService - GetQueueById - " + ex);
                return null;
            }
        }

        public StateViewModel GetStateById(int tenantid, int id)
        {
            try
            {
                var state = _mapper.Map<StateViewModel>(_db.State
                                    .Include(y => y.StateForForm).ThenInclude(b => b.StatePermissions)
                                    .Include(z => z.StateForForm).ThenInclude(c => c.CaseForm)
                                    .Where(x => x.Id == id && tenantid == x.TenantId).FirstOrDefault());

                //statelist inside viewmodel
                state.StateList = _mapper.Map<List<StateViewModel>>(_db.State
                                                                    .Where(x => x.Id != id && tenantid == x.TenantId)
                                                                    .ToList());
                //var stateToStateList = _db.StateToState.Where(x => x.FromStateId == id).ToList();
                //var statecheckList = new List<int>();
                //if (stateToStateList.Count() > 0)
                //{
                //    foreach (var item in stateToStateList)
                //    {
                //        statecheckList.Add(item.ToStateId);
                //    }
                //}
                //state.StateSelectedList = statecheckList;
                state.RoleList = _mapper.Map<List<SelectListItem>>(_db.StatePermission.Where(x => x.StateForForm.Id == x.StateForFormId && x.CanEdit == true).Select(x => new SelectListItem()
                {
                    Text = x.RoleId,
                    Value = x.RoleId
                }).ToList());

                return state;
            }
            catch (Exception ex)
            {
                _Log.LogError("QueueService - GetStateById - " + ex);
            }
            return null;
        }

        public List<int> GetStateIdsByQueueId(int queueid, int caseformid)
        {
            return _db.QueueToState.Where(x => x.QueueId == queueid && caseformid == x.CaseFormId && x.IsQueue == true).Select(y => y.StateId).ToList();
        }

        public int GetStateByName(int tenantid, string name)
        {
            try
            {

                var state = _db.State.Where(x => x.SystemName == name && tenantid == x.TenantId).Select(y => y.Id).FirstOrDefault();

                return state;
            }
            catch (Exception ex)
            {
                _Log.LogError("QueueService - GetStateByName - " + ex);
            }
            return 0;
        }

        public StateViewModel GetStatePermissionsById(int tenantid, int id)
        {
            return _mapper.Map<StateViewModel>(_db.State.Where(x => x.Id == id && tenantid == x.TenantId).FirstOrDefault());

        }

        public List<QueueViewModel> GetBackendQueuesByActiveTenantIdAndRole()
        {
            try
            {
                string loggedUser = _commonService.getLoggedInUserId();

                string roleId = _commonService.GetRoleIdByUserId(loggedUser);


                int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                if (tenantid == 0)
                {
                    return null;
                }

                var queues = _mapper.Map<List<QueueViewModel>>(_db.Queue.Include(a => a.QueueForForm).ThenInclude(b => b.QueuePermissions.Where(a => a.QueueForForm.AllUser == true || a.RoleId == roleId)).OrderBy(y => y.QueueForForm.First().Order).ToList());



                return queues;
            }
            catch (Exception ex)
            {
                _Log.LogError("QueueService - GetBackendQueuesByActiveTenantIdAndRole - " + ex);
            }
            return null;
        }

        public string GetTenantQueueNameByIdentifier(string e)
        {
            var vm = this.GetQueueByQueueIdentifier(e);
            if (vm != null)
            {
                return vm.Name;
            }
            return "All";

        }

        public List<StateViewModel> GetSelectStateList(int tenantid)
        {

            return _mapper.Map<List<StateViewModel>>(_db.State
                            .Where(x => tenantid == x.TenantId).Include(y => y.FromStates)
                            .ToList());
        }
        public List<ActionsViewModel> GetSelectActionList(int tenantid)
        {

            return _mapper.Map<List<ActionsViewModel>>(_db.Actions.Where(x => tenantid == x.TenantId).ToList());
        }
        public List<QueueViewModel> GetSelectQueueList(int tenantid)
        {

            return _mapper.Map<List<QueueViewModel>>(_db.Queue.Where(x => tenantid == x.TenantId).Include(y => y.QueueToState).ToList());
        }

        private async Task<int> CreateQueueAsync(Queue queue)
        {
            var loggedUser = _commonService.getLoggedInUserId();
            var fullName = _commonService.GetUserFullName().Result;

            queue.Id = 0;
            queue.CreatedBy = loggedUser;
            queue.CreatedAt = DateTime.Now;
            await _db.Queue.AddAsync(queue);

            await _db.SaveChangesAsync();

            await _activityLogService.CreateLog(loggedUser, "New Queue created <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/queue/" + _utils.EncryptId(queue.Id) + "/edit.html'>" + queue.Name + "</a>. Created By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

            return queue.Id;
        }

        private async Task UpdateQueueAsync(Queue queue, bool relationremove)
        {
            var loggedUser = _commonService.getLoggedInUserId();
            var fullName = _commonService.GetUserFullName().Result;

            var temp = _db.QueueForForm.Where(c => c.QueueId == queue.Id && queue.QueueForForm.Count() > 0 && c.CaseFormId == queue.QueueForForm.FirstOrDefault().CaseFormId);

            if (temp.Count() > 0)
            {

                var qffid = temp.FirstOrDefault().Id;

                _db.QueuePermission.RemoveRange(_db.QueuePermission.Where(x => x.QueueForFormId == qffid).ToList());

                _db.QueueForForm.RemoveRange(_db.QueueForForm.Where(x => x.QueueId == queue.Id && x.CaseFormId == queue.QueueForForm.FirstOrDefault().CaseFormId).ToList());
            }


            foreach (QueueForForm item in queue.QueueForForm)
            {
                QueueForForm itm = new QueueForForm
                {
                    QueueId = queue.Id,
                    CaseFormId = item.CaseFormId,
                    Order = item.Order,
                    AllUser = false,
                };
                _db.QueueForForm.Add(itm);
                _db.SaveChanges();
                if (item.QueuePermissions.Count > 0)
                {
                    foreach (var b in item.QueuePermissions)
                    {
                        QueuePermission p = new QueuePermission
                        {
                            DisplayName = b.DisplayName,
                            QueueForFormId = itm.Id,
                            RoleId = b.RoleId,

                        };
                        _db.QueuePermission.Add(p);
                        _db.SaveChanges();
                    }
                }
            }


            if (relationremove == true)
            {
                queue.QueueForForm = null;
            }

            queue.UpdatedBy = loggedUser;
            Queue updateQueue = _db.Queue.Where(x => x.Id == queue.Id).SingleOrDefault();
            updateQueue.Name = queue.Name;
            updateQueue.Icon = queue.Icon;
            updateQueue.UpdatedAt = queue.UpdatedAt;
            updateQueue.UpdatedBy = queue.UpdatedBy;
            updateQueue.Color = queue.Color;
            updateQueue.UrlIdentifier = queue.UrlIdentifier;
            updateQueue.Status = queue.Status;
            //_db.Queue.Attach(queue).State = EntityState.Modified;
            _db.Queue.Update(updateQueue);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }


            await _activityLogService.CreateLog(loggedUser, "Queue updated <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/queue/" + _utils.EncryptId(queue.Id) + "/edit.html'>" + queue.Name + "</a>. Updated By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

        }

        public async Task<QueueViewModel> CreateOrUpdateQueueAsync(QueueViewModel qvm)
        {
            bool remove = false;
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (qvm.QueueForForm != null)
                    {
                        foreach (var a in qvm.QueueForForm.ToList())
                        {
                            if (a.CaseFormId != 0)
                            {
                                foreach (var b in a.QueuePermissions.ToList())
                                {
                                    if (b.DisplayName == null)
                                        a.QueuePermissions.Remove(b);
                                }
                            }
                            else
                            {
                                remove = true;
                            }

                        }
                    }

                    if (remove)
                    {
                        int count = qvm.QueueForForm.Where(x => x.CaseFormId == 0).Count();
                        for (int i = 0; i < count; i++)
                        {
                            qvm.QueueForForm.Remove(qvm.QueueForForm.Where(x => x.CaseFormId == 0).FirstOrDefault());
                        }

                    }

                    qvm.UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now);

                    var queue = _mapper.Map<Queue>(qvm);

                    //var loggedUser = _commonService.getLoggedInUserId();
                    //var fullName = _commonService.GetUserFullName().Result;

                    if (qvm.Id == 0)
                    {
                        qvm.Id = await CreateQueueAsync(queue);

                    }
                    else
                    {
                        await UpdateQueueAsync(queue, qvm.RemoveRelation);

                    }

                    //await UpdateQueueToStateAsync(qvm);

                    transaction.Commit();
                    return qvm;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return qvm;
                }
            }

        }

        private async Task<int> CreateStateAsync(State state)
        {
            var loggedUser = _commonService.getLoggedInUserId();
            var fullName = _commonService.GetUserFullName().Result;

            state.Id = 0;
            state.CreatedBy = loggedUser;
            state.CreatedAt = DateTime.Now;
            await _db.State.AddAsync(state);

            await _db.SaveChangesAsync();

            await _activityLogService.CreateLog(loggedUser, "New State created <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/state/" + _utils.EncryptId(state.Id) + "/edit.html'>" + state.SystemName + "</a>. Created By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

            return state.Id;
        }

        private async Task UpdateStateAsync(State state, bool relationremove)
        {
            var loggedUser = _commonService.getLoggedInUserId();
            var fullName = _commonService.GetUserFullName().Result;

            var temp = _db.StateForForm.Where(c => c.StateId == state.Id && state.StateForForm.Count() > 0 && c.CaseFormId == state.StateForForm.FirstOrDefault().CaseFormId);

            if (temp.Count() > 0)
            {
                var sffid = temp.FirstOrDefault().Id;

                _db.StatePermission.RemoveRange(_db.StatePermission.Where(x => x.StateForFormId == sffid).ToList());

                _db.StateForForm.RemoveRange(_db.StateForForm.Where(x => x.StateId == state.Id && x.CaseFormId == state.StateForForm.FirstOrDefault().CaseFormId).ToList());
            }

            foreach (StateForForm item in state.StateForForm)
            {
                StateForForm itm = new StateForForm
                {
                    StateId = state.Id,
                    CaseFormId = item.CaseFormId,
                    Order = item.Order,
                    AllUser = false,
                    Icon = item.Icon,
                    FirstBackState = item.FirstBackState,
                    FirstFrontState = item.FirstFrontState
                };
                _db.StateForForm.Add(itm);
                _db.SaveChanges();
                if (item.StatePermissions.Count > 0)
                {
                    foreach (var b in item.StatePermissions)
                    {
                        StatePermission p = new StatePermission
                        {
                            DisplayName = b.DisplayName,
                            StateForFormId = itm.Id,
                            RoleId = b.RoleId,
                            CanEdit = b.CanEdit,
                            ViewMode = b.ViewMode

                        };
                        _db.StatePermission.Add(p);
                        _db.SaveChanges();
                    }
                }
            }



            if (relationremove == true)
            {
                state.StateForForm = null;
            }
            //else
            //{
            //    if (state.StateForForm.FirstOrDefault().FirstBackState == true || state.StateForForm.FirstOrDefault().FirstFrontState == true)
            //    {
            //        List<StateForForm> sff = _db.StateForForm.Where(x => (x.FirstFrontState == true || x.FirstBackState == true) && x.StateId != state.Id && x.CaseFormId == state.StateForForm.FirstOrDefault().CaseFormId).ToList();
            //        foreach (var item in sff)
            //        {
            //            if (item.FirstBackState == true && state.StateForForm.FirstOrDefault().FirstBackState == true)
            //            {
            //                item.FirstBackState = false;
            //            }
            //            if (item.FirstFrontState == true && state.StateForForm.FirstOrDefault().FirstFrontState == true)
            //            {
            //                item.FirstFrontState = false;
            //            }

            //            _db.StateForForm.Update(item);
            //        }
            //    }
            //}
            //foreach (var item in state.StateForForm)
            //{
            //    foreach (var item2 in item.StatePermissions)
            //    {
            //        if (item2.DisplayName!=null && item2.DisplayName != "")
            //            _db.StatePermission.Add(item2);
            //    }
            //}
            state.UpdatedBy = loggedUser;
            State updateState = _db.State.Where(x => x.Id == state.Id).SingleOrDefault();
            updateState.ActionName = state.ActionName;
            updateState.SystemName = state.SystemName;
            updateState.Icon = state.Icon;
            updateState.UpdatedAt = state.UpdatedAt;
            updateState.UpdatedBy = state.UpdatedBy;
            updateState.Color = state.Color;
            updateState.Status = state.Status;
            //_db.Queue.Attach(queue).State = EntityState.Modified;
            _db.State.Update(updateState);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            //_db.State.Attach(state).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            await _activityLogService.CreateLog(loggedUser, "State updated <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/state/" + _utils.EncryptId(state.Id) + "/edit.html'>" + state.SystemName + "</a>. Updated By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

        }

        public async Task<StateViewModel> CreateOrUpdateStateAsync(StateViewModel svm)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var a in svm.StateForForm.ToList())
                    {
                        foreach (var b in a.StatePermissions.ToList())
                        {
                            if (b.DisplayName == null)
                                a.StatePermissions.Remove(b);
                        }
                    }
                    svm.UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now);

                    var state = _mapper.Map<State>(svm);

                    if (svm.Id == 0)
                    {

                        svm.Id = await CreateStateAsync(state);
                    }
                    else
                    {

                        await UpdateStateAsync(state, svm.RemoveRelation);

                    }

                    transaction.Commit();
                    return svm;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return svm;
                }
            }
        }

        public async Task<bool> DeleteQueueById(int id)
        {
            var queue = await _db.Queue.FindAsync(id);
            string name = queue.Name;
            if (queue != null)
            {
                if (StepsBeforeQueueDelete(id))
                {
                    _db.Queue.Remove(queue);
                    _db.SaveChanges();

                    var loggedUser = _commonService.getLoggedInUserId();
                    var fullName = _commonService.GetUserFullName().Result;

                    await _activityLogService.CreateLog(loggedUser, "Queue Deleted " + name + ". Deleted By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                    return true;
                }
            }

            _Log.LogError("QueueService - DeleteQueueById - " + id + " - : ");
            return false;
        }

        //for steps to occur before deleting Queue
        private bool StepsBeforeQueueDelete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //Remove from Queue to States
                    _db.QueueToState.RemoveRange(_db.QueueToState.Where(x => x.QueueId == id).ToList());
                    //Reset Queue For Form
                    _db.QueueForForm.RemoveRange(_db.QueueForForm.Where(x => x.Queue.Id == id).ToList());
                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return false;

        }

        private bool checkQueueForDeletion(int id)
        {
            if (_db.QueueToState.Where(x => x.Queue.Id == id).ToList().Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> ActiveQueueById(int id)
        {
            var queue = await _db.Queue.FindAsync(id);
            if (queue != null)
            {
                queue.Status = true;
                _db.Queue.Update(queue);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "Queue changed to Active <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/queue/" + _utils.EncryptId(queue.Id) + "/edit.html'>" + queue.Name + "</a>. Changed By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            _Log.LogError("QueueService - ActiveQueueById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveQueueById(int id)
        {
            var queue = await _db.Queue.FindAsync(id);
            if (queue != null)
            {
                queue.Status = false;
                _db.Queue.Update(queue);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "Queue changed to InActive <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage_queue/queue/" + _utils.EncryptId(queue.Id) + "/edit.html'>" + queue.Name + "</a>. Changed By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            _Log.LogError("QueueService - InactiveQueueById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteStateById(int id)
        {
            var state = await _db.State.FindAsync(id);
            string name = state.SystemName;
            if (state != null)
            {

                if (StepsBeforeStateDelete(id))
                {
                    _db.State.Remove(state);
                    _db.SaveChanges();
                    var loggedUser = _commonService.getLoggedInUserId();
                    var fullName = _commonService.GetUserFullName().Result;
                    await _activityLogService.CreateLog(loggedUser, "State Deleted " + name + ". Deleted By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                    return true;
                }

            }
            _Log.LogError("QueueService - DeleteStateById - " + id + " - : ");
            return false;
        }

        private bool StepsBeforeStateDelete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //Remove from StateToState table
                    _db.StateToState.RemoveRange(_db.StateToState.Where(x => x.FromStateId == id || x.ToStateId == id).ToList());

                    //Remove from QueueToState                
                    _db.QueueToState.RemoveRange(_db.QueueToState.Where(x => x.StateId == id).ToList());

                    //Resetting State For Form 
                    _db.StateForForm.RemoveRange(_db.StateForForm.Where(x => x.StateId == id).ToList());

                    //Remove from Activity
                    List<ActivityLog> activities = _db.ActivityLog.Where(x => x.StateId == id).ToList();
                    activities.ForEach(x => _db.ActivityLog.Remove(x));
                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }

            }
            return false;
        }

        private bool CheckStateForDeletion(int id)
        {
            int count = 0;
            count = _db.StateToState.Where(x => x.FromState.Id == id || x.ToState.Id == id).ToList().Count
                + _db.QueueToState.Where(x => x.State.Id == id).ToList().Count;
            if (count > 0)
            { return false; }
            else { return true; }
        }

        public async Task<bool> ActiveStateById(int id)
        {
            var state = await _db.State.FindAsync(id);
            if (state != null)
            {
                state.Status = true;
                _db.State.Update(state);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "State changed to Active <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/state/" + _utils.EncryptId(state.Id) + "/edit.html'>" + state.SystemName + "</a>. Changed By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            _Log.LogError("QueueService - ActiveQueueById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveStateById(int id)
        {
            var state = await _db.State.FindAsync(id);
            if (state != null)
            {
                state.Status = false;
                _db.State.Update(state);
                _db.SaveChanges();

                var loggedUser = _commonService.getLoggedInUserId();
                var fullName = _commonService.GetUserFullName().Result;

                await _activityLogService.CreateLog(loggedUser, "State changed to InActive <a href ='/admin" + _utils.GetTenantForUrl(false) + "/manage/state/" + _utils.EncryptId(state.Id) + "/edit.html'>" + state.SystemName + "</a>. Changed By  <a href = '/admin" + _utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            _Log.LogError("QueueService - InactiveQueueById - " + id + " - : ");
            return false;
        }

        public bool IsDuplicateUrl(string url, int id)
        {

            int tenantid = _db.Tenant.Where(x => x.Identifier == _utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();

            if (id == 0)
            {
                return (!_db.Queue.Any(d => d.UrlIdentifier == url && (d.TenantId == tenantid || tenantid == 0)));
            }
            else
            {
                return (!_db.Queue.Any(d => d.UrlIdentifier == url && d.Id != id && (d.TenantId == tenantid || tenantid == 0)));
            }
        }

        public bool IsDuplicateQueueOrder(int id, int order, int caseformid)
        {
            if (order != 0)
            {
                int tenantid = _db.Tenant.Where(x => x.Identifier == _utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();

                if (id == 0)
                {
                    return (!_db.Queue.Any(d => d.QueueForForm.Any(x => x.CaseFormId == caseformid && x.Order == order) && (d.TenantId == tenantid || tenantid == 0)));
                }
                else
                {
                    return (!_db.Queue.Any(d => d.QueueForForm.Any(x => x.CaseFormId == caseformid && x.Order == order && x.Id != id) && (d.TenantId == tenantid || tenantid == 0)));
                }
            }
            else
            {
                return true;
            }

        }

        public bool IsDuplicateStateOrder(int id, int order, int caseformid)
        {

            if (order != 0)
            {
                int tenantid = _db.Tenant.Where(x => x.Identifier == _utils.GetTenantFromSession()).Select(b => b.Id).FirstOrDefault();

                if (id == 0)
                {
                    return (!_db.State.Any(d => d.StateForForm.Any(x => x.CaseFormId == caseformid && x.Order == order) && (d.TenantId == tenantid || tenantid == 0)));
                }
                else
                {
                    //var temp1 = (!_db.StateForForm.Any(x => x.CaseFormId == caseformid && x.Order == order));
                    //var temp3 = (_db.StateForForm.Where(x => x.CaseFormId == caseformid && x.Order == order && x.StateId != id));
                    //return (!_db.StateForForm.Any(x => x.CaseFormId == caseformid && x.Order == order && x.Id != id));
                    return (!_db.State.Any(d => d.StateForForm.Any(x => x.CaseFormId == caseformid && x.Order == order && x.Id != id) && (d.TenantId == tenantid || tenantid == 0)));
                }
            }
            else
            {
                return true;
            }

        }

        public List<QueueListViewModel> GetQueueListForClaim()
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            string loggeduser = _commonService.getLoggedInUserId();

            string roleid = _commonService.GetRoleIdByUserId(loggeduser);

            //name should be based on used form

            var result = _db.Queue.Include(x => x.QueueForForm).ThenInclude(y => y.QueuePermissions.Where(a => a.QueueForForm.AllUser == true || a.RoleId == roleid))
                    .Where(c => c.TenantId == tenantid).Select(b => new QueueListViewModel { Id = b.Id, Name = b.Name }).ToList();

            return result;
        }

        //public QueueListViewModel GetCasesByRoleIdQueueId(int queueid)
        //{

        //    string loggedUser = _commonService.getLoggedInUserId();

        //    string roleid = _commonService.GetRoleIdByUserId(loggedUser);

        //    bool isAdmin = _commonService.IsSuperAdmin().Result;

        //    var caseCount = _db.Queue
        //                        .Join(_db.QueueToState, que => que.Id, qts => qts.QueueId, (c, d) => new { que = c, qts = d })
        //                        .Where(a => a.qts.QueueId == a.que.Id && a.que.Id == queueid)
        //                        .Join(_db.Case, qts => qts.qts.StateId, cas => cas.StateId, (x, z) => new { qts = x.qts, cas = z })
        //                        .Where(y => y.cas.StateId == y.qts.StateId && (y.cas.UserAccessId == loggedUser || string.IsNullOrWhiteSpace(y.cas.UserAccessId)))
        //                        .GroupJoin(_db.RoleClaims, que => que.qts.Queue.RoleId, rc => rc.RoleId, (a, b) => new { que = a, rc = b })
        //                                .SelectMany(y => y.rc.DefaultIfEmpty(), (a, b) => new { que = a.que, rc = b })
        //                                .Where(z => z.rc.ClaimValue.Equals("backend", StringComparison.OrdinalIgnoreCase) && z.rc.ClaimType.Equals("Side", StringComparison.OrdinalIgnoreCase))
        //                        .ToList();


        //    var result = new QueueListViewModel
        //    {
        //        Count = caseCount.Count(),
        //        Id = queueid,
        //        Name = ""
        //    };

        //    return result;
        //}

        public string GetQueueNameByStateId(int stateId)
        {

            string loggeduser = _commonService.getLoggedInUserId();
            string roleid = _commonService.GetRoleIdByUserId(loggeduser);

            return _db.Queue.Include(a => a.QueueForForm).ThenInclude(b => b.QueuePermissions.Where(a => a.QueueForForm.AllUser == true || a.RoleId == roleid)).SelectMany(x => x.QueueToState).Where(b => b.StateId == stateId).Select(y => y.Queue.Name).FirstOrDefault();

        }

        public string GetStateNameById(int stateId, int caseformid)
        {
            string loggeduser = _commonService.getLoggedInUserId();
            string roleid = _commonService.GetRoleIdByUserId(loggeduser);
            string stateName = "";
            stateName = _db.StateForForm
                .Join(_db.StatePermission, sff => sff.Id, sps => sps.StateForFormId, (a, b) => new { sff = a, sps = b }).Where(a => a.sff.StateId == stateId && a.sff.CaseFormId == caseformid && a.sps.RoleId == roleid).Select(y => y.sps.DisplayName).FirstOrDefault();
            if (stateName == null || stateName == "")
                stateName = _db.State.Where(x => x.Id == stateId).Select(y => y.SystemName).FirstOrDefault();
            return stateName;
        }
        public string GetStateNameByStateId(int stateId)
        {

            string stateName = "";
            stateName = _db.State.Where(x => x.Id == stateId).Select(y => y.SystemName).FirstOrDefault();
            return stateName;
        }
        public List<SelectListItem> GetStateSelectList()
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            return _db.State
                        .Where(b => b.TenantId == tenantid)
                        //.OrderBy(j => j.Order)
                        .Select(x => new SelectListItem
                        {
                            Text = x.SystemName,
                            Value = x.Id.ToString()
                        }).ToList();
        }

        public List<SelectListItem> GetStateSelectListByFormId(int formid, string roleid)
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var temp = _db.StateForForm
                        .Where(b => b.CaseFormId == formid).Include(x => x.State)
                        .OrderBy(a => a.Order).ToList();

            List<SelectListItem> list = _db.StateForForm
                        .Where(b => b.CaseFormId == formid).Include(x => x.State).Include(y => y.StatePermissions)
                        .OrderBy(a => a.Order).ThenByDescending(b => b.FirstFrontState)
                        //.OrderBy(j => j.Order)
                        .Select(x => new SelectListItem
                        {
                            Text = (x.StatePermissions.Any(z => z.RoleId == roleid || roleid == " ") ? x.StatePermissions.Where(w => w.RoleId == roleid || roleid == " ").Select(c => c.DisplayName).FirstOrDefault() : x.State.SystemName),
                            Value = x.StateId.ToString()
                        }).ToList();

            return list;
        }

        //state roles based permissions need to change code
        public List<SelectListItem> GetStateSelectListByPermission(int formid, string roleid)
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            return _db.StateForForm
                        .Where(b => b.CaseFormId == formid)
                        //.OrderBy(j => j.Order)
                        .Select(x => new SelectListItem
                        {
                            Text = "",
                            Value = x.Id.ToString()
                        }).ToList();
        }

        public bool IsStateInRole(int id)
        {
            string loggeduser = _commonService.getLoggedInUserId();
            string roleId = _commonService.GetRoleIdByUserId(loggeduser);

            bool isAdmin = _commonService.IsSuperAdmin().Result;

            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            StateViewModel svm = GetStateById(tenantid, id);

            if (svm.StateForForm.SelectMany(x => x.StatePermissions).Where(b => b.RoleId == roleId).Count() > 0)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// gets the queue count of logged in tenant user
        /// </summary>
        /// <returns></returns>
        public int GetTenantQueueCount()
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            return _db.Queue.Where(x => x.TenantId == tenantid).ToList().Count();
        }

        //Remove Queue 
        public bool RemoveQueueById(int id)
        {
            try
            {
                List<QueueToState> queueToState = _db.QueueToState.Where(x => x.QueueId == id).ToList();
                queueToState.ForEach(x => _db.QueueToState.Remove(x));
                Queue queue = _db.Queue.Where(x => x.Id == id).FirstOrDefault();
                _db.Queue.Remove(queue);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Queue> GetAddedQueue(DateTime startDatetime)
        {
            string loggedUser = _commonService.getLoggedInUserId();
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            List<Queue> queues = _db.Queue.Where(x => x.CreatedBy == loggedUser && x.TenantId == tenantid && x.CreatedAt >= startDatetime).ToList();
            return queues;
        }

        public List<StateViewModel> GetAllStates()
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            return _mapper.Map<List<StateViewModel>>(_db.State.Where(x => x.TenantId == tenantid).ToList());
        }

        public bool SaveStateWorkFlow(int caseformid, List<JsonStateViewModel> jsvm)
        {
            var sts = new List<StateToState>();
            try
            {
                //var objsa = new List<StateActions>();m
                //_db.StateActions.RemoveRange(_db.StateActions.Where(e => e.CaseFormId == caseformid));
                _db.StateToState.RemoveRange(_db.StateToState.Where(e => e.CaseFormId == caseformid));

                _db.SaveChanges();

                foreach (var items in jsvm)
                {
                    int? actionid = null;
                    if (items.ActionId != null)
                    {
                        string[] actionids = items.ActionId.Split('-');
                        actionid = Convert.ToInt32(actionids[1]);
                    }
                    var temp1 = new StateToState
                    {
                        Id = Int32.Parse(items.First.State.ToString() + items.Last.State.ToString() + caseformid.ToString()),
                        Aero = items.Last.Aero,
                        CaseFormId = caseformid,
                        FromStateId = items.First.State,
                        ToStateId = items.Last.State,
                        LinePosX = items.First.LineXPos,
                        LinePosY = items.First.LineYPos,
                        StatePosX = items.First.StateXPos,
                        StatePosY = items.First.StateYPos,
                        ActionsId = actionid
                    };

                    var temp2 = new StateToState
                    {
                        Id = Int32.Parse(items.Last.State.ToString() + items.First.State.ToString() + caseformid.ToString()),
                        Aero = items.First.Aero,
                        CaseFormId = caseformid,
                        FromStateId = items.Last.State,
                        ToStateId = items.First.State,
                        LinePosX = items.Last.LineXPos,
                        LinePosY = items.Last.LineYPos,
                        StatePosX = items.Last.StateXPos,
                        StatePosY = items.Last.StateYPos,
                        ActionsId = actionid
                    };
                    if (temp1.Id != 0 && temp2.Id != 0)
                    {
                        sts.Add(temp1);
                        sts.Add(temp2);
                        //
                    }

                    //if (items.ActionId != null)
                    //{
                    //    string[] actionids = items.ActionId.Split('-');
                    //    var temp3 = new StateActions
                    //    {
                    //        Id = Int32.Parse(items.Last.State.ToString() + items.First.State.ToString() + caseformid.ToString()),
                    //        StatetoStateId = Int32.Parse(items.First.State.ToString() + items.Last.State.ToString() + caseformid.ToString()),
                    //        CaseFormId = caseformid,
                    //        ActionsId = Convert.ToInt32(actionids[1])
                    //    };
                    //    objsa.Add(temp3);
                    //}
                }

                _db.StateToState.AddRange(sts);
                //_db.StateActions.AddRange(objsa);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }


            return true;
        }

        public bool SaveQueueWorkFlow(int caseformid, List<JsonStateViewModel> jsvm)
        {

            _db.QueueToState.RemoveRange(_db.QueueToState.Where(e => e.CaseFormId == caseformid));
            List<QueueForForm> queueForForms = _db.QueueForForm.Where(x => x.CaseFormId == caseformid).ToList();

            _db.SaveChanges();
            foreach (var items in jsvm)
            {
                var qts = new List<QueueToState>();
                bool FirstIsQueue = items.First.Type == "queue" ? true : false;
                int FirstQueueId = items.First.Type == "queue" ? items.First.State : items.Last.State;
                int FirstStateId = items.First.Type == "queue" ? items.Last.State : items.First.State;

                QueueForForm queueForForm = new QueueForForm();
                queueForForm = queueForForms.Where(x => x.QueueId == FirstQueueId && x.CaseFormId == caseformid).FirstOrDefault();
                if (queueForForm != null)
                {
                    queueForForms.Remove(queueForForm);
                }
                else
                {
                    try
                    {
                        queueForForm = new QueueForForm();
                        queueForForm.CaseFormId = caseformid;
                        queueForForm.QueueId = FirstQueueId;
                        queueForForm.AllUser = false;
                        queueForForm.Order = 0;
                        _db.QueueForForm.Add(queueForForm);
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }

                }

                var temp1 = new QueueToState
                {
                    Id = "q" + FirstQueueId + "s" + FirstStateId + caseformid.ToString() + jsvm.IndexOf(items),
                    IsQueue = FirstIsQueue,
                    CaseFormId = caseformid,
                    QueueId = FirstQueueId,
                    StateId = FirstStateId,
                    LinePosX = items.First.LineXPos,
                    LinePosY = items.First.LineYPos,
                    PosX = items.First.StateXPos,
                    PosY = items.First.StateYPos
                };


                bool LastIsQueue = items.Last.Type == "queue" ? true : false;
                int LastQueueId = items.Last.Type == "queue" ? items.Last.State : items.First.State;
                int LastStateId = items.Last.Type == "queue" ? items.First.State : items.Last.State;
                var temp2 = new QueueToState
                {
                    Id = "s" + LastStateId + "q" + LastQueueId + caseformid.ToString() + jsvm.IndexOf(items),
                    IsQueue = LastIsQueue,
                    CaseFormId = caseformid,
                    StateId = LastStateId,
                    QueueId = LastQueueId,
                    LinePosX = items.Last.LineXPos,
                    LinePosY = items.Last.LineYPos,
                    PosX = items.Last.StateXPos,
                    PosY = items.Last.StateYPos
                };

                if (temp1.Id != null && temp2.Id != null)
                {
                    qts.Add(temp1);
                    qts.Add(temp2);
                    try
                    {
                        _db.QueueToState.AddRange(qts);
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            _db.QueueForForm.RemoveRange(queueForForms);
            _db.SaveChanges();


            return true;
        }

        public int GetCaseFormWorkFlowCount(int caseFormId)
        {
            return _db.WorkFlowState.Where(x => x.CaseFormId == caseFormId).ToList().Count();
        }

        public bool RemoveWorkflowByCaseFormId(int caseFormId)
        {
            List<StateToState> stateToStates = _db.StateToState.Where(x => x.CaseFormId == caseFormId).ToList();
            foreach (var item in stateToStates)
            {
                _db.StateToState.Remove(item);
            }
            _db.SaveChanges();
            return true;

        }

        //for from creation get frist state
        public int GetFirstState(int caseFormId, string side)
        {

            if (side == "back")
            {
                var temp = _db.StateForForm.Where(x => x.CaseFormId == caseFormId && x.FirstBackState == true).FirstOrDefault();

                if (temp != null)
                {
                    return temp.StateId;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                var temp = _db.StateForForm.Where(x => x.CaseFormId == caseFormId && x.FirstFrontState == true).FirstOrDefault();

                if (temp != null)
                {
                    return temp.StateId;
                }
                else
                {
                    return 0;
                }

            }

        }

        //should add more details about permissions later
        public List<SelectListItem> GetCaseMovement(int caseformid, int caseid)
        {

            var loggeduser = _commonService.getLoggedInUserId();

            string roleid = _commonService.GetRoleIdByUserId(loggeduser);

            var isadmin = _commonService.IsSuperAdmin().Result;

            //return _db.StateToState
            //                .Where(y => y.CaseFormId == caseformid && y.Aero == true)
            //                .Join(_db.Case, sts => sts.FromStateId, cas => cas.StateId, (a, b) => new { sts = a, cas = b })
            //                .Where(z => z.cas.Id == caseid)
            //                .Join(_db.StateForForm, sts => sts.sts.FromStateId, sff => sff.StateId, (c, d) => new { sts = c, sff = d })
            //                .GroupJoin(_db.StatePermission, sff => sff.sff.Id, sp => sp.StateForFormId, (e, f) => new { sff = e, sp = f })
            //                .SelectMany(v => v.sp.DefaultIfEmpty(), (e, f) => new { sff = e.sff, sp = f })
            //                .Where(x => x.sff.sff.CaseFormId == caseformid && (x.sff.sff.AllUser == true || x.sff.sff.StatePermissions.Any(u => u.RoleId == roleid) || isadmin == true))
            //                .Select(w => new { id = w.sff.sts.sts.ToState.Id, name = w.sff.sts.sts.ToState.ActionName, need_reason = w.sff.sts.sts.ToState.NeedReason, notify_user = w.sff.sts.sts.ToState.NotifyUser, can_delete = w.sff.sts.sts.ToState.CanDelete, can_edit = w.sff.sts.sts.ToState.CanEdit, user_access = w.sff.sts.sts.ToState.UserAccess }).ToList();


            /*
             * return _db.StateToState
                            .Where(y => y.CaseFormId == caseformid && y.Aero == true)
                            .Join(_db.Case, sts => sts.FromStateId, cas => cas.StateId, (a, b) => new { sts = a, cas = b })
                            .Where(z => z.cas.Id == caseid)
                            .Join(_db.StateForForm, sts => sts.sts.FromStateId, sff => sff.StateId, (c, d) => new { sts = c, sff = d })
                            .GroupJoin(_db.StatePermission, sff => sff.sff.Id, sp => sp.StateForFormId, (e, f) => new { sff = e, sp = f })
                            .SelectMany(v => v.sp.DefaultIfEmpty(), (e, f) => new { sff = e.sff, sp = f })
                            .Where(x => x.sff.sff.CaseFormId == caseformid && (x.sff.sff.AllUser == true || x.sff.sff.StatePermissions.Any(u => u.RoleId == roleid) || isadmin == true))
                            .Select(w => new SelectListItem { Value = w.sff.sts.sts.ToState.Id.ToString(), Text = w.sff.sts.sts.ToState.ActionName }).Distinct().ToList();

                            */

            List<SelectListItem> states = GetStateSelectListByFormId(caseformid, roleid);
            List<SelectListItem> items = _db.WorkFlowState
                           .Where(y => y.CaseFormId == caseformid)
                           .Join(_db.Case, sts => sts.FromStateId, cas => cas.StateId, (a, b) => new { sts = a, cas = b })
                           .Where(z => z.cas.Id == caseid)
                           .Join(_db.StateForForm, sts => sts.sts.FromStateId, sff => sff.StateId, (c, d) => new { sts = c, sff = d })
                           .GroupJoin(_db.StatePermission, sff => sff.sff.Id, sp => sp.StateForFormId, (e, f) => new { sff = e, sp = f })
                           .SelectMany(v => v.sp.DefaultIfEmpty(), (e, f) => new { sff = e.sff, sp = f })
                           .Where(x => x.sff.sff.CaseFormId == caseformid && (x.sff.sff.AllUser == true || x.sff.sff.StatePermissions.Any(u => u.RoleId == roleid) || isadmin == true))
                           .Select(w => new SelectListItem { Value = w.sff.sts.sts.ToStateId.ToString(), Text = "" }).Distinct().ToList();
            List<SelectListItem> return_items = new List<SelectListItem>();
            foreach (var item in items)
            {
                var txt = states.Where(x => x.Value == item.Value)?.SingleOrDefault()?.Text?.ToString() ?? "Unknown";
                return_items.Add(new SelectListItem() { Value = item.Value, Text = txt });

            }
            var stateId = from c in _db.Case
                          join s in _db.State on c.StateId equals s.Id
                          where c.Id == caseid
                          select new { Value = s.Id, Text = s.ActionName };
            if (stateId.Any())
            {
                SelectListItem selectItem = new SelectListItem()
                {
                    Disabled = true,
                    Selected = true,
                    Text = stateId.FirstOrDefault().Text,
                    Value = stateId.FirstOrDefault().Value.ToString()
                };
                return_items.Add(selectItem);
            }
            return return_items;
        }
        public string GetQueueNameByFormId(int caseformid, int stateId)
        {
            try
            {
                var username = from q in _db.Queue
                               join qts in _db.QueueToState on q.Id equals qts.QueueId
                               join qff in _db.QueueForForm on q.Id equals qff.QueueId
                               join qp in _db.QueuePermission on qff.Id equals qp.QueueForFormId into leftJoined2
                               from y in leftJoined2.DefaultIfEmpty()
                               where qts.StateId == stateId && qts.CaseFormId == caseformid && qts.IsQueue == true
                               select new { Name = q.Name };

                return username.FirstOrDefault().Name;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public UserViewModel GetCaseOwner(int caseFormId, int stateId, string userId)
        {
            string user = string.Empty;
            int queueId = _db.QueueToState.Where(x => x.StateId == stateId && x.CaseFormId == caseFormId).FirstOrDefault().QueueId;
            string roleId = _commonService.GetRoleIdByUserId(userId);
            int queueForForm = _db.QueueForForm.Where(x => x.QueueId == queueId && x.CaseFormId == caseFormId).FirstOrDefault().Id;
            QueuePermission queuePermission = _db.QueuePermission.Where(x => x.QueueForFormId == queueForForm && x.RoleId != roleId).LastOrDefault();
            if (queuePermission != null && queuePermission.RoleId != null)
            {
                user = _db.UserRoles.Where(x => x.RoleId == queuePermission.RoleId).FirstOrDefault().UserId;
            }
            else
            {
                user = userId;
            }
            ApplicationUser user1 = _db.Users.Where(x => x.Id == user).SingleOrDefault();
            UserViewModel userViewModel = _mapper.Map<UserViewModel>(_db.Users.Where(x => x.Id == user).SingleOrDefault());
            return userViewModel;
        }

        public ElementStateViewModel SaveOrUpdateStateObject(ElementStateViewModel elementStateViewModel)
        {
            ElementState elementWorkflowState = new ElementState()
            {
                Id = elementStateViewModel.Id,
                Name = elementStateViewModel.Name,
                FormId = elementStateViewModel.FormId,
                isDefaultEnd = elementStateViewModel.isDefaultEnd,
                ForEventType = elementStateViewModel.ForEventType,
                TenantId = elementStateViewModel.TenantId,
                ElementId = elementStateViewModel.ElementId,
                Type = elementStateViewModel.Type,
                CreatedAt = elementStateViewModel.CreatedAt
            };
            if (elementStateViewModel.Id != 0)
            {
                _db.ElementState.Update(elementWorkflowState);
            }
            else
            {
                _db.ElementState.Add(elementWorkflowState);
            }

            _db.SaveChanges();
            elementStateViewModel.Id = elementWorkflowState.Id;
            return elementStateViewModel;
        }

        public ElementStateViewModel GetStateObjectById(int objectId)
        {
            ElementState elementState = _db.ElementState.Where(x => x.Id == objectId).FirstOrDefault();

            if (elementState != null)
            {
                ElementStateViewModel elementStateViewModel = new ElementStateViewModel
                {
                    Id = elementState.Id,
                    FormId = elementState.FormId,
                    CreatedAt = elementState.CreatedAt,
                    isDefaultEnd = elementState.isDefaultEnd,
                    ElementId = elementState.ElementId,
                    ForEventType = elementState.ForEventType,
                    TenantId = elementState.TenantId,
                    Name = elementState.Name,
                    Type = elementState.Type

                };
                return elementStateViewModel;
            }
            else
            {
                return new ElementStateViewModel();
            }
        }

        public bool SetStateObjectStatus(int objectId, bool status)
        {
            try
            {

                ElementState elementState = _db.ElementState.Where(x => x.Id == objectId).SingleOrDefault();
                if (elementState != null)
                {
                    elementState.isDefaultEnd = status;
                    _db.ElementState.Update((Data.Entities.ElementState)elementState);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }


        }

    

        public List<SelectListItem> GetStateSelectListByUserFormRoleId(int formid, string roleid)
        {
            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var temp = _db.StateForForm
                        .Where(b => b.CaseFormId == formid).Include(x => x.State).Include(y => y.StatePermissions)
                        .OrderBy(a => a.Order).ToList();
            List<StateForForm> temp2 = new List<StateForForm>();
            foreach (var item in temp)
            {
                var perm = item.StatePermissions;
                var permYes = perm.Where(x => x.RoleId == roleid).SingleOrDefault();
                if (permYes != null)
                {
                    temp2.Add(item);
                }
            }
            List<SelectListItem> stateList = temp2.Select(x => new SelectListItem
            {
                Text = (x.StatePermissions.Any(z => z.RoleId == roleid || roleid == " ") ? x.StatePermissions.Where(w => w.RoleId == roleid || roleid == " ").Select(c => c.DisplayName).FirstOrDefault() : x.State.SystemName),
                Value = x.StateId.ToString()
            }).ToList();

            return stateList;
        }

        public bool UpdateStateForForm(int caseFormId, bool isDelete = false, string stateIds = "")
        {
            try
            {
                if (!(isDelete))
                {
                    List<StateForForm> stateForForms = new List<StateForForm>();
                    List<StateForForm> previousState = _db.StateForForm.Where(x => x.CaseFormId == caseFormId).ToList();
                    var ids = stateIds.Split(",");
                    foreach (var id in ids)
                    {
                        if (id != "")
                        {
                            if (previousState.Where(x => x.StateId == Convert.ToInt32(id) && x.CaseFormId == caseFormId).SingleOrDefault() == null)
                            {
                                StateForForm stateForForm = new StateForForm()
                                {
                                    StateId = Convert.ToInt32(id),
                                    CaseFormId = caseFormId,
                                    AllUser = false,
                                    FirstBackState = false,
                                    FirstFrontState = false,
                                    Order = 0,
                                    Icon = "/images/state/ic-draft.png"
                                };
                                stateForForms.Add(stateForForm);
                            }
                        }
                    }
                    _db.StateForForm.AddRange(stateForForms);

                }
                else
                {
                    if (stateIds != "")
                    {
                        var ids = stateIds.Split(",");
                        List<StateForForm> previousState = _db.StateForForm.Where(x => x.CaseFormId == caseFormId).ToList();
                        List<StateForForm> stateForForms = new List<StateForForm>();
                        foreach (var id in ids)
                        {
                            if (id != "")
                            {
                                StateForForm stateForForm = previousState.Where(x => x.StateId == Convert.ToInt32(id) && x.CaseFormId == caseFormId).SingleOrDefault();
                                if (stateForForm != null)
                                {
                                    stateForForms.Add(stateForForm);
                                }

                            }
                        }
                        if (stateForForms.Count > 0)
                        {
                            _db.StateForForm.RemoveRange(stateForForms);
                        }

                    }

                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
