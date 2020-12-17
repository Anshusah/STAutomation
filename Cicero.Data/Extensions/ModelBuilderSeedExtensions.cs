using System;
using Microsoft.EntityFrameworkCore;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cicero.Data.Extensions
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public static class ModelBuilderSeedExtensions
    {

        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Setting>().HasData(
                new Setting
                {
                    Id = 1,
                    FieldKey = "app_name",
                    FieldValue = "",
                    FieldDisplay = "Name",
                    FieldVisiblity = 1,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 2,
                    FieldKey = "app_name_frontend",
                    FieldValue = "",
                    FieldDisplay = "Front Title",
                    FieldVisiblity = 1,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 3,
                    FieldKey = "app_phone",
                    FieldValue = "",
                    FieldDisplay = "Phone",
                    FieldVisiblity = 1,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 4,
                    FieldKey = "app_email",
                    FieldValue = "",
                    FieldDisplay = "Email",
                    FieldVisiblity = 1,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },

                new Setting
                {
                    Id = 5,
                    FieldKey = "app_address",
                    FieldValue = "",
                    FieldDisplay = "Address",
                    FieldVisiblity = 1,
                    FieldType = "TEXTAREA",
                    FieldOptions = null,
                    FieldGridSize = "12"
                },
                new Setting
                {
                    Id = 6,
                    FieldKey = "app_claim_front",
                    FieldValue = "",
                    FieldDisplay = "Starting Claim for Claimant",
                    FieldVisiblity = 1,
                    FieldType = "TENANTCLAIM",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 7,
                    FieldKey = "app_claim_back",
                    FieldValue = "",
                    FieldDisplay = "Starting Claim for Back office",
                    FieldVisiblity = 1,
                    FieldType = "TENANTCLAIM",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 8,
                    FieldKey = "app_user_role",
                    FieldValue = "",
                    FieldDisplay = "Role for register User in front office",
                    FieldVisiblity = 1,
                    FieldType = "USERROLE",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 9,
                    FieldKey = "app_url",
                    FieldValue = "http://52.228.24.65/",
                    FieldDisplay = "Url",
                    FieldVisiblity = 1,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 10,
                    FieldKey = "app_facebook",
                    FieldValue = "http://facebook.com",
                    FieldDisplay = "Facebook Url",
                    FieldVisiblity = 1,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 11,
                    FieldKey = "app_twitter",
                    FieldValue = "http://twitter.com",
                    FieldDisplay = "Twitter Url",
                    FieldVisiblity = 1,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 12,
                    FieldKey = "Primary",
                    FieldValue = "[{\"index\":0,\"menu\":\"Home\",\"type\":\"custom\",\"url\":\"/\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]}]",
                    FieldDisplay = "Navigation - Primary",
                    FieldVisiblity = 0,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = null
                },
                new Setting
                {
                    Id = 13,
                    FieldKey = "Bottom",
                    FieldValue = "[{\"index\":0,\"menu\":\"About Cicero\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Claim Process\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":3,\"menu\":\"Blogs\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":4,\"menu\":\"Careers\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":1,\"menu\":\"Legals\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Terms and Conditions\",\"type\":\"article\",\"url\":\"1\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Terms and Conditions\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":2,\"menu\":\"Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Contact Us\",\"type\":\"article\",\"url\":\"18\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Contact Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Feedback\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Help & Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]}]",
                    FieldDisplay = "Navigation - Bottom",
                    FieldVisiblity = 0,
                    FieldType = "TEXTBOX",
                    FieldOptions = null,
                    FieldGridSize = null
                },
                new Setting
                {
                    Id = 14,
                    FieldKey = "app_themes",
                    FieldValue = "[{\"index\":0,\"menu\":\"About Cicero\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Claim Process\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":3,\"menu\":\"Blogs\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":4,\"menu\":\"Careers\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":1,\"menu\":\"Legals\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Terms and Conditions\",\"type\":\"article\",\"url\":\"1\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Terms and Conditions\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":2,\"menu\":\"Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Contact Us\",\"type\":\"article\",\"url\":\"18\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Contact Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Feedback\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Help & Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]}]",
                    FieldDisplay = null,
                    FieldVisiblity = 0,
                    FieldType = null,
                    FieldOptions = null,
                    FieldGridSize = null
                },
                new Setting
                {
                    Id = 15,
                    FieldKey = "app_theme",
                    FieldValue = "Test",
                    FieldDisplay = "Theme",
                    FieldType = "TENANTTHEME",
                    FieldVisiblity = 1,
                    FieldOptions = null,
                    FieldGridSize = "6"
                },
                new Setting
                {
                    Id = 16,
                    FieldKey = "app_case_synchronization",
                    FieldValue = null,
                    FieldDisplay = "Sync Case",
                    FieldType = "CASESYNCHRONIZATION",
                    FieldVisiblity = 1,
                    FieldOptions = null,
                    FieldGridSize = "12"
                }
            );
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "df0d5fc1-b3c9-448f-afea-a43cd08005a6",
                    FirstName = "Kishan",
                    LastName = "Sharma",
                    UserName = "kishan@vesuviois.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEO5lquYUWfSRcDWe+O4Vd+0S95cXnuUyUh89qzwVmFKR8/UXTicXZ03+SEBqdjNtwg==",
                    Email = "kishan@vesuviois.com",
                    NormalizedEmail = "KISHAN@VESUVIOIS.COM",
                    NormalizedUserName = "KISHAN@VESUVIOIS.COM",
                    SecurityStamp = "TN2MA6753JLC5JGLUXTIJJA42QTYAQXZ",
                    ConcurrencyStamp = "978d05ed-67c6-4da6-aa8f-f1cf64a3a972",
                    EmailConfirmed = true,
                    AccessFailedCount = 0,
                    Status = true,
                    PhoneNumber = "9851189079",
                    PhoneNumberConfirmed = false,
                    Address = "london",
                    CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"),
                    CreatedBy = "",
                    DisplayName = "Kishan Kishan",
                    LockoutEnabled = true,
                    LockoutEnd = null,
                    TwoFactorEnabled = false,
                    UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"),
                    UserId = "authorize",
                    IsSuperAdmin = true
                }
            );

            builder.Entity<IdentityUserClaim<string>>().HasData(
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = "df0d5fc1-b3c9-448f-afea-a43cd08005a6",
                    ClaimType = "access",
                    ClaimValue = "sa"
                }
            );

            builder.Entity<Media>().HasData(
                 new Media
                 {
                     Id = 1,
                     Title = "Test Image 1",
                     Description = "Test Image 1",
                     Url = "default-mobile.jpg"
                 },
                new Media
                {
                    Id = 2,
                    Title = "Test Image 2",
                    Description = "Test Image 2",
                    Url = "default-lawyer.jpg"
                },
                new Media
                {
                    Id = 3,
                    Title = "Test Image 3",
                    Description = "Test Image 3",
                    Url = "default-smile.jpg"
                },
                new Media
                {
                    Id = 4,
                    Title = "Test Image 4",
                    Description = "Test Image 4",
                    Url = "default-group.jpg"
                },
                new Media
                {
                    Id = 5,
                    Title = "Test Image 5",
                    Description = "Test Image 5",
                    Url = "default-banner-image.jpg"
                },
                new Media
                {
                    Id = 6,
                    Title = "Test Image 5",
                    Description = "Test Image 5",
                    Url = "default-contact-image.jpg"
                }

            );
            builder.Entity<ArticleMedia>().HasData(
                new ArticleMedia
                {
                    Id = 1,
                    MediaId = 1,
                    ArticleId = 10
                },
               new ArticleMedia
               {
                   Id = 2,
                   MediaId = 2,
                   ArticleId = 11
               },
               new ArticleMedia
               {
                   Id = 3,
                   MediaId = 3,
                   ArticleId = 12
               },
               new ArticleMedia
               {
                   Id = 4,
                   MediaId = 4,
                   ArticleId = 13
               },
                new ArticleMedia
                {
                    Id = 5,
                    MediaId = 5,
                    ArticleId = 17
                },
                new ArticleMedia
                {
                    Id = 6,
                    MediaId = 6,
                    ArticleId = 18
                }

           );

            builder.Entity<Permission>().HasData(               
                new Permission
                {
                    Id = 5,
                    Name = "View User"
                },
                new Permission
                {
                    Id = 6,
                    Name = "Create User"
                },
                new Permission
                {
                    Id = 7,
                    Name = "Update User"
                },
                new Permission
                {
                    Id = 8,
                    Name = "Delete User"
                },
                new Permission
                {
                    Id = 9,
                    Name = "Edit Own Profile"
                },
                new Permission
                {
                    Id = 10,
                    Name = "View Article"
                },
                new Permission
                {
                    Id = 11,
                    Name = "Create Article"
                },
                new Permission
                {
                    Id = 12,
                    Name = "Update Article"
                },
                new Permission
                {
                    Id = 13,
                    Name = "Delete Article"
                },
                new Permission
                {
                    Id = 14,
                    Name = "View Media"
                },
                new Permission
                {
                    Id = 15,
                    Name = "Create Media"
                },
                new Permission
                {
                    Id = 16,
                    Name = "Update Media"
                },
                new Permission
                {
                    Id = 17,
                    Name = "Delete Media"
                },
                new Permission
                {
                    Id = 18,
                    Name = "View Menu"
                },
                new Permission
                {
                    Id = 19,
                    Name = "Create Menu"
                },
                new Permission
                {
                    Id = 20,
                    Name = "Update Menu"
                },
                new Permission
                {
                    Id = 21,
                    Name = "Delete Menu"
                },
                new Permission
                {
                    Id = 22,
                    Name = "View Role"
                },
                new Permission
                {
                    Id = 23,
                    Name = "Create Role"
                },
                new Permission
                {
                    Id = 24,
                    Name = "Update Role"
                },
                new Permission
                {
                    Id = 25,
                    Name = "Delete Role"
                },
                new Permission
                {
                    Id = 26,
                    Name = "View Setting"
                },
                new Permission
                {
                    Id = 27,
                    Name = "Create Setting"
                },
                new Permission
                {
                    Id = 28,
                    Name = "Update Setting"
                },
                new Permission
                {
                    Id = 29,
                    Name = "Delete Setting"
                },
                new Permission
                {
                    Id = 30,
                    Name = "View Message"
                },
                new Permission
                {
                    Id = 31,
                    Name = "Compose Message"
                },
                new Permission
                {
                    Id = 32,
                    Name = "Reply Message"
                },
                new Permission
                {
                    Id = 33,
                    Name = "Reply All Message"
                },
                new Permission
                {
                    Id = 34,
                    Name = "View Queue"
                },
                new Permission
                {
                    Id = 35,
                    Name = "Create Queue"
                },
                new Permission
                {
                    Id = 36,
                    Name = "Update Queue"
                },
                new Permission
                {
                    Id = 37,
                    Name = "Delete Queue"
                },
                new Permission
                {
                    Id = 38,
                    Name = "Create Form"
                },
                new Permission
                {
                    Id = 39,
                    Name = "Update Form"
                },
                new Permission
                {
                    Id = 40,
                    Name = "Delete Form"
                },
                new Permission
                {
                    Id = 41,
                    Name = "View Form"
                },
                new Permission
                {
                    Id = 42,
                    Name = "View Tenants"
                },
                new Permission
                {
                    Id = 43,
                    Name = "Admin Layout"
                },
                new Permission
                {
                    Id = 44,
                    Name = "Worker Layout"
                },
                new Permission
                {
                    Id=45,
                    Name="View"
                },
                new Permission
                {
                    Id=46,
                    Name="Create",
                },
                new Permission
                {
                    Id=47,
                    Name="Update",
                },
                new Permission
                {
                    Id=48,
                    Name="Delete"
                },
                new Permission
                {
                    Id=49,
                    Name="View Tools"
                },
                new Permission
                {
                    Id=50,
                    Name="Edit Tools"
                }               
            );
            builder.Entity<MailMergeField>().HasData(
                new MailMergeField
                {
                    Id = 1,
                    FieldName = "First Name",
                    Alias = "[First_Name]",
                    TemplateType = 1,
                    DbSourceField = "FirstName",
                    DbSourceTable = "User",
                    TenantId = 1,
                    FormId =0,
                    isDeleted=false
                },
                new MailMergeField {
                    Id = 2,
                    FieldName = "Last Name",
                    Alias = "[Last_Name]",
                    TemplateType = 1,
                    DbSourceTable = "User",
                    DbSourceField = "LastName",
                    TenantId = 1,
                    FormId = 0,
                    isDeleted = false
                },
                new MailMergeField {
                    Id = 3,
                    FieldName = "Email",
                    Alias = "[Email]",
                    TemplateType = 1,
                    DbSourceTable = "User",
                    DbSourceField = "Email",
                    TenantId = 1,
                    FormId = 0,
                    isDeleted = false
                },
                new MailMergeField {
                    Id = 4,
                    FieldName = "Updated Date",
                    Alias = "[Updated_Date]",
                    TemplateType = 1,
                    DbSourceTable = "User",
                    DbSourceField = "UpdatedAt",
                    TenantId = 1,
                    FormId = 0,
                    isDeleted = false
                },
                new MailMergeField
                {
                    Id = 5,
                    FieldName = "Address",
                    Alias = "[Address]",
                    TemplateType = 1,
                    DbSourceTable = "User",
                    DbSourceField = "Address",
                    TenantId = 1,
                    FormId = 0,
                    isDeleted = false
                },
                new MailMergeField {
                    Id = 6,
                    FieldName = "Phone Number",
                    Alias = "[Phone_Number]",
                    TemplateType = 1,
                    DbSourceTable = "User",
                    DbSourceField = "PhoneNumber",
                    TenantId = 1,
                    FormId = 0,
                    isDeleted = false
                },
                new MailMergeField {
                    Id = 7,
                    FieldName = "Role Name",
                    Alias = "[Role_Name]",
                    TemplateType = 1,
                    DbSourceField = "DisplayName",
                    DbSourceTable = "Role",
                    TenantId = 1,
                    FormId = 0,
                    isDeleted = false
                },
                new MailMergeField
                {
                    Id=8,
                    FieldName = "Form Name",
                    Alias = "[Form_Name]",
                    TemplateType = 2,
                    DbSourceTable = "CaseForm",
                    DbSourceField = "Name",
                    TenantId = 1,
                    FormId = 0,
                    isDeleted = false
                });
            builder.Entity<PermissionGroup>().HasData(
                new PermissionGroup
                {
                    Id = 1,
                    Name = "Claim",
                    PermissionIds = "1,2,3,4"
                },
                new PermissionGroup
                {
                    Id = 2,
                    Name = "User",
                    PermissionIds = "5,6,7,8,9"
                },
                new PermissionGroup
                {
                    Id = 3,
                    Name = "Article",
                    PermissionIds = "10,11,12,13"
                },
                new PermissionGroup
                {
                    Id = 4,
                    Name = "Media",
                    PermissionIds = "14,15,16,17"
                },
                new PermissionGroup
                {
                    Id = 5,
                    Name = "Menu",
                    PermissionIds = "18,19,20,21"
                },
                new PermissionGroup
                {
                    Id = 6,
                    Name = "Role",
                    PermissionIds = "22,23,24,25"
                },
                new PermissionGroup
                {
                    Id = 7,
                    Name = "Setting",
                    PermissionIds = "26,27,28,29"
                },
                new PermissionGroup
                {
                    Id = 8,
                    Name = "Message",
                    PermissionIds = "30,31,32,33"
                },
                new PermissionGroup
                {
                    Id = 9,
                    Name = "Queue",
                    PermissionIds = "34,35,36,37"
                },
                new PermissionGroup
                {
                    Id = 10,
                    Name = "Form",
                    PermissionIds = "38,39,40,41"
                },
                new PermissionGroup
                {
                    Id = 11,
                    Name = "Tenant",
                    PermissionIds = "42"
                },
                new PermissionGroup
                {
                    Id = 12,
                    Name = "Dashboard Layout",
                    PermissionIds = "43,44"
                },
                new PermissionGroup
                {
                    Id=13,
                    Name = "Tools",
                    PermissionIds = "49,50"
                }
                );

            builder.Entity<Article>().HasData(
                new Article { Id = 1, Title = "Terms and Conditions", Content = "This is Terms and Condition Page", Slug = "terms-and-conditions", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 0 },
                new Article { Id = 2, Title = "About Us", Content = "This is About Us Page", Slug = "about-us", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 0 },
                new Article { Id = 3, Title = "Privacy Policy", Content = "This is Privacy Policy Page", Slug = "privacy-policy", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 0 },
                new Article { Id = 4, Title = "Footer Block - About Us", Excerpt = "Nam non ante quam. Mauris posuere nisl ac vehicula imperdiet. Vivamus at tellus a velit scelerisque aliquam vel vitae erat. Morbi vitae eleifend arcu, et rhoncus lacus.", Content = "Nam non ante quam. Mauris posuere nisl ac vehicula imperdiet. Vivamus at tellus a velit scelerisque aliquam vel vitae erat.Morbi vitae eleifend arcu, et rhoncus lacus.", Slug = "footer-block-about-us", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 0 },
                new Article { Id = 5, Title = "Get started with these simple steps", Content = "Get started with these simple steps", Slug = "get-started", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 0 },
                new Article { Id = 6, Title = "Fill out Questionaries", Excerpt = "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", Content = "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", Slug = "fill-out-questionaries", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 5 },
                new Article { Id = 7, Title = "Provide Evidence", Excerpt = "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", Content = "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", Slug = "provide-evidence", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 5 },
                new Article { Id = 8, Title = "Send", Excerpt = "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", Content = "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", Slug = "send", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 5 },
                new Article { Id = 9, Title = "Guidelines", Content = "", Slug = "guidelines", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 0 },
                new Article { Id = 10, Title = "Guidelines - Track you claim", Excerpt = "Track you claim process from anywhere", Content = "Track you claim process from anywhere", Slug = "track-you-claim-process", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 9 },
                new Article { Id = 11, Title = "Guidelines - Know about our process", Excerpt = "Know about our process for better understanding Cicero", Content = "Know about our process for better understanding Cicero", Slug = "know-about-our-process", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 9 },
                new Article { Id = 12, Title = "Guidelines - How to provide evidence", Excerpt = "How to provide evidence more genuinely", Content = "How to provide evidence more genuinely", Slug = "how-to-provide-evidence-more-genuinely", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 9 },
                new Article { Id = 13, Title = "Guidelines - Know about our process", Excerpt = "Know about our process for better understanding Cicero", Content = "Know about our process for better understanding Cicero", Slug = "know-about-our-understanding", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 9 },
                new Article { Id = 14, Title = "Guidelines - Some quick example text", Excerpt = "Some quick example text to build on the card title and make up the bulk of the card's content.", Content = "Some quick example text to build on the card title and make up the bulk of the card's content.", Slug = "some-quick-example-text", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 9 },
                new Article { Id = 15, Title = "Guidelines - How to provide evidence more genuinely", Excerpt = "How to provide evidence more genuinely", Content = "How to provide evidence more genuinely", Slug = "how-to-provide-evidence", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 9 },
                new Article { Id = 16, Title = "Having problems?", Excerpt = "Did you find problems with your goods after shipment, don't worry we are here for you.", Content = "Did you find problems with your goods after shipment, don't worry we are here for you.", Slug = "having-problems", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 0 },
                new Article { Id = 17, Title = "Home Banner", Excerpt = "", Content = "<h3 class=\"display-4 main-promo-caption heading mt-4\">Relax during your shipment</h3><p class=\"h5 mt-3 main-promo-caption subheading mx-auto mb-5\">We are here to claim for you if your goods are damaged, stolen or anyting happens to your goods</p><p class=\"mx-auto\"><a class=\"btn btn-primary btn-lg btn-rounded px-5 mr-3 mb-3 shadow-sm\" href=\"/user/claim/0/edit.html\" role=\"button\">Claim here</a><a class=\"btn btn-outline-default btn-lg btn-rounded px-5 mb-3 shadow-sm\" href=\"#\" role=\"button\">Learn more</a></p>", Slug = "home-banner", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Default", Version = 1, ParentId = 0 },
                new Article { Id = 18, Title = "Contact Us", Excerpt = "", Content = "<strong>Cicero</strong><br>GW56 + 5W City of London, London, UK <br>London SE1 9DD, UK<br><strong title = \"Phone\"> P:</strong> (123) 456 - 7890 <br><strong title=\"Email\"> E:</strong> <a href=\"#\"> info@Cicero.com </a>", Slug = "contact-us", Status = 1, CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Template = "Contact", Version = 1, ParentId = 0 },
                new Article { Id = 19, Title = "forgot-password-email", Type = "template", Content = "<p>Hi, [user_name]!</p><h3> Please reset your password </h3><p> Click here to reset you password[reset_link]</p><p><br/> Regards,<br/> Cicero Team </p>", CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Version = 1 },
                new Article { Id = 20, Title = "claim-email-notification", Type = "template", Content = "<p>Hi, [user_name]!</p><br/> Regards,<br/> Cicero Team </p>", CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Version = 1 },
                new Article { Id = 21, Title = "claim-filed-against-email", Type = "template", Content = "<p>Hi, [user_name]!</p><br/> Regards,<br/> Cicero Team </p>", CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Version = 1 },
                new Article { Id = 22, Title = "subrogation_letter", Type = "template", Content = "<p>Hi, [user_name]!</p><br/> Regards,<br/> Cicero Team </p>", CreatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), UpdatedAt = Convert.ToDateTime("2018-09-05 16:20:30"), Version = 1 }
                );

            builder.Entity<CountryList>().HasData(
                new CountryList { Id = 1, Code = "AF", Name = "Afghanistan" },
                new CountryList { Id = 2, Code = "AL", Name = "Albania" },
                new CountryList { Id = 3, Code = "DZ", Name = "Algeria" },
                new CountryList { Id = 4, Code = "DS", Name = "American Samoa" },
                new CountryList { Id = 5, Code = "AD", Name = "Andorra" },
                new CountryList { Id = 6, Code = "AO", Name = "Angola" },
                new CountryList { Id = 7, Code = "AI", Name = "Anguilla" },
                new CountryList { Id = 8, Code = "AQ", Name = "Antarctica" },
                new CountryList { Id = 9, Code = "AG", Name = "Antigua and Barbuda" },
                new CountryList { Id = 10, Code = "AR", Name = "Argentina" },
                new CountryList { Id = 11, Code = "AM", Name = "Armenia" },
                new CountryList { Id = 12, Code = "AW", Name = "Aruba" },
                new CountryList { Id = 13, Code = "AU", Name = "Australia" },
                new CountryList { Id = 14, Code = "AT", Name = "Austria" },
                new CountryList { Id = 15, Code = "AZ", Name = "Azerbaijan" },
                new CountryList { Id = 16, Code = "BS", Name = "Bahamas" },
                new CountryList { Id = 17, Code = "BH", Name = "Bahrain" },
                new CountryList { Id = 18, Code = "BD", Name = "Bangladesh" },
                new CountryList { Id = 19, Code = "BB", Name = "Barbados" },
                new CountryList { Id = 20, Code = "BY", Name = "Belarus" },
                new CountryList { Id = 21, Code = "BE", Name = "Belgium" },
                new CountryList { Id = 22, Code = "BZ", Name = "Belize" },
                new CountryList { Id = 23, Code = "BJ", Name = "Benin" },
                new CountryList { Id = 24, Code = "BM", Name = "Bermuda" },
                new CountryList { Id = 25, Code = "BT", Name = "Bhutan" },
                new CountryList { Id = 26, Code = "BO", Name = "Bolivia" },
                new CountryList { Id = 27, Code = "BA", Name = "Bosnia and Herzegovina" },
                new CountryList { Id = 28, Code = "BW", Name = "Botswana" },
                new CountryList { Id = 29, Code = "BV", Name = "Bouvet Island" },
                new CountryList { Id = 30, Code = "BR", Name = "Brazil" },
                new CountryList { Id = 31, Code = "IO", Name = "British Indian Ocean Territory" },
                new CountryList { Id = 32, Code = "BN", Name = "Brunei Darussalam" },
                new CountryList { Id = 33, Code = "BG", Name = "Bulgaria" },
                new CountryList { Id = 34, Code = "BF", Name = "Burkina Faso" },
                new CountryList { Id = 35, Code = "BI", Name = "Burundi" },
                new CountryList { Id = 36, Code = "KH", Name = "Cambodia" },
                new CountryList { Id = 37, Code = "CM", Name = "Cameroon" },
                new CountryList { Id = 38, Code = "CA", Name = "Canada" },
                new CountryList { Id = 39, Code = "CV", Name = "Cape Verde" },
                new CountryList { Id = 40, Code = "KY", Name = "Cayman Islands" },
                new CountryList { Id = 41, Code = "CF", Name = "Central African Republic" },
                new CountryList { Id = 42, Code = "TD", Name = "Chad" },
                new CountryList { Id = 43, Code = "CL", Name = "Chile" },
                new CountryList { Id = 44, Code = "CN", Name = "China" },
                new CountryList { Id = 45, Code = "CX", Name = "Christmas Island" },
                new CountryList { Id = 46, Code = "CC", Name = "Cocos (Keeling) Islands" },
                new CountryList { Id = 47, Code = "CO", Name = "Colombia" },
                new CountryList { Id = 48, Code = "KM", Name = "Comoros" },
                new CountryList { Id = 49, Code = "CG", Name = "Congo" },
                new CountryList { Id = 50, Code = "CK", Name = "Cook Islands" },
                new CountryList { Id = 51, Code = "CR", Name = "Costa Rica" },
                new CountryList { Id = 52, Code = "HR", Name = "Croatia (Hrvatska)" },
                new CountryList { Id = 53, Code = "CU", Name = "Cuba" },
                new CountryList { Id = 54, Code = "CY", Name = "Cyprus" },
                new CountryList { Id = 55, Code = "CZ", Name = "Czech Republic" },
                new CountryList { Id = 56, Code = "DK", Name = "Denmark" },
                new CountryList { Id = 57, Code = "DJ", Name = "Djibouti" },
                new CountryList { Id = 58, Code = "DM", Name = "Dominica" },
                new CountryList { Id = 59, Code = "DO", Name = "Dominican Republic" },
                new CountryList { Id = 60, Code = "TP", Name = "East Timor" },
                new CountryList { Id = 61, Code = "EC", Name = "Ecuador" },
                new CountryList { Id = 62, Code = "EG", Name = "Egypt" },
                new CountryList { Id = 63, Code = "SV", Name = "El Salvador" },
                new CountryList { Id = 64, Code = "GQ", Name = "Equatorial Guinea" },
                new CountryList { Id = 65, Code = "ER", Name = "Eritrea" },
                new CountryList { Id = 66, Code = "EE", Name = "Estonia" },
                new CountryList { Id = 67, Code = "ET", Name = "Ethiopia" },
                new CountryList { Id = 68, Code = "FK", Name = "Falkland Islands (Malvinas)" },
                new CountryList { Id = 69, Code = "FO", Name = "Faroe Islands" },
                new CountryList { Id = 70, Code = "FJ", Name = "Fiji" },
                new CountryList { Id = 71, Code = "FI", Name = "Finland" },
                new CountryList { Id = 72, Code = "FR", Name = "France" },
                new CountryList { Id = 73, Code = "FX", Name = "France, Metropolitan" },
                new CountryList { Id = 74, Code = "GF", Name = "French Guiana" },
                new CountryList { Id = 75, Code = "PF", Name = "French Polynesia" },
                new CountryList { Id = 76, Code = "TF", Name = "French Southern Territories" },
                new CountryList { Id = 77, Code = "GA", Name = "Gabon" },
                new CountryList { Id = 78, Code = "GM", Name = "Gambia" },
                new CountryList { Id = 79, Code = "GE", Name = "Georgia" },
                new CountryList { Id = 80, Code = "DE", Name = "Germany" },
                new CountryList { Id = 81, Code = "GH", Name = "Ghana" },
                new CountryList { Id = 82, Code = "GI", Name = "Gibraltar" },
                new CountryList { Id = 83, Code = "GK", Name = "Guernsey" },
                new CountryList { Id = 84, Code = "GR", Name = "Greece" },
                new CountryList { Id = 85, Code = "GL", Name = "Greenland" },
                new CountryList { Id = 86, Code = "GD", Name = "Grenada" },
                new CountryList { Id = 87, Code = "GP", Name = "Guadeloupe" },
                new CountryList { Id = 88, Code = "GU", Name = "Guam" },
                new CountryList { Id = 89, Code = "GT", Name = "Guatemala" },
                new CountryList { Id = 90, Code = "GN", Name = "Guinea" },
                new CountryList { Id = 91, Code = "GW", Name = "Guinea-Bissau" },
                new CountryList { Id = 92, Code = "GY", Name = "Guyana" },
                new CountryList { Id = 93, Code = "HT", Name = "Haiti" },
                new CountryList { Id = 94, Code = "HM", Name = "Heard and Mc Donald Islands" },
                new CountryList { Id = 95, Code = "HN", Name = "Honduras" },
                new CountryList { Id = 96, Code = "HK", Name = "Hong Kong" },
                new CountryList { Id = 97, Code = "HU", Name = "Hungary" },
                new CountryList { Id = 98, Code = "IS", Name = "Iceland" },
                new CountryList { Id = 99, Code = "IN", Name = "India" },
                new CountryList { Id = 100, Code = "IM", Name = "Isle of Man" },
                new CountryList { Id = 101, Code = "ID", Name = "Indonesia" },
                new CountryList { Id = 102, Code = "IR", Name = "Iran (Islamic Republic of)" },
                new CountryList { Id = 103, Code = "IQ", Name = "Iraq" },
                new CountryList { Id = 104, Code = "IE", Name = "Ireland" },
                new CountryList { Id = 105, Code = "IL", Name = "Israel" },
                new CountryList { Id = 106, Code = "IT", Name = "Italy" },
                new CountryList { Id = 107, Code = "CI", Name = "Ivory Coast" },
                new CountryList { Id = 108, Code = "JE", Name = "Jersey" },
                new CountryList { Id = 109, Code = "JM", Name = "Jamaica" },
                new CountryList { Id = 110, Code = "JP", Name = "Japan" },
                new CountryList { Id = 111, Code = "JO", Name = "Jordan" },
                new CountryList { Id = 112, Code = "KZ", Name = "Kazakhstan" },
                new CountryList { Id = 113, Code = "KE", Name = "Kenya" },
                new CountryList { Id = 114, Code = "KI", Name = "Kiribati" },
                new CountryList { Id = 115, Code = "KP", Name = "Korea, Democratic Peopl's Republic of" },
                new CountryList { Id = 116, Code = "KR", Name = "Korea, Republic of" },
                new CountryList { Id = 117, Code = "XK", Name = "Kosovo" },
                new CountryList { Id = 118, Code = "KW", Name = "Kuwait" },
                new CountryList { Id = 119, Code = "KG", Name = "Kyrgyzstan" },
                new CountryList { Id = 120, Code = "LA", Name = "Lao People's Democratic Republic" },
                new CountryList { Id = 121, Code = "LV", Name = "Latvia" },
                new CountryList { Id = 122, Code = "LB", Name = "Lebanon" },
                new CountryList { Id = 123, Code = "LS", Name = "Lesotho" },
                new CountryList { Id = 124, Code = "LR", Name = "Liberia" },
                new CountryList { Id = 125, Code = "LY", Name = "Libyan Arab Jamahiriya" },
                new CountryList { Id = 126, Code = "LI", Name = "Liechtenstein" },
                new CountryList { Id = 127, Code = "LT", Name = "Lithuania" },
                new CountryList { Id = 128, Code = "LU", Name = "Luxembourg" },
                new CountryList { Id = 129, Code = "MO", Name = "Macau" },
                new CountryList { Id = 130, Code = "MK", Name = "Macedonia" },
                new CountryList { Id = 131, Code = "MG", Name = "Madagascar" },
                new CountryList { Id = 132, Code = "MW", Name = "Malawi" },
                new CountryList { Id = 133, Code = "MY", Name = "Malaysia" },
                new CountryList { Id = 134, Code = "MV", Name = "Maldives" },
                new CountryList { Id = 135, Code = "ML", Name = "Mali" },
                new CountryList { Id = 136, Code = "MT", Name = "Malta" },
                new CountryList { Id = 137, Code = "MH", Name = "Marshall Islands" },
                new CountryList { Id = 138, Code = "MQ", Name = "Martinique" },
                new CountryList { Id = 139, Code = "MR", Name = "Mauritania" },
                new CountryList { Id = 140, Code = "MU", Name = "Mauritius" },
                new CountryList { Id = 141, Code = "TY", Name = "Mayotte" },
                new CountryList { Id = 142, Code = "MX", Name = "Mexico" },
                new CountryList { Id = 143, Code = "FM", Name = "Micronesia, Federated States of" },
                new CountryList { Id = 144, Code = "MD", Name = "Moldova, Republic of" },
                new CountryList { Id = 145, Code = "MC", Name = "Monaco" },
                new CountryList { Id = 146, Code = "MN", Name = "Mongolia" },
                new CountryList { Id = 147, Code = "ME", Name = "Montenegro" },
                new CountryList { Id = 148, Code = "MS", Name = "Montserrat" },
                new CountryList { Id = 149, Code = "MA", Name = "Morocco" },
                new CountryList { Id = 150, Code = "MZ", Name = "Mozambique" },
                new CountryList { Id = 151, Code = "MM", Name = "Myanmar" },
                new CountryList { Id = 152, Code = "NA", Name = "Namibia" },
                new CountryList { Id = 153, Code = "NR", Name = "Nauru" },
                new CountryList { Id = 154, Code = "NP", Name = "Nepal" },
                new CountryList { Id = 155, Code = "NL", Name = "Netherlands" },
                new CountryList { Id = 156, Code = "AN", Name = "Netherlands Antilles" },
                new CountryList { Id = 157, Code = "NC", Name = "New Caledonia" },
                new CountryList { Id = 158, Code = "NZ", Name = "New Zealand" },
                new CountryList { Id = 159, Code = "NI", Name = "Nicaragua" },
                new CountryList { Id = 160, Code = "NE", Name = "Niger" },
                new CountryList { Id = 161, Code = "NG", Name = "Nigeria" },
                new CountryList { Id = 162, Code = "NU", Name = "Niue" },
                new CountryList { Id = 163, Code = "NF", Name = "Norfolk Island" },
                new CountryList { Id = 164, Code = "MP", Name = "Northern Mariana Islands" },
                new CountryList { Id = 165, Code = "NO", Name = "Norway" },
                new CountryList { Id = 166, Code = "OM", Name = "Oman" },
                new CountryList { Id = 167, Code = "PK", Name = "Pakistan" },
                new CountryList { Id = 168, Code = "PW", Name = "Palau" },
                new CountryList { Id = 169, Code = "PS", Name = "Palestine" },
                new CountryList { Id = 170, Code = "PA", Name = "Panama" },
                new CountryList { Id = 171, Code = "PG", Name = "Papua New Guinea" },
                new CountryList { Id = 172, Code = "PY", Name = "Paraguay" },
                new CountryList { Id = 173, Code = "PE", Name = "Peru" },
                new CountryList { Id = 174, Code = "PH", Name = "Philippines" },
                new CountryList { Id = 175, Code = "PN", Name = "Pitcairn" },
                new CountryList { Id = 176, Code = "PL", Name = "Poland" },
                new CountryList { Id = 177, Code = "PT", Name = "Portugal" },
                new CountryList { Id = 178, Code = "PR", Name = "Puerto Rico" },
                new CountryList { Id = 179, Code = "QA", Name = "Qatar" },
                new CountryList { Id = 180, Code = "RE", Name = "Reunion" },
                new CountryList { Id = 181, Code = "RO", Name = "Romania" },
                new CountryList { Id = 182, Code = "RU", Name = "Russian Federation" },
                new CountryList { Id = 183, Code = "RW", Name = "Rwanda" },
                new CountryList { Id = 184, Code = "KN", Name = "Saint Kitts and Nevis" },
                new CountryList { Id = 185, Code = "LC", Name = "Saint Lucia" },
                new CountryList { Id = 186, Code = "VC", Name = "Saint Vincent and the Grenadines" },
                new CountryList { Id = 187, Code = "WS", Name = "Samoa" },
                new CountryList { Id = 188, Code = "SM", Name = "San Marino" },
                new CountryList { Id = 189, Code = "ST", Name = "Sao Tome and Principe" },
                new CountryList { Id = 190, Code = "SA", Name = "Saudi Arabia" },
                new CountryList { Id = 191, Code = "SN", Name = "Senegal" },
                new CountryList { Id = 192, Code = "RS", Name = "Serbia" },
                new CountryList { Id = 193, Code = "SC", Name = "Seychelles" },
                new CountryList { Id = 194, Code = "SL", Name = "Sierra Leone" },
                new CountryList { Id = 195, Code = "SG", Name = "Singapore" },
                new CountryList { Id = 196, Code = "SK", Name = "Slovakia" },
                new CountryList { Id = 197, Code = "SI", Name = "Slovenia" },
                new CountryList { Id = 198, Code = "SB", Name = "Solomon Islands" },
                new CountryList { Id = 199, Code = "SO", Name = "Somalia" },
                new CountryList { Id = 200, Code = "ZA", Name = "South Africa" },
                new CountryList { Id = 201, Code = "GS", Name = "South Georgia South Sandwich Islands" },
                new CountryList { Id = 202, Code = "SS", Name = "South Sudan" },
                new CountryList { Id = 203, Code = "ES", Name = "Spain" },
                new CountryList { Id = 204, Code = "LK", Name = "Sri Lanka" },
                new CountryList { Id = 205, Code = "SH", Name = "St. Helena" },
                new CountryList { Id = 206, Code = "PM", Name = "St. Pierre and Miquelon" },
                new CountryList { Id = 207, Code = "SD", Name = "Sudan" },
                new CountryList { Id = 208, Code = "SR", Name = "Suriname" },
                new CountryList { Id = 209, Code = "SJ", Name = "Svalbard and Jan Mayen Islands" },
                new CountryList { Id = 210, Code = "SZ", Name = "Swaziland" },
                new CountryList { Id = 211, Code = "SE", Name = "Sweden" },
                new CountryList { Id = 212, Code = "CH", Name = "Switzerland" },
                new CountryList { Id = 213, Code = "SY", Name = "Syrian Arab Republic" },
                new CountryList { Id = 214, Code = "TW", Name = "Taiwan" },
                new CountryList { Id = 215, Code = "TJ", Name = "Tajikistan" },
                new CountryList { Id = 216, Code = "TZ", Name = "Tanzania, United Republic of" },
                new CountryList { Id = 217, Code = "TH", Name = "Thailand" },
                new CountryList { Id = 218, Code = "TG", Name = "Togo" },
                new CountryList { Id = 219, Code = "TK", Name = "Tokelau" },
                new CountryList { Id = 220, Code = "TO", Name = "Tonga" },
                new CountryList { Id = 222, Code = "TT", Name = "Trinidad and Tobago" },
                new CountryList { Id = 223, Code = "TN", Name = "Tunisia" },
                new CountryList { Id = 224, Code = "TR", Name = "Turkey" },
                new CountryList { Id = 225, Code = "TM", Name = "Turkmenistan" },
                new CountryList { Id = 226, Code = "TC", Name = "Turks and Caicos Islands" },
                new CountryList { Id = 227, Code = "TV", Name = "Tuvalu" },
                new CountryList { Id = 228, Code = "UG", Name = "Uganda" },
                new CountryList { Id = 229, Code = "UA", Name = "Ukraine" },
                new CountryList { Id = 230, Code = "AE", Name = "United Arab Emirates" },
                new CountryList { Id = 231, Code = "GB", Name = "United Kingdom" },
                new CountryList { Id = 232, Code = "US", Name = "United States" },
                new CountryList { Id = 233, Code = "UM", Name = "United States minor outlying islands" },
                new CountryList { Id = 234, Code = "UY", Name = "Uruguay" },
                new CountryList { Id = 235, Code = "UZ", Name = "Uzbekistan" },
                new CountryList { Id = 236, Code = "VU", Name = "Vanuatu" },
                new CountryList { Id = 237, Code = "VA", Name = "Vatican City State" },
                new CountryList { Id = 238, Code = "VE", Name = "Venezuela" },
                new CountryList { Id = 239, Code = "VN", Name = "Vietnam" },
                new CountryList { Id = 240, Code = "VG", Name = "Virgin Islands (British)" },
                new CountryList { Id = 241, Code = "VI", Name = "Virgin Islands (U.S.)" },
                new CountryList { Id = 242, Code = "WF", Name = "Wallis and Futuna Islands" },
                new CountryList { Id = 243, Code = "EH", Name = "Western Sahara" },
                new CountryList { Id = 244, Code = "YE", Name = "Yemen" },
                new CountryList { Id = 245, Code = "ZR", Name = "Zaire" },
                new CountryList { Id = 246, Code = "ZM", Name = "Zambia" },
                new CountryList { Id = 247, Code = "ZW", Name = "Zimbabwe" }
                );

        }
    }
}