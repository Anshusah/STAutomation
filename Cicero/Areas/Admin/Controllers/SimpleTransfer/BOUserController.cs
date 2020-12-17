using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Cicero.Data;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Cicero.Service.Models;
using Cicero.Service.Helpers;
using Cicero.Service.Services;
using Microsoft.Extensions.Logging;
using Core.Status;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Cicero.Service.Models.Core;
using Cicero.Service.Library.Toastr;
using static Cicero.Service.Enums;
using Cicero.Service.Extensions;
using Newtonsoft.Json.Linq;
using Cicero.Service.Models.SimpleTransfer.User;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    //[Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BOUserController : BaseController
    {

        private readonly IUserService IUserService;
        private readonly IRoleService roleService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Utils Utils;
        private readonly ILogger<UserController> Log;
        private readonly IStatus status;
        private readonly IEmailSender _EmailSender;
        private readonly ICommonService commonService;
        private readonly ICaseService caseService;
        private readonly IQueueService queueService;
        private readonly ITemplateService templateService;
        private readonly IFormBuilderService formbuilderService;
        private readonly IFormService _formService;
        private readonly IRazorToStringRender razorViewToStringRenderer;
        private readonly IToastNotification _toastNotification;
        private readonly IMediaService mediaService;
        private readonly AppSetting appSetting = null;

        public BOUserController(IUserService _UserService, Utils _Utils, ILogger<UserController> _Log, IStatus _status,
            ICaseService _caseService, IRazorToStringRender _razorViewToStringRenderer, ITemplateService _templateService,
            IRoleService _roleService, UserManager<ApplicationUser> _userManager, IEmailSender emailSender,
            ICommonService _commonService, IQueueService _queueService, IFormBuilderService _formbuilderService,
            IFormService formService, IToastNotification toastNotification, IMediaService mediaService, AppSetting _appSetting) : base(_UserService)
        {
            roleService = _roleService;
            IUserService = _UserService;
            Utils = _Utils;
            Log = _Log;
            status = _status;
            userManager = _userManager;
            _EmailSender = emailSender;
            commonService = _commonService;
            caseService = _caseService;
            templateService = _templateService;
            queueService = _queueService;
            formbuilderService = _formbuilderService;
            _formService = formService;
            razorViewToStringRenderer = _razorViewToStringRenderer;
            _toastNotification = toastNotification;
            this.mediaService = mediaService;
            appSetting = _appSetting;
        }

        [Area("Admin")]
        [Authorize(Policy = "BackOffice")]
        [HttpGet]
        [Route("st/adminuser/users.html")]
        [Route("st/adminuser/{tenant_identifier}/users.html")]
        public ActionResult Index(string tenant_identifier)
        {
            return View();
        }

        [Area("Admin")]
        [Authorize(Policy = "BackOffice")]
        [HttpPost]
        [Route("st/adminuser/users.html")]
        [Route("st/adminuser/{tenant_identifier}/users.html")]
        public JsonResult Index(DTPostModel model)
        {
            var user = IUserService.GetUserListByFilter(model);
            return Json(new
            {
                draw = user.draw,
                recordsTotal = user.recordsTotal,
                recordsFiltered = user.recordsFiltered,
                data = user.data
            });
        }

        [Area("Admin")]
        [Authorize(Policy = "BackOffice")]
        [HttpGet]
        [Route("st/adminuser/user/{id}/edit.html")]
        [Route("st/adminuser/{tenant_identifier}/user/{id}/edit.html")]
        public async Task<IActionResult> Edit(string id)
        {
            UserViewModel userViewModel = new UserViewModel { Id = id, CreatedAt = Utils.GetDefaultDateFormat(DateTime.Now), UpdatedAt = Utils.GetDefaultDateFormat(DateTime.Now), Status = true };
            if (id != "0")
            {
                userViewModel = await IUserService.GetUserById(id);
            }
            userViewModel.RoleList = roleService.GetRoleList();
            return View(userViewModel);
        }

        [Area("Admin")]
        [Authorize(Policy = "BackOffice")]
        [HttpPost]
        [Route("st/adminuser/user/{id}/edit.html")]
        [Route("st/adminuser/{tenant_identifier}/user/{id}/edit.html")]
        public async Task<IActionResult> Edit(UserViewModel model)
        {

            try
            {

                if (model.Id != "0" && string.IsNullOrEmpty(model.Password))
                {
                    ModelState.Remove("Password");
                }
                if (ModelState.IsValid)
                {
                    string checkNewUser = model.Id;
                    string loggedUser = IUserService.getLoggedInUserId();
                    model.CreatedBy = loggedUser;
                    string checkNewEmail = "";
                    if(model.Id!="0")
                    {
                       checkNewEmail = IUserService.GetUserById(model.Id).GetAwaiter().GetResult().Email;
                    }
                    model = await IUserService.CreateOrUpdate(model);
                    //if (model.Ids != null && model.Ids.Count > 0)
                    //{
                    mediaService.CreateOrUpdateUserMediaGroup(model.Ids, model.Id);
                    //}
                    if (checkNewUser == "0")
                    {
                        ApplicationUser user = await userManager.FindByEmailAsync(model.Email);
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                        var callbackUrl = Url.Action("confirmemail",
                        "user", new { Area = "", userId = user.Id, code },
                        protocol: Request.Scheme);
                        string settings = templateService.GetEmailGeneralSetting();
                        JObject mailObject = (JObject)JsonConvert.DeserializeObject(settings);
                        int key = (int)EmailSettingFor.EmailConfirmation;
                        int templateId = Convert.ToInt16(mailObject[key.ToString()]);
                        TemplateViewModel templateViewModel = templateService.GetTemplateById(templateId);
                        string content = templateService.CreateEmailTemplate(templateViewModel.Content, 0, 0, 0, callbackUrl, user.Id);
                        var messageNew = new TemplateEmailViewModel { };
                        messageNew.Content = content;
                        string body = razorViewToStringRenderer.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", messageNew).GetAwaiter().GetResult();
                        string subject = "Confirm your email";
                        //string messagebody = $"<p>Email: <strong>{user.Email}</strong></p>" +
                        //    $"<p>Password: <strong>{model.Password}</strong></p>" +
                        //    $"<p>Please change your password once you login.</p>" +
                        //    $"<p>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.</p>";
                        await _EmailSender.SendEmailAsync(model.Email, subject, body);
                        _toastNotification.AddSuccessToastMessage("User Details are saved. Please Check your email now.");
                    }
                    else
                    {
                        
                        if (checkNewEmail.ToUpper() != model.Email.ToUpper()) 
                        {
                            ApplicationUser user = await userManager.FindByEmailAsync(model.Email);
                            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                            var callbackUrl = Url.Action("confirmemail",
                            "user", new { Area = "", userId = user.Id, code },
                            protocol: Request.Scheme);
                            string settings = templateService.GetEmailGeneralSetting();
                            JObject mailObject = (JObject)JsonConvert.DeserializeObject(settings);
                            int key = (int)EmailSettingFor.EmailConfirmation;
                            int templateId = Convert.ToInt16(mailObject[key.ToString()]);
                            TemplateViewModel templateViewModel = templateService.GetTemplateById(templateId);
                            string content = templateService.CreateEmailTemplate(templateViewModel.Content, 0, 0, 0, callbackUrl, user.Id);
                            var messageNew = new TemplateEmailViewModel { };
                            messageNew.Content = content;
                            string body = razorViewToStringRenderer.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", messageNew).GetAwaiter().GetResult();
                            string subject = "Confirm your email";
                            //string messagebody = $"<p>Email: <strong>{user.Email}</strong></p>" +
                            //    $"<p>Password: <strong>{model.Password}</strong></p>" +
                            //    $"<p>Please change your password once you login.</p>" +
                            //    $"<p>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.</p>";
                            await _EmailSender.SendEmailAsync(model.Email, subject, body);
                            _toastNotification.AddSuccessToastMessage("User Details are saved. Please Check your email now.");
                        }
                        else
                        {
                            _toastNotification.AddSuccessToastMessage("User Details are saved.");
                        }
                       
                    }
                    return Redirect("~/admin" + Utils.GetTenantForUrl(false) + "/user/" + model.Id + "/edit.html");
                }
                else
                {
                    Utils.addModelError(ModelState);
                }
                model.RoleList = roleService.GetRoleList();
                return View(model);
            }
            catch (Exception ex)
            {
                Log.LogError("UserServices:Edit - " + ex.ToString());

                _toastNotification.AddErrorToastMessage(ex.ToString());
                ///return Redirect(this.errorPageUrl);
                model.RoleList = roleService.GetRoleList();
                return View(model);
            }
        }

        [Area("Admin")]
        [HttpPost]
        [Route("st/adminuser/user/action.html")]
        [Route("st/adminuser/{tenant_identifier}/user/action.html")]
        public async Task<IActionResult> Action(IEnumerable<string> Ids, string action, string page)
        {
            var status = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddWarningToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin" + Utils.GetTenantForUrl(false) + "/users.html");
            }
            if (string.IsNullOrEmpty(Ids.ToString()) || Ids.Count() <= 0)
            {
                _toastNotification.AddWarningToastMessage("Please select atleast one user.");
                return Redirect("~/admin" + Utils.GetTenantForUrl(false) + "/users.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {
                bool result = false;
                if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                {
                    status = ButtonAction.delete.ToDescription();
                    result = await IUserService.DeleteUserById(item);
                }
                else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                {
                    status = ButtonAction.active.ToDescription();
                    result = await IUserService.ActiveUserById(item);
                }
                else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                {
                    status = ButtonAction.inactive.ToDescription();
                    result = await IUserService.InactiveUserById(item);
                }
                if (result)
                {
                    successCount++;
                }
            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " user(s) " + status);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddInfoToastMessage(successCount + " user(s) " + status);
            }
            else
            {
                _toastNotification.AddInfoToastMessage(successCount + " user(s) " + status);
            }

            return Redirect("~/admin" + Utils.GetTenantForUrl(false) + "/users.html");

        }

        [Area("Admin")]
        [HttpPost(Name = "CheckUserEmailIdDuplication")]
        public JsonResult CheckUserEmailIdDuplication(string Email, string Id)
        {
            int TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            return Json(IUserService.IsDuplicateEmailInTenant(Email, Id, TenantId));

        }

        //frontoffice section

        [Authorize]
        [Route("st/adminuser/dashboard.html")]
        [Route("st/adminuser/{tenant_identifier}/dashboard.html")]
        public IActionResult Index(bool all = false)
        {
            var checkRole = IUserService.UserHasPolicy();
            if (checkRole == "backend" || IUserService.IsSuperAdmin().Result == true)
            {
                return Redirect("~/admin.html");
            }
            else
            {
                return Redirect("~/admin/form" + Utils.GetTenantForUrl(false) + "/" + "transfer" + "/" + Utils.EncryptId(0) + "/edit.html");
            }

        }
        [Authorize]
        [Route("st/adminuser/userdashboard.html")]
        [Route("st/adminuser/{tenant_identifier}/userdashboard.html")]
        public IActionResult UserDashboard(int year = 0, bool all = false)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            var dates = caseService.GetDateListsForCasePreview();
            ViewBag.Dates = dates;
            if (year != -1)
            {
                year = (dates.Contains(year)) ? year : dates.LastOrDefault();
            }
            ViewBag.Year = year;


            var checkRole = IUserService.UserHasPolicy();
            if (checkRole == "backend" || IUserService.IsSuperAdmin().Result == true)
            {
                return Redirect("~/admin.html");
            }
            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            List<CaseViewModel> model = caseService.GetCaseByUserId(all, year).ToList();
            List<CaseFormViewModel> lst = commonService.GetCaseFormListForActiveTenantId();

            foreach (var item in model)
            {
                var temp = formbuilderService.GetBuilderFormById(item.CaseFormId);

                if (temp != null)
                {
                    item.CaseFormUrl = temp.UrlIdentifier;
                }
                item.StateName = queueService.GetStateNameById(item.StateId, item.CaseFormId);

                item.QueueId = caseService.GetQueueIdByStateIdAndCaseFormId(item.StateId, item.CaseFormId);

                var queueName = caseService.GetQueueNameByQueueIdAndCaseFormId(item.QueueId, item.CaseFormId);

                if (queueName != null)
                {
                    //   item.DisplayPermission = true;
                    item.QueueName = queueName.ToString();
                }

                var queueIcon = caseService.GetQueueIconByQueueId(item.QueueId);
                item.QueueIcon = queueIcon;

                var queueColor = caseService.GetQueueColorByQueueId(item.QueueId);
                item.QueueColor = queueColor;

                item.VisibleInFooterViewModel = new List<VisibleInFooterViewModel>();

                CaseFormViewModel lsts = lst.Where(d => d.Id == item.CaseFormId).FirstOrDefault();
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var allfields = JsonConvert.DeserializeObject<FormBuilderViewModel>(lsts.Fields, settings);

                List<FormBuilderViewModel.Form.Table> tables = allfields.Forms.Tables;
                var elementValue = _formService.GetTableData(item.Id, tables, allfields);

                foreach (var tab in allfields.Tab)
                {
                    foreach (var row in tab.Row)
                    {
                        foreach (var column in row.Column)
                        {
                            foreach (dynamic element in column.Element)
                            {
                                var isit = element.GetType().GetProperty("VisibleinFooter");
                                if (isit != null)
                                {
                                    if (element.VisibleinFooter != null && element.VisibleinFooter)
                                    {
                                        if (item.VisibleInFooterViewModel.Count != 2)
                                        {
                                            var textValue = string.Empty;
                                            var type = element.GetType().Name.ToLower();
                                            var iconUrl = string.Empty;
                                            if (elementValue["elm" + element.ElementId] != null)
                                            {
                                                if (elementValue["elm" + element.ElementId].Value != null)
                                                {
                                                    textValue = elementValue["elm" + element.ElementId].Value;
                                                }
                                            }

                                            if (type == "selectbox")
                                            {
                                                if (element.SelectOptions != null && element.SelectOptions.Count > 0)
                                                {
                                                    foreach (var soption in element.SelectOptions)
                                                    {
                                                        var soptionValue = soption.Value;
                                                        if (soptionValue != null)
                                                        {
                                                            soptionValue = soptionValue.Trim();
                                                            if (soptionValue == textValue.Trim())
                                                            {
                                                                iconUrl = soption.IconUrl;
                                                            }
                                                        }


                                                    }

                                                }
                                            }

                                            item.VisibleInFooterViewModel.Add(new VisibleInFooterViewModel
                                            {
                                                Text = textValue,
                                                Type = type,
                                                IconUrl = iconUrl
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            //foreach (var item in model)
            //{
            //    StateViewModel svm = _queueService.GetStateById(tenantid, item.StateId);
            //    item.StateName = svm.SystemName;
            //    if (svm.Color == null)
            //    {
            //        item.StateColor = "#DCDCDC";
            //    }
            //    else
            //    {
            //        item.StateColor = svm.Color;
            //    }

            //}
            ViewBag.All = all;
            return View("/Themes/" + this.Theme.GetName(false) + "/User/Index.cshtml", model.Where(x => !string.IsNullOrEmpty(x.QueueName)));
        }


        [Authorize]
        [Route("st/adminuser/landingpage.html")]
        [Route("st/adminuser/{tenant_identifier}/landingpage.html")]
        public IActionResult LandingPage(bool all = false)
        {

            return View("/Themes/" + this.Theme.GetName(false) + "/User/LandingPage.cshtml");
        }

        public IActionResult GetListOfCasesByQueueId(int queueId, string queueName, bool all, int year = 0)
        {
            var cases = caseService.GetListOfCasesByQueueId(queueId, queueName, all, year);
            List<CaseFormViewModel> lst = commonService.GetCaseFormListForActiveTenantId();

            foreach (var item in cases)
            {
                item.VisibleInFooterViewModel = new List<VisibleInFooterViewModel>();

                CaseFormViewModel lsts = lst.Where(d => d.Id == item.CaseFormId).FirstOrDefault();
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var allfields = JsonConvert.DeserializeObject<FormBuilderViewModel>(lsts.Fields, settings);

                List<FormBuilderViewModel.Form.Table> tables = allfields.Forms.Tables;
                var elementValue = _formService.GetTableData(item.Id, tables, allfields);

                foreach (var tab in allfields.Tab)
                {
                    foreach (var row in tab.Row)
                    {
                        foreach (var column in row.Column)
                        {
                            foreach (dynamic element in column.Element)
                            {
                                var isit = element.GetType().GetProperty("VisibleinFooter");
                                if (isit != null)
                                {
                                    if (element.VisibleinFooter != null && element.VisibleinFooter)
                                    {
                                        if (item.VisibleInFooterViewModel.Count != 2)
                                        {
                                            var textValue = string.Empty;
                                            var type = element.GetType().Name.ToLower();
                                            var iconUrl = string.Empty;
                                            if (elementValue["elm" + element.ElementId] != null)
                                            {
                                                if (elementValue["elm" + element.ElementId].Value != null)
                                                {
                                                    textValue = elementValue["elm" + element.ElementId].Value;
                                                }
                                            }

                                            if (type == "selectbox")
                                            {
                                                if (element.SelectOptions != null && element.SelectOptions.Count > 0)
                                                {
                                                    foreach (var soption in element.SelectOptions)
                                                    {
                                                        var soptionValue = soption.Value;
                                                        if (soptionValue != null)
                                                        {
                                                            soptionValue = soptionValue.Trim();
                                                            if (soptionValue == textValue.Trim())
                                                            {
                                                                iconUrl = soption.IconUrl;
                                                            }
                                                        }


                                                    }

                                                }
                                            }

                                            item.VisibleInFooterViewModel.Add(new VisibleInFooterViewModel
                                            {
                                                Text = textValue,
                                                Type = type,
                                                IconUrl = iconUrl
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return PartialView("~/Themes/Blue/User/GetListOfCasesByQueueId.cshtml", cases);
        }

        //public IActionResult GetListOfCasesByYear(int year)
        //{
        //    try
        //    {
        //        int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
        //        List<CaseViewModel> model = caseService.GetCaseByUserId(false, year).ToList();
        //        List<CaseFormViewModel> lst = commonService.GetCaseFormListForActiveTenantId();

        //        foreach (var item in model)
        //        {
        //            var temp = formbuilderService.GetBuilderFormById(item.CaseFormId);

        //            if (temp != null)
        //            {
        //                item.CaseFormUrl = temp.UrlIdentifier;
        //            }
        //            item.StateName = queueService.GetStateNameById(item.StateId, item.CaseFormId);

        //            item.QueueId = caseService.GetQueueIdByStateIdAndCaseFormId(item.StateId, item.CaseFormId);

        //            var queueName = caseService.GetQueueNameByQueueIdAndCaseFormId(item.QueueId, item.CaseFormId);

        //            if (queueName != null)
        //            {
        //                //   item.DisplayPermission = true;
        //                item.QueueName = queueName.ToString();
        //            }

        //            var queueIcon = caseService.GetQueueIconByQueueId(item.QueueId);
        //            item.QueueIcon = queueIcon;

        //            var queueColor = caseService.GetQueueColorByQueueId(item.QueueId);
        //            item.QueueColor = queueColor;

        //            item.VisibleInFooterViewModel = new List<VisibleInFooterViewModel>();

        //            CaseFormViewModel lsts = lst.Where(d => d.Id == item.CaseFormId).FirstOrDefault();
        //            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        //            var allfields = JsonConvert.DeserializeObject<FormBuilderViewModel>(lsts.Fields, settings);

        //            List<FormBuilderViewModel.Form.Table> tables = allfields.Forms.Tables;
        //            var elementValue = _formService.GetTableData(item.Id, tables, allfields);

        //            foreach (var tab in allfields.Tab)
        //            {
        //                foreach (var row in tab.Row)
        //                {
        //                    foreach (var column in row.Column)
        //                    {
        //                        foreach (dynamic element in column.Element)
        //                        {
        //                            var isit = element.GetType().GetProperty("VisibleinFooter");
        //                            if (isit != null)
        //                            {
        //                                if (element.VisibleinFooter != null && element.VisibleinFooter)
        //                                {
        //                                    if (item.VisibleInFooterViewModel.Count != 2)
        //                                    {
        //                                        var textValue = string.Empty;
        //                                        var type = element.GetType().Name.ToLower();
        //                                        var iconUrl = string.Empty;
        //                                        if (elementValue["elm" + element.ElementId] != null)
        //                                        {
        //                                            if (elementValue["elm" + element.ElementId].Value != null)
        //                                            {
        //                                                textValue = elementValue["elm" + element.ElementId].Value;
        //                                            }
        //                                        }

        //                                        if (type == "selectbox")
        //                                        {
        //                                            if (element.SelectOptions != null && element.SelectOptions.Count > 0)
        //                                            {
        //                                                foreach (var soption in element.SelectOptions)
        //                                                {
        //                                                    var soptionValue = soption.Value;
        //                                                    if (soptionValue != null)
        //                                                    {
        //                                                        soptionValue = soptionValue.Trim();
        //                                                        if (soptionValue == textValue.Trim())
        //                                                        {
        //                                                            iconUrl = soption.IconUrl;
        //                                                        }
        //                                                    }


        //                                                }

        //                                            }
        //                                        }

        //                                        item.VisibleInFooterViewModel.Add(new VisibleInFooterViewModel
        //                                        {
        //                                            Text = textValue,
        //                                            Type = type,
        //                                            IconUrl = iconUrl
        //                                        });
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //        }

        //        return View("~/Themes/Blue/User/GetListOfCasesByYear.cshtml", model.Where(x => !string.IsNullOrEmpty(x.QueueName)));
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        [Route("st/adminuser/register")]
        [Route("st/adminuser/register.html")]
        public IActionResult Register()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/User/Register.cshtml");
        }

        [HttpPost]
        [Route("st/adminuser/register")]
        [Route("st/adminuser/register.html")]
        public async Task<IActionResult> Register(Service.Models.SimpleTransfer.User.RegisterUserViewModel ruvm)
        {
            //need tenant id for registration
            if (ModelState.IsValid)
            {
                UserViewModel uvm = new UserViewModel
                {
                    Id = "0",
                    Email = ruvm.Email,
                    Password = ruvm.Password,
                    TenantId = 0
                };

                var result = await IUserService.CreateOrUpdate(uvm);

                ApplicationUser user = await userManager.FindByEmailAsync(uvm.Email);
                if (result.Id != null)
                {
                    Log.LogInformation("New User Created.");
                    _toastNotification.AddSuccessToastMessage("Please Confirm you email before you login.");
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action("confirmemail",
                    "user", new { userId = user.Id, code },
                    protocol: Request.Scheme);

                    string settings = templateService.GetEmailGeneralSetting();
                    JObject mailObject = (JObject)JsonConvert.DeserializeObject(settings);
                    int key = (int)EmailSettingFor.UserCreation;
                    int templateId = Convert.ToInt16(mailObject[key.ToString()]);
                    TemplateViewModel templateViewModel = templateService.GetTemplateById(templateId);
                    string content = templateService.CreateEmailTemplate(templateViewModel.Content, 0, 0, 0, callbackUrl, user.Id);
                    var messageNew = new TemplateEmailViewModel { };
                    messageNew.Content = content;
                    string body = razorViewToStringRenderer.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", messageNew).GetAwaiter().GetResult();
                    string subject = "Confirm your email";

                    await _EmailSender.SendEmailAsync(uvm.Email, subject,
                        body);
                    return RedirectToAction("Login");

                }
            }
            _toastNotification.AddErrorToastMessage("Invalid Fields");
            return View("/Themes/" + this.Theme.GetName(false) + "/User/Register.cshtml");
        }

        [Route("st/adminuser/login")]
        [Route("st/adminuser/login.html")]
        public IActionResult Login()
        {
            return View("~/Areas/Admin/Views/SimpleTransfer/BOUser/Login.cshtml");
        }

        [HttpPost]
        [Route("st/adminuser/login")]
        [Route("st/adminuser/login.html")]
        public async Task<IActionResult> LoginAsync(LoginViewModel lvm)
        {

            if (ModelState.IsValid)
            {
                if (!IUserService.CheckIfEmailExists(lvm.Email))
                {
                    _toastNotification.AddErrorToastMessage("Invalid User Name or Password.");
                    return Redirect("~/st/adminuser/login.html");
                }
                if (!IUserService.CheckIfActiveUser(lvm.Email))
                {
                    _toastNotification.AddErrorToastMessage("User is not activated.");
                    return Redirect("~/st/adminuser/login.html");
                }
                var user = await userManager.FindByEmailAsync(lvm.Email);
                var roles = await userManager.GetRolesAsync(user);

                if(roles.Contains("Customer SimpleTransfer"))
                {
                    _toastNotification.AddErrorToastMessage("Invalid User Name and Password Combination.");
                    return Redirect("~/st/adminuser/login.html");
                }
                var result = await IUserService.Login(lvm.Email, lvm.Password, lvm.RememberMe, true);

                if (result != null )
                {
                    if (result.Succeeded)
                    {
                        var isSuperAdmin = await IUserService.IsSuperAdminEmail(lvm.Email);
                        if (isSuperAdmin == false)
                        {
                            string tenantIdentifier = await IUserService.GetTenantIdentifierbyEmail(lvm.Email);
                            if (!string.IsNullOrEmpty(tenantIdentifier))
                            {
                                HttpContext.Session.SetString("tenant_identifier", tenantIdentifier);
                            }
                        }
                        var checkRole = IUserService.UserHasPolicy();
                        if (checkRole == "backend" || IUserService.IsSuperAdmin().Result == true|| isSuperAdmin)
                        {
                            return Redirect("~/admin.html");
                        }
                    }                
                }              
            }
            _toastNotification.AddErrorToastMessage("Invalid User Name and Password Combination.");
            return Redirect("~/st/adminuser/login.html");
        }

        [Authorize(Policy = "FrontOffice")]
        [Route("st/adminuser/profile.html")]
        [Route("st/adminuser/{tenant_identifier}/profile.html")]
        public async Task<IActionResult> Profile()
        {

            string loggedUser = commonService.getLoggedInUserId();
            var model = await IUserService.GetUserById(loggedUser);

            return View("/Themes/" + this.Theme.GetName(false) + "/User/Profile.cshtml", model);
        }

        [Area("Admin")]
        [Route("st/adminuser/profile.html")]
        [Route("st/adminuser/{tenant_identifier}/profile.html")]
        public async Task<IActionResult> AdminProfile()
        {

            string loggedUser = commonService.getLoggedInUserId();
            var model = await IUserService.GetUserById(loggedUser);

            return View("/Areas/Admin/Views/User/Profile.cshtml", model);
        }

        //[Authorize(Policy = "FrontOffice")]
        [HttpPost]
        [Route("st/adminuser/profile.html")]
        [Route("st/adminuser/{tenant_identifier}/profile.html")]
        public async Task<IActionResult> Profile(UserViewModel uvm)
        {
            uvm.RoleId = commonService.GetRoleIdByUserId(uvm.Id);
            string checkEmailChange = "";
            if (uvm.Id !="0")
            {
                checkEmailChange =  IUserService.GetUserById(uvm.Id).GetAwaiter().GetResult().Email;
            }
            var model = await IUserService.CreateOrUpdate(uvm);
            if (checkEmailChange.ToUpper() != uvm.Email.ToUpper())
            {
                ApplicationUser user = await userManager.FindByEmailAsync(uvm.Email);
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("confirmemail",
                "user", new { userId = user.Id, code },
                protocol: Request.Scheme);
                string settings = templateService.GetEmailGeneralSetting();
                JObject mailObject = (JObject)JsonConvert.DeserializeObject(settings);
                int key = (int)EmailSettingFor.EmailConfirmation;
                int templateId = Convert.ToInt16(mailObject[key.ToString()]);
                TemplateViewModel templateViewModel = templateService.GetTemplateById(templateId);
                string content = templateService.CreateEmailTemplate(templateViewModel.Content, 0, 0, 0, callbackUrl, user.Id);
                var messageNew = new TemplateEmailViewModel { };
                messageNew.Content = content;
                string body = razorViewToStringRenderer.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", messageNew).GetAwaiter().GetResult();
                string subject = "Confirm your email";

                await _EmailSender.SendEmailAsync(uvm.Email, subject,
                   body);
                _toastNotification.AddSuccessToastMessage("User details changed successfully. Please check email to confirm.");
            }
            else
            {
                _toastNotification.AddSuccessToastMessage("User details changed successfully.");
            }
                

            var checkRole = IUserService.UserHasPolicy();
            if (checkRole == "backend" || IUserService.IsSuperAdmin().Result == true)
            {
                return Redirect("~/admin/profile.html");
            }

            return Redirect("~/user/profile.html");
        }

        [Route("st/adminuser/forgot")]
        [Route("st/adminuser/forgot.html")]
        public IActionResult Forgot()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/User/Forgot.cshtml");
        }

        [HttpPost]
        [Route("st/adminuser/forgot")]
        [Route("st/adminuser/forgot.html")]
        public async Task<IActionResult> Forgot(ForgotViewModel fvm)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(fvm.Email);

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("resetpassword",
                  "user", new { code },
                   protocol: Request.Scheme);

                //string message = templateService.GetTemplateBodyByName("forgot-password-email");

                //message = message.Replace("[user_fullname]", user.FirstName + " " + user.LastName);

                //message = message.Replace("[reset_link]", callbackUrl);

                //var messageNew = new TemplateEmailViewModel { };
                //messageNew.Content = message;
                string settings = templateService.GetEmailGeneralSetting();
                JObject mailObject = (JObject)JsonConvert.DeserializeObject(settings);
                int key = (int)EmailSettingFor.PasswordReset;
                int templateId = Convert.ToInt16(mailObject[key.ToString()]);
                TemplateViewModel templateViewModel = templateService.GetTemplateById(templateId);
                string content = templateService.CreateEmailTemplate(templateViewModel.Content, 0, 0, 0, callbackUrl, user.Id);
                var messageNew = new TemplateEmailViewModel { };
                messageNew.Content = content;
                string body = razorViewToStringRenderer.RenderViewToStringAsync("Areas/Admin/Views/Email/TemplateEmail.cshtml", messageNew).GetAwaiter().GetResult();
                string subject = "Reset Password";
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                await _EmailSender.SendEmailAsync(fvm.Email, "Reset Password", body);
                _toastNotification.AddSuccessToastMessage("Email Sent.");
                return View("/Themes/" + this.Theme.GetName(false) + "/User/Forgot.cshtml", fvm);
            }
            _toastNotification.AddErrorToastMessage("Invalid Email Address.");
            return View("/Themes/" + this.Theme.GetName(false) + "/User/Forgot.cshtml", fvm);
        }

       
        [Route("st/adminuser/change-password")]
        [Route("st/adminuser/{tenant_identifier}/change-password")]
        public IActionResult ChangePassword()
        {
            var model = IUserService.GetForChangePassword();

            return PartialView("/Themes/" + this.Theme.GetName(false) + "/User/_ChangePassword.cshtml", model);
            //return View("/Themes/" + this.Theme.GetName(false) + "/User/ChangePassword.cshtml", model);
        }

        [HttpPost]
        [Route("st/adminuser/change-password")]
        [Route("st/adminuser/{tenant_identifier}/change-password")]
        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel cpvm)
        {
            var result = await IUserService.ChangePassword(cpvm);

            if (result.Succeeded)
            {
                Log.LogInformation("Password Changed.");
                _toastNotification.AddSuccessToastMessage("Password changed.");
                return Json(new { status = "success", message = "Password changed successfully" });
                //return Redirect("~/user/logout.html");
            }
            _toastNotification.AddErrorToastMessage("Invalid Password.");
            return Json(new { status = "danger", message = "Invalid Password" });
            //status.Show("error", "A problem occured while changin password", false);
            //return Redirect("~/user/change-password");
            //return PartialView("_ChangePassword", model);
        }

        [Route("st/adminuser/signout")]
        [Route("st/adminuser/signout.html")]
        public async Task<IActionResult> SignOut()
        {
            var result = await IUserService.Logout();
            return Redirect("/st/adminuser/login.html");
        }

        [Route("st/adminuser/logout")]
        [Route("st/adminuser/logout.html")]
        public IActionResult LogOut()
        {
            IUserService.Logout();
            return Redirect("/st/adminuser/login.html");
        }

        [HttpGet]
        public IActionResult ResetPasswordSimpleTransfer(string code = null)
        {
            if (code == null)
            {
                _toastNotification.AddErrorToastMessage("A code must be supplied for password reset or Invalid Code.");
                throw new ApplicationException("A code must be supplied for password reset or Invalid Code.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View("/Themes/" + this.Theme.GetName(false) + "/User/ResetPassword.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordSimpleTransfer(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                _toastNotification.AddSuccessToastMessage("Password changed.");

                return Redirect("Login");
            }
            _toastNotification.AddErrorToastMessage("Invalid Email Address.");
            return View("/Themes/" + this.Theme.GetName(false) + "/User/ResetPassword.cshtml", model);
            //return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailSimpleTransfer(string userId = null, string code = null)
        {
            if (code == null || userId == null)
            {
                _toastNotification.AddErrorToastMessage("A code must be supplied for password reset or Invalid Code.");
                throw new ApplicationException("A code must be supplied for password reset or Invalid Code.");
            }
            var user = await userManager.FindByIdAsync(userId);
            await userManager.ConfirmEmailAsync(user, code);
            _toastNotification.AddSuccessToastMessage("Activate Sucessfully");
            return Redirect("/user/login.html");
        }

        [HttpGet]
        [Route("~/st/adminuser/confirm-email-sent")]
        [Route("~/st/adminuser/confirm-email-sent.html")]
        public IActionResult ConfirmEmailSent(string userId = null, string code = null)
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/User/ConfirmEmailSent.cshtml");
        }

        [HttpPost(Name = "CheckIfEmailExists")]
        public JsonResult CheckIfEmailExistsSimpleTransfer(string Email)
        {
            return Json(IUserService.IsDuplicateEmail(Email));

        }

        [HttpPost]
        [Route("/st/admin/media/remove.html")]
        public async Task<bool> RemoveGroup(int mediaId)
        {
            try
            {
                var result = await mediaService.DeleteById(mediaId);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}