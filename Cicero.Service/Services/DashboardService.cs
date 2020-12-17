using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Services
{
    public interface IDashboardService
    {
        DashboardViewModel GetDashboard();
        List<ChartDefinitionModel> GetChartDefinitions();
    }

    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DashboardService> _log;
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppSetting _appSetting;
        private readonly IMapper _mapper;
        private readonly IArticleService _articleService;
        private readonly ICommonService _commonService;
        private readonly Utils _utils;

        public DashboardService(ApplicationDbContext db, ILogger<DashboardService> log, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, AppSetting appSetting, IMapper mapper, IArticleService articleService, Utils utils, ICommonService commonService)
        {
            _db = db;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _appSetting = appSetting;
            _mapper = mapper;
            _articleService = articleService;
            _commonService = commonService;
            _utils = utils;

        }

        public DashboardViewModel GetDashboard()
        {

            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            DashboardViewModel dvm = new DashboardViewModel { };

            dvm.AppName = _appSetting.Get("app_name");

            dvm.TotalUsers = _db.Users.Where(x => x.TenantUsers.FirstOrDefault().TenantId == tenantid || tenantid == 0).Count();

            dvm.TotalArticles = _db.Article.Where(x => x.TenantId == tenantid || tenantid == 0).Count();

            var articles = _db.Article
                                .Where(x => x.TenantId == tenantid || tenantid == 0)
                                .OrderByDescending(b => b.UpdatedAt)
                                .Take(4).ToList();
            foreach (var item in articles)
            {
                item.Content = Utils.Truncate(item.Content);
                
            }

            dvm.Articles = _mapper.Map<List<ArticleViewModel>>(articles);

            dvm.Users = _db.Users
                            .Where(x => x.TenantUsers.FirstOrDefault().TenantId == tenantid || tenantid == 0)
                            .OrderByDescending(b => b.CreatedAt)
                            .Select(x => new DashboardUserViewModel
                            {
                                DisplayName = x.FirstName + " " + x.LastName,
                                Id = x.Id,
                                CreatedAt = Utils.GetDefaultDateFormat(x.CreatedAt)
                            })
                            .Take(6).ToList();

            string stateName = _appSetting.Get("app_claim");

            //submitted date logic
            dvm.NewClaims = _db.Case
                                    .Where(x => x.TenantId == tenantid || tenantid == 0)
                                    .Count();

            dvm.TotalClaims = _db.Case.Where(x => x.TenantId == tenantid || tenantid == 0).Count();

            dvm.ChartDefinition = GetChartDefinitions();

            return dvm;
        }

        public List<ChartDefinitionModel> GetChartDefinitions()
        {

            int tenantid = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());

            var cdm = new List<ChartDefinitionModel>();

            //number of months
            var date = DateTime.Now.Month - 6;
            //number of months - 1
            var viewMonth = 5;
            for (int i = date; i< DateTime.Now.Month; i++)
            {
                var baseDate = DateTime.Now.AddMonths(-viewMonth);
                DateTime startDate = new DateTime(baseDate.Year, baseDate.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                cdm.Add(new ChartDefinitionModel
                {
                    DimensionOne = DateTime.Now.AddMonths(-viewMonth).ToString("MMMM"),
                    Quantity = _db.Case.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && (x.TenantId == tenantid || tenantid == 0)).Count(),
                    Settled = _db.Case.Where(x => x.UpdatedAt >= startDate && x.UpdatedAt <= endDate && (x.TenantId == tenantid || tenantid == 0)).Count()
                });
                --viewMonth;
            }
            return cdm;
        }

    }
}
