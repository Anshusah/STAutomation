using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MessageController : BaseController
    {
        private readonly IMessageService messageService;
        private readonly IUserService userService;
        private readonly Utils utils;
        private readonly ICommonService commonService;

        private bool isBackendUser;

        public MessageController(IMessageService _messageService, IUserService _userService, Utils _utils, ICommonService _commonService) : base(_userService)
        {
            messageService = _messageService;
            userService = _userService;
            utils = _utils;
            commonService = _commonService;
            isBackendUser = (_userService.GetBackendUserList().Select(x => x.Id).ToList().Contains(_commonService.getLoggedInUserId())) ? true : false;
        }

        [Area("Admin")]
        [Authorize(Policy = "BackOffice")]
        [HttpGet]
        [Route("/admin/{parentid}/messages.html")]
        [Route("/admin/{tenant_identifier}/{encryptedid}/messages.html")]
        public async Task<IActionResult> Index(string encryptedid)
        {

            int id = utils.DecryptId(encryptedid);

            if (id != 0)
            {
                var model = await messageService.GetMessagesByParentId(id, userService.getLoggedInUserId());
                return View(model);
            }

            return View();
        }

        [Area("Admin")]
        [Authorize(Policy = "BackOffice")]
        [HttpPost]
        [Route("/admin/{encryptedid}/messages.html")]
        [Route("/admin/{tenant_identifier}/{encryptedid}/messages.html")]
        public IActionResult Index(MessageViewModel mvm, string tenant_identifier, string[] recipients, int[] images)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    mvm.TenantId = commonService.GetTenantIdByIdentifier(tenant_identifier);
                    mvm.From = userService.getLoggedInUserId();
                    mvm.Attachment = JsonConvert.SerializeObject(images);
                    messageService.SendMessage(mvm, recipients);

                    return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/" + utils.EncryptId(mvm.Id) + "/messages.html");

                }

                return View(mvm);
            }
            catch (Exception)
            {
                return View(mvm);
            }

        }

        [Area("Admin")]
        [Authorize(Policy = "BackOffice")]
        [HttpPost]
        [Route("/admin/{tenant_identifier}/message/send-notice.html")]
        public JsonResult SendNotice(string tenant_identifier, int id, string subject, string message, bool isNotify)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(tenant_identifier);

            if (id != 0 && subject != null && message != null)
            {
                MessageViewModel mvm = new MessageViewModel
                {
                    ClaimId = id,
                    //ParentId = parentid,
                    Subject = subject,
                    Content = message,
                    ClientNotified = isNotify,
                    TenantId = tenantid
                };

                mvm = messageService.SendNotice(mvm);

                var userdetails = commonService.GetUserById(mvm.From).Result;

                mvm.NameSender = userdetails.FirstName + " " + userdetails.LastName;

                return Json(mvm);
            }

            return Json(null);
        }

        //frontoffice section
        [HttpGet]
        [Authorize(Policy = "FrontOffice")]
        [Route("/user/{encryptedid}/messages.html")]
        [Route("/user/{tenant_identifier}/{encryptedid}/messages.html")]
        public async Task<IActionResult> FrontIndex(string encryptedid)
        {

            int id = utils.DecryptId(encryptedid);
            //var model = new list<MessageViewModel>
            //{
            //    Id = id
            //};
            string vp = "/Themes/" + this.Theme.GetName(false) + "/Message/Index.cshtml";
            if (id != 0)
            {
                var model = await messageService.GetMessagesByParentId(id, userService.getLoggedInUserId());
                return View(vp, model);
            }

            return View(vp);
        }

        //[HttpGet]
        //[Route("/user/messagesView.html")]
        //public IActionResult FrontIndexView()
        //{
        //    string vp = "/Themes/" + this.Theme.GetName(false) + "/Message/Messages.cshtml";
        //    return View(vp);
        //}

        // [Authorize(Policy = "FrontOffice")]
        [HttpPost]
        //[Route("/user/{encryptedid}/messages.html")]
        [Route("/user/{tenant_identifier}/{encryptedid}/messages.html")]
        public async Task<IActionResult> Message(MessageViewModel mvm, string tenant_identifier, string encryptedid, string[] recipients, int[] images)
        {
            string vp = "/Themes/" + this.Theme.GetName(false) + "/Message/Index.cshtml";
            try
            {
                int id = utils.DecryptId(encryptedid);

                mvm.TenantId = commonService.GetTenantIdByIdentifier(tenant_identifier);

                mvm.From = userService.getLoggedInUserId();

                mvm.Attachment = JsonConvert.SerializeObject(images);
                messageService.SendMessage(mvm, recipients);
                var isSuperAdmin = await commonService.IsSuperAdmin();
                if (isBackendUser || isSuperAdmin)
                {
                    return Redirect("~/admin/" + utils.EncryptId(mvm.Id) + "/messages.html");
                }
                return Redirect("~/user/" + utils.EncryptId(mvm.Id) + "/messages.html");
            }
            catch (Exception)
            {
                return View(vp, mvm);
            }
        }

        [HttpGet]
        //  [Authorize(Policy = "FrontOffice")]
        //[Route("/user/{encryptedid}/messagesDetail.html")]
        [Route("/user/messageDetail.html")]
        public async Task<IActionResult> FrontIndexMessageDetail(int id, bool isRead)
        {

            string vp = "/Themes/" + this.Theme.GetName(false) + "/Message/MessageDetail.cshtml";
            var isSuperAdmin = await commonService.IsSuperAdmin();
            if (isBackendUser || isSuperAdmin)
            {
                vp = "/Areas/Admin/Views/Message/MessageDetail.cshtml";
            }
            if (id != 0)
            {
                var model = await messageService.GetMessageByMessageId(id);
                string loggedUser = commonService.getLoggedInUserId();
                if (!isRead)
                {
                    await messageService.GetMessagesByUserIdOrMarkMessageAsRead(loggedUser, new List<int>() { id }, true);
                }
                return View(vp, model);
            }

            return View(vp);
        }

        [Area("Admin")]
        [HttpGet]
        [Route("/admin/messageDetail.html")]
        public async Task<IActionResult> MessageDetail(int id, bool isRead)
        {

            string vp = "/Themes/" + this.Theme.GetName(false) + "/Message/MessageDetail.cshtml";
            var isSuperAdmin = await commonService.IsSuperAdmin();
            if (isBackendUser || isSuperAdmin)
            {
                vp = "/Areas/Admin/Views/Message/MessageDetail.cshtml";
            }
            if (id != 0)
            {
                var model = await messageService.GetMessageByMessageId(id);
                string loggedUser = commonService.getLoggedInUserId();
                if (!isRead)
                {
                    await messageService.GetMessagesByUserIdOrMarkMessageAsRead(loggedUser, new List<int>() { id }, true);
                }
                return View(vp, model);
            }

            return View(vp);
        }

        [HttpPost]
        [Route("/user/{tenant_identifier}/messages.html")]
        public async Task<JsonResult> FrontIndex(DTPostModel model)
        {
            var role = await messageService.GetMessageListByFilter(model);
            return Json(new
            {
                draw = role.draw,
                recordsTotal = role.recordsTotal,
                recordsFiltered = role.recordsFiltered,
                data = role.data
            });


        }

        [HttpPost]
        [Route("/user/markMessageAsRead.html")]
        public async Task<IActionResult> MarkMessageAsRead(List<int> Ids, string action = "")
        {
            string vp = "/Themes/" + this.Theme.GetName(false) + "/Message/Index.cshtml";
            try
            {
                var isSuperAdmin = await commonService.IsSuperAdmin();
                if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.markAsRead) || action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                {
                    if (Ids.Count == 0)
                    {
                        if (isBackendUser || isSuperAdmin)
                        {
                            return Redirect("~/admin/" + utils.EncryptId(0) + "/messages.html");
                        }
                        return Redirect("~/user/" + utils.GetTenantForUrl(false) + "/" + utils.EncryptId(0) + "/messages.html");
                    }
                }
                string loggedUser = commonService.getLoggedInUserId();
                if (Ids.Count > 0)
                {
                    if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.markAsRead))
                    {
                        await messageService.GetMessagesByUserIdOrMarkMessageAsRead(loggedUser, Ids, true);
                    }

                    if (action == "delete")
                    {
                        await messageService.DeleteMessage(loggedUser, Ids);
                    }
                }
                else
                {
                    await messageService.GetMessagesByUserIdOrMarkMessageAsRead(loggedUser, new List<int>(), true);
                }

                if (isBackendUser || isSuperAdmin)
                {
                    return Redirect("~/admin/" + utils.EncryptId(0) + "/messages.html");
                }
                return Redirect("~/user/" + utils.GetTenantForUrl(false) + "/" + utils.EncryptId(0) + "/messages.html");
            }
            catch (Exception ex)
            {
                return View(vp);
            }
        }

        [HttpGet]
        [Route("api/getcountries")]
        public IActionResult GetCountries()
        {
            object response;
            try
            {
                var caseList = commonService.CountryList();
                caseList.Insert(0, new SelectListItem { Text = "Select Country", Value = "" });
                object country = new { country = caseList.ToJson() };

                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = country };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Json(response);
        }
    }
}