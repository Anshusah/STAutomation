using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Cicero.Service.Models;
using Cicero.Service.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Cicero.Data.Entities;
using Cicero.Data;
using Cicero.Service.Helpers;
using System.Security.Policy;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cicero.Service.Models.SimpleTransfer.AutoSchedulerSetting;
using Cicero.Data.Entities.SimpleTransfer;
using static Cicero.Service.SimpleTransferEnums;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IAutoSchedulerSettingService
    {
        DTResponseModel GetAutoSchedulerSettingByFilter(DTPostModel model);
        Task<AutoSchedulerSettingViewModel> GetAutoSchedulerSettingByIdAsync(int id);
        Task<AutoSchedulerSettingViewModel> CreateOrUpdate(AutoSchedulerSettingViewModel model);

    }
    public class AutoSchedulerSettingService : IAutoSchedulerSettingService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<IAutoSchedulerSettingService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;

        public AutoSchedulerSettingService(SimpleTransferApplicationDbContext _db, Utils _Utils, ILogger<IAutoSchedulerSettingService> _Log, IMapper _mapper, ICommonService _commonService, IActivityLogService _activityLogService)
        {
            db = _db;
            Utils = _Utils;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
        }

        public DTResponseModel GetAutoSchedulerSettingByFilter(DTPostModel model)
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

            var AutoSchedulerSettingList = (from c in db.AutoSchedulerSetting
                                            select new
                               {
                                   id = c.Id,
                                   name = c.Name,
                                   created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedAt),
                                   updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedAt),
                                   status = (c.IsActive) ? "Active" : "Inactive",
                                   action = "<a href='/admin/AutoSchedulerSetting/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit AutoSchedulerSetting' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit AutoSchedulerSetting</span></a>"
                               });
            totalResultsCount = AutoSchedulerSettingList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                AutoSchedulerSettingList = AutoSchedulerSettingList.Where(o => o.name.ToDescription().ToLower().Contains(searchBy.ToLower()));

            }
            AutoSchedulerSettingList = AutoSchedulerSettingList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = AutoSchedulerSettingList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<AutoSchedulerSettingViewModel> CreateOrUpdate(AutoSchedulerSettingViewModel model)
        {
            AutoSchedulerSetting autoSchedulerSetting = new AutoSchedulerSetting
            {
                Id = model.Id,
                Hour = model.Hour,
                Minutes = model.Minutes,
                Interval = model.Interval,
                Name = model.Name,
                CreatedBy = model.CreatedBy,
                UpdatedBy = commonService.getLoggedInUserId(),
                CreatedAt = Convert.ToDateTime(model.CreatedDate),
                UpdatedAt = DateTime.Now,
                IsActive = model.Status
            };

            if (model.Id == 0)
            {
                autoSchedulerSetting.CreatedBy = autoSchedulerSetting.UpdatedBy;
                autoSchedulerSetting.CreatedAt = autoSchedulerSetting.UpdatedAt;
                db.AutoSchedulerSetting.Add(autoSchedulerSetting);
                await db.SaveChangesAsync();

                return Mapper.Map<AutoSchedulerSettingViewModel>(autoSchedulerSetting);
            }
            else
            {
                var AutoSchedulerSettingData = Mapper.Map<AutoSchedulerSetting>(autoSchedulerSetting);

                db.AutoSchedulerSetting.Attach(AutoSchedulerSettingData).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }


            return model;
        }

        public async Task<AutoSchedulerSettingViewModel> GetAutoSchedulerSettingByIdAsync(int id)
        {
            var AutoSchedulerSettingList = await (from c in db.AutoSchedulerSetting
                                                  where c.Id == id
                                     select c).FirstOrDefaultAsync();

            return Mapper.Map<AutoSchedulerSettingViewModel>(AutoSchedulerSettingList);
        }

    }
}
