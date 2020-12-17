﻿using AutoMapper.Configuration;
using Cicero.Data.Entities;
using Cicero.Data.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.JazzCash;
using Cicero.Service.Services;
using Cicero.Service.Services.JazzCash;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cicero.Service.Enums;
namespace Cicero.Areas.Admin.Controllers.JazzCash
{
    public class PayeeController : BaseController
    {
        private readonly IUserService IUserService;
        private readonly IRoleService roleService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Utils Utils;
        private readonly ILogger<PayeeController> Log;
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
        private readonly ISmsService smsService;
        private readonly IOnfidoService onfidoService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IPayeeService PayeeService;
        private readonly IRecipientService recipientService;
        private readonly INotificationService notificationService;

        public PayeeController(IUserService _UserService, Utils _Utils, ILogger<PayeeController> _Log, IStatus _status,
          ICaseService _caseService, IRazorToStringRender _razorViewToStringRenderer, ITemplateService _templateService,
          IRoleService _roleService, UserManager<ApplicationUser> _userManager, IEmailSender emailSender,
          ICommonService _commonService, IQueueService _queueService, IFormBuilderService _formbuilderService,
          IFormService formService, IToastNotification toastNotification, IMediaService mediaService, AppSetting _appSetting, ISmsService smsService,
          IOnfidoService onfidoService, IHostingEnvironment HostingEnvironment, IPayeeService PayeeService, IRecipientService recipientService, INotificationService notificationService) : base(_UserService)
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
            this.smsService = smsService;
            this.onfidoService = onfidoService;
            hostingEnvironment = HostingEnvironment;
            this.PayeeService = PayeeService;
            this.recipientService = recipientService;
            this.notificationService = notificationService;
        }

