using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Services
{
    public interface INotificationService
    {
        int GetAllNotificationCount(string userId,string roleId);
        List<NotificationViewModel> GetAllNotification(string userId,string roleId);
        List<NotificationViewModel> GetPaymentRequestNotification(string userId, string roleId);

    }

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _db;
        private readonly Utils utils;
        private readonly ILogger<NotificationService> _Log;
        private readonly IHttpContextAccessor _IHttpContextAccessor = null;
        private readonly IHostingEnvironment _HostingEnvironment;
        private readonly IMapper mapper;

        public NotificationService(ApplicationDbContext db, ILogger<NotificationService> Log, IHttpContextAccessor IHttpContextAccessor, IHostingEnvironment HostingEnvironment, IMapper _mapper, Utils _utils)
        {
            _db = db;
            _Log = Log;
            _IHttpContextAccessor = IHttpContextAccessor;
            _HostingEnvironment = HostingEnvironment;
            mapper = _mapper;
            utils = _utils;

        }

        public List<NotificationViewModel> GetAllNotification(string userId, string roleId)
        {
            List<NotificationViewModel> notifications = new List<NotificationViewModel>();
            notifications.AddRange(GetPaymentRequestNotification(userId, roleId));
            return notifications;
        }

        public int GetAllNotificationCount(string userId, string roleId)
        {
            var count = GetPaymentRequestNotification(userId, roleId).Count;
            return count;
        }

        public List<NotificationViewModel> GetPaymentRequestNotification(string userId,string roleId)
        {
            List<NotificationViewModel> notifications = new List<NotificationViewModel>();
            NotificationViewModel notificationViewModel = new NotificationViewModel()
            {
                Link = "",
                Title = "Payment Request",
                UserId = userId,
                RoleId = roleId,
                Detail = "Payment of 200GBP is requested by Khalid.",
                IsRead = false,
                CreatedOn = DateTime.Now
            };
            notifications.Add(notificationViewModel);
            return notifications;
            
        }
    }
}
