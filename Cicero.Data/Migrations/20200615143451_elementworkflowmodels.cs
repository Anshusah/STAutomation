using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class elementworkflowmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 1,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 2,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 3,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 4,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 5,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 6,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 7,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 8,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 9,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 10,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 11,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 12,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 13,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 14,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 15,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 16,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 17,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 18,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 19,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 20,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 21,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 22,
                column: "RecipientType",
                value: "0");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "ElementWorkflowObject");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 1,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 2,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 3,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 4,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 5,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 6,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 7,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 8,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 9,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 10,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 11,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 12,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 13,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 14,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 15,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 16,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 17,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 18,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 19,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 20,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 21,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 22,
                column: "RecipientType",
                value: null);
        }
    }
}
