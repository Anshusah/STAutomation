using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.JazzCash;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;

namespace Cicero.Service.Services.JazzCash
{
    public interface IRecipientService
    {
        DTResponseModel GetRecipientListByFilter(DTPostModel model);
        Task<RecipientViewModel> CreateOrUpdate(RecipientViewModel model);
        Task<RecipientViewModel> GetRecipientByIdAsync(int id);

        Task<bool> DeleteRecipientById(int id);
    }

    public class RecipientService : IRecipientService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<RecipientService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly UserManager<ApplicationUser> userManager;

        public RecipientService(SimpleTransferApplicationDbContext _db, ILogger<RecipientService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils, UserManager<ApplicationUser> _UserManager)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
            userManager = _UserManager;
        }

        public DTResponseModel GetRecipientListByFilter(DTPostModel model)
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

            var recipientList = (from c in db.Beneficiary
                                select new
                                {
                                    id = c.Id,
                                    name = c.FirstName + " " + c.LastName,
                                    address = c.AddressLine1,
                                    created_at = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                    updated_at = Utils.GetDefaultDateFormatToDetail(c.UpdatedDate),
                                    action = "<a href='/jazzcash/recipient/" + c.Id + "/edit.html' class='btn btn-light btn-icon datatable__action datatable__edit-btn' title='Edit Recipient' data-toggle='tooltip' data-placement='top'><i class='ri-pencil-line'></i><span class='sr-only'>Edit Recipient</span></a>"
                                });

            totalResultsCount = recipientList.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                recipientList = recipientList.Where(o => o.name.ToLower().Contains(searchBy.ToLower()));

            }
            recipientList = recipientList.Skip(skip).Take(take).OrderBy(sortBy, sortDir);



            var list = recipientList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<RecipientViewModel> CreateOrUpdate(RecipientViewModel model)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                var recipient = new Beneficiary();
                recipient = Mapper.Map<Beneficiary>(model);
                recipient.CreatedBy = model.CreatedBy;
                recipient.UpdatedBy = commonService.getLoggedInUserId();
                recipient.CreatedDate = Convert.ToDateTime(model.CreatedDate);
                recipient.UpdatedDate = DateTime.Now;

                if (model.Id == 0)
                {
                    recipient.CreatedBy = recipient.UpdatedBy;
                    recipient.CreatedDate = recipient.UpdatedDate;
                    db.Beneficiary.Add(recipient);
                    await db.SaveChangesAsync();
                    return Mapper.Map<RecipientViewModel>(recipient);
                }
                else
                {
                    db.Beneficiary.Attach(recipient).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RecipientViewModel> GetRecipientByIdAsync(int id)
        {
            var recipient = await (from r in db.Beneficiary
                                          where r.Id == id
                                          select r).FirstOrDefaultAsync();

            return Mapper.Map<RecipientViewModel>(recipient);
        }

        public async Task<bool> DeleteRecipientById(int id)
        {
            var recipient = await db.Beneficiary.FindAsync(id);
            if (recipient != null)
            {
                db.Beneficiary.Remove(recipient);
                await db.SaveChangesAsync();

                var loggedUser = commonService.getLoggedInUserId();
                var fullName = commonService.GetUserFullName().Result;

                await activityLogService.CreateLog(loggedUser, "Recipient deleted <a href ='/jazzcash" + "/recipient/" + utils.EncryptId(recipient.Id) + "/edit.html'>" + recipient.FirstName + "</a>. Changed By  <a href = '/jazzcash" + "/recipient/" + loggedUser + "/edit.html' data - toggle = 'tooltip' > " + fullName + " </a> ");
                return true;
            }

            Log.LogError("Recipient - DeleteRecipientById - " + id + " - : ");
            return false;
        }
    }
}
