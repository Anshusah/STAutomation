using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Claim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "varchar(100)", nullable: true),
                    ClaimFields = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DamageCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Legal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LegalDetails = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FieldKey = table.Column<string>(type: "varchar(50)", nullable: true),
                    FieldValue = table.Column<string>(type: "varchar(5000)", nullable: true),
                    FieldDisplay = table.Column<string>(type: "varchar(200)", nullable: true),
                    FieldVisiblity = table.Column<int>(nullable: false),
                    FieldType = table.Column<string>(type: "varchar(50)", nullable: true),
                    FieldOptions = table.Column<string>(type: "varchar(500)", nullable: true),
                    FieldGridSize = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    Identifier = table.Column<string>(type: "varchar(50)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    AddressPrimary = table.Column<string>(type: "varchar(150)", nullable: true),
                    AddressSecondary = table.Column<string>(type: "varchar(150)", nullable: true),
                    PostCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    City = table.Column<string>(type: "varchar(100)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<short>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    DisplayName = table.Column<string>(type: "varchar(30)", nullable: true),
                    UserId = table.Column<string>(maxLength: 450, nullable: true),
                    Address = table.Column<string>(type: "varchar(200)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IsSuperAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(500)", nullable: true),
                    Content = table.Column<string>(type: "varchar(5000)", nullable: true),
                    Slug = table.Column<string>(type: "varchar(500)", nullable: true),
                    Template = table.Column<string>(type: "varchar(500)", nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Excerpt = table.Column<string>(type: "varchar(5000)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Version = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(type: "varchar(50)", nullable: true),
                    Title = table.Column<string>(type: "varchar(50)", nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Media_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Queue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Side = table.Column<string>(nullable: true),
                    UrlIdentifier = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Queue_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<short>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameBackend = table.Column<string>(type: "varchar(50)", nullable: true),
                    NameFrontend = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Color = table.Column<string>(type: "varchar(50)", nullable: true),
                    NotifyUser = table.Column<bool>(nullable: false),
                    CanEdit = table.Column<bool>(nullable: false),
                    CanDelete = table.Column<bool>(nullable: false),
                    NeedReason = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                    table.ForeignKey(
                        name: "FK_State_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Details = table.Column<string>(type: "varchar(1000)", nullable: true),
                    DisplayTo = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLog_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityLog_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    ClaimId = table.Column<string>(type: "varchar(50)", nullable: true),
                    Subject = table.Column<string>(type: "varchar(50)", nullable: true),
                    Content = table.Column<string>(type: "varchar(500)", nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    Attachment = table.Column<string>(type: "varchar(100)", nullable: true),
                    To = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_User_From",
                        column: x => x.From,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_To",
                        column: x => x.To,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantUser_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenantUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    UserId = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleId = table.Column<int>(nullable: false),
                    MediaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleMedia_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleMedia_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    MediaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMedia_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMedia_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: true),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    RoleId = table.Column<string>(unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Case",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true),
                    CaseGeneratedId = table.Column<string>(type: "varchar(50)", nullable: true),
                    OrganisationId = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    FullName = table.Column<string>(type: "varchar(50)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    GeoLocation = table.Column<string>(type: "varchar(250)", nullable: true),
                    OtherInformation = table.Column<string>(type: "varchar(250)", nullable: true),
                    Extras = table.Column<string>(nullable: true),
                    BillOfLadingNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    NumberOfContainers = table.Column<int>(maxLength: 3, nullable: false),
                    From = table.Column<string>(type: "varchar(50)", nullable: true),
                    To = table.Column<string>(type: "varchar(50)", nullable: true),
                    Vessel = table.Column<string>(type: "varchar(50)", nullable: true),
                    CargoDeliveryDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    DamageTypeId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    ClaimTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Case", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Case_Claim_ClaimTypeId",
                        column: x => x.ClaimTypeId,
                        principalTable: "Claim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Case_DamageCategory_DamageTypeId",
                        column: x => x.DamageTypeId,
                        principalTable: "DamageCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Case_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Case_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Case_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QueueToState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateId = table.Column<int>(nullable: false),
                    QueueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueToState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueueToState_Queue_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QueueToState_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StateToState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromStateId = table.Column<int>(nullable: false),
                    ToStateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateToState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateToState_State_FromStateId",
                        column: x => x.FromStateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateToState_State_ToStateId",
                        column: x => x.ToStateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseChangeReason",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", nullable: true),
                    ChangedBy = table.Column<string>(nullable: true),
                    ChangedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseChangeReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseChangeReason_Case_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<int>(nullable: false),
                    MediaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseMedia_Case_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseMedia_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "Content", "CreatedAt", "Excerpt", "ParentId", "Slug", "Status", "Template", "TenantId", "Title", "UpdatedAt", "UserId", "Version" },
                values: new object[,]
                {
                    { 17, "<h3 class=\"display-4 main-promo-caption heading mt-4\">Relax during your shipment</h3><p class=\"h5 mt-3 main-promo-caption subheading mx-auto mb-5\">We are here to claim for you if your goods are damaged, stolen or anyting happens to your goods</p><p class=\"mx-auto\"><a class=\"btn btn-primary btn-lg btn-rounded px-5 mr-3 mb-3 shadow-sm\" href=\"/user/claim/0/edit.html\" role=\"button\">Claim here</a><a class=\"btn btn-outline-default btn-lg btn-rounded px-5 mb-3 shadow-sm\" href=\"#\" role=\"button\">Learn more</a></p>", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "", 0, "home-banner", (short)1, "Default", null, "Home Banner", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 18, "<strong>Cicero</strong><br>GW56 + 5W City of London, London, UK <br>London SE1 9DD, UK<br><strong title = \"Phone\"> P:</strong> (123) 456 - 7890 <br><strong title=\"Email\"> E:</strong> <a href=\"#\"> info@Cicero.com </a>", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "", 0, "contact-us", (short)1, "Contact", null, "Contact Us", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 16, "Did you find problems with your goods after shipment, don't worry we are here for you.", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Did you find problems with your goods after shipment, don't worry we are here for you.", 0, "having-problems", (short)1, "Default", null, "Having problems?", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 15, "How to provide evidence more genuinely", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "How to provide evidence more genuinely", 9, "how-to-provide-evidence", (short)1, "Default", null, "Guidelines - How to provide evidence more genuinely", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 14, "Some quick example text to build on the card title and make up the bulk of the card's content.", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Some quick example text to build on the card title and make up the bulk of the card's content.", 9, "some-quick-example-text", (short)1, "Default", null, "Guidelines - Some quick example text", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 13, "Know about our process for better understanding Cicero", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Know about our process for better understanding Cicero", 9, "know-about-our-understanding", (short)1, "Default", null, "Guidelines - Know about our process", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 11, "Know about our process for better understanding Cicero", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Know about our process for better understanding Cicero", 9, "know-about-our-process", (short)1, "Default", null, "Guidelines - Know about our process", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 10, "Track you claim process from anywhere", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Track you claim process from anywhere", 9, "track-you-claim-process", (short)1, "Default", null, "Guidelines - Track you claim", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 9, "", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, "guidelines", (short)1, "Default", null, "Guidelines", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 12, "How to provide evidence more genuinely", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "How to provide evidence more genuinely", 9, "how-to-provide-evidence-more-genuinely", (short)1, "Default", null, "Guidelines - How to provide evidence", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 7, "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", 5, "provide-evidence", (short)1, "Default", null, "Provide Evidence", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 6, "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", 5, "fill-out-questionaries", (short)1, "Default", null, "Fill out Questionaries", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 5, "Get started with these simple steps", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, "get-started", (short)1, "Default", null, "Get started with these simple steps", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 4, "Nam non ante quam. Mauris posuere nisl ac vehicula imperdiet. Vivamus at tellus a velit scelerisque aliquam vel vitae erat.Morbi vitae eleifend arcu, et rhoncus lacus.", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Nam non ante quam. Mauris posuere nisl ac vehicula imperdiet. Vivamus at tellus a velit scelerisque aliquam vel vitae erat. Morbi vitae eleifend arcu, et rhoncus lacus.", 0, "footer-block-about-us", (short)1, "Default", null, "Footer Block - About Us", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 3, "This is Privacy Policy Page", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, "privacy-policy", (short)1, "Default", null, "Privacy Policy", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 2, "This is About Us Page", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, "about-us", (short)1, "Default", null, "About Us", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 1, "This is Terms and Condition Page", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, "terms-and-conditions", (short)1, "Default", null, "Terms and Conditions", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 },
                    { 8, "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "Sed pretium enim quis metus feugiat, ac auctor neque ullamcorper. Mauris quis arcu efficitur", 5, "send", (short)1, "Default", null, "Send", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 }
                });

            migrationBuilder.InsertData(
                table: "DamageCategory",
                columns: new[] { "Id", "Name", "ParentId" },
                values: new object[,]
                {
                    { 24, "Off power", 4 },
                    { 19, "Freezing damage", 3 },
                    { 20, "Cargo leakage", 3 },
                    { 21, "Cargo shifting", 3 },
                    { 22, "Other", 3 },
                    { 23, "Delay", 4 },
                    { 25, "Flooding", 4 },
                    { 31, "Water ingress Ripped Tarpaulin", 5 },
                    { 27, "Reefer malfunction", 4 },
                    { 28, "Reefer physical damage", 4 },
                    { 29, "Other", 4 },
                    { 30, "Flooding", 5 },
                    { 32, "Water ingress Damaged/Holed Container", 5 },
                    { 33, "Water ingress Corroded/wears & tears", 5 },
                    { 18, "Heating damage", 3 },
                    { 26, "Pilferage", 4 },
                    { 17, "Impact", 3 },
                    { 8, "Gas", 1 },
                    { 15, "other", 2 },
                    { 1, "Contamination", 0 },
                    { 2, "Loss", 0 },
                    { 3, "Physical Damage", 0 },
                    { 4, "Reefer Damage", 0 },
                    { 5, "Wetting Damage", 0 },
                    { 6, "Liquid", 1 },
                    { 16, "Fire", 3 },
                    { 7, "Solid", 1 },
                    { 9, "Smelling", 1 },
                    { 10, "Vermin", 1 },
                    { 11, "Other", 1 },
                    { 12, "cargo pilferage", 2 },
                    { 13, "container disappearance", 2 },
                    { 14, "container lost overboard", 2 }
                });

            migrationBuilder.InsertData(
                table: "Media",
                columns: new[] { "Id", "CreatedBy", "Description", "TenantId", "Title", "Url" },
                values: new object[,]
                {
                    { 6, null, "Test Image 5", null, "Test Image 5", "default-contact-image.jpg" },
                    { 5, null, "Test Image 5", null, "Test Image 5", "default-banner-image.jpg" },
                    { 4, null, "Test Image 4", null, "Test Image 4", "default-group.jpg" },
                    { 2, null, "Test Image 2", null, "Test Image 2", "default-lawyer.jpg" },
                    { 1, null, "Test Image 1", null, "Test Image 1", "default-mobile.jpg" },
                    { 3, null, "Test Image 3", null, "Test Image 3", "default-smile.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 20, "Update Menu" },
                    { 21, "Delete Menu" },
                    { 22, "View Role" },
                    { 23, "Create Role" },
                    { 24, "Update Role" },
                    { 25, "Delete Role" },
                    { 26, "View Setting" },
                    { 27, "Create Setting" },
                    { 29, "Delete Setting" },
                    { 30, "View Message" },
                    { 31, "Compose Message" },
                    { 32, "Reply Message" },
                    { 33, "Reply All Message" },
                    { 34, "View Queue" },
                    { 35, "Create Queue" },
                    { 36, "Update Queue" },
                    { 28, "Update Setting" },
                    { 19, "Create Menu" },
                    { 18, "View Menu" },
                    { 17, "Delete Media" },
                    { 1, "View Claim" },
                    { 2, "Create Claim" },
                    { 3, "Update Claim" },
                    { 4, "Delete Claim" },
                    { 5, "View User" },
                    { 6, "Create User" },
                    { 7, "Update User" },
                    { 37, "Delete Queue" },
                    { 8, "Delete User" },
                    { 10, "View Article" },
                    { 11, "Create Article" },
                    { 12, "Update Article" },
                    { 13, "Delete Article" },
                    { 14, "View Media" },
                    { 15, "Create Media" },
                    { 16, "Update Media" },
                    { 9, "Edit Own Profile" },
                    { 38, "View Tenants" }
                });

            migrationBuilder.InsertData(
                table: "Setting",
                columns: new[] { "Id", "FieldDisplay", "FieldGridSize", "FieldKey", "FieldOptions", "FieldType", "FieldValue", "FieldVisiblity" },
                values: new object[,]
                {
                    { 8, "Url", "6", "app_url", null, "TEXTBOX", "http://52.228.24.65/", 1 },
                    { 10, "Twitter Url", "6", "app_twitter", null, "TEXTBOX", "http://twitter.com", 1 },
                    { 7, "Role for register User", "6", "app_user_role", null, "USERROLE", "User", 1 },
                    { 9, "Facebook Url", "6", "app_facebook", null, "TEXTBOX", "http://facebook.com", 1 },
                    { 6, "Sea Ports", "12", "app_ports", null, "TEXTAREA", "Algeciras,Antwerp,Bremerhaven,Busan,Colombo,Colon,Dalian,Dongguan,Dubai,Felixstowe,Guangzhou,Hamburg,Hong Kong,Jeddah,Kaohsiung,Khor Fakkan,Laem Chabang,Lianyungang,Long Beach,Los Angeles,Manila,Marsaxlokk,Mumbai,Mundra,Nanjing,New York,Ningbo-Zhoushan,Piraeus,Port Klang,Port Said,Qingdao,Rizhao,Rotterdam,Saigon,Salalah,Santos,Savannah,Seattle/Tacoma,Shanghai,Shenzhen,Singapore,Taicang,Tanjung Pelepas,Tanjung Perak,Tanjung Priok,Tianjin,Tokyo,Valencia,Xiamen,Yingkou", 1 },
                    { 12, "Navigation - Bottom", null, "Bottom", null, "TEXTBOX", "[{\"index\":0,\"menu\":\"About Cicero\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Claim Process\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":3,\"menu\":\"Blogs\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":4,\"menu\":\"Careers\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":1,\"menu\":\"Legals\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Terms and Conditions\",\"type\":\"article\",\"url\":\"1\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Terms and Conditions\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":2,\"menu\":\"Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Contact Us\",\"type\":\"article\",\"url\":\"18\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Contact Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Feedback\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Help & Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]}]", 0 },
                    { 4, "Email", "6", "app_email", null, "TEXTBOX", "info@cep.com", 1 },
                    { 3, "Phone", "6", "app_phone", null, "TEXTBOX", "9851189071", 1 },
                    { 2, "Front Title", "6", "app_name_frontend", null, "TEXTBOX", "Cargo Carrier Levitate", 1 },
                    { 1, "Name", "6", "app_name", null, "TEXTBOX", "Cicero", 1 },
                    { 11, "Navigation - Primary", null, "Primary", null, "TEXTBOX", "[{\"index\":0,\"menu\":\"Home\",\"type\":\"custom\",\"url\":\"/\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]}]", 0 },
                    { 5, "Address", "12", "app_address", null, "TEXTAREA", "London", 1 }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "DisplayName", "Email", "EmailConfirmed", "FirstName", "IsSuperAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserId", "UserName" },
                values: new object[] { "df0d5fc1-b3c9-448f-afea-a43cd08005a6", 0, "london", "978d05ed-67c6-4da6-aa8f-f1cf64a3a972", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "", "Kishan Kishan", "kishan@vesuviois.com", true, "Kishan", true, "Sharma", true, null, "KISHAN@VESUVIOIS.COM", "KISHAN@VESUVIOIS.COM", "AQAAAAEAACcQAAAAEO5lquYUWfSRcDWe+O4Vd+0S95cXnuUyUh89qzwVmFKR8/UXTicXZ03+SEBqdjNtwg==", "9851189079", false, "TN2MA6753JLC5JGLUXTIJJA42QTYAQXZ", (short)1, false, new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), "authorize", "kishan@vesuviois.com" });

            migrationBuilder.InsertData(
                table: "ArticleMedia",
                columns: new[] { "Id", "ArticleId", "MediaId" },
                values: new object[,]
                {
                    { 1, 10, 1 },
                    { 2, 11, 2 },
                    { 3, 12, 3 },
                    { 4, 13, 4 },
                    { 5, 17, 5 },
                    { 6, 18, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_TenantId",
                table: "ActivityLog",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_UserId",
                table: "ActivityLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_TenantId",
                table: "Article",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleMedia_ArticleId",
                table: "ArticleMedia",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleMedia_MediaId",
                table: "ArticleMedia",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Case_ClaimTypeId",
                table: "Case",
                column: "ClaimTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Case_DamageTypeId",
                table: "Case",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Case_StateId",
                table: "Case",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Case_TenantId",
                table: "Case",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Case_UserId",
                table: "Case",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseChangeReason_CaseId",
                table: "CaseChangeReason",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseMedia_CaseId",
                table: "CaseMedia",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseMedia_MediaId",
                table: "CaseMedia",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Media_TenantId",
                table: "Media",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_From",
                table: "Message",
                column: "From");

            migrationBuilder.CreateIndex(
                name: "IX_Message_TenantId",
                table: "Message",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_To",
                table: "Message",
                column: "To");

            migrationBuilder.CreateIndex(
                name: "IX_Queue_TenantId",
                table: "Queue",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueToState_QueueId",
                table: "QueueToState",
                column: "QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueToState_StateId",
                table: "QueueToState",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Role_TenantId",
                table: "Role",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_State_TenantId",
                table: "State",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StateToState_FromStateId",
                table: "StateToState",
                column: "FromStateId");

            migrationBuilder.CreateIndex(
                name: "IX_StateToState_ToStateId",
                table: "StateToState",
                column: "ToStateId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantUser_TenantId",
                table: "TenantUser",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantUser_UserId",
                table: "TenantUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedia_MediaId",
                table: "UserMedia",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedia_UserId",
                table: "UserMedia",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "ArticleMedia");

            migrationBuilder.DropTable(
                name: "CaseChangeReason");

            migrationBuilder.DropTable(
                name: "CaseMedia");

            migrationBuilder.DropTable(
                name: "Legal");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "QueueToState");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "StateToState");

            migrationBuilder.DropTable(
                name: "TenantUser");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserMedia");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Case");

            migrationBuilder.DropTable(
                name: "Queue");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Claim");

            migrationBuilder.DropTable(
                name: "DamageCategory");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Tenant");
        }
    }
}
