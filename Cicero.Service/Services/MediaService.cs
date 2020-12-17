using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using LazZiya.ImageResize;

namespace Cicero.Service.Services
{

    public interface IMediaService
    {
        Task<bool> DeleteById(int id);
        Task<bool> CreateOrUpdate(MediaViewModel mvm);
        Task<int> CreateOrUpdate(MediaViewModel mvm, bool getData);
        dynamic GetImagesByTenantIdAndUser(int tenantid, string userId);
        List<MediaByParentId> GetImagesByTenantId(int tenantid);
        List<MediaViewModel> GetImagesByTenantIdandCaseId(int tenantid, int caseId);
        Task<bool> CreateOrUpdateMediaGroup(string groupName, int tenantId);
        Task<int> CreateOrUpdateMediaGroups(string groupName, int tenantId, int mediaId = 0);
        Task<List<MediaViewModel>> GetMediaGroup(int tenantId);
        List<MediaViewModel> GetImagesByIds(List<int> mediaIds);
        List<CaseMedia> GetCaseMediaByUserId(string userId);
        MediaViewModel GetMediaById(int id);
        List<MediaViewModel> GetQueueIconMedia();
        string GetFileSize(string file);
        bool CreateOrUpdateUserMediaGroup(List<string> MediaIds, string userId);
    }



    public class MediaService : IMediaService
    {

        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<MediaService> Log;
        private readonly IHttpContextAccessor IHttpContextAccessor = null;
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly IMapper IMapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;

        public MediaService(ApplicationDbContext _db, Utils _utils, ILogger<MediaService> _log, IHttpContextAccessor _httpContextAccessor, IHostingEnvironment _hostingEnvironment, IMapper _IMapper, IActivityLogService _activityLogService, ICommonService _commonService)
        {
            db = _db;
            Utils = _utils;
            Log = _log;
            IHttpContextAccessor = _httpContextAccessor;
            HostingEnvironment = _hostingEnvironment;
            IMapper = _IMapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
        }

