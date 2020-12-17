using AutoMapper;
using Cicero.Data.Entities;
using Cicero.Data.Entities.JazzCash;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Models.API.LexisNexis;
using Cicero.Service.Models.JazzCash;
using Cicero.Service.Models.PaymentRequest;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer.BankSetting;
using Cicero.Service.Models.SimpleTransfer.Beneficiary;
using Cicero.Service.Models.SimpleTransfer.City;
using Cicero.Service.Models.SimpleTransfer.Country;
using Cicero.Service.Models.SimpleTransfer.CountryPayout;
using Cicero.Service.Models.SimpleTransfer.Onfido;
using Cicero.Service.Models.SimpleTransfer.RateSupplier;
using Cicero.Service.Models.SimpleTransfer.RelationshipSetup;
using Cicero.Service.Models.SimpleTransfer.TransactionLimitConfig;
using Cicero.Service.Models.SimpleTransfer.User;
using System;
using System.Collections.Generic;
using System.Text;
using Cicero;

namespace Cicero.Service
{
    public class SimpleTransferModelProfile : Profile
    {
        public SimpleTransferModelProfile()
        {
            CreateMap<RateSupplier, RateSupplierViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedAt)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedAt)));

            CreateMap<RateSupplierViewModel, RateSupplier>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedAt)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedAt)));

            CreateMap<SupplierBank, BankSettingViewModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedAt)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedAt)));

            CreateMap<BankSettingViewModel, SupplierBank>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedAt)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedAt)));

            CreateMap<CountryPayoutConfig, CountryPayoutViewModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)))
                 .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<CountryPayoutViewModel, CountryPayoutConfig>()
                  .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)))
                   .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Data.Entities.SimpleTransfer.CountryList, CountryViewModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<CountryViewModel, Data.Entities.SimpleTransfer.CountryList>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<SupplierCity, CityViewModel>()
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedAt)))
               .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedAt)))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<CityViewModel, SupplierCity>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedAt)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedAt)));
            CreateMap<Gender, GenderSetupViewModel>()
              .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
              .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<GenderSetupViewModel, Gender>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));
            CreateMap<PaymentPurpose, PaymentPurposeSetupViewModel>()
             .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
             .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<PaymentPurposeSetupViewModel, PaymentPurpose>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));
            CreateMap<IdentificationType, IdentificationTypeSetupViewModel>()
                         .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                         .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<IdentificationTypeSetupViewModel, IdentificationType>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));
            CreateMap<MaritalStatus, MaritalStatusSetupViewModel>()
                         .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                         .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<MaritalStatusSetupViewModel, MaritalStatus>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));
            CreateMap<BeneficiaryRelationship, RelationshipSetupViewModel>()
                         .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                         .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<RelationshipSetupViewModel, BeneficiaryRelationship>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<Beneficiary, BeneficiarySetupViewModel>()
                        .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                        .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<BeneficiarySetupViewModel, Beneficiary>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));
            CreateMap<RateSupplierFeeConfig, RateSupplierFeeConfigViewModel>()
                       .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                       .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<RateSupplierFeeConfigViewModel, RateSupplierFeeConfig>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<TransactionLimitConfig, TransactionLimitConfigViewModel>()
                       .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                       .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<TransactionLimitConfigViewModel, TransactionLimitConfig>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<Customer, SenderDetailViewModel>()
                     .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryCode))
                     .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
                     .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<SenderDetailViewModel, Customer>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<OnfidoApplicant, OnfidoApplicantViewModel>()
                  .ForMember(dest => dest.created_at, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.created_at)))
                  .ForMember(dest => dest.updated_at, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.updated_at)));

            CreateMap<OnfidoApplicantViewModel, OnfidoApplicant>()
                .ForMember(dest => dest.created_at, opt => opt.MapFrom(src => Convert.ToDateTime(src.created_at)))
                .ForMember(dest => dest.updated_at, opt => opt.MapFrom(src => Convert.ToDateTime(src.updated_at)));

            CreateMap<OnfidoChecks, OnfidoChecksViewModel>()
               .ForMember(dest => dest.created_at, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.created_at)))
               .ForMember(dest => dest.updated_at, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.updated_at)));

            CreateMap<OnfidoChecksViewModel, OnfidoChecks>()
                .ForMember(dest => dest.created_at, opt => opt.MapFrom(src => Convert.ToDateTime(src.created_at)))
                .ForMember(dest => dest.updated_at, opt => opt.MapFrom(src => Convert.ToDateTime(src.updated_at)));

            CreateMap<Transaction, TransactionMgmtViewModel>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
               .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<TransactionMgmtViewModel, Transaction>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<TransactionHistory, TransactionHistoryViewModel>()
              .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
              .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<TransactionHistoryViewModel, TransactionHistory>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<Beneficiary, RecipientViewModel>()
             .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
             .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<RecipientViewModel, Beneficiary>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));
            CreateMap<SupplierBankBranch, TransfastBankBranchModel>()
            .ForMember(dest => dest.BankBranchID, opt => opt.MapFrom(src => src.BranchCode))
            .ForMember(dest => dest.BankBranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankCode))
            .ForMember(dest => dest.CityCode, opt => opt.MapFrom(src => src.CityCode));
            CreateMap<TransfastBankModel, TransfastRemittancePurpose>();
            CreateMap<TransfastRemittancePurpose, TransfastBankModel>();
            CreateMap<TransfastSourceOfFund, TransfastBankModel>();
            CreateMap<TransfastBankModel, TransfastSourceOfFund>();
            CreateMap<SourceOfFund, SourceOfFundSetupViewModel>();
            CreateMap<SourceOfFundSetupViewModel, SourceOfFund>();


            CreateMap<PaymentRequest, PaymentRequestModel>();
            // .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
            // .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<PaymentRequestModel, PaymentRequest>();
            // .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
            //   .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));


            CreateMap<PaymentRequest, JazzCashPaymentRequestViewModel>()
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
          .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<PaymentRequestModel, JazzCashPaymentRequestViewModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<STPaymentRequest, PaymentRequestViewModel>()
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
          .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<PaymentRequestViewModel, STPaymentRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)(src.Status)))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));


            CreateMap<STPaymentRequestDetails, STPaymentRequestDetailsViewModel>()
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
          .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<STPaymentRequestDetailsViewModel, STPaymentRequestDetails>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<JazzCashTransaction, JazzCashTransactionMgmtViewModel>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<JazzCashTransactionMgmtViewModel, JazzCashTransaction>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<JazzCashTransactionHistory, JazzCashTransactionHistoryViewModel>()
              .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
              .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<JazzCashTransactionHistoryViewModel, JazzCashTransactionHistory>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));
            CreateMap<UatSettingViewModel, UatSetting>();
            CreateMap<UatSetting, UatSettingViewModel>();

            #region Lexis Nexis

            CreateMap<LexisNexis, LexixNexisViewModel>()
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<LexixNexisViewModel, LexisNexis>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
               .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<SanctionPepCustomer, SanctionPepCustomerViewModel>()
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
          .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<SanctionPepCustomerViewModel, SanctionPepCustomer>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
               .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<SanctionPepBeneficiary, SanctionPepBeneficiaryViewModel>()
        .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
        .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<SanctionPepBeneficiaryViewModel, SanctionPepBeneficiary>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
               .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            CreateMap<SanctionPepPerson, SanctionPepPersonViewModel>()
 .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.CreatedDate)))
 .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Utils.GetDefaultDateFormatToDetail(src.UpdatedDate)));

            CreateMap<SanctionPepPersonViewModel, SanctionPepPerson>()
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.CreatedDate)))
               .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.UpdatedDate)));

            #endregion
        }
    }
}
