using AutoMapper;
using Cicero.Data;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using Cicero.Service.Models.SimpleTransfer.BranchMapper;
using Cicero.Data.Entities.SimpleTransfer;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IBranchMapperService
    {
        DTResponseModel GetBranchMapperListByFilter(DTPostModel model, string countryCode, string cityCode, string bankCode);
        Task<bool> CreateOrUpdate(BranchMapperDataModel datas);
        Task<List<SelectListItem>> GetCountryList();
        Task<List<SelectListItem>> GetRateSupplierList();
        Task<BankCityList> GetBankAndCityList(string countryCode, int supplierId);
        Task<List<SelectListItem>> GetCities(string countryCode);
        Task<List<SupplierBankBranch>> GetSupplierBankBranchList(string countryCode, int supplierId, string bankCode);

        Task<List<SelectListItem>> GetBankListByCityCode(string cityCode);
        Task<List<SelectListItem>> GetBranchList(string csb, string cityCode, string bankCode);
        Task<List<SelectListItem>> GetBranchListForBene(string csb, string cityCode, string bankCode, string beneBankCode, string type);
        Task<List<SelectListItem>> GetBranchListByBank(string bankCode);

        Task<bool> ActiveBranchMapperById(int id);
        Task<bool> InActiveBranchMapperById(int id);
        Task<bool> DeleteBranchMapperById(int id);
        List<SelectListItem> GetCityByCode(string countryCode, string cityCode);
        List<SelectListItem> GetBranchListByBankCode(string csb, string cityCode, string bankCode, string bankBranchCode);
        List<SelectListItem> GetBankByCityCode(string bankCode, string cityCode);
    }
    public class BranchMapperService : IBranchMapperService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IBranchMapperService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public BranchMapperService(SimpleTransferApplicationDbContext _db, ILogger<IBranchMapperService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetBranchMapperListByFilter(DTPostModel model, string countryCode, string cityCode, string bankCode)
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

            var branchMapperList = (from c in db.SupplierBankBranch.Where(x => x.CountryCode == countryCode && x.CityCode == cityCode && x.BankCode == bankCode)
                                    select new
                                    {
                                        id = c.Id,
                                        country = db.CountryList.Where(x => x.Code == c.CountryCode).Select(x => x.Name).FirstOrDefault(),
                                        city = db.SupplierCity.Where(x => x.CityCode == c.CityCode).Select(x => x.CityName).FirstOrDefault(),
                                        supplier = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                                        bank = db.SupplierBank.Where(x => x.BankCode == c.BankCode).Select(x => x.BankName).FirstOrDefault(),
                                        branch = c.BranchName,
                                        branchCode = c.BranchCode,
                                        created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedAt),
                                        updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedAt),
                                        status = (c.Status) ? "Active" : "Inactive",
                                        action = "<a href='/admin/branchmapper/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Branch Mapper' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Branch Mapper</span></a>"
                                    });
            if (!String.IsNullOrEmpty(searchBy))
            {
                branchMapperList = branchMapperList.Where(o => o.branch.ToLower().Contains(searchBy.ToLower()) || o.branchCode.ToLower().Contains(searchBy.ToLower()) || o.created_at.ToLower().Contains(searchBy.ToLower()) || o.updated_at.ToLower().Contains(searchBy.ToLower()));

            }
            totalResultsCount = branchMapperList.Count();
            branchMapperList = branchMapperList.OrderBy(sortBy, sortDir).Skip(skip).Take(take);



            var list = branchMapperList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

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
        public async Task<bool> ActiveBranchMapperById(int id)
        {
            var branchMapper = await db.SupplierBankBranch.FindAsync(id);
            if (branchMapper != null)
            {
                branchMapper.Status = true;
                var result = db.SupplierBankBranch.Update(branchMapper);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Branch Mapper changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/Branch Mapper/" + utils.EncryptId(branchMapper.Id) + "/edit.html'>" + branchMapper.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BranchMapperService - ActiveBranchMapperById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveBranchMapperById(int id)
        {
            var branchMapper = await db.SupplierBankBranch.FindAsync(id);
            if (branchMapper != null)
            {
                branchMapper.Status = false;
                var result = db.SupplierBankBranch.Update(branchMapper);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Branch Mapper changed to InActive <a href ='/admin" + utils.GetTenantForUrl(false) + "/Branch Mapper/" + utils.EncryptId(branchMapper.Id) + "/edit.html'>" + branchMapper.CountryCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BranchMapperService - InActiveBranchMapperById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteBranchMapperById(int id)
        {
            var branchMapper = await db.SupplierBankBranch.FindAsync(id);
            if (branchMapper != null)
            {
                db.SupplierBankBranch.Remove(branchMapper);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Branch Mapper deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/BranchMapper/" + utils.EncryptId(branchMapper.Id) + "/edit.html'>" + branchMapper.CountryCode + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("BranchMapperService - DeleteBranchMapperById - " + id + " - : ");
            return false;
        }

        public async Task<BankCityList> GetBankAndCityList(string countryCode, int supplierId)
        {
            var bankCityList = new BankCityList();
            bankCityList.BankList = new List<SelectListItem>();
            bankCityList.CityList = new List<SelectListItem>();

            var bankList = await db.SupplierBank.Where(x => x.Status && x.SupplierId == supplierId && x.CountryCode == countryCode).Select(x => new SelectListItem
            {
                Text = x.BankName,
                Value = x.BankCode,
            }).OrderBy(x => x.Text).ToListAsync();


            bankCityList.BankList.AddRange(bankList);


            var cityList = await GetCities(countryCode);

            bankCityList.CityList.AddRange(cityList);


            return bankCityList;
        }

        public async Task<List<SelectListItem>> GetCities(string countryCode)
        {
            var cityList = await db.SupplierCity.Where(x => x.Status && x.SupplierId == 1 && x.CountryCode == countryCode).Select(x => new SelectListItem
            {
                Text = x.CityName,
                Value = x.CityCode,
            }).OrderBy(x => x.Text).ToListAsync();

            return cityList;
        }
        public List<SelectListItem> GetCityByCode(string countryCode, string cityCode)
        {
            var cityList = db.SupplierCity.Where(x => x.Status && x.CountryCode.ToUpper() == countryCode.ToUpper() && x.CityCode.ToUpper() != cityCode.ToUpper())
                .Select(x => new SelectListItem { Text = x.CityName, Value = x.CityCode }).ToList();

            var citySelect = db.SupplierCity.Where(x => x.Status && x.CountryCode.ToUpper() == countryCode.ToUpper() && x.CityCode.ToUpper() == cityCode.ToUpper()).FirstOrDefault();

            cityList.Add(new SelectListItem()
            {
                Text = citySelect.CityName,
                Value = citySelect.CityCode,
                Selected = true
            });

            cityList = cityList.OrderBy(x => x.Text).ToList();

            cityList.Insert(0, new SelectListItem()
            {
                Text = "Select City",
                Value = ""
            });
            return cityList;
        }

        public List<SelectListItem> GetBankByCityCode(string bankCode, string cityCode)
        {
            var bankList = (from sbb in db.SupplierBankBranch
                            where sbb.CityCode == cityCode && sbb.BankCode != bankCode
                            select new SelectListItem
                            {
                                Text = db.SupplierBank.Where(x => x.BankCode == sbb.BankCode && x.BankCode != bankCode).Select(x => x.BankName).FirstOrDefault(),
                                Value = sbb.BankCode
                            }).ToList();


            var bankSelect = (from sbb in db.SupplierBankBranch
                              where sbb.CityCode == cityCode && sbb.BankCode == bankCode
                              select new SelectListItem
                              {
                                  Text = db.SupplierBank.Where(x => x.BankCode == sbb.BankCode && x.BankCode == bankCode).Select(x => x.BankName).FirstOrDefault(),
                                  Value = sbb.BankCode,
                                  Selected = true
                              }).ToList();

            bankList.AddRange(bankSelect);
            bankList = bankList.OrderBy(x => x.Text).ToList();

            bankList.Insert(0, new SelectListItem()
            {
                Text = "Select Bank",
                Value = ""
            });
            return bankList;
        }

        public async Task<List<SelectListItem>> GetRateSupplierList()
        {
            var rateSupplierList = await db.RateSupplier.Where(x => x.IsActive).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToListAsync();

            return rateSupplierList;
        }

        public async Task<bool> CreateOrUpdate(BranchMapperDataModel datas)
        {
            try
            {
                var checkBranchMap = db.SupplierBankBranch.Where(x => x.CountryCode == datas.CountryCode && x.SupplierId == datas.SupplierId && x.BankCode == datas.BankCode).ToList();

                if (checkBranchMap.Count > 0)
                {
                    db.SupplierBankBranch.RemoveRange(checkBranchMap);
                    db.SaveChanges();
                }

                var branchMapperList = new List<SupplierBankBranch>();
                for (var i = 0; i < datas.Cities.Count; i++)
                {
                    branchMapperList.Add(new SupplierBankBranch
                    {
                        CountryCode = datas.CountryCode,
                        SupplierId = datas.SupplierId,
                        BankCode = datas.BankCode,
                        CityCode = datas.Cities[i],
                        BranchCode = datas.BranchCode[i],
                        BranchName = datas.BranchName[i],
                        CreatedBy = commonService.getLoggedInUserId(),
                        UpdatedBy = commonService.getLoggedInUserId(),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Status = true
                    });
                }

                db.SupplierBankBranch.AddRange(branchMapperList);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<SupplierBankBranch>> GetSupplierBankBranchList(string countryCode, int supplierId, string bankCode)
        {
            var data = await db.SupplierBankBranch.Where(x => x.CountryCode == countryCode && x.SupplierId == supplierId && x.BankCode == bankCode).ToListAsync();
            return data;
        }

        public async Task<List<SelectListItem>> GetBankListByCityCode(string cityCode)
        {
            var datas = await db.SupplierBankBranch.Where(x => x.CityCode == cityCode).Select(x => x.BankCode).Distinct().ToListAsync();
            // var bankMapperDatas = await db.SupplierBankMap.Where(x => /*datas.Contains(x.NecMoneyBankCode) || */datas.Contains(x.TransfastBankCode)).ToListAsync();
            //   var transfastBankCode = bankMapperDatas./*Where(x => datas.Contains(x.NecMoneyBankCode)).*/Select(x => x.TransfastBankCode).ToList();

            //  datas = datas.Where(x => /*!*/transfastBankCode.Contains(x)).ToList();
            var bankList = new List<SelectListItem>();
            foreach (var item in datas)
            {
                bankList.Add(new SelectListItem
                {
                    Text = db.SupplierBank.Where(x => x.BankCode == item).Select(x => x.BankName).FirstOrDefault(),
                    Value = item
                });
            }
            return bankList.OrderBy(x => x.Text).ToList();
        }

        public async Task<List<SelectListItem>> GetBranchList(string csb, string cityCode, string bankCode)
        {
            //  var bankMapperDatas = await db.SupplierBankMap.Where(x => /*x.NecMoneyBankCode == bankCode || */x.TransfastBankCode == bankCode).FirstOrDefaultAsync();
            //var exchangeRatesData = await db.ExchangeRates.Where(x => x.ToCountryCode == csb && (/*x.BankCode == bankMapperDatas.NecMoneyBankCode || */x.BankCode == /*bankMapperDatas.TransfastBankCode*/ bankCode)).OrderBy(x => x.ExchangeRate).FirstOrDefaultAsync();
            //int supplierId = 0;
            //var newBankCode = string.Empty;
            //if (exchangeRatesData != null)
            //{
            //    newBankCode = exchangeRatesData.BankCode;
            //    supplierId = (int)Enum.Parse(typeof(Cicero.Service.SimpleTransferEnums.RateSupplier), exchangeRatesData.Source);
            //}
            var supplierId = (int)RateSupplierEnum.Transfast;
            var data = await (from sbb in db.SupplierBankBranch
                              where sbb.BankCode == bankCode && sbb.SupplierId == supplierId && sbb.CountryCode == csb && sbb.CityCode == cityCode
                              select new SelectListItem
                              {
                                  Text = sbb.BranchName,
                                  Value = sbb.BranchCode
                              }).ToListAsync();
            return data;
        }

        public async Task<List<SelectListItem>> GetBranchListForBene(string csb, string cityCode, string bankCode, string beneBankCode, string type)
        {
            int supplierId = 0;
            supplierId = (int)RateSupplierEnum.Transfast;
            //var exchangeRates = await db.ExchangeRates.Where(x => x.ToCountryCode == csb && x.BankCode != "" && x.ModeOfPayment == 1).OrderBy(x => x.ExchangeRate).GroupBy(x => x.ExchangeRate).FirstOrDefaultAsync();

            //  var beneBankMapperDatas = new SupplierBankMap();
            // beneBankMapperDatas = await db.SupplierBankMap.Where(x =>/* x.NecMoneyBankCode == beneBankCode || */x.TransfastBankCode == beneBankCode).FirstOrDefaultAsync();

            //if ((int)Enum.Parse(typeof(PayoutMode), type.Replace(" ", "")) == 1)
            //{
            //    // var source = exchangeRates.Select(x => x.Source).FirstOrDefault();
            //    // supplierId = (int)Enum.Parse(typeof(RateSupplierEnum), source);

            //    //if (beneBankCode != null)
            //    //{
            //    //    beneBankCode = await db.SupplierBankBranch.Where(x => x.SupplierId == supplierId && (/*x.BankCode == beneBankMapperDatas.NecMoneyBankCode || */x.BankCode == beneBankMapperDatas.TransfastBankCode)).Select(x => x.BankCode).FirstOrDefaultAsync();
            //    //}

            //    var datas = await (from sbb in db.SupplierBankBranch
            //                       where sbb.BankCode == beneBankCode && sbb.SupplierId == supplierId && sbb.CountryCode == csb && sbb.CityCode == cityCode
            //                       select new SelectListItem
            //                       {
            //                           Text = sbb.BranchName,
            //                           Value = sbb.BranchCode
            //                       }).ToListAsync();
            //    return datas;
            //}

            //var bankMapperDatas = await db.SupplierBankMap.Where(x => /*x.NecMoneyBankCode == bankCode || */x.TransfastBankCode == bankCode).FirstOrDefaultAsync();
            //var exchangeRatesData = await db.ExchangeRates.Where(x => x.ToCountryCode == csb && (/*x.BankCode == bankMapperDatas.NecMoneyBankCode || */x.BankCode == bankMapperDatas.TransfastBankCode)).OrderBy(x => x.ExchangeRate).FirstOrDefaultAsync();
            //  var newBankCode = string.Empty;


            //if (exchangeRatesData != null)
            //{
            //    supplierId = (int)Enum.Parse(typeof(Cicero.Service.SimpleTransferEnums.RateSupplier), exchangeRatesData.Source);

            //    if (beneBankMapperDatas != null)
            //    {
            //        beneBankCode = await db.SupplierBankBranch.Where(x => x.SupplierId == supplierId && (/*x.BankCode == beneBankMapperDatas.NecMoneyBankCode || */x.BankCode == beneBankMapperDatas.TransfastBankCode)).Select(x => x.BankCode).FirstOrDefaultAsync();
            //    }

            //    newBankCode = beneBankCode == null ? exchangeRatesData.BankCode : beneBankCode;
            //}
            var data = await (from sbb in db.SupplierBankBranch
                              where sbb.BankCode == beneBankCode && sbb.SupplierId == supplierId && sbb.CountryCode == csb && sbb.CityCode == cityCode
                              select new SelectListItem
                              {
                                  Text = sbb.BranchName,
                                  Value = sbb.BranchCode
                              }).ToListAsync();
            return data;
        }

        public async Task<List<SelectListItem>> GetBranchListByBank(string bankCode)
        {
            var branchList = await db.SupplierBankBranch.Where(x => x.BankCode == bankCode).Select(x => new SelectListItem
            {
                Value = x.BranchCode,
                Text = x.BranchName
            }).OrderBy(x => x.Text).ToListAsync();
            return branchList;
        }

        public List<SelectListItem> GetBranchListByBankCode(string csb, string cityCode, string bankCode, string bankBranchCode)
        {
            var branchList = (from sbb in db.SupplierBankBranch
                              where sbb.BankCode == bankCode && sbb.CountryCode == csb && sbb.CityCode == cityCode && sbb.BranchCode != bankBranchCode
                              select new SelectListItem
                              {
                                  Text = sbb.BranchName,
                                  Value = sbb.BranchCode
                              }).ToList();

            var branchSelect = (from sbb in db.SupplierBankBranch
                                where sbb.BankCode == bankCode && sbb.CountryCode == csb && sbb.CityCode == cityCode && sbb.BranchCode == bankBranchCode
                                select new SelectListItem
                                {
                                    Text = sbb.BranchName,
                                    Value = sbb.BranchCode,
                                    Selected = true
                                }).ToList();

            branchList.AddRange(branchSelect);

            branchList = branchList.OrderBy(x => x.Text).ToList();

            branchList.Insert(0, new SelectListItem()
            {
                Text = "Select Branch",
                Value = ""
            });
            return branchList;
        }
    }

    public class BankCityList
    {
        public List<SelectListItem> BankList { get; set; }
        public List<SelectListItem> CityList { get; set; }
    }
}
