using Cicero.Service.Services;
using Cicero.Service.Services.API;
using Cicero.Service.Services.JazzCash;
using Cicero.Service.Services.SimpleTransfer;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using static Cicero.Service.Services.ISynchronizeService;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<ICaseService, CaseService>();
            //services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IActivityLogService, ActivityLogService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IFormBuilderService, FormBuilderService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ICiceroCoreFormService, CiceroCoreFormService>();
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<ComponentService>();
            services.AddScoped<ISynchronizeService, SynchronizeService>();
            services.AddScoped<IAutomationService, AutomationService>();
            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICountryPayoutService, CountryPayoutService>();
            services.AddScoped<IRateSupplierService, RateSupplierService>();
            services.AddScoped<IAutoSchedulerSettingService, AutoSchedulerSettingService>();
            services.AddScoped<IExchangeRateSettingService, ExchangeRateSettingService>();
            services.AddScoped<IBankSettingService, BankSettingService>();
            services.AddScoped<IBankMapperService, BankMapperService>();
            services.AddScoped<IRelationshipSetupService, RelationshipSetupService>();
            services.AddScoped<IGenderSetupService, GenderSetupService>();
            services.AddScoped<IIdentificationTypeSetupService, IdentificationTypeSetupService>();
            services.AddScoped<IMaritalStatusSetupService, MaritalStatusSetupService>();
            services.AddScoped<IPaymentPurposeSetupService, PaymentPurposeSetupService>();
            services.AddScoped<IBranchMapperService, BranchMapperService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IBeneficiarySetupService, BeneficiarySetupService>();
            services.AddScoped<ITransactionLimitConfigService, TransactionLimitConfigService>();
            services.AddScoped<IRateSupplierFeeConfigService, RateSupplierFeeConfigService>();
            services.AddScoped<IListService, ListService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IOnfidoService, OnfidoService>();
            services.AddScoped<ITransactionMgmtService, TransactionMgmtService>();
            services.AddScoped<IJazzCashTransactionMgmtService, JazzCashTransactionMgmtService>();
            services.AddScoped<ICustomerUserService, CustomerUserService>();
            services.AddScoped<ISecureTradingService, SecureTradingService>();
            services.AddScoped<ISourceOfFundSetupService, SourceOfFundSetupService>();
            services.AddScoped<IPayOutModeService, PayOutModeService>();
            services.AddScoped<IFileUploaderService, FileUploaderService>();

            services.AddScoped<ISkillSetService, SkillSetService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IEmailGroupService, EmailGroupService>();
            services.AddScoped<IElementWorkflowService, ElementWorkflowService>();

            //Jazz Cash
            services.AddScoped<IPayeeService, PayeeService>();
            services.AddScoped<IPayerService, PayerService>();
            services.AddScoped<ISecurityQuestionService, SecurityQuestionService>();
            services.AddScoped<IRecipientService, RecipientService>(); 
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IBankBranchServcie, BankBranchServcie>();
            services.AddScoped<IExchangeRateServices, ExchangeRateServices>();
            services.AddScoped<IMapperService, MapperService>();
            services.AddScoped<ISimpleTransferService, SimpleTransferService>();
            services.AddScoped<IPaymentRequestService, PaymentRequestService>();
            services.AddScoped<ILexisNexisService, LexisNexisService>();
            return services;
        }
    }
}
