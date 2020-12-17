using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstimateId",
                table: "Message",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimateId",
                table: "InvolvedParties",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Estimate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    SurName = table.Column<string>(type: "varchar(50)", nullable: true),
                    MaritialStatus = table.Column<string>(type: "varchar(50)", nullable: true),
                    Address1 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Address2 = table.Column<string>(type: "varchar(100)", nullable: true),
                    PostCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    City = table.Column<string>(type: "varchar(100)", nullable: true),
                    Country = table.Column<string>(type: "varchar(100)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    ContactDay = table.Column<string>(type: "varchar(50)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    VatRegistered = table.Column<string>(nullable: true),
                    VatRate = table.Column<int>(nullable: false),
                    PolicyNumber = table.Column<string>(type: "varchar(100)", nullable: true),
                    PolicyStartDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PolicyEndDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    HasChildren = table.Column<string>(nullable: true),
                    NumberOfChildren = table.Column<int>(nullable: false),
                    InsuranceType = table.Column<string>(type: "varchar(100)", nullable: true),
                    Excess = table.Column<float>(nullable: false),
                    BankAccountName = table.Column<string>(type: "varchar(100)", nullable: true),
                    BankSortCode = table.Column<string>(type: "varchar(250)", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "varchar(250)", nullable: true),
                    GeoLocation = table.Column<string>(type: "varchar(250)", nullable: true),
                    OtherInformation = table.Column<string>(type: "varchar(250)", nullable: true),
                    Extras = table.Column<string>(nullable: true),
                    CaseGeneratedId = table.Column<string>(type: "varchar(50)", nullable: true),
                    OrganisationId = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    SignatureId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    UserAccessId = table.Column<string>(nullable: true),
                    SignatureDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Message_EstimateId",
                table: "Message",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedParties_EstimateId",
                table: "InvolvedParties",
                column: "EstimateId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_InvolvedParties_Estimate_EstimateId",
                table: "InvolvedParties",
                column: "EstimateId",
                principalTable: "Estimate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Estimate_EstimateId",
                table: "Message",
                column: "EstimateId",
                principalTable: "Estimate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvolvedParties_Estimate_EstimateId",
                table: "InvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Estimate_EstimateId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Estimate");

            migrationBuilder.DropIndex(
                name: "IX_Message_EstimateId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_InvolvedParties_EstimateId",
                table: "InvolvedParties");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DropColumn(
                name: "EstimateId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "EstimateId",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Case");
        }
    }
}
