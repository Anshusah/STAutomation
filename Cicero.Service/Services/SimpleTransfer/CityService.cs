using AutoMapper;
using Cicero.Data;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.City;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using Cicero.Data.Entities.SimpleTransfer;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ICityService
    {
        DTResponseModel GetCityListByFilter(DTPostModel model);
        Task<List<CityViewModel>> GetAllCity(string countryCode);
        Task<CityViewModel> CreateOrUpdate(CityViewModel datas);
        Task<List<SelectListItem>> GetCountryList();
        Task<List<SelectListItem>> GetRateSupplierList();
        Task<CityViewModel> GetCityByIdAsync(int id);

        Task<bool> ActiveCityById(int id);
        Task<bool> InActiveCityById(int id);
        Task<bool> DeleteCityById(int id);

    }

    public class CityService : ICityService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<ICityService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;

        public CityService(SimpleTransferApplicationDbContext _db, ILogger<ICityService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
        }

        public DTResponseModel GetCityListByFilter(DTPostModel model)
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

            var cityList = (from c in db.SupplierCity
                            select new
                            {
                                id = c.Id,
                                cityCode = c.CityCode,
                                cityName = c.CityName,
                                country = db.CountryList.Where(x => x.Code == c.CountryCode).Select(x => x.Name).FirstOrDefault(),
                                supplier = db.RateSupplier.Where(x => x.Id == c.SupplierId).Select(x => x.Name).FirstOrDefault(),
                                stateId = c.StateId,
                                stateName = c.StateName,
                                created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedAt),
                                updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedAt),
                                status = (c.Status) ? "Active" : "Inactive",
                                action = "<a href='/admin/city/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit City' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit City</span></a>"
                            }).OrderBy(x=>x.cityName).AsQueryable();
            if (!String.IsNullOrEmpty(searchBy))
            {
                var searchByLower = searchBy.ToLower();
                cityList = cityList.Where(o => o.cityCode.ToLower().Contains(searchByLower) || o.cityName.ToLower().Contains(searchByLower) || o.country.ToLower().Contains(searchByLower) || o.stateId.ToLower().Contains(searchByLower) || o.stateName.ToLower().Contains(searchByLower) || o.created_at.ToLower().Contains(searchByLower) || o.updated_at.ToLower().Contains(searchByLower));

            }

            totalResultsCount = cityList.Count();
            cityList = cityList.OrderBy(sortBy, sortDir).Skip(skip).Take(take);



            var list = cityList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<CityViewModel> CreateOrUpdate(CityViewModel model)
        {
            try
            {
                SupplierCity city = new SupplierCity
                {
                    Id = model.Id,
                    CountryCode = model.CountryCode,
                    SupplierId = model.SupplierId,
                    CityCode = model.CityCode,
                    CityName = model.CityName,
                    StateId = model.StateId,
                    StateName = model.StateName,
                    CreatedBy = model.CreatedBy,
                    UpdatedBy = commonService.getLoggedInUserId(),
                    CreatedAt = Convert.ToDateTime(model.CreatedAt),
                    UpdatedAt = DateTime.Now,
                    Status = model.Status
                };

                if (model.Id == 0)
                {
                    city.CreatedBy = city.UpdatedBy;
                    city.CreatedAt = DateTime.Now;
                    db.SupplierCity.Add(city);
                    await db.SaveChangesAsync();
                    return Mapper.Map<CityViewModel>(city);
                }
                else
                {
                    var cityData = Mapper.Map<SupplierCity>(city);
                    db.SupplierCity.Attach(cityData).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }


                return model;
            }
            catch (Exception ex)
            {
                return model;
            }
        }

        public async Task<CityViewModel> GetCityByIdAsync(int id)
        {
            var cityList = await (from c in db.SupplierCity
                                           where c.Id == id
                                           select c).FirstOrDefaultAsync();

            return Mapper.Map<CityViewModel>(cityList);
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

        public async Task<List<SelectListItem>> GetRateSupplierList()
        {
            var rateSupplierList = await db.RateSupplier.Where(x => x.IsActive).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToListAsync();

            return rateSupplierList;
        }

        public async Task<bool> ActiveCityById(int id)
        {
            var city = await db.SupplierCity.FindAsync(id);
            if (city != null)
            {
                city.Status = true;
                var result = db.SupplierCity.Update(city);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "City changed to Active <a href ='/admin" + utils.GetTenantForUrl(false) + "/City/" + utils.EncryptId(city.Id) + "/edit.html'>" + city.CityCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("CityService - ActiveCityById - " + id + " - : ");
            return false;
        }

        public async Task<bool> InActiveCityById(int id)
        {
            var city = await db.SupplierCity.FindAsync(id);
            if (city != null)
            {
                city.Status = false;
                var result = db.SupplierCity.Update(city);
                db.SaveChanges();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "City changed to InActive <a href ='/admin" + utils.GetTenantForUrl(false) + "/City/" + utils.EncryptId(city.Id) + "/edit.html'>" + city.CityCode + "</a>. Changed By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("CityService - InActiveCityById - " + id + " - : ");
            return false;
        }

        public async Task<bool> DeleteCityById(int id)
        {
            var city = await db.SupplierCity.FindAsync(id);
            if (city != null)
            {
                db.SupplierCity.Remove(city);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "City deleted<a href ='/admin" + utils.GetTenantForUrl(false) + "/City/" + utils.EncryptId(city.Id) + "/edit.html'>" + city.CityCode + "</a>. Deleted By  <a href = '/admin" + utils.GetTenantForUrl(false) + "/user/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("CityService - DeleteCityById - " + id + " - : ");
            return false;
        }

        public async Task<List<CityViewModel>> GetAllCity(string countryCode)
        {
            var cityList = await db.SupplierCity.Where(x => x.CountryCode.ToUpper() == countryCode.ToUpper()).ToListAsync();
            return Mapper.Map<List<CityViewModel>>(cityList);
        }
    }
}
