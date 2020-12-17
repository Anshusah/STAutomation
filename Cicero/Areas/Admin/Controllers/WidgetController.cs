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
using Themes;
using Cicero.Service.Models.Core;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WidgetController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly Utils utils;
        private readonly IUserService userService;
        private readonly AppSetting Setting = null;

        public WidgetController(Utils _utils, ApplicationDbContext _ApplicationDbContext, AppSetting _Setting, IUserService _UserService):base(_UserService)
        {
            db = _ApplicationDbContext;
            utils = _utils;
            Setting = _Setting;
            userService = _UserService;
        }

        [Route("admin/widgets.html")]
        [Route("admin/{tenant_identifier}/widgets.html")]
        public ActionResult Index()
        {
            var a = db.Article.ToList();
            
            Theme theme = this.Theme as Theme;
            var WidgetNameAsString = "app_theme_widgets_" + theme.GetName();

            var Widget = Setting.Get(WidgetNameAsString, "");

            if (Widget == null || Widget == "")
            {
                Widget = "[]";
            }

            var obj = JsonConvert.DeserializeObject<List<Widget>>(Widget);



            return View(obj);


        }
    }
}