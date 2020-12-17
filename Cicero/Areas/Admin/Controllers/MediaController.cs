using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Services;
using Cicero.Data;
using Cicero.Data.Entities;
using Core.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cicero.Service.Models;
using Cicero.Service.Library.Toastr;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MediaController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly IUserService userService;
        private readonly ILogger<MediaController> Log;
        private readonly Utils utils;
        private readonly IMapper IMapper;
        private readonly IStatus Status;
        private readonly IMediaService IMediaService;
        private readonly ITenantService tenantService;
        private readonly IToastNotification _toastNotification;

        public MediaController(IUserService _UserService, ApplicationDbContext _db, ILogger<MediaController> _Log,
            IStatus _status, Utils _utils, IMapper _IMapper, IMediaService _IMediaService, ITenantService _tenantService
            , IToastNotification toastNotification) : base(_UserService)
        {
            userService = _UserService;
            Log = _Log;
            Status = _status;
            utils = _utils;
            IMapper = _IMapper;
            db = _db;
            IMediaService = _IMediaService;
            tenantService = _tenantService;
            _toastNotification = toastNotification;
        }
        [Area("Admin")]
        [Route("/admin/medias.html")]
        [Route("/admin/{tenant_identifier}/medias.html")]
        public IActionResult Index(string tenant_identifier)
        {
            int tenantid = tenantService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            string userId = userService.getLoggedInUserId();
            var userRole = userService.UserHasPolicy();
            bool userAdmin = userService.IsSuperAdmin().Result;
            if (userRole == "frontend" && userAdmin == false)
            {
                var medias = IMediaService.GetImagesByTenantIdAndUser(tenantid, userId);
                //var medias = db.Media.Where(x => x.CreatedBy == userId && x.TenantId == tenantid).ToList();
                return View(medias);
            }
            var mdis = IMediaService.GetImagesByTenantId(tenantid);
            //var mdis = IMapper.Map<List<MediaViewModel>>(db.Media.Where(x => x.TenantId == tenantid).ToList());
            return View(mdis);
        }

        [Route("/user/medias.html")]
        [Route("/user/{tenant_identifier}/medias.html")]
        public IActionResult FrontIndex(string tenant_identifier, string from)
        {
            string vp = "/Themes/" + this.Theme.GetName(false) + "/Media/Index.cshtml";
            int tenantid = tenantService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            string userId = userService.getLoggedInUserId();
            var userRole = userService.UserHasPolicy();
            bool userAdmin = userService.IsSuperAdmin().Result;
            if (userRole == "frontend" && userAdmin == false)
            {
                var mediaIds = IMediaService.GetCaseMediaByUserId(userId).Select(x => x.MediaId).ToList();
                List<MediaByParentId> medias = IMediaService.GetImagesByTenantIdAndUser(tenantid, userId);
                foreach (var item in medias)
                {
                    item.Media = item.Media.Where(x => mediaIds.Contains(x.Id)).ToList();
                }

                ViewBag.From = from;
                return View(vp, medias);
            }
            var mdis = IMediaService.GetImagesByTenantId(tenantid);
            //var mdis = IMapper.Map<List<MediaViewModel>>(db.Media.Where(x => x.TenantId == tenantid).ToList());
            return View(vp, mdis);
        }

        [Area("Admin")]
        [Route("/admin/media/pick.html")]
        [Route("/admin/{tenant_identifier}/media/pick.html")]
        public async Task<IActionResult> Pick(string tenant_identifier, DateTime? timestamp, int? caseId = null)
        {
            if (timestamp == null)
            {
                ViewBag.timestamp = DateTime.Now;
            }
            else
            {
                ViewBag.timestamp = timestamp;
            }
            string userId = userService.getLoggedInUserId();
            int tenantid = tenantService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            ViewBag.Group = await IMediaService.GetMediaGroup(tenantid);
            var userRole = userService.UserHasPolicy();
            if (userRole == "frontend")
            {
                var caseMediaDatas = IMediaService.GetCaseMediaByUserId(userId).ToList();
                if (caseId != null)
                {
                    caseMediaDatas = IMediaService.GetCaseMediaByUserId(userId).Where(x => x.CaseId == caseId).ToList();
                }
                var mediaIds = caseMediaDatas.Select(x => x.MediaId).ToList();
                List<MediaByParentId> medias = IMediaService.GetImagesByTenantIdAndUser(tenantid, userId);

                if (caseId != null)
                {
                    foreach (var item in medias)
                    {
                        item.Media = item.Media.Where(x => mediaIds.Contains(x.Id)).ToList();
                    }
                }

                ViewBag.CaseId = caseId;
                return PartialView(medias.Where(x => x.ParentId != 0).ToList());
            }
            var mdis = IMediaService.GetImagesByTenantId(tenantid);
            return PartialView(mdis);
        }
        [Area("Admin")]
        [HttpGet]
        [Route("/admin/media/delete-by-id.html")]
        [Route("/admin/{tenant_identifier}/media/delete-by-id.html")]
        public async Task<IActionResult> DeleteById(int selected_image_id)
        {

            var result = await IMediaService.DeleteById(selected_image_id);
            if (result == true)
            {
                return Json(new { status = "true" });
            }

            return Json(new { status = "false" });
        }

        [Route("/admin/{filename}/media/download.html")]
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "uploads", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats  officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }


        [Area("Admin")]
        [HttpPost]
        [Route("admin/media/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/media/{id}/edit.html")]
        public async Task<IActionResult> Edit(MediaViewModel mvm, string tenant_identifier, DateTime? timestamp, string hide = null)
        {
            if (ModelState.IsValid)
            {
                if (mvm.UploadImage != null)
                {
                    if (timestamp == null)
                    {
                        timestamp = DateTime.Now;
                    }
                    mvm.CreatedAt = Convert.ToDateTime(timestamp);

                    mvm.TenantId = tenantService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                    var result = await IMediaService.CreateOrUpdate(mvm);
                    if (result == true)
                    {
                        return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/media/pick.html?timestamp=" + timestamp + "&hide=" + hide);
                    }
                }
                else
                {
                    _toastNotification.AddWarningToastMessage("Please select a file to upload.");
                }
            }
            else if (mvm.Title == null)
            {
                _toastNotification.AddWarningToastMessage("Please enter a title.");
            }
            else if (mvm.ParentId == 0)
            {
                _toastNotification.AddWarningToastMessage("Please select a group.");
            }
            else
            {
                _toastNotification.AddWarningToastMessage("Could not upload file.");
            }
            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/media/pick.html?timestamp=" + timestamp + "&hide=" + hide);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/media/{id}/caseEdit.html")]
        [Route("admin/{tenant_identifier}/media/{id}/caseEdit.html")]
        public async Task<MediaByParentId> Edit(MediaViewModel mvm, string tenant_identifier, DateTime? timestamp, string hide = null, int? caseId = null)
        {
            if (ModelState.IsValid)
            {
                if (mvm.UploadImage != null)
                {
                    if (timestamp == null)
                    {
                        timestamp = DateTime.Now;
                    }
                    mvm.CreatedAt = Convert.ToDateTime(timestamp);

                    mvm.TenantId = tenantService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                    var result = await IMediaService.CreateOrUpdate(mvm, true);
                    mvm.Id = result;

                    var mbpi = new MediaByParentId();
                    var groupName = (IMediaService.GetMediaById(mvm.ParentId) != null) ? IMediaService.GetMediaById(mvm.ParentId).Title : "";
                    mbpi.ParentId = mvm.ParentId;
                    mbpi.Parent = groupName;
                    mbpi.Media = new List<MediaViewModel>();
                    mbpi.Media.Add(mvm);
                    return mbpi;
                }
                else
                {
                    _toastNotification.AddWarningToastMessage("Please select a file to upload.");
                }
            }
            else if (mvm.Title == null)
            {
                _toastNotification.AddWarningToastMessage("Please enter a title.");
            }
            else if (mvm.ParentId == 0)
            {
                _toastNotification.AddWarningToastMessage("Please select a group.");
            }
            else
            {
                _toastNotification.AddWarningToastMessage("Could not upload file.");
            }
            return new MediaByParentId();
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/{tenant_identifier}/media/creategroup.html")]
        public async Task<IActionResult> CreateGroup(string tenant_identifier, IFormCollection form, DateTime? timestamp, string hide = null)
        {
            if (ModelState.IsValid)
            {
                string groupName = form["inputGroupName"].ToString();
                if (groupName != "" && groupName.Length <= 20)
                {
                    if (timestamp == null)
                    {
                        timestamp = DateTime.Now;
                    }
                    int tenantId = tenantService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                    await IMediaService.CreateOrUpdateMediaGroup(groupName, tenantId);
                }
                else
                {
                    var message = "Please enter group name";
                    if(groupName.Length > 20)
                    {
                        message += " less than 20 characters";
                    }
                    _toastNotification.AddWarningToastMessage(message + ".");
                }
            }
            else
            {
                _toastNotification.AddWarningToastMessage("Please enter group name.");
            }
            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/media/pick.html?timestamp=" + timestamp + "&hide=" + hide);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/mediaGroup/creategroup.html")]
        public async Task<int?> CreateGroups(string groupName, int mediaId = 0)
        {
            try
            {
                int tenantId = tenantService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                var result = await IMediaService.CreateOrUpdateMediaGroups(groupName, tenantId, mediaId);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/{tenant_identifier}/media/grouplist.html")]
        public async Task<IActionResult> GroupList(string tenant_identifier)
        {
            return Json(await IMediaService.GetMediaGroup(tenantService.GetTenantIdByIdentifier(tenant_identifier)));
        }
    }
}