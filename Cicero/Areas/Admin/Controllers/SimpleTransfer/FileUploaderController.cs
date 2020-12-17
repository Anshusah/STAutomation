using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FileUploaderController : BaseController
    {
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly IFileUploaderService fileUploaderService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;

        public FileUploaderController(IPermissionService _permissionService, IToastNotification toastNotification, IUserService _IUserService, ICommonService _commonService, IFileUploaderService fileUploaderService) : base(_IUserService)
        {
            IUserService = _IUserService;
            commonService = _commonService;
            this.fileUploaderService = fileUploaderService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/fileuploader.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/FileUploader/Index.cshtml", new FileUploaderViewModel());
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/upload.html")]
        public IActionResult Upload(FileUploaderViewModel data)
        {
            if (data.UploadFile == null || data.UploadFile.Length == 0)
                return Content("file not selected");

            var value = ReadAsList(data);

            if(data.Type == "bank")
            {
                var supplierBank = SupplierBankDatas(value, data.SupplierId);
                var result = fileUploaderService.SaveBankData(supplierBank).Result;
                if (!result)
                {
                    _toastNotification.AddErrorToastMessage("Internal Server Error");
                    return View("/Areas/Admin/Views/SimpleTransfer/FileUploader/Index.cshtml", data);
                }
            }
            else
            {
                var supplierBankBranch = SupplierBankBranchDatas(value, data.SupplierId);
                var result = fileUploaderService.SaveBankBranchData(supplierBankBranch).Result;
                if (!result)
                {
                    _toastNotification.AddErrorToastMessage("Internal Server Error");
                    return View("/Areas/Admin/Views/SimpleTransfer/FileUploader/Index.cshtml", data);
                }
            }

            _toastNotification.AddSuccessToastMessage("Saved Successfully");
            return RedirectToAction("Index");
        }

        public List<SupplierBank> SupplierBankDatas(List<string> value, int supplierId)
        {
            value.RemoveAt(0);
            var supplierBankList = new List<SupplierBank>();

            foreach (var item in value)
            {
                var values = item.Replace(" ", "").Split(',');
                if(values.Length != 3)
                {
                    continue;
                }
                supplierBankList.Add(new SupplierBank
                {
                    BankCode = values[0],
                    BankName = values[1],
                    CountryCode = values[2],
                    SupplierId = supplierId,
                    Status = true
                });

            }

            return supplierBankList;
        }

        public List<SupplierBankBranch> SupplierBankBranchDatas(List<string> value, int supplierId)
        {
            value.RemoveAt(0);
            var supplierBankBranchList = new List<SupplierBankBranch>();

            foreach (var item in value)
            {
                var values = item.Replace(" ", "").Split(',');
                if (values.Length != 7)
                {
                    continue;
                }
                supplierBankBranchList.Add(new SupplierBankBranch
                {
                    BankCode = values[0],
                    BranchCode = values[1],
                    CityCode = values[2],
                    BranchName = values[3],
                    CountryCode = values[5],
                    SupplierId = supplierId,
                    Status = true
                });

            }

            return supplierBankBranchList;
        }

        public List<string> ReadAsList(FileUploaderViewModel data)
        {
            var list = new List<string>();
            using (var reader = new StreamReader(data.UploadFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    list.Add(reader.ReadLine());
            }
            return list;
        }

    }
}
