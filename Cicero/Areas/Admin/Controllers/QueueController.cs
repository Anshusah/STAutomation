using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Cicero.Service.Library.Toastr;
using static Cicero.Service.Enums;
using Cicero.Service.Extensions;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class QueueController : BaseController
    {

        private readonly ILogger<QueueController> Log;
        private readonly IStatus status;
        private readonly Utils utils;
        private readonly IUserService userService;
        private readonly IQueueService queueService;
        private readonly IActionsService actionsService;
        private readonly ICommonService commonService;
        private readonly IToastNotification _toastNotification;

        public QueueController(ILogger<QueueController> _Log, IStatus _status, Utils _utils, IUserService _userService, 
            IQueueService _queueService, IActionsService _actionsService, ICommonService _commonService, IToastNotification toastNotification) : base(_userService)
        {
            Log = _Log;
            status = _status;
            utils = _utils;
            userService = _userService;
            queueService = _queueService;
            actionsService = _actionsService;
            commonService = _commonService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        [Route("admin/manage/queues.html")]
        [Route("admin/{tenant_identifier}/manage/queues.html")]
        public IActionResult QueueIndex()
        {
            return View();
        }

        [HttpPost]
        [Route("admin/manage/queues.html")]
        [Route("admin/{tenant_identifier}/manage/queues.html")]
        public JsonResult QueueIndex(DTPostModel model, string isNew, string dateTime)
        {


            var queue = queueService.GetQueueList(model, isNew, Convert.ToDateTime(dateTime));


            return Json(new
            {
                draw = queue.draw,
                recordsTotal = queue.recordsTotal,
                recordsFiltered = queue.recordsFiltered,
                data = queue.data
            });
        }

        [HttpGet]
        [Route("admin/manage/states.html")]
        [Route("admin/{tenant_identifier}/manage/states.html")]
        public IActionResult StateIndex()
        {
            return View();
        }

        [HttpPost]
        [Route("admin/manage/states.html")]
        [Route("admin/{tenant_identifier}/manage/states.html")]
        public JsonResult StateIndex(DTPostModel model)
        {

            var state = queueService.GetStateList(model);
            return Json(new
            {
                draw = state.draw,
                recordsTotal = state.recordsTotal,
                recordsFiltered = state.recordsFiltered,
                data = state.data
            });
        }

        [HttpGet]
        [Route("admin/manage/queue/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/manage/queue/{encryptedqueueid}/edit.html")]
        public IActionResult QueueEdit(string encryptedqueueid, string tenant_identifier)
        {
            int id = utils.DecryptId(encryptedqueueid);
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                QueueViewModel qvm = new QueueViewModel
                {
                    Id = id,
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now)
                };
                if (id != 0)
                {
                    qvm = queueService.GetQueueById(tenantid, id);
                }
                else
                {
                    qvm.StateList = queueService.GetSelectStateList(tenantid);
                }

                qvm.FormList = commonService.GetFormListForActiveTenant();
                //qvm.RoleList = commonService.GetRoleList();

                return View(qvm);
            }
            catch (Exception ex)
            {
                Log.LogError("QueueService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }

        }

        [HttpPost]
        [Route("admin/manage/queue/{encryptedqueueid}/edit.html")]
        [Route("admin/{tenant_identifier}/manage/queue/{encryptedqueueid}/edit.html")]
        public async Task<IActionResult> QueueEdit(QueueViewModel qvm, string tenant_identifier, string isNew)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                if (ModelState.IsValid)
                {
                    qvm.TenantId = tenantid;
                    qvm = await queueService.CreateOrUpdateQueueAsync(qvm);
                    _toastNotification.AddSuccessToastMessage("Queue is saved.");
                    if (isNew == "true")
                    {
                        return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/queue/" + utils.EncryptId(qvm.Id) + "/edit.html?isNew=true");
                    }
                    else
                    {
                        return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/queue/" + utils.EncryptId(qvm.Id) + "/edit.html");
                    }

                }
                utils.addModelError(ModelState);

                return View(qvm);
            }
            catch (Exception ex)
            {
                Log.LogError("QueueService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(qvm);
            }
        }

        [HttpGet]
        [Route("admin/manage/stateworkflow.html")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow.html")]
        public IActionResult StateWorkFlow(string tenant_identifier)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {

                WorkFlowViewModel wfvm = new WorkFlowViewModel
                {
                    CaseFormList = commonService.GetFormListForActiveTenant(),
                    StateList = queueService.GetSelectStateList(tenantid),
                    QueueList = queueService.GetSelectQueueList(tenantid),
                    RoleList = commonService.GetRoleList(),
                    ActionList = queueService.GetSelectActionList(tenantid)
                };

                foreach (var queuelist in wfvm.QueueList)
                {
                    //var statelistaltered = statelist.FromStates.Where(a => a.FromStateId != a.ToStateId);

                    foreach (var fromqueues in queuelist.QueueToState)
                    {

                        var tostatedata = wfvm.QueueList
                            .Where(x => x.QueueToState.Count > 0)
                            .SelectMany(y => y.QueueToState)
                            .Where(z => z.IsQueue == false && string.IsNullOrWhiteSpace(z.JsonState)).Select(b => b).FirstOrDefault();

                        if (tostatedata != null)
                        {
                            JsonStateViewModel jsvm = new JsonStateViewModel
                            {

                                First = new First
                                {
                                    State = fromqueues.StateId,
                                    StateXPos = fromqueues.PosX,
                                    StateYPos = fromqueues.PosY,
                                    LineXPos = fromqueues.LinePosX,
                                    LineYPos = fromqueues.LinePosY,
                                    Type = "queue",
                                    Aero = false,
                                    IsLock = true
                                },
                                Last = new Last
                                {
                                    State = tostatedata.StateId,
                                    StateXPos = tostatedata.PosX,
                                    StateYPos = tostatedata.PosY,
                                    LineXPos = tostatedata.LinePosX,
                                    LineYPos = tostatedata.LinePosY,
                                    Type = "state",
                                    Aero = true,
                                    IsLock = true
                                },
                                CaseFormId = fromqueues.CaseFormId

                            };
                            fromqueues.JsonState = JsonConvert.SerializeObject(jsvm);

                        }

                    }

                }

                foreach (var statelist in wfvm.StateList)
                {

                    foreach (var fromstates in statelist.FromStates)
                    {

                        var tostatedata = wfvm.StateList
                            .Where(x => x.FromStates.Count > 0)
                            .SelectMany(y => y.FromStates)
                            .Where(z => z.FromStateId == fromstates.ToStateId && z.ToStateId == fromstates.FromStateId && string.IsNullOrWhiteSpace(z.JsonState)).Select(b => b).FirstOrDefault();

                        if (tostatedata != null)
                        {
                            JsonStateViewModel jsvm = new JsonStateViewModel
                            {

                                First = new First
                                {
                                    State = fromstates.FromStateId,
                                    StateXPos = fromstates.StatePosX,
                                    StateYPos = fromstates.StatePosY,
                                    LineXPos = fromstates.LinePosX,
                                    LineYPos = fromstates.LinePosY,
                                    Aero = tostatedata.Aero,
                                    IsLock = true
                                },
                                Last = new Last
                                {
                                    State = fromstates.ToStateId,
                                    StateXPos = tostatedata.StatePosX,
                                    StateYPos = tostatedata.StatePosY,
                                    LineXPos = tostatedata.LinePosX,
                                    LineYPos = tostatedata.LinePosY,
                                    Aero = fromstates.Aero,
                                    IsLock = true
                                },
                                Style = new Style
                                {
                                    Color = statelist.Color,
                                    HoverColor = statelist.Color
                                },
                                CaseFormId = fromstates.CaseFormId

                            };
                            fromstates.JsonState = JsonConvert.SerializeObject(jsvm);
                        }

                    }

                }

                return View(wfvm);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }

        }

        [HttpPost]
        [Route("admin/manage/stateworkflow.html")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow.html")]
        public JsonResult StateWorkFlow(string tenant_identifier, int form_id, string Type, List<JsonStateViewModel> points)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                bool result = false;
                if (Type == "queue")
                {
                    if (form_id != 0)
                    {
                        result = queueService.SaveQueueWorkFlow(form_id, points);
                    }
                    _toastNotification.AddSuccessToastMessage("Queue Workflow is Saved.");
                    return Json(new { status = "success", message = "Queue Workflow is Successfully Saved" });
                }
                else
                {
                    if (form_id != 0)
                    {
                        result = queueService.SaveStateWorkFlow(form_id, points);
                    }
                    _toastNotification.AddSuccessToastMessage("State Workflow is Saved.");
                    return Json(new { status = "success", message = "State Workflow is Successfully Saved" });
                }

            }
            catch (Exception ex)
            {
                Log.LogError("StateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json(new { status = "error", message = ex });
            }
        }

        [HttpPost]
        [Route("admin/manage/stateworkflow/savestate")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow/savestate")]
        public JsonResult SaveStateInWorkFlow(string tenant_identifier, StateViewModel svm)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);

            try
            {

                if (ModelState.IsValid)
                {
                    svm.TenantId = tenantid;
                    var model = queueService.CreateOrUpdateStateAsync(svm).Result;
                    _toastNotification.AddSuccessToastMessage("State is saved.");
                    return Json(new { status = "success", message = "State is Successfully Saved" });
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("");
                    return Json(new { status = "error", message = "" });
                }


            }
            catch (Exception ex)
            {
                Log.LogError("StateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json("[" + ex + "]");
            }
        }

        [HttpPost]
        [Route("admin/manage/stateworkflow/savequeue")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow/savequeue")]
        public JsonResult SaveQueueInWorkFlow(string tenant_identifier, QueueViewModel qvm)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);

            try
            {
                if (ModelState.IsValid)
                {
                    qvm.TenantId = tenantid;
                    var model = queueService.CreateOrUpdateQueueAsync(qvm).Result;
                    _toastNotification.AddSuccessToastMessage("Queue is Saved.");

                    return Json(new { status = "success", message = "Queue is Successfully Saved" });
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("");
                    return Json(new { status = "error", message = "" });
                }

            }
            catch (Exception ex)
            {
                Log.LogError("StateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json("[" + ex + "]");
            }
        }

        [HttpPost]
        [Route("admin/manage/stateworkflow/saveaction")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow/saveaction")]
        public JsonResult SaveActionInWorkFlow(string tenant_identifier, ActionsViewModel avm)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);

            try
            {
                //if (ModelState.IsValid)
                //{
                avm.TenantId = tenantid;
                var model = actionsService.CreateOrUpdateActionsAsync(avm).Result;
                _toastNotification.AddSuccessToastMessage("Action is saved.");
                return Json(new { status = "success", message = "Action is Successfully Saved" });
                //}
                //else
                //{

                //    return Json(new { status = "error", message = "" });
                //}

            }
            catch (Exception ex)
            {
                Log.LogError("StateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json("[" + ex + "]");
            }
        }


        [HttpPost]
        [Route("admin/manage/stateworkflow/AddStateAndQueue")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow/AddStateAndQueue")]
        public JsonResult AddStateAndQueueInWorkFlow(string tenant_identifier, string stateName, string stateColor, string stateAction, bool stateAndQueue)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);


            try
            {

                //if (!string.IsNullOrWhiteSpace(stateName) && !string.IsNullOrWhiteSpace(stateAction))
                //{

                //    StateViewModel svm = new StateViewModel
                //    {
                //        Id = 0,
                //        ActionName = stateAction,
                //        SystemName = stateAction,
                //        Color = stateColor,
                //        TenantId = tenantid
                //    };

                //    var model = queueService.CreateOrUpdateStateAsync(svm).Result;

                //    if (stateAndQueue == true)
                //    {
                //        QueueViewModel qvm = new QueueViewModel
                //        {
                //            Id = 0,
                //            Name = stateName,
                //            TenantId = tenantid,
                //            StateSelectedList = new List<int>() { model.Id }
                //        };

                //        qvm = queueService.CreateOrUpdateQueueAsync(qvm).Result;
                //    }
                //}

                return Json("success");
            }
            catch (Exception ex)
            {
                Log.LogError("StateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json("[" + ex + "]");
            }
        }

        [HttpPost]
        [Route("admin/manage/pull-stateworkflow.html")]
        [Route("admin/{tenant_identifier}/manage/pull-stateworkflow.html")]
        public JsonResult PullStateWorkFlow(string tenant_identifier, int form_id, string Type)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                if (Type == "queue")
                {
                    WorkFlowViewModel wfvm = new WorkFlowViewModel
                    {
                        QueueList = queueService.GetSelectQueueList(tenantid)
                    };

                    foreach (var queuelist in wfvm.QueueList)
                    {
                        //var statelistaltered = statelist.FromStates.Where(a => a.FromStateId != a.ToStateId);
                        var ql = queuelist.QueueToState.Where(x => x.IsQueue == true);
                        foreach (var fromqueues in ql)
                        {

                            var tostatedata = wfvm.QueueList
                                                 .Where(x => x.QueueToState.Count > 0)
                                                 .SelectMany(y => y.QueueToState)
                                                 .Where(z => z.IsQueue == false && z.CaseFormId == form_id && fromqueues.StateId == z.StateId && fromqueues.QueueId == z.QueueId && string.IsNullOrWhiteSpace(z.JsonState)).Select(b => b).FirstOrDefault();
                            if (tostatedata != null)
                            {
                                JsonStateViewModel jsvm = new JsonStateViewModel
                                {

                                    First = new First
                                    {
                                        State = fromqueues.QueueId,
                                        StateXPos = fromqueues.PosX,
                                        StateYPos = fromqueues.PosY,
                                        LineXPos = fromqueues.LinePosX,
                                        LineYPos = fromqueues.LinePosY,
                                        Aero = false,
                                        //IsQueue = fromqueues.IsQueue,
                                        IsLock = true,
                                        Type = "queue"
                                    },
                                    Last = new Last
                                    {
                                        State = tostatedata.StateId,
                                        StateXPos = tostatedata.PosX,
                                        StateYPos = tostatedata.PosY,
                                        LineXPos = tostatedata.LinePosX,
                                        LineYPos = tostatedata.LinePosY,
                                        //IsQueue = tostatedata.IsQueue,
                                        Aero = true,
                                        IsLock = true,
                                        Type = "state"
                                    },
                                    CaseFormId = fromqueues.CaseFormId

                                };
                                fromqueues.JsonState = JsonConvert.SerializeObject(jsvm);
                            }

                        }
                    }

                    string AeroJson = string.Empty;

                    var ModelItem = wfvm.QueueList.Select(x => x.QueueToState);

                    foreach (var _item in ModelItem)
                    {

                        foreach (var __item in _item)
                        {
                            if (!string.IsNullOrWhiteSpace(__item.JsonState) && __item.CaseFormId == form_id)
                            {
                                var itm = __item.JsonState;
                                if (string.IsNullOrWhiteSpace(AeroJson))
                                {
                                    AeroJson = itm;

                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(itm))
                                    {
                                        AeroJson = AeroJson + "," + itm;
                                    }
                                }
                            }

                        }

                    }
                    return Json(new { status = "success", type = Type, form_id = form_id, json = "[" + AeroJson + "]" });
                }
                else if (Type == "state")
                {
                    WorkFlowViewModel wfvm = new WorkFlowViewModel
                    {
                        StateList = queueService.GetSelectStateList(tenantid)
                    };

                    foreach (var statelist in wfvm.StateList)
                    {
                        //var statelistaltered = statelist.FromStates.Where(a => a.FromStateId != a.ToStateId);

                        foreach (var fromstates in statelist.FromStates)
                        {

                            var tostatedata = wfvm.StateList
                                .Where(x => x.FromStates.Count > 0)
                                .SelectMany(y => y.FromStates)
                                .Where(z => z.FromStateId == fromstates.ToStateId && z.CaseFormId == form_id && z.ToStateId == fromstates.FromStateId && string.IsNullOrWhiteSpace(z.JsonState)).Select(b => b).FirstOrDefault();

                            if (tostatedata != null && fromstates.CaseFormId == form_id)
                            {
                                JsonStateViewModel jsvm = new JsonStateViewModel
                                {

                                    First = new First
                                    {
                                        State = fromstates.FromStateId,
                                        StateXPos = fromstates.StatePosX,
                                        StateYPos = fromstates.StatePosY,
                                        LineXPos = fromstates.LinePosX,
                                        LineYPos = fromstates.LinePosY,
                                        Aero = tostatedata.Aero,
                                        IsLock = true,
                                        Type = "state"
                                    },
                                    Last = new Last
                                    {
                                        State = fromstates.ToStateId,
                                        StateXPos = tostatedata.StatePosX,
                                        StateYPos = tostatedata.StatePosY,
                                        LineXPos = tostatedata.LinePosX,
                                        LineYPos = tostatedata.LinePosY,
                                        Aero = fromstates.Aero,
                                        IsLock = true,
                                        Type = "state"
                                    },
                                    Style = new Style
                                    {
                                        Color = statelist.Color,
                                        HoverColor = statelist.Color
                                    },
                                    CaseFormId = fromstates.CaseFormId

                                };
                                fromstates.JsonState = JsonConvert.SerializeObject(jsvm);
                            }

                        }
                    }
                    string AeroJson = string.Empty;
                    var ModelItem = wfvm.StateList.Select(x => x.FromStates);
                    foreach (var _item in ModelItem)
                    {

                        foreach (var __item in _item)
                        {

                            var itm = __item.JsonState;
                            if (string.IsNullOrWhiteSpace(AeroJson))
                            {
                                AeroJson = itm;

                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(itm))
                                {

                                    AeroJson = AeroJson + "," + itm;
                                }

                            }


                        }

                    }
                    return Json(new { status = "success", type = Type, form_id = form_id, json = "[" + AeroJson + "]" });
                }


            }
            catch (Exception ex)
            {
                Log.LogError("StateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return Json(new { status = "error", message = "Error" });
            }
            return Json(new { status = "error", message = "Error" });
        }

        [HttpGet]
        [Route("admin/manage/stateworkflow/loadstatebyid")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow/loadstatebyid")]
        public IActionResult LoadState(string tenant_identifier, int id, int case_id)
        {
            //int id = utils.DecryptId(encrypted_state_id);
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                StateViewModel svm = new StateViewModel
                {
                    Id = id,
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now)
                };
                if (id != 0)
                {
                    svm = queueService.GetStateByCaseTypeId(tenantid, id, case_id);

                }
                else
                {
                    svm.StateList = queueService.GetSelectStateList(tenantid);
                }

                svm.RoleList = commonService.GetRoleList();
                //svm.CaseFormList = commonService.GetFormListForActiveTenant();

                return PartialView("_StateModal", svm);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }

        }

        [HttpGet]
        [Route("admin/manage/stateworkflow/loadqueuebyid")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow/loadqueuebyid")]
        public IActionResult LoadQueue(string tenant_identifier, int id, int case_id)
        {
            //int id = utils.DecryptId(encryptedqueueid);
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                QueueViewModel qvm = new QueueViewModel
                {
                    Id = id,
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now)
                };
                if (id != 0)
                {
                    qvm = queueService.GetQueueByCaseTypeId(tenantid, id, case_id);
                }
                else
                {
                    qvm.StateList = queueService.GetSelectStateList(tenantid);
                }

                //qvm.FormList = commonService.GetFormListForActiveTenant();
                qvm.RoleList = commonService.GetRoleList();

                return PartialView("_QueueModal", qvm);
            }
            catch (Exception ex)
            {
                Log.LogError("QueueService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }

        }

        [HttpGet]
        [Route("admin/manage/stateworkflow/loadactionbyid")]
        [Route("admin/{tenant_identifier}/manage/stateworkflow/loadactionbyid")]
        public IActionResult LoadAction(string tenant_identifier, int id, int case_id)
        {
            //int id = utils.DecryptId(encryptedqueueid);
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                ActionsViewModel avm = new ActionsViewModel
                {
                    Id = id,
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now)
                };
                //if (id != 0)
                //{
                //    qvm = queueService.GetQueueByCaseTypeId(tenantid, id, case_id);
                //}
                //else
                //{
                //    qvm.StateList = queueService.GetSelectStateList(tenantid);
                //}

                avm.TemplateList = commonService.GetTemplateList("template");
                avm.ActionsReceiverLst = commonService.GetRoleList();
                avm.ActionsSenderlst = commonService.GetRoleList();
                return PartialView("_ActionsModal", avm);
            }
            catch (Exception ex)
            {
                Log.LogError("QueueService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }

        }


        [HttpGet]
        [Route("admin/manage/state/{encrypted_state_id}/edit.html")]
        [Route("admin/{tenant_identifier}/manage/state/{encrypted_state_id}/edit.html")]
        public IActionResult StateEdit(string tenant_identifier, string encrypted_state_id)
        {
            int id = utils.DecryptId(encrypted_state_id);
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                StateViewModel svm = new StateViewModel
                {
                    Id = id,
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now)
                };
                if (id != 0)
                {
                    svm = queueService.GetStateById(tenantid, id);

                }
                else
                {
                    svm.StateList = queueService.GetSelectStateList(tenantid);
                }

                svm.RoleList = commonService.GetRoleList();
                svm.CaseFormList = commonService.GetFormListForActiveTenant();

                return View(svm);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }

        }

        [HttpPost]
        [Route("admin/manage/state/{encrypted_state_id}/edit")]
        [Route("admin/{tenant_identifier}/manage/state/{encrypted_state_id}/edit.html")]
        public async Task<IActionResult> StateEdit(StateViewModel svm, string tenant_identifier)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);
            try
            {
                if (ModelState.IsValid)
                {
                    svm.TenantId = tenantid;

                    foreach (var temp in svm.StateForForm)
                    {
                        if (temp.AllUser == true)
                        {
                            temp.StatePermissions = new List<StatePermissionView> { };
                        }

                        if (temp.CaseFormId == 0)
                        {
                            svm.StateForForm.Remove(temp);
                        }

                    }


                    svm = await queueService.CreateOrUpdateStateAsync(svm);
                    _toastNotification.AddSuccessToastMessage("State is saved.");
                    return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/state/" + utils.EncryptId(svm.Id) + "/edit.html");
                }
                utils.addModelError(ModelState);

                return View(svm);
            }
            catch (Exception ex)
            {
                Log.LogError("StateService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(svm);
            }
        }
        [HttpPost]
        [Route("admin/manage/delQueuesState.html")]
        public async Task<IActionResult> DelQueueState(string qsId, string formId, bool isQueue)
        {
            bool result = false;
            if (isQueue)
            {
                result = await queueService.DeleteQueueById(Convert.ToInt32(qsId));
                if (result)
                {
                    _toastNotification.AddSuccessToastMessage("Queue deleted.");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Queue was not deleted. Please try again.");
                }

            }
            else
            {
                result = await queueService.DeleteStateById(Convert.ToInt32(qsId));
                if (result)
                {
                    _toastNotification.AddSuccessToastMessage("State deleted.");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("State was not deleted. Please try again.");
                }
            }

            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/stateworkflow.html?formid=" + formId);
        }
        [HttpPost]
        [Route("admin/manage/queue/action.html")]
        [Route("admin/{tenant_identifier}/manage/queue/action.html")]
        public async Task<IActionResult> QueueAction(IEnumerable<int> Ids, string action, string page)
        {
            var status = "";
            if (action == "")
            {
                _toastNotification.AddWarningToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/queues.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddWarningToastMessage("Please select atleast one article.");
                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/queues.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {

                bool result = false;
                if (item != 0)
                {
                    if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        status = ButtonAction.delete.ToDescription();
                        result = await queueService.DeleteQueueById(item);

                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                    {
                        status = ButtonAction.active.ToDescription();
                        result = await queueService.ActiveQueueById(item);

                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        status = ButtonAction.inactive.ToDescription();
                        result = await queueService.InactiveQueueById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count()+ " queue(s) " + status);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddInfoToastMessage(successCount + " queue(s) " + status);
            }
            else
            {
                _toastNotification.AddInfoToastMessage(successCount + " queue(s) " + status);
            }

            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/queues.html");

        }

        [HttpPost]
        [Route("admin/manage/state/action.html")]
        [Route("admin/{tenant_identifier}/manage/state/action.html")]
        public async Task<IActionResult> StateAction(IEnumerable<int> Ids, string action, string page)
        {
            var status = "";
            if (action == "")
            {
                _toastNotification.AddWarningToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/states.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddWarningToastMessage("Please select atleast one article.");
                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/states.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {

                bool result = false;
                if (item != 0)
                {
                    if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        status = ButtonAction.delete.ToDescription();
                        result = await queueService.DeleteStateById(item);

                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                    {
                        status = ButtonAction.active.ToDescription();
                        result = await queueService.ActiveStateById(item);

                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        status = ButtonAction.inactive.ToDescription();
                        result = await queueService.InactiveStateById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " state(s) " + status);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddInfoToastMessage(successCount + " state(s) " + status);
            }
            else
            {
                _toastNotification.AddInfoToastMessage(successCount + " state(s) " + status);
            }
            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/manage/states.html");

        }

        [HttpPost(Name = "CheckUrlIdentifierDuplicate")]
        public JsonResult CheckUrlIdentifierDuplicate(string UrlIdentifier, int Id)
        {
            return Json(queueService.IsDuplicateUrl(UrlIdentifier, Id));
        }

        [HttpPost(Name = "CheckQueueOrderDuplication")]
        public JsonResult CheckQueueOrderDuplication(IFormCollection sff)
        {
            int id = 0;
            int order = 0;
            int caseFormId = 0;
            Int32.TryParse(sff["QueueForForm[0].Order"], out order);
            Int32.TryParse(sff["QueueForForm[0].Id"], out id);
            Int32.TryParse(sff["QueueForForm[0].CaseFormId"], out caseFormId);

            return Json(queueService.IsDuplicateQueueOrder(id, order, caseFormId));

        }

        [HttpPost(Name = "CheckStateOrderDuplication")]
        public JsonResult CheckStateOrderDuplication(IFormCollection sff)
        {
            //var x = sff.[0];

            int id = 0;
            int order = 0;
            int caseFormId = 0;
            Int32.TryParse(sff["StateForForm[0].Order"], out order);
            Int32.TryParse(sff["StateForForm[0].Id"], out id);
            Int32.TryParse(sff["StateForForm[0].CaseFormId"], out caseFormId);
            //return Json(false);
            return Json(queueService.IsDuplicateStateOrder(id, order, caseFormId));

        }

        [HttpPost]
        [Route("admin/manage/state/getStateList.html")]
        [Route("admin/{tenant_identifier}/manage/state/getStateList.html")]
        public JsonResult GetStateList(int tenantId)
        {
           List<StateViewModel> stateViewModels = queueService.GetSelectStateList(tenantId);
            JArray states = new JArray();
            foreach (var item in stateViewModels)
            {
                JObject obj = new JObject();
                obj.Add("id", item.Id);
                obj.Add("title", item.SystemName);
                states.Add(obj);
            }
            return Json(states);
        }
    }
}