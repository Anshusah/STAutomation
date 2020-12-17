using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TenantController : BaseController
    {
        private readonly ILogger<TenantController> Log;
        private readonly IStatus status;
        private readonly Utils utils;
        private readonly IUserService userService;
        private readonly ITenantService tenantService;
        private readonly IToastNotification _toastNotification;
        public TenantController(ITenantService _tenantService, IUserService _userService, ILogger<TenantController> _Log,
            Utils _utils, IStatus _status, IToastNotification toastNotification) : base(_userService)
        {
            userService = _userService;
            Log = _Log;
            status = _status;
            utils = _utils;
            tenantService = _tenantService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        [Route("admin/tenants.html")]
        [Route("admin/{tenant_identifier}/tenants.html")]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [Route("admin/tenants.html")]
        [Route("admin/{tenant_identifier}/tenants.html")]
        public JsonResult Index(DTPostModel model)
        {

            var tenant = tenantService.GetTenantList(model);
            return Json(new
            {
                draw = tenant.draw,
                recordsTotal = tenant.recordsTotal,
                recordsFiltered = tenant.recordsFiltered,
                data = tenant.data
            });
        }


        [HttpGet]
        [Route("admin/tenant/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/tenant/{encryptedid}/edit.html")]
        public IActionResult Edit(string encryptedid)
        {
            int id = utils.DecryptId(encryptedid);
            try
            {
                TenantViewModel avm = new TenantViewModel
                {
                    Id = id,
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now)
                };
                if (id != 0)
                {
                    avm = tenantService.GetTenantById(id);
                }
                return View(avm);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }

        }

        [HttpPost]
        [Route("~/admin/tenant/{encryptedid}/edit.html")]
        [Route("~/admin/{tenant_identifier}/tenant/{encryptedid}/edit.html")]
        public async Task<IActionResult> Edit(TenantViewModel tvm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tvm = await tenantService.CreateOrUpdate(tvm);
                    _toastNotification.AddSuccessToastMessage("Tenant is saved.");
                    return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/tenant/" + utils.EncryptId(tvm.Id) + "/edit.html");
                }
                utils.addModelError(ModelState);

                return View(tvm);
            }
            catch (Exception ex)
            {
                Log.LogError("TenantService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(tvm);
            }
        }

        [HttpPost]
        [Route("admin/tenant/action.html")]
        [Route("admin/{tenant_identifier}/tenant/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var status = "";
            if (action == "")
            {
                _toastNotification.AddWarningToastMessage("Please select any action from Action select box.");

                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/tenants.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddWarningToastMessage("Please select atleast one article.");

                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/tenants.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {

                bool result = false;
                if (item != 0)
                {
                    if (action == Enum.GetName(typeof(ButtonAction),ButtonAction.delete))
                    {
                        status = ButtonAction.delete.ToDescription();
                        result = await tenantService.DeleteTenantById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                    {
                        status = ButtonAction.active.ToDescription();
                        result = await tenantService.ActiveTenantById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        status = ButtonAction.inactive.ToDescription();
                        result = await tenantService.InactiveTenantById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " tenant(s) " + status);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddInfoToastMessage(successCount + " tenant(s) " + status);
            }
            else
            {
                _toastNotification.AddInfoToastMessage(successCount + " tenant(s) " + status);
            }

            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/tenants.html");

        }

        [HttpPost(Name = "CheckEmailIdDuplication")]
        public JsonResult CheckEmailIdDuplication(string Email, int Id)
        {

            return Json(tenantService.IsDuplicateEmail(Email, Id));

        }

        [HttpPost(Name = "CheckIdentifierIdDuplication")]
        public JsonResult CheckIdentifierIdDuplication(string identifier, int Id)
        {

            return Json(tenantService.IsDuplicateIdentifier(identifier, Id));

        }

    }
}