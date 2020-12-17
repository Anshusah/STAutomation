using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Data;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;


namespace Cicero.Service.Services
{
    public interface IMessageService
    {
        int GetUnreadMessagesCount(string id);
        Task<List<MessageViewModel>> GetAllMessages();
        Task<List<MessageViewModel>> GetMessagesByUserIdOrMarkMessageAsRead(string id, List<int> ids, bool markAsRead = false);
        Task<List<MessageViewModel>> GetNoticeMessages(int claimId);
        MessageViewModel SendMessage(MessageViewModel mvm, string[] recipients);
        MessageViewModel SendNotice(MessageViewModel mvm);
        Task<List<MessageViewModel>> GetMessagesByParentId(int parentId, string id);

        IEnumerable<MediaViewModel> GetImagesByMessageId(int id);
        Task<DTResponseModel> GetMessageListByFilter(DTPostModel model);
        Task<List<MessageViewModel>> GetAllMessagesByUserId(string id);
        Task<MessageViewModel> GetMessageByMessageId(int id);
        Task<bool> DeleteMessage(string id, List<int> ids);

    }

    public class MessageService : IMessageService
    {

        private readonly ApplicationDbContext db;
        private readonly ILogger<MessageService> log;
        private readonly IHttpContextAccessor httpContextAccessor = null;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IMapper mapper;
        private readonly Utils utils;
        private readonly ICommonService commonService;
        private readonly IUserService userService;

        public MessageService(ApplicationDbContext _db, ILogger<MessageService> _Log, IHttpContextAccessor _IHttpContextAccessor, IHostingEnvironment _HostingEnvironment, IMapper _mapper, Utils _utils, ICommonService _commonService, IUserService userService)
        {
            db = _db;
            log = _Log;
            httpContextAccessor = _IHttpContextAccessor;
            hostingEnvironment = _HostingEnvironment;
            mapper = _mapper;
            utils = _utils;
            commonService = _commonService;
            this.userService = userService;
        }

        public int GetUnreadMessagesCount(string id)
        {
            try
            {
                int count = db.Message.Where(y => y.MessageUsers.Any(z => z.UserId == id) && y.ReceiverDelete == false && y.IsRead == false).Count();
                return count;

            }
            catch (Exception ex)
            {
                log.LogError("CiceroMessageService - GetAllMessagesLogAsync - " + ex);
                return 0;
            }
        }

        public async Task<List<MessageViewModel>> GetAllMessages()
        {
            try
            {
                int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

                var model = mapper.Map<List<MessageViewModel>>(await db.Message.Include(x => x.MessageUsers).ThenInclude(c => c.UserForMessage).Include(x => x.Sender)
                                                                            .Where(b => b.ParentId == 0 && (b.TenantId == tenantid || tenantid == 0) && b.IsNotice == false)
                                                                            .OrderByDescending(x => x.CreatedAt)
                                                                            .ToListAsync());

                return model;

            }
            catch (Exception ex)
            {
                log.LogError("MessageService - GetAllMessagesLogAsync - " + ex);
                return null;
            }
        }

        #region oldGetMessageByUserId
        //public async Task<List<MessageViewModel>> GetMessagesByUserId(string id)
        //{

        //    try
        //    {
        //        int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

        //        var messageId = db.Message.Where(b => (b.MessageUsers.Any(y => y.UserId == id) || b.From == id) && b.IsNotice == false && b.ParentId != 0 && (b.TenantId == tenantid || tenantid == 0)).OrderBy(c => c.Id).GroupBy(e => e.ParentId).Select(f => f.First()).Select(d => d.ParentId).ToList();

        //        if (messageId.Count() > 0)
        //        {
        //            var baseModel = new List<Message>();
        //            foreach (var item in messageId)
        //            {
        //                bool isReadCheck = true;
        //                if (db.Message
        //                    .Include(x => x.MessageUsers).ThenInclude(b => b.UserForMessage).Include(x => x.Sender)
        //                    .Where(y => (y.ParentId == item || y.Id == item) && y.MessageUsers.Any(a => a.UserId == id) && y.IsNotice == false)
        //                    .OrderByDescending(x => x.CreatedAt).Any(b => b.IsRead == false) == true)
        //                {
        //                    isReadCheck = false;
        //                }
        //                baseModel.Add(await db.Message
        //                                    .Include(x => x.MessageUsers).Include(x => x.Sender)
        //                                    .Where(y => y.Id == item && y.IsNotice == false)
        //                                    .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync());

