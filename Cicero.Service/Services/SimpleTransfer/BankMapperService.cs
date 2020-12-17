using AutoMapper;
using Cicero.Data;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.BankMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Cicero.Data.Entities.SimpleTransfer;
using Microsoft.EntityFrameworkCore;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IBankMapperService
    {
        DTResponseModel GetBankMapperListByFilter(DTPostModel model);
        Task<bool> CreateOrUpdate(List<string> necMoneyBankList, List<string> transfastBankList, List<bool> status, string countryCode);
        Task<List<SelectListItem>> GetCountryList();
        Task<List<SelectListItem>> GetNecMoneyBankList(string countryCode);
        Task<List<SelectListItem>> GetTransfastBankList(string countryCode);
        Task<List<SelectListItem>> GetAllBankList(string countryCode);
        Task<BankListViewModel> GetBankMapperByCountryCode(string countryCode);

        Task<bool> ActiveBankMapperById(int id);
        Task<bool> InActiveBankMapperById(int id);
        Task<bool> DeleteBankMapperById(int id);
        Task<SupplierBankMap> GetBankMapperDataByBankCode(string bankCode);

    }
    public class BankMapperService : IBankMapperService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IBankMapperService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public BankMapperService(SimpleTransferApplicationDbContext _db, ILogger<IBankMapperService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetBankMapperListByFilter(DTPostModel model)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "name";
            bool sortDir = false;

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
                    sortDir = model.order[0].dir.ToLower() == "desc";
                }
            }

            var bankMapperList = (from c in db.SupplierBankMap
                                  select new
                                  {
                                      id = c.Id,
                                      country = db.CountryList.Where(x => x.Code == c.CountryCode).Select(x => x.Name).FirstOrDefault(),
                                      necMoneyBank = db.SupplierBank.Where(x => x.BankCode == c.NecMoneyBankCode).Select(x => x.BankName).FirstOrDefault(),
                                      transfastBank = db.SupplierBank.Where(x => x.BankCode == c.TransfastBankCode).Select(x => x.BankName).FirstOrDefault(),
                                      created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedAt),
                                      updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedAt),
                                      status = (c.Status) ? "Active" : "Inactive",
                                      action = "<a href='/admin/bankmapper/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Bank Mapper' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Bank Mapper</span></a>"
                                  }).OrderBy(x => x.transfastBank).AsQueryable();
            totalResultsCount = bankMapperList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                bankMapperList = bankMapperList.Where(o => o.country.ToLower().Contains(searchBy.ToLower()));

            }
            bankMapperList = bankMapperList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = bankMapperList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<bool> CreateOrUpdate(List<string> necMoneyBankList, List<string> transfastBankList, List<bool> status, string countryCode)
        {
            try
            {
                var checkBankMap = db.SupplierBankMap.Where(x => x.CountryCode == countryCode).ToList();

                if (checkBankMap.Count > 0)
                {
                    db.SupplierBankMap.RemoveRange(checkBankMap);
                    db.SaveChanges();
                }

                var bankMapperList = new List<SupplierBankMap>();
                for (var i = 0; i < necMoneyBankList.Count; i++)
                {
                    bankMapperList.Add(new SupplierBankMap
                    {
                        CountryCode = countryCode,
                        NecMoneyBankCode = necMoneyBankList[i],
                        TransfastBankCode = transfastBankList[i],
                        CreatedBy = commonService.getLoggedInUserId(),
                        UpdatedBy = commonService.getLoggedInUserId(),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Status = status[i]
                    });
                }

                db.SupplierBankMap.AddRange(bankMapperList);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<SelectListItem>> GetCountryList()
        {
            var countryList = await db.CountryList.Where(x => x.IsActive).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Code,
            }).ToListAsync();

            return countryList;
        }
        public async Task<bool> ActiveBankMapperById(int id)
        {
            var bankMapper = await db.SupplierBankMap.FindAsync(id);
            if (bankMapper != null)
            {
                bankMapper.Status = true;
                var result = db.SupplierBankMap.Update(bankMapper);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Bank Mapper changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/Bank Mapper/" + utils.EncryptId(bankMapper.Id) + "/edit.html'>" + bankMapper.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BankMapperService - ActiveBankMapperById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveBankMapperById(int id)
        {
            var bankMapper = await db.SupplierBankMap.FindAsync(id);
            if (bankMapper != null)
            {
                bankMapper.Status = false;
                var result = db.SupplierBankMap.Update(bankMapper);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Bank Mapper changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/Bank Mapper/" + utils.EncryptId(bankMapper.Id) + "/edit.html'>" + bankMapper.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BankMapperService - InActiveBankMapperById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteBankMapperById(int id)
        {
            var bankMapper = await db.SupplierBankMap.FindAsync(id);
            if (bankMapper != null)
            {
                db.SupplierBankMap.Remove(bankMapper);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Bank Mapper deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/BankMapper/" + utils.EncryptId(bankMapper.Id) + "/edit.html'>" + bankMapper.CountryCode + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BankMapperService - DeleteBankMapperById - " + id + " - : ");
            return false;
        }

        public async Task<List<SelectListItem>> GetNecMoneyBankList(string countryCode)
        {
            //var necMoneyBankList = await db.SupplierBank.Where(x => x.Status && x.SupplierId == (int)RateSupplierEnum.NecMoney && x.CountryCode == countryCode).Select(x => new SelectListItem
            //{
            //    Text = x.BankName,
            //    Value = x.BankCode,
            //}).OrderBy(x => x.Text).ToListAsync();

            var necMoneyBankList = new List<SelectListItem>();
            return necMoneyBankList;
        }

        public async Task<List<SelectListItem>> GetTransfastBankList(string countryCode)
        {
            var transfastBankList = await db.SupplierBank.Where(x => x.Status && x.SupplierId == (int)RateSupplierEnum.Transfast && x.CountryCode == countryCode).Select(x => new SelectListItem
            {
                Text = x.BankName,
                Value = x.BankCode,
            }).OrderBy(x => x.Text).ToListAsync();

            return transfastBankList;
        }

        public async Task<List<SelectListItem>> GetAllBankList(string countryCode)
        {
            var bankList = await db.SupplierBank.Where(x => x.CountryCode == countryCode).Select(x => new SelectListItem
            {
                Value = x.BankCode,
                Text = x.BankName
            }).OrderBy(x => x.Text).ToListAsync();
            return bankList;
        }

        public async Task<BankListViewModel> GetBankMapperByCountryCode(string countryCode)
        {
            var data = await (from sbm in db.SupplierBankMap
                              where sbm.CountryCode == countryCode
                              select new
                              {
                                  NecMoneyBankCode = sbm.NecMoneyBankCode,
                                  NecMoneyBankName = db.SupplierBank.Where(x => x.BankCode == sbm.NecMoneyBankCode).Select(x => x.BankName).FirstOrDefault(),
                                  TransfastBankCode = sbm.TransfastBankCode,
                                  TransfastBankName = db.SupplierBank.Where(x => x.BankCode == sbm.TransfastBankCode).Select(x => x.BankName).FirstOrDefault(),
                                  Status = sbm.Status
                              }).OrderBy(x => x.TransfastBankName).ToListAsync();

            if (data.Count == 0)
            {
                return null;
            }
            var bankListModel = new BankListViewModel();

            bankListModel.NecMoneyBankList = new List<SelectListItem>();
            bankListModel.TransfastBankList = new List<SelectListItem>();
            bankListModel.Status = new List<bool>();

            foreach (var item in data)
            {
                bankListModel.NecMoneyBankList.Add(new SelectListItem
                {
                    Text = item.NecMoneyBankName,
                    Value = item.NecMoneyBankCode
                });

                bankListModel.TransfastBankList.Add(new SelectListItem
                {
                    Text = item.TransfastBankName,
                    Value = item.TransfastBankCode
                });

                bankListModel.Status.Add(item.Status);
            }


            var necMoneyBankList = GetNecMoneyBankList(countryCode).Result;
            var transfastBankList = GetTransfastBankList(countryCode).Result;

            var remaningNecMoneyBankList = Except(necMoneyBankList, bankListModel.NecMoneyBankList);
            var remaningTransfastBankList = Except(transfastBankList, bankListModel.TransfastBankList);

            bankListModel.NecMoneyBankList.AddRange(remaningNecMoneyBankList);
            bankListModel.TransfastBankList.AddRange(remaningTransfastBankList);

            foreach (var item in remaningTransfastBankList)
            {
                bankListModel.Status.Add(false);
            }

            return bankListModel;

        }

        public static List<SelectListItem> Except(List<SelectListItem> array1, List<SelectListItem> array2)
        {
            var newArray = new List<SelectListItem>();
            foreach (var item in array1)
            {
                if (array2.Where(x => x.Value == item.Value).Count() == 0)
                {
                    newArray.Add(item);
                }
            }
            return newArray;
        }

        public async Task<SupplierBankMap> GetBankMapperDataByBankCode(string bankCode)
        {
            var data = await db.SupplierBankMap.Where(x => /*x.NecMoneyBankCode == bankCode ||*/ x.TransfastBankCode == bankCode).FirstOrDefaultAsync();
            return data;
        }
    }
}
