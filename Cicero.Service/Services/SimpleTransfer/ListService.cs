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

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IListService
    {
        Task<List<SelectListItem>> GetCountryList();
        Task<List<SelectListItem>> GetSupplierList();
        Task<List<SelectListItem>> GetPayoutModeList();
        Task<List<SelectListItem>> GetCurrencyList();
        Task<List<SelectListItem>> GetActiveCurrencyList();
        Task<List<SelectListItem>> GetActiveCountryList();
        Task<List<SelectListItem>> GetActivePaymentPurposeList();
        List<SelectListItem> GetActiveSourceOfFundList(string tenantId);
        List<SelectListItem> GetSenderCountryList();
        List<SelectListItem> GetReceiverCountryList();
        List<SelectListItem> GetSenderCurrencyList();
        List<SelectListItem> GetReceiverCurrencyList();
  }
    public class ListService : IListService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly Utils Utils;
        private readonly ILogger<IListService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;

        public ListService(SimpleTransferApplicationDbContext _db, Utils _Utils, ILogger<IListService> _Log, IMapper _mapper, ICommonService _commonService, IActivityLogService _activityLogService)
        {
            db = _db;
            Utils = _Utils;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
        }

        public Task<List<SelectListItem>> GetCountryList()
        {
            return db.CountryList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Code }).ToListAsync();
        }

        public Task<List<SelectListItem>> GetSupplierList()
        {
            return db.RateSupplier.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToListAsync();
        }
        public Task<List<SelectListItem>> GetPayoutModeList()
        {
            return db.PayoutModeConfig.Select(x => new SelectListItem() { Text = x.PayoutModeName, Value = x.Id.ToString() }).ToListAsync();
        }
        public Task<List<SelectListItem>> GetCurrencyList()
        {
            return db.CountryList.Select(x => new SelectListItem() { Text = x.CurrencyCode, Value = x.CurrencyCode.ToString() }).ToListAsync();
        }
        public Task<List<SelectListItem>> GetActiveCountryList()
        {
            return db.CountryList.Where(x=>x.IsActive).Select(x => new SelectListItem() { Text = x.Name, Value = x.Code }).ToListAsync();
        }
        public Task<List<SelectListItem>> GetActiveCurrencyList()
        {
            return db.CountryList.Where(x => x.IsActive).Select(x => new SelectListItem() { Text = x.CurrencyCode, Value = x.CurrencyCode.ToString() }).ToListAsync();
        }

        public Task<List<SelectListItem>> GetActivePaymentPurposeList()
        {
            return db.PaymentPurpose.Where(x => x.Status).Select(x => new SelectListItem() { Text = x.PurposeName, Value = x.Id.ToString() }).ToListAsync();
        }

        public List<SelectListItem> GetActiveSourceOfFundList(string tenantId)
        {
            return db.SourceOfFund.Where(x => x.Status&&x.TenantId==Convert.ToInt32(tenantId)).Select(x => new SelectListItem() { Text = x.SourceOfFundName, Value = x.Id.ToString() }).ToList();
        }
        public List<SelectListItem> GetSenderCountryList()
        {
            return db.CountryList.Where(x => x.Code=="GB").Select(x => new SelectListItem() { Text = x.Name, Value = x.Code.ToString() }).ToList();
        }
        public List<SelectListItem> GetReceiverCountryList()
        {
            return db.CountryList.Where(x => x.IsActive).Select(x => new SelectListItem() { Text = x.Name, Value = x.Code }).ToList();
        }
        public List<SelectListItem> GetSenderCurrencyList()
        {
            return db.CountryList.Where(x =>  x.Code == "GB").Select(x => new SelectListItem() { Text = x.CurrencyCode, Value = x.CurrencyCode }).ToList();
        }
        public List<SelectListItem> GetReceiverCurrencyList()
        {
            return db.CountryList.Where(x => x.IsActive).Select(x => new SelectListItem() { Text = x.CurrencyCode, Value = x.CurrencyCode }).ToList();
        }
    }
}
