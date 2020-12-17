using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class _cicero_entity_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Object = table.Column<string>(nullable: true),
                    ObjectId = table.Column<string>(nullable: true),
                    FieldName = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    OperationType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    ParentObject = table.Column<string>(nullable: true),
                    IsManual = table.Column<bool>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeletedTimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElementComponent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FieldKey = table.Column<string>(type: "varchar(50)", nullable: true),
                    FieldValue = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    FormId = table.Column<int>(nullable: true),
                    ElementId = table.Column<string>(nullable: true),
                    EventType = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElementComponent_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElementState",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true),
                    isDefaultEnd = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ElementId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ForEventType = table.Column<int>(nullable: false),
                    FormId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElementWorkflow",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaseFormId = table.Column<int>(nullable: false),
                    ElementId = table.Column<string>(nullable: true),
                    EventType = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    JsonData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementWorkflow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElementWorkflowObject",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    ElementId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    EventType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementWorkflowObject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElementWorkflowState",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FromStateId = table.Column<long>(nullable: false),
                    ToStateId = table.Column<long>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    ElementId = table.Column<string>(nullable: true),
                    EventType = table.Column<int>(nullable: false),
                    BeforeChangeActionsId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    AfterChangeActionsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementWorkflowState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElementWorkflowState_CaseForm_CaseFormId",
                        column: x => x.CaseFormId,
                        principalTable: "CaseForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailGroup_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CaseLimit = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillSet_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElementWorkflowPoint",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    FWObjectId = table.Column<string>(nullable: true),
                    LWObjectId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    ElementId = table.Column<string>(nullable: true),
                    EventType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementWorkflowPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElementWorkflowPoint_ElementWorkflowObject_FWObjectId",
                        column: x => x.FWObjectId,
                        principalTable: "ElementWorkflowObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElementWorkflowPoint_ElementWorkflowObject_LWObjectId",
                        column: x => x.LWObjectId,
                        principalTable: "ElementWorkflowObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Emailstring = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    EmailGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emails_EmailGroup_EmailGroupId",
                        column: x => x.EmailGroupId,
                        principalTable: "EmailGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkillSet",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    SkillSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillSet", x => new { x.SkillSetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserSkillSet_SkillSet_SkillSetId",
                        column: x => x.SkillSetId,
                        principalTable: "SkillSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillSet_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElementComponent_TenantId",
                table: "ElementComponent",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementWorkflowPoint_FWObjectId",
                table: "ElementWorkflowPoint",
                column: "FWObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementWorkflowPoint_LWObjectId",
                table: "ElementWorkflowPoint",
                column: "LWObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementWorkflowState_CaseFormId",
                table: "ElementWorkflowState",
                column: "CaseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailGroup_TenantId",
                table: "EmailGroup",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_EmailGroupId",
                table: "Emails",
                column: "EmailGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillSet_TenantId",
                table: "SkillSet",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillSet_UserId",
                table: "UserSkillSet",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "ElementComponent");

            migrationBuilder.DropTable(
                name: "ElementState");

            migrationBuilder.DropTable(
                name: "ElementWorkflow");

            migrationBuilder.DropTable(
                name: "ElementWorkflowPoint");

            migrationBuilder.DropTable(
                name: "ElementWorkflowState");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "UserSkillSet");

            migrationBuilder.DropTable(
                name: "ElementWorkflowObject");

            migrationBuilder.DropTable(
                name: "EmailGroup");

            migrationBuilder.DropTable(
                name: "SkillSet");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "EmailGroupId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "RecipientDatabaseTable",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "RecipientField",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "RecipientType",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Article");

        }
    }
}
