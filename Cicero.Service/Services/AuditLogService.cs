using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Extensions;
using Cicero.Service.Models;
using Cicero.Service.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Cicero.Service.Models.Core;
using Cicero.Service.Models.Case;
using Cicero.Service;
using System.Reflection;
using static Cicero.Service.Enums;

namespace Cicero.Service.Services
{
    public interface IAuditLogService
    {
        Task<bool> LogWriter<T>(T newValue, int operation, string objectId, T parentObject, T oldValue = null, string parentId = null) where T : class;
        Task<bool> LogWriterManual(string description, int operation, string objectId, string objectValue, string parentObjectValue, string parentId = null);
        Task<List<AuditLogViewModel>> LogReader(string objectId, string parentObject, int skip, int take);
        Task<bool> UpdateManualLog(AuditLogViewModel auditLogViewModel);
        Task<bool> DeleteManualLog(long auditId);
        Task<List<AuditLogViewModel>> LogReaderForClaim(string objectId, string parentObject, int skip, int take);

    }


    public class AuditLogService : IAuditLogService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICommonService _commonService;
        private readonly Utils _utils;
        private readonly IMapper _mapper;
        private readonly IQueueService _queueService;

        public AuditLogService(ApplicationDbContext db
            , ICommonService commonService
            , Utils utils
            , IMapper mapper
            , IQueueService queueService)
        {
            _db = db;
            _commonService = commonService;
            _utils = utils;
            _mapper = mapper;
            _queueService = queueService;
        }

