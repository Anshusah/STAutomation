using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial58 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Case_StateId",
                table: "Case");
       
            migrationBuilder.DropForeignKey(
                name: "FK_Case_State_StateId",
                table: "Case");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Estimate_EstimateId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "InvolvedParties");

            migrationBuilder.DropTable(
                name: "Legal");

            migrationBuilder.DropTable(
                name: "Estimate");

            migrationBuilder.DropIndex(
                name: "IX_Message_EstimateId",
                table: "Message");

            

            migrationBuilder.DropColumn(
                name: "EstimateId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "BankAccountName",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "BankSortCode",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "ContactDay",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Excess",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Extras",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "GeoLocation",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "HasChildren",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "InsuranceType",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "JsonExtras",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "MaritialStatus",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "NumberOfChildren",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "OtherInformation",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "PolicyEndDate",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "PolicyStartDate",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "SchemeId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "SignatureDate",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "TelephoneNumber",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "UserAccessId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "VatRegistered",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Case");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstimateId",
                table: "Message",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Case",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankSortCode",
                table: "Case",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactDay",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Excess",
                table: "Case",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Extras",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeoLocation",
                table: "Case",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HasChildren",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceType",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonExtras",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritialStatus",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChildren",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OtherInformation",
                table: "Case",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PolicyEndDate",
                table: "Case",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PolicyStartDate",
                table: "Case",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchemeId",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SignatureDate",
                table: "Case",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Case",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelephoneNumber",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccessId",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VatRate",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VatRegistered",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Estimate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address1 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Address2 = table.Column<string>(type: "varchar(100)", nullable: true),
                    BankAccountName = table.Column<string>(type: "varchar(100)", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "varchar(250)", nullable: true),
                    BankSortCode = table.Column<string>(type: "varchar(250)", nullable: true),
                    CaseFormId = table.Column<int>(nullable: false),
                    CaseGeneratedId = table.Column<string>(type: "varchar(50)", nullable: true),
                    City = table.Column<string>(type: "varchar(100)", nullable: true),
                    ContactDay = table.Column<string>(type: "varchar(50)", nullable: true),
                    Country = table.Column<string>(type: "varchar(100)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    Excess = table.Column<float>(nullable: false),
                    Extras = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    GeoLocation = table.Column<string>(type: "varchar(250)", nullable: true),
                    HasChildren = table.Column<string>(nullable: true),
                    InsuranceType = table.Column<string>(type: "varchar(100)", nullable: true),
                    MaritialStatus = table.Column<string>(type: "varchar(50)", nullable: true),
                    NumberOfChildren = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    OrganisationId = table.Column<int>(nullable: false),
                    OtherInformation = table.Column<string>(type: "varchar(250)", nullable: true),
                    PolicyEndDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "varchar(100)", nullable: true),
                    PolicyStartDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PostCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    SignatureDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SignatureId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    SurName = table.Column<string>(type: "varchar(50)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UserAccessId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    VatRate = table.Column<int>(nullable: false),
                    VatRegistered = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estimate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estimate_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estimate_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Estimate_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "InvolvedParties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address1 = table.Column<string>(type: "varchar(50)", nullable: true),
                    Address2 = table.Column<string>(type: "varchar(50)", nullable: true),
                    CaseId = table.Column<int>(nullable: false),
                    City = table.Column<string>(type: "varchar(50)", nullable: true),
                    Country = table.Column<string>(type: "varchar(20)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    EstimateId = table.Column<int>(nullable: true),
                    FullName = table.Column<string>(type: "varchar(100)", nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", nullable: true),
                    IdDocument = table.Column<string>(type: "varchar(50)", nullable: true),
                    IdNumber = table.Column<string>(type: "varchar(20)", nullable: true),
                    InvolvementType = table.Column<string>(type: "varchar(100)", nullable: true),
                    MartialStatus = table.Column<string>(type: "varchar(50)", nullable: true),
                    Nationality = table.Column<string>(type: "varchar(50)", nullable: true),
                    Phone = table.Column<string>(type: "varchar(15)", nullable: true),
                    PostCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvolvedParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvolvedParties_Case_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Case",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvolvedParties_Estimate_EstimateId",
                        column: x => x.EstimateId,
                        principalTable: "Estimate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvolvedParties_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_EstimateId",
                table: "Message",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_Case_StateId",
                table: "Case",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimate_StateId",
                table: "Estimate",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimate_TenantId",
                table: "Estimate",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimate_UserId",
                table: "Estimate",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedParties_CaseId",
                table: "InvolvedParties",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedParties_EstimateId",
                table: "InvolvedParties",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedParties_UserId",
                table: "InvolvedParties",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_State_StateId",
                table: "Case",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Estimate_EstimateId",
                table: "Message",
                column: "EstimateId",
                principalTable: "Estimate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
