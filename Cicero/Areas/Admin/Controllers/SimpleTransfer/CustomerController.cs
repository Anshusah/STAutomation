using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.API.LexisNexis;
using Cicero.Service.Models.SimpleTransfer.Customer;
using Cicero.Service.Services;
using Cicero.Service.Services.API;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomerController : BaseController
    {
        private readonly IUserService IUserService;
        private readonly IRoleService roleService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Utils Utils;
        private readonly ILogger<CustomerController> Log;
        private readonly IStatus status;
        private readonly IEmailSender _EmailSender;
        private readonly ICommonService commonService;
        private readonly ICaseService caseService;
        private readonly IQueueService queueService;
        private readonly ITemplateService templateService;
        private readonly IRazorToStringRender razorViewToStringRenderer;
        private readonly IToastNotification _toastNotification;
        private readonly IMediaService mediaService;
        private readonly ICustomerUserService customerUserService;
        private readonly ICustomerService customerService;
        private readonly ILexisNexisService lexisNexisService;

        public CustomerController(IUserService _UserService, Utils _Utils, ILogger<CustomerController> _Log, IStatus _status,
            ICaseService _caseService, IRazorToStringRender _razorViewToStringRenderer, ITemplateService _templateService,
            IRoleService _roleService, UserManager<ApplicationUser> _userManager, IEmailSender emailSender,
            ICommonService _commonService, IQueueService _queueService,
            IToastNotification toastNotification, IMediaService mediaService,
            ICustomerUserService customerUserService, ICustomerService customerService, ILexisNexisService lexisNexisService) : base(_UserService)
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
            razorViewToStringRenderer = _razorViewToStringRenderer;
            _toastNotification = toastNotification;
            this.mediaService = mediaService;
            this.customerUserService = customerUserService;
            this.customerService = customerService;
            this.lexisNexisService = lexisNexisService;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("st/admin/customers.html")]
        [Route("st/admin/{tenant_identifier}/customers.html")]
        public ActionResult Index(string tenant_identifier)
        {
            return View("/Areas/Admin/Views/SimpleTransfer/Customer/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("st/admin/customers.html")]
        [Route("st/admin/{tenant_identifier}/customers.html")]
        public JsonResult Index(DTPostModel model)
        {
            var user = customerUserService.GetUserListByFilter(model, "frontend");
            return Json(new
            {
                draw = user.draw,
                recordsTotal = user.recordsTotal,
                recordsFiltered = user.recordsFiltered,
                data = user.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("st/admin/customers/{id}/edit.html")]
        [Route("st/admin/{tenant_identifier}/customers/{id}/edit.html")]
        public async Task<IActionResult> Edit(string id)
        {
            CustomerUserViewModel userViewModel = new CustomerUserViewModel { Id = id, CreatedAt = Utils.GetDefaultDateFormat(DateTime.Now), UpdatedAt = Utils.GetDefaultDateFormat(DateTime.Now), Status = true, OnfidoDocuments = new List<string>(), BeneficiaryList = new List<SelectListItem>() };
            if (id != "0")
            {
                userViewModel = await customerUserService.GetUserById(id);
                var spcData = await lexisNexisService.GetSanctionPepCustomer(userViewModel.Id);
                if (spcData != null)
                {
                    userViewModel.PersonId = spcData.PersonId;
                    userViewModel.Match = spcData.Match ? "confirmMatch" : "noMatch";
                    userViewModel.Remarks = spcData.Remarks;
                }
            }
            userViewModel.RoleList = roleService.GetRoleList();
            userViewModel.RoleList.Where(x => x.Value == ApplicationConstants.CustomerRoleId).FirstOrDefault().Selected = true;
            return View("/Areas/Admin/Views/SimpleTransfer/Customer/Edit.cshtml", userViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("st/admin/customers/{id}/edit.html")]
        [Route("st/admin/{tenant_identifier}/customers/{id}/edit.html")]
        public async Task<IActionResult> Edit(CustomerUserViewModel model)
        {

            try
            {
                model.SanctionPepPerson = await customerUserService.GetPersonByUserId(model.Id);
                if (model.Id != "0" && string.IsNullOrEmpty(model.Password))
                {
                    model.Password = "Abcd1#";
                    model.ConfirmPassword = "Abcd1#";
                }
                ModelState.Clear();
                TryValidateModel(model);
                if (ModelState.IsValid)
                {
                    if (model.Id != "0" && string.IsNullOrEmpty(model.Password))
                    {
                        model.Password = "";
                        model.ConfirmPassword = "";
                        //ModelState.Remove("Password");
                    }
                    string checkNewUser = model.Id;
                    string loggedUser = IUserService.getLoggedInUserId();
                    model.CreatedBy = loggedUser;
                    string checkNewEmail = "";
                    if (model.Id != "0")
                    {
                        checkNewEmail = customerUserService.GetUserById(model.Id).GetAwaiter().GetResult().Email;
                    }
                    model = await customerUserService.CreateOrUpdate(model);
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

                        if (model.Match == "noMatch")
                        {
                            var result = await SaveSanctionPepCustomerData(model.Id, model.PersonId, model.Match, model.Remarks);
                        }
                        else
                        {
                            if (model.Match == "confirmMatch")
                            {
                                var result = await SaveSanctionPepCustomerData(model.Id, model.PersonId, model.Match, model.Remarks);
                            }
                        }

                    }
                    return Redirect("~/st/admin" + Utils.GetTenantForUrl(false) + "/customers/" + model.Id + "/edit.html");
                }
                else
                {
                    Utils.addModelError(ModelState);
                }
                model.RoleList = roleService.GetRoleList();
                return View("/Areas/Admin/Views/SimpleTransfer/Customer/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.LogError("UserServices:Edit - " + ex.ToString());

                _toastNotification.AddErrorToastMessage(ex.ToString());
                ///return Redirect(this.errorPageUrl);
                model.RoleList = roleService.GetRoleList();
                return View("/Areas/Admin/Views/SimpleTransfer/Customer/Edit.cshtml", model);
            }
        }

        private async Task<bool> SaveSanctionPepCustomerData(string userId, int personId, string type, string remarks)
        {
            try
            {
                var sanctionPepCustomerData = new SanctionPepCustomerViewModel();
                var lexisNexisData = await lexisNexisService.GetLexisNexisData(userId);
                sanctionPepCustomerData.LexisNexisId = lexisNexisData.Id;
                sanctionPepCustomerData.IsMatch = type == "noMatch" ? false : true;
                sanctionPepCustomerData.IsVerified = true;
                sanctionPepCustomerData.Remarks = remarks;

                var res = await lexisNexisService.CreateOrUpdate(sanctionPepCustomerData);
                var result = type == "noMatch" ? await lexisNexisService.Remove(userId) : await lexisNexisService.RemoveExceptPassed(userId, personId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [Area("Admin")]
        [HttpPost]
        [Route("st/admin/customers/action.html")]
        [Route("st/admin/{tenant_identifier}/customers/action.html")]
        public async Task<IActionResult> Action(IEnumerable<string> Ids, string action, string page)
        {
            var status = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddWarningToastMessage("Please select any action from Action select box.");
                return Redirect("~/st/admin" + Utils.GetTenantForUrl(false) + "/customers.html");
            }
            if (string.IsNullOrEmpty(Ids.ToString()) || Ids.Count() <= 0)
            {
                _toastNotification.AddWarningToastMessage("Please select atleast one user.");
                return Redirect("~/st/admin" + Utils.GetTenantForUrl(false) + "/customers.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {
                bool result = false;
                if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                {
                    status = ButtonAction.delete.ToDescription();
                    result = await customerUserService.DeleteUserById(item);
                }
                else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                {
                    status = ButtonAction.active.ToDescription();
                    result = await customerUserService.ActiveUserById(item);
                }
                else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                {
                    status = ButtonAction.inactive.ToDescription();
                    result = await customerUserService.InactiveUserById(item);
                }
                if (result)
                {
                    successCount++;
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Cannot delete loggedin user.");
                }
            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " customer(s) " + status);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddInfoToastMessage(successCount + " customer(s) " + status);
            }
            else
            {
                _toastNotification.AddInfoToastMessage(successCount + " customer(s) " + status);
            }

            return Redirect("~/st/admin" + Utils.GetTenantForUrl(false) + "/customers.html");

        }


        [Route("admin/customer/getcarddetails")]
        [HttpGet]
        public async Task<IActionResult> GetCardDetails(string userId)
        {
            object response;
            try
            {
                var cardDetails = await customerService.GetCardDetails(userId);
                cardDetails.Insert(0, new SelectListItem { Text = "Select Card Number", Value = "" });
                object data = new { cardDetails = cardDetails.ToJson() };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = data };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [Route("admin/customer/getcarddetail")]
        [HttpGet]
        public async Task<IActionResult> GetCardDetail(int cardNumber)
        {
            object response;
            try
            {
                var cardDetail = await customerService.GetCardDetail(cardNumber);
                var number = "";
                var cardExpMonth = GetExpMonth();
                var cardExpYear = GetExpYear();
                var cardSecurityCode = "";

                if (cardDetail != null)
                {
                    var expDates = cardDetail.ExpDate.Split('/');
                    number = cardDetail.Number;
                    cardExpMonth.Where(x => x.Value == expDates[0]).Select(x => x).FirstOrDefault().Selected = true;
                    cardExpYear.Where(x => x.Value == expDates[1]).Select(x => x).FirstOrDefault().Selected = true;
                    cardSecurityCode = cardDetail.Csv;
                }

                object data = new { number = number, expMonth = cardExpMonth, expYear = cardExpYear, csv = cardSecurityCode };
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = data };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        private List<SelectListItem> GetExpMonth()
        {
            var expMonthList = new List<SelectListItem>();

            expMonthList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = ""
            });

            expMonthList.Add(new SelectListItem
            {
                Text = "Jan",
                Value = "1"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Feb",
                Value = "2"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Mar",
                Value = "3"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Apr",
                Value = "4"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "May",
                Value = "5"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Jun",
                Value = "6"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Jul",
                Value = "7"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Aug",
                Value = "8"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Sep",
                Value = "9"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Oct",
                Value = "10"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Nov",
                Value = "11"
            });
            expMonthList.Add(new SelectListItem
            {
                Text = "Dec",
                Value = "12"
            });

            return expMonthList;
        }

        private List<SelectListItem> GetExpYear()
        {
            var expYearList = new List<SelectListItem>();

            expYearList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = ""
            });

            expYearList.Add(new SelectListItem
            {
                Text = "2020",
                Value = "2020"
            });
            expYearList.Add(new SelectListItem
            {
                Text = "2021",
                Value = "2021"
            });
            expYearList.Add(new SelectListItem
            {
                Text = "2022",
                Value = "2022"
            });
            expYearList.Add(new SelectListItem
            {
                Text = "2023",
                Value = "2023"
            });
            expYearList.Add(new SelectListItem
            {
                Text = "2024",
                Value = "2024"
            });
            expYearList.Add(new SelectListItem
            {
                Text = "2025",
                Value = "2025"
            });
            expYearList.Add(new SelectListItem
            {
                Text = "2026",
                Value = "2026"
            });

            return expYearList;
        }

    }
}