        public async Task<bool> DeleteManualLog(long auditId)
        {
            try
            {
                AuditLog log = await _db.AuditLog.Where(x => x.Id == auditId && x.IsManual == true && x.IsDeleted == false).SingleOrDefaultAsync();
                if (log != null)
                {
                    log.IsDeleted = true;
                    log.DeletedTimeStamp = DateTime.UtcNow;
                    _db.AuditLog.Update(log);
                    await _db.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<List<AuditLogViewModel>> LogReader(string objectId, string parentObject, int skip, int take)
        {
            try
            {
                List<AuditLogViewModel> logs = await _db.AuditLog.Where(e => e.ObjectId == objectId && e.ParentObject == parentObject).OrderByDescending(x => x.TimeStamp).Skip(skip).Take(take).Select(x => new AuditLogViewModel
                {
                    Id = x.Id,
                    ParentObject = x.ParentObject,
                    Description = x.Description,
                    ParentId = x.ParentId,
                    TenantId = x.TenantId,
                    Object = x.Object,
                    ObjectId = x.ObjectId,
                    OldValue = x.OldValue,
                    NewValue = x.NewValue,
                    FieldName = x.FieldName,
                    User = x.User,
                    UserId = x.UserId,
                    OperationType = x.OperationType,
                    TimeStamp = x.TimeStamp.ToLocalTime()
                }).ToListAsync();
                return logs;
            }
            catch (Exception ex)
            {
                return new List<AuditLogViewModel>();
            }
        }

        public async Task<List<AuditLogViewModel>> LogReaderForClaim(string objectId, string parentObject, int skip, int take)
        {
            string anonymous = "a2d36535-eb10-4721-89b0-a53986d1b4c4";
            try
            {
                List<AuditLogViewModel> logs = await _db.AuditLog.Where(e => e.ObjectId == objectId && e.UserId != anonymous && e.IsDeleted == false && e.ParentObject == parentObject &&
                                                                            (e.ParentObject != e.Object ||
                                                                                (e.ParentObject == e.Object &&
                                                                                    (e.FieldName == "UpdatedAt" || e.FieldName == "StateId" || e.FieldName == "AssignedTo" || e.IsManual == true
                                                                                    )
                                                                                 )
                                                                             )
                                                                         )
                                                                    .OrderByDescending(x => x.TimeStamp).Skip(skip).Take(take).Select(x => new AuditLogViewModel
                                                                    {
                                                                        Id = x.Id,
                                                                        ParentObject = x.ParentObject,
                                                                        Description = x.Description,
                                                                        User = x.User,
                                                                        ObjectId = x.ObjectId,
                                                                        Object = x.Object,
                                                                        FieldName = x.FieldName,
                                                                        OldValue = x.OldValue,
                                                                        NewValue = x.NewValue,
                                                                        IsManual = (x.IsManual == null ? false : (bool)x.IsManual),
                                                                        OperationType = x.OperationType,
                                                                        TimeStamp = x.TimeStamp.ToLocalTime()
                                                                    }).ToListAsync();

                foreach (var item in logs)
                {
                    if (item.Description == null || item.Description == string.Empty || item.Description == "")
                    {
                        item.Description = GetDescription(item.OldValue, item.ObjectId, item.FieldName, item.NewValue, item.OperationType, item.ParentObject, item.Object).Result;
                    }

                }
                return logs;
            }
            catch (Exception ex)
            {
                return new List<AuditLogViewModel>();
            }
        }

        private async Task<string> GetDescription(string oldValue, string ObjectId, string field, string newValue, int operationType, string parent, string objectValue)
        {
            string description = string.Empty;
            Case cvm = _db.Case.Find(Convert.ToInt32(ObjectId));
            if (parent == objectValue)
            {
                if (operationType == (int)AuditLogOperation.Insert)
                {
                    switch (field)
                    {
                        case "UpdatedAt":
                            description = "added new Lead";
                            break;
                        case "AssignedTo":
                            if (newValue != string.Empty && newValue != "")
                            {
                                var user = await _commonService.GetUserById(newValue);
                                description = "assigned lead to " + user.FirstName + " " + user.LastName;
                            }

                            break;
                        case "StateId":
                            description = "updated Lead state to " + _queueService.GetStateNameById(Convert.ToInt32(newValue), cvm.CaseFormId);
                            break;
                    }

                }
                else if (operationType == (int)AuditLogOperation.Update)
                {
                    switch (field)
                    {
                        case "UpdatedAt":
                            description = "updated Lead";
                            break;
                        case "AssignedTo":
                            if (newValue != string.Empty && newValue != "")
                            {
                                var user = await _commonService.GetUserById(newValue);
                                description = "assigned lead to " + user.FirstName + " " + user.LastName;
                            }
                            break;
                        case "StateId":
                            description = "updated Lead state to " + _queueService.GetStateNameById(Convert.ToInt32(newValue), cvm.CaseFormId);
                            break;
                    }
                }
            }
            else
            {
                if (operationType == (int)AuditLogOperation.Insert)
                {
                    try
                    {
                        JObject obj = (JObject)JsonConvert.DeserializeObject(newValue);
                        description = "added " + obj.Count + " " + field;
                    }
                    catch (Exception ex)
                    {
                        description = "added " + field + " : " + newValue;
                    }

                }
                else if (operationType == (int)AuditLogOperation.Update)
                {
                    try
                    {
                        JObject objNew = (JObject)JsonConvert.DeserializeObject(newValue);
                        JObject objOld = (JObject)JsonConvert.DeserializeObject(oldValue);
                        if (objNew.Count > objOld.Count)
                        {
                            description = "added " + (objNew.Count - objOld.Count) + " " + field;
                        }
                        else
                        {
                            description = "updated " + field;
                        }
                    }
                    catch (Exception ex)
                    {
                        description = "updated " + field + " from : " + oldValue + " to : " + newValue;
                    }

                }
            }
            return description;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue"></param>
        /// <param name="operation"></param>
        /// <param name="objectId"></param>
        /// <param name="oldValue"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<bool> LogWriter<T>(T newValue, int operation, string objectId, T parentObject, T oldValue = null, string parentId = null) where T : class
        {
            try
            {
                dynamic props = newValue.GetType().GetRuntimeProperties();
                List<AuditLog> logs = new List<AuditLog>();
                string userId = _commonService.getLoggedInUserId();
                string user = await _commonService.GetUserFullName();
                int tenantId = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());


                foreach (var prop in props)
                {
                    AuditLog log = new AuditLog();
                    log.OperationType = operation;
                    log.FieldName = prop.Name;
                    log.NewValue = Convert.ToString(newValue.GetType().GetProperty(log.FieldName).GetValue(newValue));
                    log.TimeStamp = DateTime.UtcNow;
                    log.User = user;
                    log.UserId = userId;
                    log.TenantId = tenantId;
                    log.ObjectId = objectId;
                    log.Object = newValue.GetType().Name;
                    log.IsManual = false;
                    log.ParentObject = parentObject.GetType().Name;
                    log.IsDeleted = false;
                    if (operation == (int)AuditLogOperation.Update)
                    {
                        var isit = oldValue.GetType().GetProperty(log.FieldName);
                        if (isit != null)
                        {
                            string oVal = Convert.ToString(oldValue.GetType().GetProperty(log.FieldName).GetValue(oldValue));
                            if (oVal != log.NewValue)
                            {
                                log.OldValue = oVal;
                                logs.Add(log);
                            }
                        }
                        else // if update and old value doesn't exist
                        {
                            log.OperationType = (int)AuditLogOperation.Insert;
                            logs.Add(log);
                        }

                    }
                    else
                    {
                        logs.Add(log);
                    }
                }
                await _db.AuditLog.AddRangeAsync(logs);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Manual Notes addition 
        /// </summary>
        /// <param name="description">Note description</param>
        /// <param name="operation">Note Operation</param>
        /// <param name="objectId">Note for Object Id</param>
        /// <param name="objectValue">Note for Object</param>
        /// <param name="parentId">Note parent AuditLog Id</param>
        /// <returns></returns>
        public async Task<bool> LogWriterManual(string description, int operation, string objectId, string objectValue, string parentObjectValue, string parentId = null)
        {
            try
            {
                string userId = _commonService.getLoggedInUserId();
                string user = await _commonService.GetUserFullName();
                int tenantId = _commonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
                AuditLog log = new AuditLog()
                {
                    OperationType = operation,
                    User = user,
                    UserId = userId,
                    TenantId = tenantId,
                    Description = description,
                    Object = objectValue,
                    ObjectId = objectId,
                    TimeStamp = DateTime.UtcNow,
                    ParentObject = parentObjectValue,
                    IsManual = true,
                    IsDeleted = false
                };

                await _db.AuditLog.AddAsync(log);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public async Task<bool> UpdateManualLog(AuditLogViewModel auditLogViewModel)
        {
            try
            {
                AuditLog auditLog = await _db.AuditLog.Where(x => x.Id == auditLogViewModel.Id && x.IsManual == true && x.IsDeleted == false).SingleOrDefaultAsync();
                if (auditLog != null)
                {
                    auditLog.Description = auditLogViewModel.Description;
                    _db.Update(auditLog);
                    await _db.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
