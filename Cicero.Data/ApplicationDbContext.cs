using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Cicero.Data.Extensions;
using Cicero.Data.Entities;

namespace Cicero.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string> 
    {
        private string customTableName;
        public string CustomTableName => customTableName ?? "DefaultCustomTableName";
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=tcp:simpletransfer.database.windows.net,1433;Initial Catalog=simpletransfer;Persist Security Info=False;User ID=simpletransfer;Password=InsurTech2020!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", builder =>
                {
                    builder.EnableRetryOnFailure(10,TimeSpan.FromSeconds(10), null);
                });
            }

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Seed();

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            builder.Entity<ApplicationUser>().ToTable("User");

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            builder.Entity<ApplicationRole>().ToTable("Role");

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");

            builder.Entity<RolePermission>().HasOne(x => x.ApplicationRole).WithMany(a => a.RolePermissions).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ActivityLog>().HasOne(x => x.User).WithMany(b => b.ActivityLogs).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Message>().HasOne(x => x.Sender).WithMany(a => a.Messages).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<MessageUser>().HasOne(x => x.MessageForUser).WithMany(a => a.MessageUsers).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<MessageUser>().HasOne(x => x.UserForMessage).WithMany(a => a.UserMessages).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUser>().HasMany(z => z.ActivityLogs).WithOne(b => b.User).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<StateToState>().HasOne(x => x.FromState).WithMany(a => a.FromStates).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<StateToState>().HasOne(x => x.ToState).WithMany(a => a.ToStates).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<StateToState>().HasOne(x => x.ToState).WithMany(a => a.ToStates).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<QueueToState>().HasOne(x => x.State).WithMany(a => a.QueueToState).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<QueueToState>().HasOne(x => x.Queue).WithMany(a => a.QueueToState).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TenantUser>().HasOne(x => x.TenantForUser).WithMany(a => a.TenantUsers).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TenantUser>().HasOne(x => x.UserForTenant).WithMany(a => a.TenantUsers).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ArticleMedia>().HasOne(x => x.Media).WithMany(a => a.ArticleMedias).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserMedia>().HasOne(z => z.User).WithMany(b => b.UserMedias).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CaseMedia>().HasOne(x => x.Media).WithMany(a => a.CaseMedias).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Case>().HasOne(x => x.Tenant).WithMany(a => a.TenantCases).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CoreCaseTable>().HasOne(x => x.Tenant);
            builder.Entity<Article>().HasOne(x => x.Tenant).WithMany(a => a.TenantArticles).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CaseForm>().HasOne(b => b.Tenant).WithMany(c => c.TenantCaseClaims).OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<State>().HasOne(b => b.RoleForState).WithMany(c => c.RoleStates).OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<Queue>().HasOne(b => b.RoleForQueue).WithMany(c => c.RoleQueues).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ActionsReceiver>().HasOne(x => x.Actions).WithMany(a => a.ActionsReceiverLst).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ActionsSender>().HasOne(x => x.Actions).WithMany(a => a.ActionsSenderLst).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<StateToState>().HasOne(x => x.Actions).WithMany(a => a.StateToStatelst).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<UserSkillSet>()
           .HasKey(o => new { o.SkillSetId, o.UserId });
        }

        #region DbSets

        public virtual DbSet<ActivityLog> ActivityLog { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<ArticleMedia> ArticleMedia { get; set; }
        public virtual DbSet<Case> Case { get; set; }
        public virtual DbSet<CaseMedia> CaseMedia { get; set; }
        public virtual DbSet<CaseForm> CaseForm { get; set; }
        public virtual DbSet<CountryList> CountryList { get; set; }
        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<MessageUser> MessageUser { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PermissionGroup> PermissionGroup { get; set; }
        public virtual DbSet<PolicyManagement> PolicyManagement { get; set; }
        public virtual DbSet<Queue> Queue { get; set; }
        public virtual DbSet<QueueForForm> QueueForForm { get; set; }
        public virtual DbSet<QueuePermission> QueuePermission { get; set; }
        public virtual DbSet<QueueToState> QueueToState { get; set; }
        public virtual DbSet<Actions> Actions { get; set; }
        public virtual DbSet<ActionsSender> ActionsSender { get; set; }
        public virtual DbSet<ActionsReceiver> ActionsReceiver { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<StateForForm> StateForForm { get; set; }
        public virtual DbSet<StatePermission> StatePermission { get; set; }
        public virtual DbSet<StateToState> StateToState { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<TenantUser> TenantUser { get; set; }
        public virtual DbSet<UserMedia> UserMedia { get; set; }
        public virtual DbSet<CoreCaseTable> CoreCaseTable { get; set; }
        public virtual DbSet<AdminConfig> AdminConfig { get; set; }
        public virtual DbSet<Component> Component { get; set; }
        public virtual DbSet<Workflow> Workflow { get; set; }
        public virtual DbSet<WorkflowObject> WorkflowObject { get; set; }
        public virtual DbSet<WorkflowPoint> WorkflowPoint { get; set; }
        public virtual DbSet<WorkFlowState> WorkFlowState { get; set; }
        public virtual DbSet<MailMergeField> MailMergeField { get; set; }
        public virtual DbSet<MailMergeObject> MailMergeObject { get; set; }
        public virtual DbSet<UserMediaGroup> UserMediaGroup { get; set; }
        //public virtual DbSet<StateActions> StateActions { get; set; }
        public virtual DbSet<CaseStateHistory> CaseStateHistory { get; set; }
        public virtual DbSet<SkillSet> SkillSet { get; set; }
        public virtual DbSet<UserSkillSet> UserSkillSet { get; set; }
        public virtual DbSet<ElementWorkflow> ElementWorkflow { get; set; }
        public virtual DbSet<ElementWorkflowObject> ElementWorkflowObject { get; set; }
        public virtual DbSet<ElementWorkflowPoint> ElementWorkflowPoint { get; set; }
        public virtual DbSet<ElementComponent> ElementComponent { get; set; }
        public virtual DbSet<ElementWorkflowState> ElementWorkflowState { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }

        public virtual DbSet<Emails> Emails { get; set; }
        public virtual DbSet<EmailGroup> EmailGroup { get; set; }
        public virtual DbSet<ElementState> ElementState { get; set; }


        //score card
        public virtual DbSet<Scoreing> Scoreing { get; set; }
        public virtual DbSet<ProductConfig> ProductConfig { get; set; }

        #endregion

        #region DbQuery

        public DbQuery<CustomEntity> CustomEntities { get; set; }
        public DbQuery<CustomEntity1> CustomEntities1 { get; set; }
        public DbQuery<DBTables> CustomEntities2 { get; set; }
        public DbQuery<DBTableColumn> CustomEntities3 { get; set; }
        public DbQuery<DBTablesProperties> CustomEntities4 { get; set; }

        #endregion
    }
}