        //                for (int i = 0; i < baseModel.Count(); i++)
        //                {
        //                    if (messageId[i] == baseModel[i].Id && isReadCheck == false)
        //                    {
        //                        baseModel[i].IsRead = isReadCheck;
        //                    }

        //                }
        //            }

        //            if (baseModel != null)
        //            {
        //                var modelMessage = mapper.Map<List<MessageViewModel>>(baseModel);
        //                return modelMessage;
        //            }

        //        }

        //        var model = mapper.Map<List<MessageViewModel>>(await db.Message
        //                                                        .Include(x => x.MessageUsers).Include(x => x.Sender)
        //                                                        .Where(y => (y.MessageUsers.Any(a => a.UserId == id) || y.From == id) && y.ParentId == 0 && y.IsNotice == false)
        //                                                        .OrderByDescending(x => x.CreatedAt).ToListAsync());
        //        return model;

        //    }
        //    catch (Exception ex)
        //    {
        //        log.LogError("MessageService - GetAllMessagesAsync - " + ex);
        //        return null;
        //    }
        //}
        #endregion

        public async Task<List<MessageViewModel>> GetMessagesByUserIdOrMarkMessageAsRead(string id, List<int> ids, bool markAsRead = false)
        {

            try
            {

                var messages = new List<Message>();
                if (!markAsRead)
                {
                    var messagesData = mapper.Map<List<MessageViewModel>>(await db.Message.Where(y => y.MessageUsers.Any(z => z.UserId == id) && y.ReceiverDelete == false && y.IsRead == false).Include(x => x.MessageUsers).ThenInclude(c => c.UserForMessage).Include(y => y.Sender).ToListAsync());
                    foreach (var item in messagesData)
                    {
                        if (item.ParentId != 0)
                        {
                            item.Subject = db.Message.Where(x => x.Id == item.ParentId).Select(x => x.Subject).FirstOrDefault();
                            item.Id = item.ParentId;
                        }

                        item.SenderImage = await userService.GetDefaultOrFirstImagesByUserId(item.From, "");
                    }

                    return messagesData;
                }
                else
                {

                    if (ids.Count > 0)
                    {
                        messages = await db.Message.Where(y => y.MessageUsers.Any(z => z.UserId == id) && y.IsRead == false && (ids.Contains(y.Id) || ids.Contains(y.ParentId))).ToListAsync();
                    }
                    else
                    {
                        messages = await db.Message.Where(y => y.MessageUsers.Any(z => z.UserId == id) && y.IsRead == false).ToListAsync();
                    }

                    if (messages.Count > 0)
                    {
                        foreach (var item in messages)
                        {
                            item.IsRead = true;
                        }
                        db.Message.UpdateRange(messages);
                        await db.SaveChangesAsync();
                    }
                }

                return mapper.Map<List<MessageViewModel>>(messages);
            }
            catch (Exception ex)
            {
                log.LogError("MessageService - GetAllMessagesAsync - " + ex);
                return null;
            }
        }

        public async Task<List<MessageViewModel>> GetNoticeMessages(int claimId)
        {
            try
            {
                int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

                var model = mapper.Map<List<MessageViewModel>>(await db.Message.Include(x => x.MessageUsers).Include(x => x.Sender)
                                                                            .Where(b => b.ParentId == 0 && (b.TenantId == tenantid || tenantid == 0) && b.IsNotice == true && b.ClaimId == claimId)
                                                                            .OrderByDescending(x => x.CreatedAt)
                                                                            .ToListAsync());

                return model;

            }
            catch (Exception ex)
            {
                log.LogError("MessageService - GetNoticeMessagesAsync - " + ex);
                return null;
            }
        }

