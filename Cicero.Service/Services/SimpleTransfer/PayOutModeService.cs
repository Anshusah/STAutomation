using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Cicero.Service.Extensions;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IPayOutModeService
    {
        DTResponseModel GetPayOutModeListByFilter(DTPostModel model);
    }

    public class PayOutModeService: IPayOutModeService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<RateSupplierService> Log;
        private readonly IMapper IMapper;
        private ICommonService commonService;
        private readonly IActivityLogService activityLogService;

        public PayOutModeService(SimpleTransferApplicationDbContext _db, Utils _utils, ILogger<RateSupplierService> _log,  IMapper _IMapper, ICommonService _commonService, IActivityLogService _activityLogService)
        {
            db = _db;
            Utils = _utils;
            Log = _log;
            IMapper = _IMapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
        }

        public DTResponseModel GetPayOutModeListByFilter(DTPostModel model)
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

            var payOutMode = (from c in db.PayoutModeConfig
                                select new
                                {
                                    id = c.Id,
                                    name = c.PayoutModeName,
                                    created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                    updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                    status = (c.Status) ? "Active" : "Inactive",
                                });
            totalResultsCount = payOutMode.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                payOutMode = payOutMode.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = payOutMode.Count();
            payOutMode = payOutMode.Skip(skip).Take(take).OrderBy(sortBy, sortDir);
            var list = payOutMode.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }
    }
}
