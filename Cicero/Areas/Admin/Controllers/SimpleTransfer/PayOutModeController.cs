using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PayOutModeController: BaseController
    {
        private readonly IRolePermissionService IRolePermissionService;
        private readonly ILogger<PayOutModeController> Log;
        private readonly IStatus status;
        private readonly Utils utils;
        private readonly IPayOutModeService payOutModeService;
        private readonly IUserService IUserService;
        private readonly IMediaService IMediaService;
        private readonly ITenantService tenantService;
        private readonly IToastNotification _toastNotification;

        public PayOutModeController(IPayOutModeService payOutModeService, IUserService _UserService,
            ILogger<PayOutModeController> _Log, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IMediaService _IMediaService,
            ITenantService _tenantService,
            IToastNotification toastNotification) : base(_UserService)
        {
            this.payOutModeService = payOutModeService;
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
        [Route("admin/payoutmode.html")]
        public IActionResult Index()
        {

            return View("/Areas/Admin/Views/SimpleTransfer/PayOutMode/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/payoutmode.html")]
        public JsonResult Index(DTPostModel model)
        {

            var payoutmode = payOutModeService.GetPayOutModeListByFilter(model);
            return Json(new
            {
                draw = payoutmode.draw,
                recordsTotal = payoutmode.recordsTotal,
                recordsFiltered = payoutmode.recordsFiltered,
                data = payoutmode.data
            });
        }
    }
}