        public MessageViewModel SendMessage(MessageViewModel mvm, string[] recipients)
        {
            try
            {
                //var result = db.Message.Where(x => x.Id == mvm.Id).Count();

                mvm.IsRead = false;

                if (recipients.Count() > 0)
                {
                    if (mvm.Id != 0 && mvm.ParentId == 0)
                    {
                        mvm.ParentId = mvm.Id;
                    }
                    mvm.Id = 0;
                    var model = mapper.Map<Message>(mvm);

                    model.CreatedAt = DateTime.Now;
                    if (model.ClaimId == 0)
                    {
                        model.ClaimId = null;
                    }

                    db.Message.Add(model);
                    db.SaveChanges();
                    mvm.Id = model.Id;

                    foreach (var item in recipients)
                    {
                        var usermodel = new MessageUser()
                        {
                            Id = 0,
                            MessageId = mvm.Id,
                            UserId = item
                        };

                        db.MessageUser.Add(usermodel);
                    }

                    db.SaveChanges();

                }

                return mvm;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MessageViewModel SendNotice(MessageViewModel mvm)
        {
            try
            {
                string loggeduser = commonService.getLoggedInUserId();

                if (mvm.Id != 0 && mvm.ParentId == 0)
                {
                    mvm.ParentId = mvm.Id;
                }

                mvm.Id = 0;
                mvm.From = loggeduser;
                mvm.To = null;
                mvm.IsNotice = true;
                mvm.CreatedAt = DateTime.Now;
                var model = mapper.Map<Message>(mvm);
                //mvm.CreatedAt = DateTime.Now;

                db.Message.Add(model);
                db.SaveChanges();
                mvm.Id = model.Id;

                return mvm;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<MessageViewModel>> GetMessagesByParentId(int parentId, string id)
        {
            try
            {

                int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

                var messageModel = db.Message
                                    .Include(x => x.Sender)
                                    .Include(a => a.MessageUsers)
                                            .ThenInclude(b => b.UserForMessage)
                                    .Include(c => c.Case)
                                    .Where(y => (y.ParentId == parentId || y.Id == parentId) && (y.TenantId == tenantid || tenantid == 0))
                                    .OrderByDescending(x => x.CreatedAt).ToList();

                var model = mapper.Map<List<MessageViewModel>>(messageModel);

                //foreach (var item in messageModel)
                //{

                //    if (item.IsRead == false && item.MessageUsers.Any(x => x.UserId == id))
                //    {
                //        item.IsRead = true;
                //        db.Update(item);
                //        db.SaveChanges();
                //    }
                //}

                foreach (var item in model)
                {
                    item.SenderImage = await userService.GetDefaultOrFirstImagesByUserId(item.From, "");
                }

                return model;

            }
            catch (Exception ex)
            {
                log.LogError("MessageService - GetMessagesByParentId - " + ex);
                return null;
            }
        }

        public IEnumerable<MediaViewModel> GetImagesByMessageId(int id)
        {

            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            string attachment = db.Message.Where(x => x.Id == id && (x.TenantId == tenantid || tenantid == 0)).Select(b => b.Attachment).FirstOrDefault();
            if (attachment != null)
            {
                var imagesId = JsonConvert.DeserializeObject<List<int>>(attachment);

                List<MediaViewModel> mediaList = new List<MediaViewModel> { };

                var mediaListTry = new MediaViewModel { };
                foreach (var item in imagesId)
                {
                    var mediaItems = db.Media.Where(x => x.Id == item)
                                        .Select(b => new MediaViewModel
                                        {
                                            Id = b.Id,
                                            CreatedBy = b.CreatedBy,
                                            Description = b.Description,
                                            Title = b.Title,
                                            Url = b.Url
                                        }).FirstOrDefault();

                    if (mediaItems != null)
                    {
                        mediaList.Add(mediaItems);
                    }
                }
                return mediaList;
            }
            return null;
        }

        public async Task<List<MessageViewModel>> GetAllMessagesByUserId(string id)
        {

            try
            {
                var model = new List<MessageViewModel>();
                var senderData = mapper.Map<List<MessageViewModel>>(db.Message
                                                                .Include(x => x.MessageUsers).Include(x => x.Sender)
                                                                .Where(y => y.From == id /*&& y.SenderDelete == false*/ && y.IsNotice == false)
                                                                //    .OrderByDescending(x => x.CreatedAt)
                                                                .ToList());

                var receiverData = mapper.Map<List<MessageViewModel>>(db.Message
                                                               .Include(x => x.MessageUsers).Include(x => x.Sender)
                                                               .Where(y => (y.MessageUsers.Any(a => a.UserId == id)) /*&& y.ReceiverDelete == false*/ && y.IsNotice == false)
                                                               //    .OrderByDescending(x => x.CreatedAt)
                                                               .ToList());

                model.AddRange(senderData);
                model.AddRange(receiverData);
                model = model.OrderBy(x => x.CreatedAt).ToList();//model.Union(senderData).Union(receiverData).OrderBy(x => x.CreatedAt).ToList();
                var ids = new List<int>();

                foreach (var item in model.Where(x => x.From == id).ToList())
                {
                    var childData = model.Where(x => x.ParentId == item.Id).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    item.SenderImage = await userService.GetDefaultOrFirstImagesByUserId(item.From, "");
                    item.ReceiverImage = new List<string>();
                    foreach (var itm in item.To)
                    {
                        var receiverImage = await userService.GetDefaultOrFirstImagesByUserId(itm, "");
                        item.ReceiverImage.Add(receiverImage);
                    }

                    item.IsRead = true;
                    if (childData != null)
                    {
                        item.From = childData.From;
                        item.Content = childData.Content;
                        item.CreatedAt = childData.CreatedAt;
                        if (childData.From != id)
                        {
                            if (childData.ReceiverDelete)
                            {
                                ids.Add(item.Id);
                            }
                            item.IsRead = childData.IsRead;
                        }
                        else
                        {
                            if (childData.SenderDelete)
                            {
                                ids.Add(item.Id);
                            }
                        }

                        item.SenderImage = await userService.GetDefaultOrFirstImagesByUserId(childData.From, "");
                        foreach (var itm in childData.To)
                        {
                            var receiverImage = await userService.GetDefaultOrFirstImagesByUserId(itm, "");
                            item.ReceiverImage.Add(receiverImage);
                        }
                    }
                    else
                    {
                        if (item.SenderDelete)
                        {
                            ids.Add(item.Id);
                        }
                    }
                }

                foreach (var item in model.Where(x => x.To.Contains(id)).ToList())
                {
                    var childData = model.Where(x => x.ParentId == item.Id).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    item.SenderImage = await userService.GetDefaultOrFirstImagesByUserId(item.From, "");
                    item.ReceiverImage = new List<string>();
                    foreach (var itm in item.To)
                    {
                        var receiverImage = await userService.GetDefaultOrFirstImagesByUserId(itm, "");
                        item.ReceiverImage.Add(receiverImage);
                    }

                    if (childData != null)
                    {
                        if (childData.From != id)
                        {
                            if (childData.ReceiverDelete)
                            {
                                ids.Add(item.Id);
                            }
                        }
                        else
                        {
                            if (childData.SenderDelete)
                            {
                                ids.Add(item.Id);
                            }
                        }
                        item.From = childData.From;
                        item.Content = childData.Content;
                        item.CreatedAt = childData.CreatedAt;
                        item.IsRead = childData.IsRead;

                        item.SenderImage = await userService.GetDefaultOrFirstImagesByUserId(childData.From, "");
                        foreach (var itm in childData.To)
                        {
                            var receiverImage = await userService.GetDefaultOrFirstImagesByUserId(itm, "");
                            item.ReceiverImage.Add(receiverImage);
                        }
                    }
                    else
                    {
                        if (item.ReceiverDelete)
                        {
                            ids.Add(item.Id);
                        }
                    }
                }
                model = model.Where(x => !(ids.Contains(x.Id))).ToList();
                return model.Where(x => x.ParentId == 0).ToList();

            }
            catch (Exception ex)
            {
                log.LogError("MessageService - GetAllMessagesAsync - " + ex);
                return null;
            }
        }

        public async Task<DTResponseModel> GetMessageListByFilter(DTPostModel model)
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

            string loggedUser = commonService.getLoggedInUserId();

            var messagelist = await GetAllMessagesByUserId(loggedUser);
            totalResultsCount = messagelist.Count();
            if (!String.IsNullOrEmpty(searchBy))
            {
                messagelist = messagelist.Where(o => o.Subject.ToLower().Contains(searchBy.ToLower())).ToList();

            }
            totalResultsCount = messagelist.Count();
            messagelist = messagelist.Skip(skip).Take(take).ToList();//.OrderBy(sortBy, sortDir);

            var list = messagelist.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }

        public async Task<MessageViewModel> GetMessageByMessageId(int id)
        {
            try
            {
                var data = mapper.Map<MessageViewModel>(db.Message
                                                               .Include(x => x.MessageUsers).Include(x => x.Sender)
                                                               .Where(y => (y.Id == id)).FirstOrDefault());

                data.SenderImage = await userService.GetDefaultOrFirstImagesByUserId(data.From, "");
                return data;
            }
            catch (Exception ex)
            {
                log.LogError("MessageService - GetMessages - " + ex);
                return null;
            }
        }

        public async Task<bool> DeleteMessage(string id, List<int> ids)
        {
            try
            {
                var messageData = new List<Message>();
                foreach (var item in ids)
                {
                    var messages = db.Message.Where(x => x.Id == item || x.ParentId == item).ToList();
                    foreach (var itm in messages)
                    {
                        if (itm.From == id)
                        {
                            itm.SenderDelete = true;
                        }
                        else
                        {
                            itm.ReceiverDelete = true;
                        }
                        messageData.Add(itm);
                    }
                }

                db.Message.UpdateRange(messageData);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false; ;
            }
        }
    }
}
