using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.BranchMapper;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BranchMapperController: BaseController
    {
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly IBranchMapperService branchMapperService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;

        public BranchMapperController(IBranchMapperService _branchMapperService, IPermissionService _permissionService, IStatus _status, Utils _utils, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            Status = _status;
            utils = _utils;
            IUserService = _IUserService;
            commonService = _commonService;
            branchMapperService = _branchMapperService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/branch.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/BranchMapper/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/branch.html")]
        public JsonResult Index(DTPostModel model, string countryCode, string cityCode, string bankCode)
        {
            var branchMapper = branchMapperService.GetBranchMapperListByFilter(model, countryCode, cityCode, bankCode);
            return Json(new
            {
                draw = branchMapper.draw,
                recordsTotal = branchMapper.recordsTotal,
                recordsFiltered = branchMapper.recordsFiltered,
                data = branchMapper.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/branchmapper/mapbranchs.html")]
        public IActionResult Edit()
        {
            var data = new BranchMapperViewModel();
            return View("/Areas/Admin/Views/SimpleTransfer/BranchMapper/Edit.cshtml", data);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/branchmapper/mapbranchs.html")]
        public IActionResult Edit(BranchMapperDataModel datas)
        {
            var result = branchMapperService.CreateOrUpdate(datas).Result;

            return Ok(result);
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/branchmapper/getbanksandcities.html")]
        public IActionResult GetBanksAndCities(string countryCode, int supplierId)
        {
            var data = new BankCityList();
            data = branchMapperService.GetBankAndCityList(countryCode, supplierId).Result;
            return Ok(data);
        }

        [HttpGet]
        [Route("payment/getbranchesbybank.html")]
        public IActionResult GetBranchesByBank(string bankCode)
        {
            var data = branchMapperService.GetBranchListByBank(bankCode).Result;
            return Ok(data);
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/branchmapper/getSavedDatas.html")]
        public IActionResult GetSavedDatas(string countryCode, int supplierId, string bankCode)
        {
            var data = new List<SupplierBankBranch>();
            data = branchMapperService.GetSupplierBankBranchList(countryCode, supplierId, bankCode).Result;
            return Ok(data);
        }

        [HttpPost]
        [Route("admin/branchmapper/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/branchmapper.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Branch.");
                return Redirect("~/admin/branchmapper.html");
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
                        result = await branchMapperService.ActiveBranchMapperById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await branchMapperService.InActiveBranchMapperById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await branchMapperService.DeleteBranchMapperById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " branch(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " branch(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " branch(s) " + state);
            }

            return Redirect("~/admin/branchmapper.html");

        }
    }
}
