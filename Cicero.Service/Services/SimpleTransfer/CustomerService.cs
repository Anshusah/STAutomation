using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models.SimpleTransfer.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Models.SimpleTransfer.Customer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ICustomerService
    {
        Task<SenderDetailViewModel> CreateOrUpdate(SenderDetailViewModel model);
        Task<string> GetCustomerCountryCode(string userId);
        Task<SenderDetailViewModel> GetCustomerById(string userId);
        Task<bool> OnfidoVerifyUpdate(string userId);
        Task<string> GetApplicantIdByEmail(string email);
        Task<string> SaveCustomerCardDetails(CustomerCardDetail customerCardDetail);
        Task<CustomerCardDetail> GetCustomerCardDetails(string userId);
        Task<List<SelectListItem>> GetCardDetails(string userId);
        Task<CustomerCardDetail> GetCardDetail(int id);

    }

    public class CustomerService : ICustomerService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<CustomerService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly UserManager<ApplicationUser> userManager;

        public CustomerService(SimpleTransferApplicationDbContext _db, ILogger<CustomerService> _Log, IMapper _mapper,
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


        public async Task<SenderDetailViewModel> CreateOrUpdate(SenderDetailViewModel model)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                Customer customer = new Customer
                {
                    Id = model.Id,
                    UserId = model.UserId,
                    TenantId = tenantId,
                    CountryCode = model.Country,
                    DOB = model.DOB,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    Title = model.Title,
                    Address = model.AddressLine,
                    City = model.City,
                    PostCode = model.PostCode,
                    IdType = Convert.ToInt32(model.IdType),
                    IdNumber = model.IdNumber,
                    IdExpiryDate = model.IdExpiryDate,
                    CreatedBy = model.CreatedBy,
                    UpdatedBy = commonService.getLoggedInUserId(),
                    CreatedDate = Convert.ToDateTime(model.CreatedDate),
                    UpdatedDate = DateTime.Now,
                    Status = true
                };

                if (model.Id == null)
                {
                    db.Customer.Add(customer);
                    await db.SaveChangesAsync();
                    return Mapper.Map<SenderDetailViewModel>(customer);
                }
                else
                {
                    db.Customer.Attach(customer).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }


                return model;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public async Task<string> GetApplicantIdByEmail(string email)
        {
            var customerId = await db.Customer.Where(x => x.Email == email).Select(x => x.Id).FirstOrDefaultAsync();
            var applicantId = await db.OnfidoApplicant.Where(x => x.CustomerId == customerId).Select(x => x.ApplicantId).FirstOrDefaultAsync();
            return applicantId;
        }

        public async Task<CustomerCardDetail> GetCustomerCardDetails(string userId)
        {
            CustomerCardDetail detail = await db.CustomerCardDetail.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            return detail;
        }

        public async Task<CustomerCardDetail> GetCardDetail(int id)
        {
            CustomerCardDetail detail = await db.CustomerCardDetail.Where(x => x.Id == id).FirstOrDefaultAsync();
            return detail;
        }

        public async Task<List<SelectListItem>> GetCardDetails(string userId)
        {
            List<SelectListItem> cardDetails = await db.CustomerCardDetail.Where(x=>x.UserId == userId).Select(x => new SelectListItem() { Text = x.Number, Value = x.Id.ToString() }).ToListAsync();
            return cardDetails;
        }

        public async Task<CustomerCardDetail> GetCustomerPaymentDetails(string userId)
        {
            CustomerCardDetail detail = await db.CustomerCardDetail.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            return detail;
        }
        public async Task<bool> OnfidoVerifyUpdate(string userId)
        {
            try
            {
                var customer = await db.Customer.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                customer.IsOnfidoVerify = true;
                customer.KycVerifiedDate = DateTime.Now;
                db.Customer.Attach(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> SaveCustomerCardDetails(CustomerCardDetail customerCardDetail)
        {
            try
            {
                var duplicateCardNumber = await db.CustomerCardDetail.Where(x => x.Number == customerCardDetail.Number).FirstOrDefaultAsync();
                if(duplicateCardNumber == null)
                {
                    db.CustomerCardDetail.Add(customerCardDetail);
                    await db.SaveChangesAsync();
                }
              
                return "";

            }
            catch (Exception ex)
            {
                return "";

            }
        }

        public async Task<SenderDetailViewModel> GetCustomerById(string userId)
        {
            var customer = await db.Customer.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            return Mapper.Map<SenderDetailViewModel>(customer);
        }

        public async Task<string> GetCustomerCountryCode(string userId)
        {
            var countryCode = await db.Customer.Where(x => x.UserId == userId).Select(x=>x.CountryCode).FirstOrDefaultAsync();
            return countryCode;
        }
    }
}
