using AutoMapper;
using AutoMapper.Configuration;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models.API.LexisNexis;
using Cicero.Service.Services.SimpleTransfer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Cicero.Service.Services.API
{
    public interface ILexisNexisService
    {
        Task<LexixNexisViewModel> CreateOrUpdate(LexixNexisViewModel model);
        Task<SanctionPepCustomerViewModel> CreateOrUpdate(SanctionPepCustomerViewModel model);
        Task<SanctionPepBeneficiaryViewModel> CreateOrUpdate(SanctionPepBeneficiaryViewModel model);
        Task<List<SanctionPepPersonViewModel>> Create(List<SanctionPepPersonViewModel> model);
        Task<bool> Remove(string userId);
        Task<bool> RemoveExceptPassed(string userId, int personId);
        Task<LexixNexisViewModel> GetLexisNexisData(string userId);
        Task<SanctionPepCustomerDataModel> GetSanctionPepCustomer(string userId);
    }
    public class LexisNexisService : ILexisNexisService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<ILexisNexisService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly ApplicationDbContext applicationDb;
        private readonly ICustomerService customerService;
        private readonly ICountryService countryService;

        public LexisNexisService(SimpleTransferApplicationDbContext _db, ApplicationDbContext _adb, ILogger<ILexisNexisService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils, ApplicationDbContext applicationDbContext, ICustomerService customerService, ICountryService countryService)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
            applicationDb = applicationDbContext;
            this.customerService = customerService;
            this.countryService = countryService;
        }

        public async Task<LexixNexisViewModel> CreateOrUpdate(LexixNexisViewModel model)
        {
            try
            {
                LexisNexis lexisNexis = new LexisNexis();
                lexisNexis = Mapper.Map<LexisNexis>(model);
                lexisNexis.CreatedBy = model.CreatedBy;
                lexisNexis.UpdatedBy = commonService.getLoggedInUserId();
                lexisNexis.CreatedDate = Convert.ToDateTime(model.CreatedDate);
                lexisNexis.UpdatedDate = DateTime.Now;

                if (model.Id == 0)
                {
                    lexisNexis.CreatedBy = model.UpdatedBy;
                    lexisNexis.CreatedDate = Convert.ToDateTime(model.UpdatedDate);
                    db.LexisNexis.Add(lexisNexis);
                    await db.SaveChangesAsync();

                    return Mapper.Map<LexixNexisViewModel>(lexisNexis);
                }
                else
                {
                    db.LexisNexis.Attach(lexisNexis).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SanctionPepCustomerViewModel> CreateOrUpdate(SanctionPepCustomerViewModel model)
        {
            try
            {
                SanctionPepCustomer sanctionPepCustomer = new SanctionPepCustomer();
                sanctionPepCustomer = Mapper.Map<SanctionPepCustomer>(model);
                sanctionPepCustomer.CreatedBy = model.CreatedBy;
                sanctionPepCustomer.UpdatedBy = commonService.getLoggedInUserId();
                sanctionPepCustomer.CreatedDate = Convert.ToDateTime(model.CreatedDate);
                sanctionPepCustomer.UpdatedDate = DateTime.Now;

                if (model.Id == 0)
                {
                    sanctionPepCustomer.CreatedBy = model.UpdatedBy;
                    sanctionPepCustomer.CreatedDate = Convert.ToDateTime(model.UpdatedDate);
                    db.SanctionPepCustomer.Add(sanctionPepCustomer);
                    await db.SaveChangesAsync();

                    return Mapper.Map<SanctionPepCustomerViewModel>(sanctionPepCustomer);
                }
                else
                {
                    db.SanctionPepCustomer.Attach(sanctionPepCustomer).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SanctionPepBeneficiaryViewModel> CreateOrUpdate(SanctionPepBeneficiaryViewModel model)
        {
            try
            {
                SanctionPepBeneficiary sanctionPepBeneficiary = new SanctionPepBeneficiary();
                sanctionPepBeneficiary = Mapper.Map<SanctionPepBeneficiary>(model);
                sanctionPepBeneficiary.CreatedBy = model.CreatedBy;
                sanctionPepBeneficiary.UpdatedBy = commonService.getLoggedInUserId();
                sanctionPepBeneficiary.CreatedDate = Convert.ToDateTime(model.CreatedDate);
                sanctionPepBeneficiary.UpdatedDate = DateTime.Now;

                if (model.Id == 0)
                {
                    sanctionPepBeneficiary.CreatedBy = model.UpdatedBy;
                    sanctionPepBeneficiary.CreatedDate = Convert.ToDateTime(model.UpdatedDate);
                    db.SanctionPepBeneficiary.Add(sanctionPepBeneficiary);
                    await db.SaveChangesAsync();

                    return Mapper.Map<SanctionPepBeneficiaryViewModel>(sanctionPepBeneficiary);
                }
                else
                {
                    db.SanctionPepBeneficiary.Attach(sanctionPepBeneficiary).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<SanctionPepPersonViewModel>> Create(List<SanctionPepPersonViewModel> model)
        {
            try
            {
                List<SanctionPepPerson> sanctionPepPerson = new List<SanctionPepPerson>();
                sanctionPepPerson = Mapper.Map<List<SanctionPepPerson>>(model);

                foreach (var item in sanctionPepPerson)
                {
                    item.CreatedBy = commonService.getLoggedInUserId();
                    item.UpdatedBy = commonService.getLoggedInUserId();
                    item.CreatedDate = DateTime.Now;
                    item.UpdatedDate = DateTime.Now;
                }

                db.SanctionPepPerson.AddRange(sanctionPepPerson);
                await db.SaveChangesAsync();

                return Mapper.Map<List<SanctionPepPersonViewModel>>(sanctionPepPerson);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Remove(string userId)
        {
            try
            {
                var lexisNexisId = await db.LexisNexis.Where(x => x.UserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
                if (lexisNexisId > 0)
                {
                    var personData = await db.SanctionPepPerson.Where(x => x.LexisNexisId == lexisNexisId).ToListAsync();
                    if (personData.Count > 0)
                    {
                        db.SanctionPepPerson.RemoveRange(personData);
                        await db.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<LexixNexisViewModel> GetLexisNexisData(string userId)
        {
            try
            {
                var lexisNexis = await db.LexisNexis.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                return Mapper.Map<LexixNexisViewModel>(lexisNexis);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> RemoveExceptPassed(string userId, int personId)
        {
            try
            {
                var lexisNexisId = await db.LexisNexis.Where(x => x.UserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
                if (lexisNexisId > 0)
                {
                    var personData = await db.SanctionPepPerson.Where(x => x.LexisNexisId == lexisNexisId && x.Id != personId).ToListAsync();
                    if (personData.Count > 0)
                    {
                        db.SanctionPepPerson.RemoveRange(personData);
                        await db.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<SanctionPepCustomerDataModel> GetSanctionPepCustomer(string userId)
        {
            try
            {
                var data = await (from ln in db.LexisNexis
                                  join spc in db.SanctionPepCustomer on ln.Id equals spc.LexisNexisId
                                  join spp in db.SanctionPepPerson on ln.Id equals spp.LexisNexisId
                                  where ln.UserId == userId
                                  select new SanctionPepCustomerDataModel
                                  {
                                      PersonId = spp.Id,
                                      Match = spc.IsMatch,
                                      Remarks = spc.Remarks
                                  }).FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class SanctionPepCustomerDataModel
    {
        public int PersonId { get; set; }
        public bool Match { get; set; }
        public string Remarks { get; set; }
    }
}
