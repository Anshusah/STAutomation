using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Cicero.Service.Services;
using Newtonsoft.Json;
using Cicero.Service.Helpers;
using Cicero.Data;
using Cicero.Service.Models;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MenuController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly Utils utils;
        private readonly IUserService userService;
        private readonly AppSetting Setting = null;
        private readonly ICommonService commonService;

        public MenuController(Utils _utils, ApplicationDbContext _ApplicationDbContext, AppSetting _Setting, IUserService _UserService, ICommonService _commonService) : base(_UserService)
        {
            db = _ApplicationDbContext;
            utils = _utils;
            Setting = _Setting;
            userService = _UserService;
            commonService = _commonService;
        }

        [Route("admin/menus.html")]
        [Route("admin/{tenant_identifier}/menus.html")]
        public ActionResult Index()
        {

            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            var a = db.Article.Where(x => x.TenantId == tenantid).ToList();
            var lo = this.Theme.Navigation;
            var SelectedLocation = lo[0].ToString();
            var location = Request.Query["location"].ToString();
            if (location != "")
            {
                SelectedLocation = location;
            }
            
            var ExistingMenus = Setting.Get(SelectedLocation, "");
            
            if (ExistingMenus == null || SelectedLocation == "")
            {
                ExistingMenus = "[]";
            }

            var obj = JsonConvert.DeserializeObject<List<NavigationJsonItems>>(ExistingMenus);

            var navigation = new NavigationViewModel
            {
                Article = a,
                Locations = lo,
                SelectedLocation = SelectedLocation,
                ExistingMenusDecoded = obj,
                ExistingMenusEncoded = ExistingMenus
            };

            return View(navigation);
           

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("admin/menus.html")]
        [Route("admin/{tenant_identifier}/menus.html")]
        public ActionResult Index(string json)
        {
            var a = db.Article.ToList();
            var lo = this.Theme.Navigation;
            var SelectedLocation = lo[0].ToString();
            var location = Request.Query["location"].ToString();
            if (location != "")
            {
                SelectedLocation = location;
            }

            Setting.Update(SelectedLocation, json);

            var ExistingMenus = json;
            if (ExistingMenus == null || SelectedLocation == "")
            {
                ExistingMenus = "[]";
            }
            var obj = JsonConvert.DeserializeObject<List<NavigationJsonItems>>(ExistingMenus);
            var navigation = new NavigationViewModel
            {
                Article = a,
                Locations = lo,
                SelectedLocation = SelectedLocation,
                ExistingMenusDecoded = obj,
                ExistingMenusEncoded = ExistingMenus
            };
            return View(navigation);
            
        }
    }
}