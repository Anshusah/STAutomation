using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Cicero.Data.Extensions;
using Cicero.Data.Entities;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Data.Entities.JazzCash;

namespace Cicero.Data
{
    public class SimpleTransferApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private string customTableName;
        public string CustomTableName => customTableName ?? "DefaultCustomTableName";
        public SimpleTransferApplicationDbContext(DbContextOptions<SimpleTransferApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=tcp:simpletransfer.database.windows.net,1433;Initial Catalog=simpletransfer;Persist Security Info=False;User ID=simpletransfer;Password=InsurTech2020!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", builder =>
                {
                    builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), null);
                });
            }

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.SeedSimpleTransfer();

        }

        #region DbSets

        public virtual DbSet<ApiUserToken> ApiUserToken { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<RateSupplier> RateSupplier { get; set; }
        public virtual DbSet<ExchangeRates> ExchangeRates { get; set; }
        public virtual DbSet<ExchangeRatesHistory> ExchangeRatesHistory { get; set; }
        public virtual DbSet<AutoSchedulerSetting> AutoSchedulerSetting { get; set; }
        public virtual DbSet<Entities.SimpleTransfer.CountryList> CountryList { get; set; }
        public virtual DbSet<SupplierBank> SupplierBank { get; set; }
        public virtual DbSet<SupplierBankBranch> SupplierBankBranch { get; set; }
        public virtual DbSet<SupplierCity> SupplierCity { get; set; }
        public virtual DbSet<CountryPayoutConfig> CountryPayoutConfig { get; set; }
        public virtual DbSet<SupplierBankMap> SupplierBankMap { get; set; }
        public virtual DbSet<Beneficiary> Beneficiary { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionHistory> TransactionHistory { get; set; }
        public virtual DbSet<PaymentPurpose> PaymentPurpose { get; set; }
        public virtual DbSet<BeneficiaryRelationship> BeneficiaryRelationship { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<MaritalStatus> MaritalStatus { get; set; }
        public virtual DbSet<IdentificationType> IdentificationType { get; set; }
        public virtual DbSet<RateSupplierFeeConfig> RateSupplierFeeConfig { get; set; }
        public virtual DbSet<TransactionLimitConfig> TransactionLimitConfig { get; set; }
        public virtual DbSet<PayoutModeConfig> PayoutModeConfig { get; set; }

        public virtual DbSet<SmsCodeCustomerRegistraion> SmsCodeCustomerRegistraion { get; set; }
        public virtual DbSet<SmsLog> SmsLog { get; set; }
        public virtual DbSet<SmsCodeCustomerRegistraionLog> SmsCodeCustomerRegistraionLog { get; set; }
        public virtual DbSet<OnfidoApplicant> OnfidoApplicant { get; set; }
        public virtual DbSet<OnfidoApplicantDocument> OnfidoApplicantDocument { get; set; }
        public virtual DbSet<OnfidoApplicantLivePhoto> OnfidoApplicantLivePhoto { get; set; }
        public virtual DbSet<OnfidoChecks> OnfidoCheck { get; set; }
        public virtual DbSet<SourceOfFund> SourceOfFund { get; set; }
        public virtual DbSet<CustomerCardDetail> CustomerCardDetail { get; set; }
        public virtual DbSet<SecureTradingPaymentDetail> SecureTradingPaymentDetail { get; set; }
        public virtual DbSet<TransfastSourceOfFund> TransfastSourceOfFund { get; set; }
        public virtual DbSet<TransfastRemittancePurpose> TransfastRemittancePurpose { get; set; }


        //Jazz Cash
        public virtual DbSet<Payee> Payee { get; set; }
        public virtual DbSet<Payer> Payer { get; set; }
        public virtual DbSet<SecurityQuestion> SecurityQuestion { get; set; }
        public virtual DbSet<PaymentRequest> PaymentRequest { get; set; }
        public virtual DbSet<PaymentApiPartner> PaymentApiPartner { get; set; }
        public virtual DbSet<JazzCashTransaction> JazzCashTransaction { get; set; }
        public virtual DbSet<JazzCashTransactionHistory> JazzCashTransactionHistory { get; set; }
        // public virtual DbSet<NotificationHistory> NotificationHistory { get; set; }
        public virtual DbSet<STPaymentRequest> STPaymentRequest { get; set; }
        public virtual DbSet<STPaymentRequestDetails> STPaymentRequestDetails { get; set; }
        public virtual DbSet<UatSetting> UatSetting { get; set; }

        //Lexis Nexis
        public virtual DbSet<LexisNexis> LexisNexis { get; set; }
        public virtual DbSet<SanctionPepCustomer> SanctionPepCustomer { get; set; }
        public virtual DbSet<SanctionPepBeneficiary> SanctionPepBeneficiary { get; set; }
        public virtual DbSet<SanctionPepPerson> SanctionPepPerson { get; set; }

        #endregion

        #region DbQuery
        #endregion
    }
}
