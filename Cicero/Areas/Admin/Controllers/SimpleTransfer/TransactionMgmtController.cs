using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer.Transaction;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Cicero.Data.Enumerations;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TransactionMgmtController : BaseController
    {
        private readonly ITransactionMgmtService ITransactionMgmtService;
      //  private readonly ILogger<TransactionMgmtService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;
        private readonly IListService listService;
        private readonly IWorkflowService workflowService;
        private readonly ICaseService caseService;
        private readonly IFormService formService;
        private readonly string baseApiUrl;
        private readonly IConfiguration _config;

        public TransactionMgmtController(IPermissionService _permissionService,
            ICountryService _ICountryService, ITransactionMgmtService _ITransactionMgmtService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, 
            IToastNotification toastNotification,IListService listService, IWorkflowService workflowService, ICaseService caseService, IFormService formService,
             IConfiguration config) : base(_IUserService)
        {
            ITransactionMgmtService = _ITransactionMgmtService;
            utils = _utils;            
            _toastNotification = toastNotification;
            this.listService = listService;
            this.workflowService = workflowService;
            this.caseService = caseService;
            this.formService = formService;
            _config = config;
            baseApiUrl = _config.GetSection("BaseApiUrl").Value;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/TransactionMgmt.html")]
        [Route("admin/{tenant_identifier}/TransactionMgmt.html")]
        public  async Task<IActionResult> Index()
        {
            TransactionMgmtViewModel transactionMgmtViewModel = new TransactionMgmtViewModel();
            transactionMgmtViewModel.TransactionSearchCriterias = new TransactionSearchCriteria();
            transactionMgmtViewModel.TransactionSearchCriterias.SenderCountryList = listService.GetSenderCountryList();
            transactionMgmtViewModel.TransactionSearchCriterias.RecieverCountryList = listService.GetReceiverCountryList();
            transactionMgmtViewModel.TransactionSearchCriterias.RecieverCurrencyList = listService.GetReceiverCurrencyList();
            transactionMgmtViewModel.TransactionSearchCriterias.SenderCurrencyList = listService.GetSenderCurrencyList();
            ViewData["TxnMgmtPage"] = true;
            return View("/Areas/Admin/Views/SimpleTransfer/TransactionMgmt/Index.cshtml",transactionMgmtViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/TransactionMgmt.html")]
        [Route("admin/{tenant_identifier}/TransactionMgmt.html")]
        public JsonResult Index(DTPostModel model, string type, string senderCountry = "", string receiverCountry = "")
        {
            var TransactionMgmt = ITransactionMgmtService.GetTransactionMgmtListByFilter(model, type, senderCountry, receiverCountry);
            return Json(new
            {
                draw = TransactionMgmt.draw,
                recordsTotal = TransactionMgmt.recordsTotal,
                recordsFiltered = TransactionMgmt.recordsFiltered,
                data = TransactionMgmt.data
            });
        }

        [Area("Admin")]
        [Route("admin/transactiondetails.html")]
        [Route("admin/{tenant_identifier}/transactiondetails.html")]
        public IActionResult Details(int transactionId)
        {
            var td = new TransactionDetailsViewModel();
            td = ITransactionMgmtService.GetTransactionDetails(transactionId).Result;
            return View("/Areas/Admin/Views/SimpleTransfer/TransactionMgmt/Details.cshtml", td);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/transactionevents.html")]
        [Route("admin/{tenant_identifier}/transactionevents.html")]
        public JsonResult TransactionEvents(DTPostModel model)
        {
            var transactionEvents = new DTResponseModel();
            transactionEvents.data = new List<TransactionEvents>();
            return Json(new
            {
                draw = transactionEvents.draw,
                recordsTotal = transactionEvents.recordsTotal,
                recordsFiltered = transactionEvents.recordsFiltered,
                data = transactionEvents.data
            });
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/beneficiarychangerequest.html")]
        [Route("admin/{tenant_identifier}/beneficiarychangerequest.html")]
        public JsonResult BeneficiaryChangeRequest(DTPostModel model)
        {
            var beneficiaryChangeRequest = new DTResponseModel();
            beneficiaryChangeRequest.data = new List<BeneficiaryChangeRequest>();
            return Json(new
            {
                draw = beneficiaryChangeRequest.draw,
                recordsTotal = beneficiaryChangeRequest.recordsTotal,
                recordsFiltered = beneficiaryChangeRequest.recordsFiltered,
                data = beneficiaryChangeRequest.data
            });
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/sms.html")]
        [Route("admin/{tenant_identifier}/sms.html")]
        public JsonResult Sms(DTPostModel model)
        {
            var sms = new DTResponseModel();
            sms.data = new List<TransactionDetailsSMS>();
            return Json(new
            {
                draw = sms.draw,
                recordsTotal = sms.recordsTotal,
                recordsFiltered = sms.recordsFiltered,
                data = sms.data
            });
        }

        [Area("Admin")]
        [Route("admin/TransactionTimeStamp.html")]
        [Route("admin/{tenant_identifier}/TransactionTimeStamp.html")]
        public IActionResult TransactionTimeStamp(string referenceNo)
        {
            var datas = ITransactionMgmtService.GetTransactionTimeStampByReferenceNo(referenceNo).Result;
            return Ok(datas);
        }

        [Area("Admin")]
        [Route("admin/TransactionMgmtListQueue.html")]
        [Route("admin/{tenant_identifier}/TransactionMgmtListQueue.html")]
        public IActionResult GetTransactionMgmtListQueue(string referenceNo)
        {
            var datas = ITransactionMgmtService.GetTransactionMgmtListQueue().Result;
            return Ok(datas);
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/TransactionMgmt/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/TransactionMgmt/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            TransactionMgmtViewModel TransactionMgmtViewModel = new TransactionMgmtViewModel();
            if (id != 0)
            {
                TransactionMgmtViewModel = await ITransactionMgmtService.GetTransactionMgmtByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/TransactionMgmt/Edit.cshtml", TransactionMgmtViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/TransactionMgmt/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/TransactionMgmt/{id}/edit.html")]

        public async Task<IActionResult> Edit(TransactionMgmtViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(!ITransactionMgmtService.CheckDuplicate(model))
                    {
                        _toastNotification.AddErrorToastMessage("Duplicate Gender Code.");
                        return View("/Areas/Admin/Views/SimpleTransfer/TransactionMgmt/Edit.cshtml", model);
                    }
                    model = await ITransactionMgmtService.CreateOrUpdate(model);                   
                    _toastNotification.AddSuccessToastMessage("Gender is saved.");
                    return Redirect("~/admin" + "/TransactionMgmt/" + model.TransactionId + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/TransactionMgmt/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/TransactionMgmt/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/TransactionMgmt/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/TransactionMgmt.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Gender.");
                return Redirect("~/admin/TransactionMgmt.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {

                bool result = false;
                if (item != 0)
                {
                    if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                    {
                        state = ButtonAction.active.ToDescription();
                        result = await ITransactionMgmtService.ActiveTransactionMgmtById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await ITransactionMgmtService.InActiveTransactionMgmtById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await ITransactionMgmtService.DeleteTransactionMgmtById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " Gender(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " Gender(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " Gender(s) " + state);
            }

            return Redirect("~/admin/TransactionMgmt.html");

        }

        [Route("admin/confirmpaymentreceipt")]
        [Route("admin/confirmpaymentreceipt.html")]
        public async Task<IActionResult> ConfirmPaymentReceipt(int caseId, int type = 0)
        {
            try
            {
                var dataList = await ITransactionMgmtService.GetCaseData(caseId);
                if (dataList.Count > 0)
                {
                    var toStateId = (type == 2) ? await ITransactionMgmtService.GetStateId("PR_TrxKY") : await ITransactionMgmtService.GetStateId("TrxKY");
                    var res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), dataList.LastOrDefault(), toStateId, caseId, baseApiUrl);
                    var cvm = caseService.GetCaseById(caseId);
                    cvm.StateId = res;
                    cvm.UpdatedAt = DateTime.Now;
                    var form = type == 2 ? "jazzcash" : "transfer";
                    var caseresult = await formService.SaveCaseAsync(cvm, form);
                }

                return Ok("success");
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, "Internal Server Error");
            }
           
        }

        [Route("admin/initiatetransaction")]
        [Route("admin/initiatetransaction.html")]
        public async Task<IActionResult> InitiateTransaction(int caseId, int type = 0)
        {
            try
            {
                var dataList = await ITransactionMgmtService.GetCaseData(caseId);
                if (dataList.Count > 0)
                {
                    if(type == 2)
                    {
                        var toStateIdd = await ITransactionMgmtService.GetStateId("PR_InitiateTransaction");
                        var ress = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), dataList.LastOrDefault(), toStateIdd, caseId, baseApiUrl);
                        await UpdateCaseData(caseId, ress, "jazzcash");

                        if (ress == toStateIdd)
                        {
                            toStateIdd = await ITransactionMgmtService.GetStateId("PR_KYCCheckCompleted");
                            ress = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), ress, toStateIdd, caseId, baseApiUrl);
                            await UpdateCaseData(caseId, ress, "jazzcash");
                        }

                        if (ress == toStateIdd)
                        {
                            toStateIdd = await ITransactionMgmtService.GetStateId("PR_RulesCheckCompleted");
                            ress = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), ress, toStateIdd, caseId, baseApiUrl);
                            await UpdateCaseData(caseId, ress, "jazzcash");
                        }

                        if (ress == toStateIdd)
                        {
                            toStateIdd = await ITransactionMgmtService.GetStateId("PR_SanctionCheckCompleted");
                            ress = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), ress, toStateIdd, caseId, baseApiUrl);
                            await UpdateCaseData(caseId, ress, "jazzcash");
                        }

                        if (ress == toStateIdd)
                        {
                            toStateIdd = await ITransactionMgmtService.GetStateId("PR_Authorized");
                            ress = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), ress, toStateIdd, caseId, baseApiUrl);
                            await UpdateCaseData(caseId, ress, "jazzcash");
                        }

                        return Ok("success");
                    }

                    var toStateId = await ITransactionMgmtService.GetStateId("Initiate Transaction");
                    var res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), dataList.LastOrDefault(), toStateId, caseId, baseApiUrl);
                    await UpdateCaseData(caseId, res, "transfer");

                    if (res == toStateId)
                    {
                        toStateId = await ITransactionMgmtService.GetStateId("KYC Check Completed");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, caseId, baseApiUrl);
                        await UpdateCaseData(caseId, res, "transfer");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await ITransactionMgmtService.GetStateId("Rules Check Completed");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, caseId, baseApiUrl);
                        await UpdateCaseData(caseId, res, "transfer");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await ITransactionMgmtService.GetStateId("Sanction Check Completed");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, caseId, baseApiUrl);
                        await UpdateCaseData(caseId, res, "transfer");
                    }

                    if (res == toStateId)
                    {
                        toStateId = await ITransactionMgmtService.GetStateId("Authorized");
                        res = workflowService.RunWorflowActionObject(this, dataList.FirstOrDefault(), res, toStateId, caseId, baseApiUrl);
                        await UpdateCaseData(caseId, res, "transfer");
                    }
                }

                return Ok("success");
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, "Internal Server Error");
            }

        }

        private async Task<bool> UpdateCaseData(int caseId, int res, string form)
        {
            try
            {
                var cvm = caseService.GetCaseById(caseId);
                cvm.StateId = res;
                cvm.UpdatedAt = DateTime.Now;
                var caseresult = await formService.SaveCaseAsync(cvm, form);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}