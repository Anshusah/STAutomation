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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ArticleController : BaseController
    {       
        private readonly IRolePermissionService IRolePermissionService;
        private readonly ILogger<ArticleController> Log;
        private readonly IStatus status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly IArticleService IArticleService;
        private readonly IMediaService IMediaService;
        private readonly ITenantService tenantService;
        private readonly IToastNotification _toastNotification;

        public ArticleController(IArticleService _IArticleService, IUserService _UserService, 
            ILogger<ArticleController> _Log, IStatus _status, Utils _utils, 
            IRolePermissionService _IRolePermissionService, IMediaService _IMediaService, 
            ITenantService _tenantService,
            IToastNotification toastNotification) : base(_UserService)
        {
            IArticleService = _IArticleService;
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
        [Route("admin/articles.html")]
        [Route("admin/{tenant_identifier}/articles.html")]
        public IActionResult Index()
        {

            return View();
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/articles.html")]
        [Route("admin/{tenant_identifier}/articles.html")]
        public JsonResult Index(DTPostModel model)
        {
            
            var article = IArticleService.GetArticleListByFilter(model);
            return Json(new
            {
                draw = article.draw,
                recordsTotal = article.recordsTotal,
                recordsFiltered = article.recordsFiltered,
                data = article.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/article/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/article/{encryptedid}/edit.html")]
        public IActionResult Edit(string encryptedid)
        {
            int id = utils.DecryptId(encryptedid);
            try
            {
                ArticleViewModel avm = new ArticleViewModel
                {
                    Id = id,
                    CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now),
                    Version = 1
                };
                if (id != 0)
                {
                    avm = IArticleService.GetArticleById(id);
                }
                return View(avm);
            }
            catch(Exception ex){
                Log.LogError("ArticleService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View();
            }

        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/article/{encryptedid}/edit.html")]
        [Route("admin/{tenant_identifier}/article/{encryptedid}/edit.html")]
        public async Task<IActionResult> Edit(ArticleViewModel avm, string tenant_identifier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    avm.TenantId = tenantService.GetTenantIdByIdentifier(tenant_identifier);
                    if (avm.TenantId != 0)
                    {
                        avm.Version = avm.Version + 1;
                        avm = await IArticleService.CreateOrUpdate(avm);

                        if (avm.Id != 0)
                        {
                            _toastNotification.AddSuccessToastMessage("Article is saved.");
                            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/article/" + utils.EncryptId(avm.Id) + "/edit.html");
                        }
                        else
                        {
                            _toastNotification.AddSuccessToastMessage("Duplicate slug.");
                            return View(avm);
                        }
                    }
                    _toastNotification.AddErrorToastMessage("Please choose a Tenant.");
                }
                utils.addModelError(ModelState);

                return View(avm);
            }
            catch (Exception ex)
            {
                Log.LogError("ArticleService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(avm);
            }
        }

        [HttpPost]
        [Route("admin/{tenant_identifier}/article/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "")
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/articles.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one article.");
                return Redirect("~/admin/articles.html");
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
                        result = await IArticleService.DeleteArticleById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                    {
                        state = ButtonAction.active.ToDescription(); 
                        result = await IArticleService.ActiveArticleById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IArticleService.InactiveArticleById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " article(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " article(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " article(s) " + state);
            }

            return Redirect("~/admin/articles.html");

        }

        [Authorize(Policy = "FrontOffice")]
        public IActionResult FrontIndex(string url)
        {
            ArticleViewModel avm = IArticleService.GetArticleByIdentifier(System.IO.Path.GetFileNameWithoutExtension(url));
            if (avm.Id != 0)
            {
                ViewData["Title"] = avm.Title;
                return View("~/Themes/" + this.Theme.GetName(false) + "/" + avm.Template + ".cshtml", avm);
            }
            return Redirect("/404.html");
        }
    }
}
