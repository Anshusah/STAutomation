using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.BankMapper;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BankMapperController : BaseController
    {
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly IBankMapperService bankMapperService;
        private readonly ICommonService commonService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;

        public BankMapperController(IPermissionService _permissionService, IStatus _status, Utils _utils, IUserService _IUserService, IBankMapperService _bankMapperService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            Status = _status;
            utils = _utils;
            IUserService = _IUserService;
            bankMapperService = _bankMapperService;
            commonService = _commonService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/bankmapper.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/BankMapper/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/bankmapper.html")]
        public JsonResult Index(DTPostModel model)
        {
            var bankMapper = bankMapperService.GetBankMapperListByFilter(model);
            return Json(new
            {
                draw = bankMapper.draw,
                recordsTotal = bankMapper.recordsTotal,
                recordsFiltered = bankMapper.recordsFiltered,
                data = bankMapper.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/bankmapper/mapbanks.html")]
        public IActionResult Edit()
        {
            var data = new BankMapperViewModel();
            return View("/Areas/Admin/Views/SimpleTransfer/BankMapper/Edit.cshtml", data);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/bankmapper/mapbanks.html")]
        public IActionResult Edit(List<string> necMoneyBankList, List<string> transfastBankList, List<bool> status, string countryCode)
        {
            var result = bankMapperService.CreateOrUpdate(necMoneyBankList, transfastBankList, status, countryCode).Result;

            return Ok(result);
        }

        //[Area("Admin")]
        //[Route("bankmapper/getbanklist.html")]
        //public IActionResult GetBankList(string countryCode)
        //{
        //    var data = bankMapperService.GetBankMapperByCountryCode(countryCode).Result;

        //    if(data == null)
        //    {
        //        data = GetData(countryCode);
        //    }

        //    return PartialView("/Areas/Admin/Views/SimpleTransfer/BankMapper/BankListPartialView.cshtml", data);
        //}

        private BankListViewModel GetData(string countryCode)
        {
            var bankListModel = new BankListViewModel();
            var necMoneyBankList = bankMapperService.GetNecMoneyBankList(countryCode).Result;
            var transfastBankList = bankMapperService.GetTransfastBankList(countryCode).Result;

            bankListModel.NecMoneyBankList = necMoneyBankList.OrderBy(x => x.Text).ToList();
            bankListModel.TransfastBankList = transfastBankList;

            var statusList = new List<bool>();
            foreach (var item in transfastBankList)
            {
                statusList.Add(true);
            }
            bankListModel.Status = statusList;

            return bankListModel;
        }

        [HttpPost]
        [Route("admin/bankmapper/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/bankmapper.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Branch.");
                return Redirect("~/admin/bankmapper.html");
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
                        result = await bankMapperService.ActiveBankMapperById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await bankMapperService.InActiveBankMapperById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await bankMapperService.DeleteBankMapperById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " bank(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " bank(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " bank(s) " + state);
            }

            return Redirect("~/admin/bankmapper.html");

        }

        [Area("Admin")]
        [HttpGet]
        [Route("bankmapper/getbanklist.html")]
        public IActionResult GetBankList(string countryCode)
        {
            var data = bankMapperService.GetBankMapperByCountryCode(countryCode).Result;

            if (data == null)
            {
                data = GetData(countryCode);
                var newData = GetData(countryCode);
                var a = new List<SelectListItem>();
                var b = new List<SelectListItem>();
                var c = new List<double>();

                for (int i = 0; i < data.TransfastBankList.Count; i++)
                {
                    for (int j = 0; j < data.NecMoneyBankList.Count; j++)
                    {

                        var compute = CalculateSimilarity(data.TransfastBankList[i].Text.ToLower(), data.NecMoneyBankList[j].Text.ToLower());
                        c.Add(compute);

                        if (j == data.NecMoneyBankList.Count - 1)
                        {
                            var percentageValue = c.OrderByDescending(x => x).FirstOrDefault();
                            var percentage = percentageValue * 100;
                            var index = c.IndexOf(percentageValue);
                            //if (b.Select(x => x.Value).ToList().Contains(data.NecMoneyBankList[j].Value))
                            //{
                            //    continue;
                            //}
                            if (percentage < 50)
                            {
                                c = new List<double>();
                                continue;
                            }
                            a.Add(data.TransfastBankList[i]);
                            b.Add(data.NecMoneyBankList[index]);
                            data.NecMoneyBankList.Remove(data.NecMoneyBankList[index]);
                            c = new List<double>();
                        }
                        Console.WriteLine();
                    }
                }

                data.TransfastBankList = a;
                data.NecMoneyBankList = b;

                var remaningNecMoneyBankList = Except(newData.NecMoneyBankList, b);
                var remaningTransfastBankList = Except(newData.TransfastBankList, a);

                data.NecMoneyBankList.AddRange(remaningNecMoneyBankList);
                data.TransfastBankList.AddRange(remaningTransfastBankList);
            }

            return PartialView("/Areas/Admin/Views/SimpleTransfer/BankMapper/BankListPartialView.cshtml", data);
        }

        int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

        public static List<SelectListItem> Except(List<SelectListItem> array1, List<SelectListItem> array2)
        {
            var newArray = new List<SelectListItem>();
            foreach (var item in array1)
            {
                if (array2.Where(x => x.Value == item.Value).Count() == 0)
                {
                    newArray.Add(item);
                }
            }
            return newArray;
        }
    }
}