        private async Task Create(Media model)
        {

            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            model.CreatedBy = loggedUser;
            await db.Media.AddAsync(model);

            await db.SaveChangesAsync();

            await activityLogService.CreateLog(loggedUser, "New Media created " + model.Title + ". Created By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

        }

        private async Task Update(MediaViewModel mvm)
        {
            var loggedUser = commonService.getLoggedInUserId();
            var fullName = commonService.GetUserFullName().Result;

            var updateModel = await db.Media.FindAsync(mvm.Id);

            updateModel.Title = mvm.Title;
            updateModel.Description = mvm.Description;
            if (!string.IsNullOrEmpty(mvm.Url))
            {
                updateModel.Url = mvm.Url;
            }

            db.Media.Update(updateModel);
            await db.SaveChangesAsync();

            await activityLogService.CreateLog(loggedUser, "Media updated " + mvm.Title + ". Updated By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

        }

        private async Task<string> CreateFile(MediaViewModel mvm)
        {
            var extension = System.IO.Path.GetExtension(mvm.UploadImage.FileName);
            string filename = System.Guid.NewGuid() + "." + extension;
            string folderpath = HostingEnvironment.ContentRootPath;
            string filepath = folderpath + "\\wwwroot\\uploads\\" + filename;
            if (mvm.Type == 3)
            {
                filepath = folderpath + "\\wwwroot\\images\\state\\" + filename;
            }
            //FileInfo fileinfo = new FileInfo(filepath);

            try
            {
                using (var stream = System.IO.File.Create(filepath))
                {
                    await mvm.UploadImage.CopyToAsync(stream);
                    // await mvm.UploadImage.CopyToAsync(new FileStream(filepath, FileMode.Create));
                }
                if (mvm.Type != 3)
                {
                    var extensionList = new List<string>() { ".jpg", ".jpeg", ".gif", ".png" };
                    if (extensionList.Contains(extension.ToLower()))
                    {
                        Bitmap img = (Bitmap)Image.FromFile(filepath);
                        Bitmap newBitmap = new Bitmap(img.Width, img.Height);
                        Graphics graphics = Graphics.FromImage(newBitmap);
                        graphics.DrawImage(img, 0, 0);


                        //var img = Image.FromFile(filepath);
                        var newImg = Image.FromFile(filepath);
                        var newFilePath = folderpath + "\\wwwroot\\uploads\\thumbnail\\" + filename;

                        #region scale by width, new width = 100, new heigth 100 
                        if(newBitmap.Width <= 150 || newBitmap.Height <= 150)
                        {
                            newBitmap.Crop(100, 100, TargetSpot.Center).SaveAs(newFilePath);
                        }
                        else
                        {
                            if(newBitmap.Width > newBitmap.Height)
                            {
                                newImg = newBitmap.ScaleByHeight(100);
                            }
                            else
                            {
                                newImg = newBitmap.ScaleByWidth(100);
                            }
                            newImg.Crop(100, 100, TargetSpot.Center).SaveAs(newFilePath);
                        }
                        //var newImg = ImageResize.Crop(img, 100, 100, TargetSpot.Center);

                        //newImg.SaveAs(newFilePath);
                        #endregion

                        if (img.Width >= 300)
                        {
                            #region scale by width, new width = 300 
                            newImg = ImageResize.ScaleByWidth(img, 300);
                            newFilePath = folderpath + "\\wwwroot\\uploads\\small\\" + filename;

                            newImg.SaveAs(newFilePath);
                            #endregion
                        }

                        if (img.Width >= 600)
                        {
                            #region scale by width, new width = 600 
                            newImg = ImageResize.ScaleByWidth(img, 600);
                            newFilePath = folderpath + "\\wwwroot\\uploads\\medium\\" + filename;

                            newImg.SaveAs(newFilePath);
                            #endregion
                        }

                        if (img.Width >= 900)
                        {
                            #region scale by width, new width = 900 
                            newImg = ImageResize.ScaleByWidth(img, 900);
                            newFilePath = folderpath + "\\wwwroot\\uploads\\large\\" + filename;

                            newImg.SaveAs(newFilePath);
                            #endregion
                        }

                        //dispose to free up memory
                        img.Dispose();
                        newImg.Dispose();
                    }
                }

                return filename;
            }
            catch (Exception ex)
            {
                Log.LogError("MediaService - CreateOrUpdate - " + ex);
                return null;
            }

        }

        private bool DeleteFile(MediaViewModel mvm)
        {

            string filename = mvm.Url;
            string folderpath = HostingEnvironment.ContentRootPath;
            string filepath = folderpath + "\\wwwroot\\uploads\\" + filename;
            if (mvm.Type == 3)
            {
                filepath = folderpath + "\\wwwroot\\images\\state\\" + filename;
            }
            //FileInfo fileinfo = new FileInfo(filepath);

            try
            {
                if (File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.LogError("MediaService - CreateOrUpdate - " + ex);
                return false;
            }

        }

        public async Task<bool> CreateOrUpdate(MediaViewModel mvm)
        {

            try
            {
                if (mvm.UploadImage != null)
                {

                    mvm.Url = await CreateFile(mvm);

                }
                if (mvm.Type == 3)
                {
                    mvm.Type = Convert.ToInt16(Utils.MediaType.Icon);
                }
                else
                {
                    mvm.Type = Convert.ToInt16(Utils.MediaType.Image);
                }
                var model = IMapper.Map<Media>(mvm);
                model.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
                if (mvm.Id == 0)
                {

                    await Create(model);

                }
                else
                {
                    await Update(mvm);
                }

            }
            catch (Exception ex)
            {
                Log.LogError("MediaService - CreateOrUpdate - " + ex);
                return false;
            }

            return true;
        }

        public async Task<int> CreateOrUpdate(MediaViewModel mvm, bool getData)
        {

            try
            {
                if (mvm.UploadImage != null)
                {

                    mvm.Url = await CreateFile(mvm);

                }

                if (mvm.Type == 3)
                {
                    mvm.Type = Convert.ToInt16(Utils.MediaType.Icon);
                }
                else
                {
                    mvm.Type = Convert.ToInt16(Utils.MediaType.Image);
                }
                var model = IMapper.Map<Media>(mvm);
                model.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
                if (mvm.Id == 0)
                {

                    await Create(model);

                }
                else
                {
                    await Update(mvm);
                }
                return model.Id;
            }
            catch (Exception ex)
            {
                Log.LogError("MediaService - CreateOrUpdate - " + ex);
                return 0;
            }


        }

        public async Task<bool> DeleteById(int id)
        {
            var image = await db.Media.FindAsync(id);
            var caseMedia = await db.CaseMedia.Where(x => x.MediaId == id).ToListAsync();
            string title = image.Title;
            if (image != null)
            {
                var userMediaGroup = new List<UserMediaGroup>();
                if (!(image.Type == Convert.ToInt16(Utils.MediaType.Group)))
                {
                    var result = DeleteFile(IMapper.Map<MediaViewModel>(image));
                    if (!result)
                    {
                        //  return result;
                    }
                }
                else
                {
                    userMediaGroup = db.UserMediaGroup.Where(x => x.GroupId.Contains(id.ToString())).ToList();
                    foreach (var item in userMediaGroup)
                    {
                        var ids = item.GroupId;
                        var idsList = new List<string>(ids.Split(','));
                        var index = idsList.IndexOf(id.ToString());
                        idsList.Remove(idsList[index]);
                        var newIds = string.Join(',', idsList);
                        item.GroupId = newIds;
                    }

                    if (userMediaGroup.Count > 0)
                    {
                        db.UserMediaGroup.UpdateRange(userMediaGroup);
                        db.SaveChanges();
                    }
                }

                if (caseMedia.Count > 0)
                {
                    db.CaseMedia.RemoveRange(caseMedia);
                }

                db.Media.Remove(image);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Media Deleted " + title + ". Deleted By  <a href = '/admin/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            Log.LogError("MediaService - DeleteMediaById - " + id + " - : ");
            return false;
        }

        public dynamic GetImagesByTenantIdAndUser(int tenantid, string userId)
        {
            List<MediaViewModel> listMediaMap = IMapper.Map<List<MediaViewModel>>(db.Media.Where(x => x.CreatedBy == userId && x.TenantId == tenantid).ToList());
            //var temp = listMediaMap.GroupBy(x => x.ParentId).ToList();
            List<MediaByParentId> results = listMediaMap
              .GroupBy(p => p.ParentId,
                       (k, c) => new MediaByParentId()
                       {
                           Parent = db.Media.Where(x => x.Id == k).FirstOrDefault() == null ? "UnCategorised Media" : db.Media.Where(x => x.Id == k).FirstOrDefault().Title,
                           ParentId = k,
                           Media = c.Where(x => x.ParentId != x.Id).ToList()
                       }
                      ).OrderByDescending(p => p.ParentId).ToList();

            //foreach (var item in results)
            //{
            //    if (db.Media.Where(x => x.Id == item.ParentId).FirstOrDefault() != null)
            //    {
            //        item.Parent = db.Media.Where(x => x.Id == item.ParentId).SingleOrDefault().Title;
            //    }
            //    else
            //    {
            //        item.Parent = "UnCategorised Media";
            //    }
            //}
            return results;
        }

        public List<MediaByParentId> GetImagesByTenantId(int tenantid)
        {
            List<MediaViewModel> listMediaMap = IMapper.Map<List<MediaViewModel>>(db.Media.Where(x => x.TenantId == tenantid).ToList());
            //var temp = listMediaMap.GroupBy(x => x.ParentId).ToList();
            List<MediaByParentId> results = listMediaMap
              .GroupBy(p => p.ParentId,
                       (k, c) => new MediaByParentId()
                       {
                           Parent = db.Media.Where(x => x.Id == k).FirstOrDefault() == null ? "UnCategorised Media" : db.Media.Where(x => x.Id == k).FirstOrDefault().Title,
                           ParentId = k,
                           Media = c.Where(x => x.ParentId != x.Id).ToList()
                       }
                      ).OrderByDescending(p => p.ParentId).ToList();
            return results;
        }

        public List<MediaViewModel> GetImagesByTenantIdandCaseId(int tenantid, int caseId)
        {

            List<MediaViewModel> medias = new List<MediaViewModel>();
            var result = from m in db.Media
                         join c in db.CaseMedia
                         on m.Id equals c.MediaId
                         where m.TenantId == tenantid && c.Id == caseId
                         select m;

            medias = IMapper.Map<List<MediaViewModel>>(result);
            return medias;
        }

        public async Task<bool> CreateOrUpdateMediaGroup(string groupName, int tenantId)
        {
            Media mb = new Media()
            {
                Title = groupName,
                CreatedAt = DateTime.Now,
                TenantId = tenantId,
                Type = Convert.ToInt16(Utils.MediaType.Group),

            };
            await Create(mb);
            mb.ParentId = mb.Id;
            MediaViewModel mvm = IMapper.Map<MediaViewModel>(mb);
            await Update(mvm);
            return true;
        }

        public async Task<int> CreateOrUpdateMediaGroups(string groupName, int tenantId, int mediaId = 0)
        {
            try
            {
                if (mediaId == 0)
                {
                    Media mb = new Media()
                    {
                        Title = groupName,
                        CreatedAt = DateTime.Now,
                        TenantId = tenantId,
                        Type = Convert.ToInt16(Utils.MediaType.Group),

                    };
                    await Create(mb);
                    mb.ParentId = mb.Id;
                    MediaViewModel mvm = IMapper.Map<MediaViewModel>(mb);
                    await Update(mvm);
                    return mb.Id;
                }
                else
                {
                    var mediaData = db.Media.Where(x => x.Id == mediaId).FirstOrDefault();
                    if (mediaData != null)
                    {
                        mediaData.Title = groupName;
                        db.Media.Update(mediaData);
                        db.SaveChanges();
                    }
                    return mediaId;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        //Todo Media Group
        public async Task<List<MediaViewModel>> GetMediaGroup(int tenantId)
        {
            string loggedIn = commonService.getLoggedInUserId();


            var isSuperAdmin = await commonService.IsSuperAdmin();
            var isTenantAdmin = commonService.IsTenantAdmin();

            if (!isSuperAdmin && !isTenantAdmin)
            {
                var groupData = db.UserMediaGroup.Where(x => x.UserId == loggedIn).Select(y => y.GroupId).FirstOrDefault();
                var groupIds = new List<string>();
                if (groupData != null)
                {
                    groupIds = new List<string>(groupData.Split(','));
                }

                var data = new List<Media>();
                data = (from m in db.Media.Where(x => x.TenantId == tenantId)
                        where groupIds.Contains(m.Id.ToString())
                        select m).ToList();
                return IMapper.Map<List<MediaViewModel>>(data);
            }

            return IMapper.Map<List<MediaViewModel>>(db.Media.Where(x =>
                                                                   x.Type == Convert.ToInt16(Utils.MediaType.Group)
                                                                   && x.TenantId == tenantId
                                                                   /*&& x.CreatedBy == loggedIn*/)
                                                                   .ToList()
                                                                   );

        }

        public List<MediaViewModel> GetImagesByIds(List<int> mediaIds)
        {
            var mediaDatas = IMapper.Map<List<MediaViewModel>>(db.Media.Where(x => mediaIds.Contains(x.Id)).ToList());
            return mediaDatas;
        }

        public List<CaseMedia> GetCaseMediaByUserId(string userId)
        {
            try
            {
                var caseIds = db.Case.Where(x => x.UserId == userId).Select(y => y.Id).ToList();
                var caseMediaDatas = db.CaseMedia.Where(x => caseIds.Contains(x.CaseId)).ToList();
                return caseMediaDatas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public MediaViewModel GetMediaById(int id)
        {
            var mediaData = db.Media.Where(x => x.Id == id).FirstOrDefault();
            return IMapper.Map<MediaViewModel>(mediaData);
        }

        public List<MediaViewModel> GetQueueIconMedia()
        {
            var mediaDatas = db.Media.Where(x => x.Type == 3).ToList();
            return IMapper.Map<List<MediaViewModel>>(mediaDatas);
        }

        public string GetFileSize(string filename)
        {
            if (filename == null)
            {
                return "";
            }

            var path = Path.Combine(
                          Directory.GetCurrentDirectory(),
                          "wwwroot", "uploads", filename);

            if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                if (fi != null)
                {
                    var size = FormatSize(fi.Length);
                    return size.ToString();
                }
            }

            return "";
        }

        static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        public static string FormatSize(Int64 bytes)
        {
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }

        public bool CreateOrUpdateUserMediaGroup(List<string> mediaIds, string userId)
        {
            try
            {
                var userMediaGroup = db.UserMediaGroup.Where(x => x.UserId == userId).FirstOrDefault();
                if (userMediaGroup != null)
                {
                    db.UserMediaGroup.Remove(userMediaGroup);
                    db.SaveChanges();
                }

                if (mediaIds == null || mediaIds.Count == 0)
                {
                    return true;
                }
                userMediaGroup = new UserMediaGroup();

                userMediaGroup.GroupId = string.Join(',', mediaIds);
                userMediaGroup.UserId = userId;

                db.UserMediaGroup.Add(userMediaGroup);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
