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
using Cicero.Service.Extensions;
using Cicero.Data.Entities;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cicero.Service.Services
{
    public interface IArticleService
    {
        DTResponseModel GetArticleListByFilter(DTPostModel model);
        ArticleViewModel GetArticleById(int id);
        ArticleViewModel GetArticleByIdentifier(string id,int tenantid=0);
        Task<ArticleViewModel> CreateOrUpdate(ArticleViewModel avm);
        Task<List<ArticleViewModel>> GetArticleListByParentId(int id);
        Task<string> GetDefaultOrFirstImagesByArticleId(int id, string _default);
        Task<bool> DeleteArticleById(int id);
        Task<bool> ActiveArticleById(int id);
        Task<bool> InactiveArticleById(int id);
        IEnumerable<MediaViewModel> GetImagesByArticleId(int id);
        List<SelectListItem> GetArticleList();

        Task<bool> CreateArticleForTenant(int id);
    }

    public class ArticleService:IArticleService
    {

        private readonly ApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<ArticleService> Log;
        private readonly IHttpContextAccessor IHttpContextAccessor = null;
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly IMapper IMapper;
        private ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        public ArticleService(ApplicationDbContext _db, Utils _utils, ILogger<ArticleService> _log, IHttpContextAccessor _httpContextAccessor, IHostingEnvironment _hostingEnvironment, IMapper _IMapper, ICommonService _commonService, IActivityLogService _activityLogService)
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

        public DTResponseModel GetArticleListByFilter(DTPostModel model)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
            bool sortDir = true;

            int totalResultsCount = 0;
            int filteredResultsCount = 0;
            int draw = 0;

            if (model != null)
            {
                searchBy = (model.search != null) ? model.search.value : null;
                take = model.length;
                skip = model.start;
                draw = model.draw;

                if (model.order != null)
                {
                    sortBy = model.columns[model.order[0].column].data;
                    sortDir = model.order[0].dir.ToLower() == "asc";
                }
            }

            int tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());

            var article = db.Article.Where(d => (d.TenantId == tenantid || tenantid == 0) && d.TenantId != null && d.Type != "template").Select(x => new
            {
                id = x.Id,
                title = x.Title,
                created_at = Utils.GetDefaultDateFormat(x.CreatedAt),
                updated_at = Utils.GetDefaultDateFormat(x.UpdatedAt),
                status = (x.Status == 1) ? "Active" : "Inactive",
                version = x.Version,
                parent = (db.Article.Where(y => y.Id == x.ParentId).Select(z => z.Title).FirstOrDefault() == null ) ? "No Parent" : db.Article.Where(y => y.Id == x.ParentId).Select(z => z.Title).FirstOrDefault(),
                //user_id = x.UserId,
                action = "<a href='/admin"+ Utils.GetTenantForUrl(false) + "/article/" + Utils.EncryptId(x.Id) + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Article' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Article</span></a>"
            });

            totalResultsCount = article.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                article = article.Where(o => o.title.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = article.Count();
            article = article.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = article.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public ArticleViewModel GetArticleById(int id)
        {
           try
            {

                var articleData = db.Article
                    .Where(x => x.Id == id && x.Type != "template")
                    .FirstOrDefault();

                    if(articleData==null){
                        return new ArticleViewModel { };
                    }

                var article = IMapper.Map<ArticleViewModel>(articleData);
                
                return article;
            }
            catch (Exception ex)
            {
                Log.LogError("ArticleService - GetArticleById - " + ex);
            }

            return new ArticleViewModel { };
        }

        public async Task<List<ArticleViewModel>> GetArticleListByParentId(int id)
        {
            try
            {

                var article = await db.Article.Where(e => e.ParentId == id).Select(x => new ArticleViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Excerpt = x.Excerpt,
                    Content = x.Content,
                    CreatedAt = Utils.GetDefaultDateFormat(x.CreatedAt),
                    UpdatedAt = Utils.GetDefaultDateFormat(x.UpdatedAt),
                    Status = x.Status,
                    Version = x.Version,
                    Slug = x.Slug

                }).ToListAsync();
                return article;
            }
            catch (Exception ex)
            {
                Log.LogError("ArticleService - GetArticleListByParentId - " + ex);
            }

            return new List<ArticleViewModel> { };
        }

        public ArticleViewModel GetArticleByIdentifier(string id,int tenantid=0)
        {
            if (tenantid == 0)
            {
                if(commonService==null) commonService = IHttpContextAccessor.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
                tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            }
            var articleData = db.Article.Where(x => x.Slug == id && x.TenantId==tenantid).FirstOrDefault();
            if (articleData != null)
            {
                var article = IMapper.Map<ArticleViewModel>(articleData);

                return article;
            }
            else
            {
                return new ArticleViewModel { };
            }
        }

        private string GetSlug(string type, string slug, string title)
        {
            Regex re = new Regex("(?:[^a-z0-9]|(?<=['\"])s)");

            if (type != "template")
            {
                if (string.IsNullOrEmpty(slug))
                {
                    slug = re.Replace(title.ToLower(), "-");
                }
                else
                {
                    slug = re.Replace(slug, "-");
                }
            }
            return slug;
        }

        private bool CheckDuplicateSlug(string slug, int id, int tenantid)
        {
            var check = db.Article.Where(x => x.Slug == slug && x.Id != id && x.TenantId == tenantid && x.Type != "template").FirstOrDefault();

            if (check != null)
            {
                
                return true;
            }
            return false;
        }

        private async Task<Article> Create(Article model)
        {
            try
            {
                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;
                if (model.TenantId == null || model.TenantId == 0)
                {
                    model.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
                }

                await db.Article.AddAsync(model);

                await db.SaveChangesAsync();

                await activityLogService.CreateLog(loggedUser, "New Article created <a href ='/admin" + Utils.GetTenantForUrl(false) + "/article/" + Utils.EncryptId(model.Id) + "/edit.html'>" + model.Title + "</a>. Created By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ", (model.TenantId != null) ? (int)model.TenantId : 0);

                return model;
            }
            catch(Exception)
            {
                return model;
            }

        }

        private async Task<bool> Update(Article model)
        {
            try
            {
                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;
                if (model.TenantId == null || model.TenantId == 0)
                {
                    model.TenantId = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
                }

                db.Article.Attach(model).State = EntityState.Modified;

                await db.SaveChangesAsync();

                await activityLogService.CreateLog(loggedUser, "Article updated <a href ='/admin" + Utils.GetTenantForUrl(false) + "/article/" + Utils.EncryptId(model.Id) + "/edit.html'>" + model.Title + "</a>. Updated By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ", (model.TenantId != null) ? (int)model.TenantId : 0);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private async Task ArticleMediaUpdate(ArticleViewModel avm)
        {
            db.ArticleMedia.RemoveRange(db.ArticleMedia.Where(e => e.ArticleId == avm.Id));
            foreach (var item in avm.Images)
            {
                ArticleMedia modelMedia = new ArticleMedia
                {
                    ArticleId = avm.Id,
                    MediaId = item
                };
                db.ArticleMedia.Add(modelMedia);
            }
            await db.SaveChangesAsync();
        }

        public async Task<ArticleViewModel> CreateOrUpdate(ArticleViewModel avm)
        {

            avm.Slug = GetSlug(avm.Type, avm.Slug,avm.Title);

            avm.UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now);
            var model = IMapper.Map<Article>(avm);

            model.UserId = commonService.getLoggedInUserId();

            if (CheckDuplicateSlug(avm.Slug,avm.Id,avm.TenantId))
            {
                return avm;
            }

            if (avm.Id == 0)
            {
                model = await Create(model);
                avm.Id = model.Id;
            }
            else
            {

                await Update(model);
            }

            if (avm.Images != null && avm.Id != 0)
            {

                await ArticleMediaUpdate(avm);

            }

            return avm;
        }

        public async Task<bool> DeleteArticleById(int id)
        {
            var article = await db.Article.FindAsync(id);
            string title = article.Title;
            if (article != null)
            {
                db.Article.Remove(article);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Article Deleted " + title + ". Deleted By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("ArticleService - DeleteArticleById - " + id + " - : ");
            return false;
        }

        public async Task<bool> ActiveArticleById(int id)
        {
            var article = await db.Article.FindAsync(id);
            if (article != null)
            {
                article.Status = 1;
                var result = db.Article.Update(article);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Article changed to Active <a href ='/admin" + Utils.GetTenantForUrl(false) + "/article/" + Utils.EncryptId(article.Id) + "/edit.html'>" + article.Title + "</a>. Changed By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("ArticleService - ActiveArticleById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InactiveArticleById(int id)
        {
            var article = await db.Article.FindAsync(id);
            if (article != null)
            {
                article.Status = 0;
                var result = db.Article.Update(article);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Article changed to InActive <a href ='/admin" + Utils.GetTenantForUrl(false) + "/article/" + Utils.EncryptId(article.Id) + "/edit.html'>" + article.Title + "</a>. Changed By  <a href = '/admin" + Utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");

                return true;
            }

            Log.LogError("ArticleService - InactiveArticleById - " + id + " - : ");
            return false;
        }

        public IEnumerable<MediaViewModel> GetImagesByArticleId(int id)
        {
            var mediaList = db.ArticleMedia
                              .Join(db.Media, w => w.MediaId, z => z.Id, (w, z) => new { w, z })
                              .Where(x => x.w.ArticleId == id)
                              .Select(b => new MediaViewModel
                              {
                                  Id = b.z.Id,
                                  CreatedBy = b.z.CreatedBy,
                                  Description = b.z.Description,
                                  Title = b.z.Title,
                                  Url = b.z.Url
                              }).ToList();

            return mediaList;
        }

        public async Task<string> GetDefaultOrFirstImagesByArticleId(int id, string _default = "placeholder.png")
        {
            MediaViewModel mediaList = await db.ArticleMedia
                                              .Join(db.Media, w => w.MediaId, z => z.Id, (w, z) => new { w, z })
                                              .Where(x => x.w.ArticleId == id)
                                              .Select(b => new MediaViewModel
                                              {
                                                  Id = b.z.Id,
                                                  CreatedBy = b.z.CreatedBy,
                                                  Description = b.z.Description,
                                                  Title = b.z.Title,
                                                  Url = b.z.Url
                                              }).FirstOrDefaultAsync();

            if (mediaList != null)
            {
                return mediaList.Url;
            }
            return _default;
        }

        public List<SelectListItem> GetArticleList()
        {
            var tenantid = commonService.GetTenantIdByIdentifier(Utils.GetTenantFromSession());
            return db.Article.Where(y => y.TenantId == tenantid && (y.Type == "article" || y.Type==null)).Select(x => new SelectListItem
                                    {
                                        Value = x.Id.ToString(),
                                        Text = x.Title
                                    }).ToList();
        }

        public async Task<bool> CreateArticleForTenant(int id)
        {
            try
            {

                var articleList = IMapper.Map<List<ArticleViewModel>>(db.Article
                                                                        .Where(x => x.TenantId == null && x.ParentId == 0)
                                                                        .OrderBy(b => b.Id)
                                                                        .ToList());

                var childList = IMapper.Map<List<ArticleViewModel>>(db.Article
                                                        .AsNoTracking()
                                                        .Where(x => x.TenantId == null && x.ParentId != 0)
                                                        .OrderBy(b => b.Id)
                                                        .ToList());

                if (articleList.Count() > 0)
                {

                    var articleBase = IMapper.Map<List<ArticleViewModel>>(db.Article
                                                                        .AsNoTracking()
                                                                        .Where(x => x.TenantId == null && x.ParentId == 0)
                                                                        .OrderBy(b => b.Id)
                                                                        .ToList()).AsReadOnly();

                    articleList = articleList.Select(c => {
                                                        c.Id = 0;
                                                        c.TenantId = id;
                                                        c.CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now);
                                                        return c;
                                                    }).ToList();

                    childList = childList.Select(c => {
                                                        c.Id = 0;
                                                        c.TenantId = id;
                                                        c.CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now);
                                                        return c;
                                                    }).ToList();

                    foreach (var item in articleBase)
                    {

                        var result = articleList.Where(x => x.Slug == item.Slug).FirstOrDefault();
                        var check = await CreateOrUpdate(result);

                        if (childList != null && childList.Count>0)
                        {
                            var childs = childList.Where(x => x.ParentId == item.Id).ToList();
                            foreach (var child in childs)
                            {
                                child.ParentId = check.Id;
                                var childCheck = await CreateOrUpdate(child);
                            }
                        }                      
                    }
                }

                return true;
            }
            catch (Exception)
            {
                Log.LogError("CaseClaimService - CreateArticleForTenant - " + id + " - : ");
                return false;
            }
        }

    }
}