        [Route("jazzcash/Payee/onlinepayment")]
        [Route("jazzcash/Payee/onlinepayment.html")]
        public IActionResult OnlinePayment(int id)
        {
            var data = PayeeService.GetPaymentRequestData(id).Result;
            if (data == null)
            {
                data = new JazzCashPaymentRequestViewModel();
                data.Id = 1;
                data.JazzCashAccountNumber = "33fffff";
                data.PayeeName = "John Doe";
                data.RequestAmount = 2300;
                data.Status = 1;
                data.RequestId = "asd7878";
                data.Email = "johndoe@sample.com";
            }
            HttpContext.Session.SetString("id", data.Id.ToString());
            var themeName = this.Theme.GetName(false);
            themeName = "JazzCash";
            return View("/Themes/" + themeName + "/Payee/OnlinePayment.cshtml", data);
        }
        [Route("jazzcash/Payee/landingpage")]
        [Route("jazzcash/Payee/landingpage.html")]
        public IActionResult LandingPage()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/LandingPage.cshtml");
        }


        [Route("jazzcash/Payee/login")]
        [Route("jazzcash/Payee/login.html")]
        public IActionResult Login()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/User/Login.cshtml");
        }

        [HttpPost]
        [Route("jazzcash/Payee/login")]
        [Route("jazzcash/Payee/login.html")]
        public async Task<IActionResult> LoginAsync(LoginViewModel lvm)
        {

            if (ModelState.IsValid)
            {
                if (!IUserService.CheckIfEmailExists(lvm.Email))
                {
                    _toastNotification.AddErrorToastMessage("Invalid User Name or Password.");
                    return Redirect("~/jazzcash/Payee/login.html");
                }
                if (!IUserService.CheckIfActiveUser(lvm.Email))
                {
                    _toastNotification.AddErrorToastMessage("User is not activated.");
                    return Redirect("~/jazzcash/Payee/login.html");
                }
                var result = await IUserService.Login(lvm.Email, lvm.Password, lvm.RememberMe, true);
                if (result != null)
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
                                var check = HttpContext.Session.GetString("tenant_identifier");
                            }
                        }
                        var checkRole = IUserService.UserHasPolicy();
                        if (checkRole == "backend" || IUserService.IsSuperAdmin().Result == true)
                        {
                            return Redirect("~/st/adminuser/login.html");
                        }
                        return Redirect("~/jazzcash/Payee/index.html");
                    }

                    if (result.IsLockedOut)
                    {
                        _toastNotification.AddErrorToastMessage("Account is Locked. Please contact customer care.");
                        return Redirect("~/jazzcash/Payee/login.html");
                    }
                }
                _toastNotification.AddErrorToastMessage("Invalid User Name or Password.");
                return Redirect("~/jazzcash/Payee/login.html");
            }
            _toastNotification.AddErrorToastMessage("Invalid User Name and Password Combination Or Confirm your email.");
            return Redirect("~/jazzcash/Payee/login.html");
        }


        [Route("jazzcash/Payee/register")]
        [Route("jazzcash/Payee/register.html")]
        public IActionResult Register()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/Register.cshtml");
        }

        [Route("jazzcash/Payee/index")]
        [Route("jazzcash/Payee/index.html")]
        public IActionResult Index()
        {
            var dashboardViewModel = new Cicero.Service.Models.JazzCash.DashboardViewModel();
            dashboardViewModel.PaymentDetails = new List<Service.Models.JazzCash.PaymentDetails>();
            //dashboardViewModel.PaymentDetails = GetPaymentDetailsData();
            dashboardViewModel.FullName = IUserService.GetUserFullName().Result;
            dashboardViewModel.CustomerId = IUserService.getLoggedInUserId();
            dashboardViewModel.NotificationCount = notificationService.GetAllNotificationCount(commonService.getLoggedInUserId(), commonService.GetRoleIdByUserId(commonService.getLoggedInUserId()));
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/index.cshtml", dashboardViewModel);
        }

        [Route("jazzcash/Payee/register")]
        [Route("jazzcash/Payee/register.html")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterPayeeViewModel data)
        {
            try
            {
                UserViewModel uvm = new UserViewModel
                {
                    Id = "0",
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.Username,
                    Password = data.Password,
                    TenantId = 0,
                    Address = data.Address,
                    PhoneNumber = data.MobileNumber,
                };
                if (!smsService.IsSMSVerified(data.MobileNumber,data.CountryCode))
                {
                    _toastNotification.AddErrorToastMessage("Mobile number not verfied.");
                    return View("/Themes/" + this.Theme.GetName(false) + "/Payee/Register.cshtml", data);
                }
                var result = await IUserService.CreateOrUpdate(uvm);

                ApplicationUser user = await userManager.FindByEmailAsync(uvm.Email);
                if (result.Id != null)
                {
                    Log.LogInformation("New User Created.");
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
                    HttpContext.Session.SetString("userId", result.Id);


                    var Payee = new PayeeViewModel();

                    if (data.PayeeType == PayeeType.Individual)
                    {
                        Payee.DOB = data.DOB;
                    }
                    else
                    {
                        Payee.CompanyName = data.CompanyName;
                        Payee.TypeOfBusinessEntity = data.TypeOfBusinessEntity;
                        Payee.CompanyWebsite = data.CompanyWebsite;
                        Payee.CompanyRegistrationNumber = data.CompanyRegistrationNumber;
                    }

                    Payee.PayeeType = data.PayeeType;
                    Payee.UserId = result.Id;
                    Payee.FirstName = data.FirstName;
                    Payee.LastName = data.LastName;
                    Payee.Email = data.Email;
                    Payee.CountryCode = data.CountryCode;
                    Payee.Address = data.Address;
                    Payee.Address2 = data.Address2;
                    Payee.City = data.City;
                    Payee.PostCode = data.PostCode;
                    Payee.MobileNumber = data.MobileNumber;
                    Payee.IssuingCountry = data.IssuingCountry;
                    Payee.IdType = data.IdType;
                    Payee.IdNumber = data.IdNumber;

                    Payee.JazzCashAccount = data.JazzCashAccount;
                    Payee.Status = true;
                    var payeeData = PayeeService.CreateOrUpdate(Payee).Result;
                    return Ok();

                }

                _toastNotification.AddErrorToastMessage("Invalid Fields");
                return StatusCode(404);
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }

        }

        [Authorize]
        [Route("jazzcash/Payee/dashboard.html")]
        [Route("jazzcash/Payee/{tenant_identifier}/dashboard.html")]
        public IActionResult Index(bool all = false)
        {
            var checkRole = IUserService.UserHasPolicy();
            if (checkRole == "backend" || IUserService.IsSuperAdmin().Result == true)
            {
                return Redirect("~/st/adminuser/login.html");
            }
            else
            {
                return View("/Themes/" + this.Theme.GetName(false) + "/User/Index.cshtml");
            }

        }

        //public List<Service.Models.JazzCash.PaymentDetails> GetPaymentDetailsData()
        //{
        //    var data = new List<Service.Models.JazzCash.PaymentDetails>();
        //    data.Add(new Service.Models.JazzCash.PaymentDetails
        //    {
        //        PayeeName = "Test Test",
        //        Date = "03 June 2020",
        //        Amount = 300,
        //        Currency = "GBP",
        //        Status = Service.Models.JazzCash.PaymentStatus.PaymentInProgress
        //    });

        //    data.Add(new Service.Models.JazzCash.PaymentDetails
        //    {
        //        PayeeName = "Test1 Test1",
        //        Date = "03 June 2020",
        //        Amount = 600,
        //        Currency = "GBP",
        //        Status = Service.Models.JazzCash.PaymentStatus.PaymentPending
        //    });

        //    data.Add(new Service.Models.JazzCash.PaymentDetails
        //    {
        //        PayeeName = "Test2 Test2",
        //        Date = "03 June 2020",
        //        Amount = 380,
        //        Currency = "GBP",
        //        Status = Service.Models.JazzCash.PaymentStatus.PaymentReceived
        //    });

        //    return data;
        //}

        //Recipient List

        [Route("jazzcash/Payee/recipientlist")]
        [Route("jazzcash/Payee/recipientlist.html")]
        public IActionResult RecipientList()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/ListOfRecipient.cshtml");
        }

        [HttpPost]
        [Route("jazzcash/Payee/recipientlist")]
        [Route("jazzcash/Payee/recipientlist.html")]
        public JsonResult RecipientList(DTPostModel model)
        {
            var recipient = recipientService.GetRecipientListByFilter(model);
            return Json(new
            {
                draw = recipient.draw,
                recordsTotal = recipient.recordsTotal,
                recordsFiltered = recipient.recordsFiltered,
                data = recipient.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("jazzcash/recipient/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {
            var data = new RecipientViewModel { Id = id, CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now) };
            if (id != 0)
            {
                data = await recipientService.GetRecipientByIdAsync(id);
                data.FullName = data.FirstName + " " + data.LastName;
            }
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/edit.cshtml", data);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("jazzcash/recipient/{id}/edit.html")]
        public async Task<IActionResult> Edit(RecipientViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model = await recipientService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("Recipient is saved.");
                    return View("/Themes/" + this.Theme.GetName(false) + "/Payee/edit.cshtml", model);
                }

                Utils.addModelError(ModelState);
                return View("/Themes/" + this.Theme.GetName(false) + "/Payee/edit.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.LogError("Recipient:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Themes/" + this.Theme.GetName(false) + "/Payee/edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("jazzcash/recipient/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/jazzcash/Payee/recipientlist.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one question.");
                return Redirect("~/jazzcash/Payee/recipientlist.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {

                bool result = false;
                if (item != 0)
                {
                    state = ButtonAction.delete.ToDescription();
                    result = await recipientService.DeleteRecipientById(item);
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " recipient(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " recipient(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " recipient(s) " + state);
            }

            return Redirect("~/jazzcash/Payee/recipientlist.html");

        }
        //Pending Information Dashboard

        [Route("jazzcash/Payee/transactiondashboard")]
        [Route("jazzcash/Payee/transactionsdashboard.html")]
        public IActionResult InformationDashboard()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/InformationDashboard.cshtml");
        }

        //Transactions

        [Route("jazzcash/Payee/transactions")]
        [Route("jazzcash/Payee/transactions.html")]
        public IActionResult Transactions()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/Transactions.cshtml");
        }

        //Transactions details

        [Route("jazzcash/Payee/transactiondetails")]
        [Route("jazzcash/Payee/transactiondetails.html")]
        public IActionResult TransactionDetails()
        {
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/TransactionDetails.cshtml");
        }


        [Route("jazzcash/Payee/notifications")]
        [Route("jazzcash/Payee/notifications.html")]
        public IActionResult Notifications()
        {
            List<NotificationViewModel> notifications = new List<NotificationViewModel>();
            notifications = notificationService.GetAllNotification(commonService.getLoggedInUserId(), commonService.GetRoleIdByUserId(commonService.getLoggedInUserId()));
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/Notifications.cshtml", notifications);
        }

        [HttpGet]
        [Route("st/api/getpaymentrequestdetail")]
        public IActionResult GetPaymentRequestDetail(string formid)
        {
            object response;
            try
            {
                var paymentRequestData = PayeeService.GetPaymentRequestData(Utils.DecryptId(formid)).Result;
                object data = new { feeamount = "10.00 GBP",
                                    totalamount=paymentRequestData.RequestAmount,
                                    payeereceiveamount=paymentRequestData.RequestAmount,
                                    payeename=paymentRequestData.PayeeName,
                                    requestamount=paymentRequestData.RequestAmount,
                                    requestid=paymentRequestData.RequestId};
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = data };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }
    }
}

