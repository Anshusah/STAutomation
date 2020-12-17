using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Models;
using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Cicero.Service.Models.User;
using Cicero.Service.Models.SimpleTransfer.Onfido;
using Cicero.Data.Entities.SimpleTransfer;

namespace Cicero.Service.Backend
{
    public class ModelProfile : Profile
    {

        public ModelProfile()
        {

            CreateMap<Case, CaseViewModel>();
            CreateMap<CaseViewModel, Case>();
            CreateMap<Actions, ActionsViewModel>();
            CreateMap<ActionsViewModel, Actions>();
            //CreateMap<CaseViewModel, Case>().ForSourceMember(dest => dest.FormBuilder,opt=>opt.DoNotValidate()).ForSourceMember(dest => dest.StateList, opt => opt.DoNotValidate()).ForSourceMember(dest => dest.CaseFormName, opt => opt.DoNotValidate())
            //.ForMember(opt=>opt.Tenant,a=>a.Ignore()).ForMember(opt => opt.CaseForm, a => a.Ignore()).ForMember(opt => opt.User, a => a.Ignore());

            CreateMap<CaseForm, CaseFormViewModel>();
            //.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedAt)))
            //.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedAt)));

            CreateMap<CaseFormViewModel, CaseForm>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedAt)));

            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();

            CreateMap<UserViewModel, ChangePasswordViewModel>();

            CreateMap<Article, ArticleViewModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedAt)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedAt)));

            CreateMap<ArticleViewModel, Article>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedAt)));

            CreateMap<AddArticleViewModel, Article>().ReverseMap();

            CreateMap<Article, TemplateViewModel>()
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedAt)))
                    .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedAt)));

            CreateMap<TemplateViewModel, Article>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedAt)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedAt)));

            CreateMap<Media, MediaViewModel>().ReverseMap();

            CreateMap<ActivityLog, ActivityLogViewModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.State.SystemName));

            CreateMap<Message, MessageViewModel>()
                //.ForMember(dest => dest.NameReceiver, opt => opt.MapFrom(src => src.Receiver.FirstName + " " + src.Receiver.LastName))
                .ForMember(dest => dest.NameSender, opt => opt.MapFrom(src => src.Sender.FirstName + " " + src.Sender.LastName))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.MessageUsers.Where(x => x.MessageId == src.Id).Select(b => b.UserId).ToList()))
                .ForMember(dest => dest.ClaimGeneratedId, opt => opt.MapFrom(src => src.Case.CaseGeneratedId))
                .ForMember(dest => dest.NameReceiver, opt => opt.MapFrom(src => src.MessageUsers.Where(x => x.MessageId == src.Id).Select(a => a.UserForMessage.FirstName + " " + a.UserForMessage.LastName).ToList()));

            CreateMap<MessageViewModel, Message>();

            CreateMap<Tenant, TenantViewModel>().ReverseMap();

            CreateMap<Queue, QueueViewModel>().ReverseMap();

            CreateMap<State, StateViewModel>().ReverseMap();

            CreateMap<StateToState, FromState>()
                .ForMember(dest => dest.JsonState, opt => opt.Ignore());

            CreateMap<StateForFormView, StateForForm>();

            CreateMap<StateForForm, StateForFormView>()
                    .ForMember(dest => dest.CaseFormName, opt => opt.MapFrom(src => src.CaseForm.Name));

            //CreateMap<StateForFormView, StateForForm>().ReverseMap();

            CreateMap<StatePermissionView, StatePermission>().ReverseMap();

            CreateMap<QueueToState, QueueState>()
                .ForMember(dest => dest.JsonState, opt => opt.Ignore());

            CreateMap<QueueForFormView, QueueForForm>();

            CreateMap<QueueForForm, QueueForFormView>()
                    .ForMember(dest => dest.CaseFormName, opt => opt.MapFrom(src => src.CaseForm.Name));

            //CreateMap<QueueForFormView, QueueForForm>().ReverseMap();

            CreateMap<QueuePermissionView, QueuePermission>().ReverseMap();

            CreateMap<Setting, SettingViewModel>().ReverseMap();


            CreateMap<ApplicationUser, UserViewModel>();

            CreateMap<UserViewModel, ApplicationUser>();
            CreateMap<PermissionGroup, PermissionGroupViewModel>();
            CreateMap<PermissionGroupViewModel, PermissionGroup>();
            CreateMap<AdminConfig, AdminConfigViewModel>();
            CreateMap<AdminConfigViewModel, AdminConfig>();
            CreateMap<UserMedia, UserMediaViewModel>();
            CreateMap<UserViewModel, CaseStateHistoryViewModel>();
            CreateMap<CaseStateHistory, CaseStateHistoryViewModel>()
                .ForMember(x => x.CurrentStateName, opt => opt.MapFrom(src => src.CurrentState.ActionName))
                .ForMember(x => x.PreviousStateName, opt => opt.MapFrom(src => src.PreviousState.ActionName));
            CreateMap<CaseStateHistoryViewModel, CaseStateHistory>();

            CreateMap<CaseStateHistoryViewModel, UserViewModel>();

            CreateMap<MediaViewModel, UserMediaViewModel>()
                .ForMember(x => x.MediaId, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserMediaViewModel, UserViewModel>();

            CreateMap<UserViewModel, UserMediaViewModel>()
                 .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<SkillSet, SkillSetViewModel>();
            CreateMap<SkillSetViewModel, SkillSet>();
            CreateMap<AuditLog, AuditLogViewModel>();
            CreateMap<AuditLogViewModel, AuditLog>();

            CreateMap<EmailGroup, EmailGroupViewModel>().ReverseMap();
            CreateMap<Emails, EmailsViewModel>().ReverseMap();
            CreateMap<ElementState, ElementStateViewModel>();
            CreateMap<ElementStateViewModel, ElementState>();
            CreateMap<GetApplicantsResponseViewModel, CiceroStandardResponse>()
             .ForMember(dest => dest.Datas, opt => opt.MapFrom(src => src.applicants))
             .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.applicants.Count > 0 ? true : false))
             .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.error != null ? src.error.message : "Success"));

            CreateMap<Applicant, CiceroStandardResponse>()
               .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
               .ForMember(dest => dest.Success, opt => opt.MapFrom(src => (src != null && src.error == null) ? true : false))
               .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.error != null ? String.Concat(src.error.message, src.error.fields.ToString()) : "Success"));
           

        }

    }
}
