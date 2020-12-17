using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Services;
using Cicero.Service.Models;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Cicero.Service.Library.Toastr;
using static Cicero.Service.Enums;
using Cicero.Service.Extensions;
using Cicero.Service.Services.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer.RateSupplier;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RateSupplierController : BaseController
    {       
        private readonly IRolePermissionService IRolePermissionService;
        private readonly ILogger<RateSupplierController> Log;
        private readonly IStatus status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly IRateSupplierService IRateSupplierService;
        private readonly IMediaService IMediaService;
        private readonly ITenantService tenantService;
        private readonly IToastNotification _toastNotification;

        public RateSupplierController(IRateSupplierService _IRateSupplierService, IUserService _UserService, 
            ILogger<RateSupplierController> _Log, IStatus _status, Utils _utils, 
            IRolePermissionService _IRolePermissionService, IMediaService _IMediaService, 
            ITenantService _tenantService,
            IToastNotification toastNotification) : base(_UserService)
        {
            IRateSupplierService = _IRateSupplierService;
            IUserService = _UserService;
            Log = _Log;
            status = _status;
            utils = _utils;
            IRolePermissionService = _IRolePermissionService;
            IMediaService = _IMediaService;
            tenantService = _tenantService;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/ratesupplier.html")]
        public IActionResult Index()
        {

            return View("/Areas/Admin/Views/SimpleTransfer/RateSupplier/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/ratesupplier.html")]
        public JsonResult Index(DTPostModel model)
        {
            
            var RateSupplier = IRateSupplierService.GetRateSupplierListByFilter(model);
            return Json(new
            {
                draw = RateSupplier.draw,
                recordsTotal = RateSupplier.recordsTotal,
                recordsFiltered = RateSupplier.recordsFiltered,
                data = RateSupplier.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/ratesupplier/{id}/edit.html")]
        public IActionResult Edit(string id)
        {
            try
            {
                RateSupplierViewModel avm = new RateSupplierViewModel
                {
                    Id = Convert.ToInt32(id),
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                };
                if (Convert.ToInt32(id) != 0)
                {
                    avm = IRateSupplierService.GetRateSupplierById(Convert.ToInt32(id));
                }
                return View("/Areas/Admin/Views/SimpleTransfer/RateSupplier/Edit.cshtml",avm);
            }
            catch(Exception ex){
                Log.LogError("RateSupplierService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/RateSupplier/Edit.cshtml");
            }

        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/ratesupplier/{id}/edit.html")]
        public async Task<IActionResult> Edit(RateSupplierViewModel avm, string tenant_identifier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                        avm = await IRateSupplierService.CreateOrUpdate(avm);

                            _toastNotification.AddSuccessToastMessage("RateSupplier is saved.");
                        
                    
                }
                utils.addModelError(ModelState);

                return View("/Areas/Admin/Views/SimpleTransfer/RateSupplier/Edit.cshtml", avm);
            }
            catch (Exception ex)
            {
                Log.LogError("RateSupplierService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/RateSupplier/Edit.cshtml", avm);
            }
        }

        [HttpPost]
        [Route("admin/ratesupplier/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/RateSuppliers.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one RateSupplier.");
                return Redirect("~/admin/RateSuppliers.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {

                bool result = false;
                if (item != 0)
                {
                    if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await IRateSupplierService.DeleteRateSupplierById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                    {
                        state = ButtonAction.active.ToDescription(); 
                        result = await IRateSupplierService.ActiveRateSupplierById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IRateSupplierService.InactiveRateSupplierById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " RateSupplier(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " RateSupplier(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " RateSupplier(s) " + state);
            }

            return Redirect("~/admin/RateSuppliers.html");

        }
        [Area("Admin")]
        [HttpPost]
        [Route("admin/ratesupplier/updateratepriority")]
        public IActionResult UpdateRatePriority(List<string> id,List<string> priority)
        {
            try
            {
                IRateSupplierService.UpdateRatePriorityAsync(id,priority);
                return View("/Areas/Admin/Views/SimpleTransfer/RateSupplier/Index.cshtml");
            }
            catch (Exception ex)
            {
                Log.LogError("RateSupplierService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/RateSupplier/Index.cshtml");
            }

        }

        [Authorize(Policy = "FrontOffice")]
        public IActionResult FrontIndex(string url)
        {
            RateSupplierViewModel avm = IRateSupplierService.GetRateSupplierByIdentifier(System.IO.Path.GetFileNameWithoutExtension(url));
            if (avm.Id != 0)
            {
                ViewData["Title"] = avm.Name;
                return View("~/Themes/" + this.Theme.GetName(false) + "/" + avm.Name + ".cshtml", avm);
            }
            return Redirect("/404.html");
        }
    }
}
