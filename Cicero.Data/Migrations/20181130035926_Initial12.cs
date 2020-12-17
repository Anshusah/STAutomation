using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_DamageCategory_DamageTypeId",
                table: "Case");

            migrationBuilder.DropTable(
                name: "CaseChangeReason");

            migrationBuilder.DropIndex(
                name: "IX_Case_DamageTypeId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "BillOfLadingNumber",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "From",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "NumberOfContainers",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Case");

            migrationBuilder.RenameColumn(
                name: "Vessel",
                table: "Case",
                newName: "PostCode");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Case",
                newName: "PaymentDetails");

            migrationBuilder.RenameColumn(
                name: "DamageTypeId",
                table: "Case",
                newName: "VatRate");

            migrationBuilder.RenameColumn(
                name: "CargoDeliveryDate",
                table: "Case",
                newName: "InceptionDate");

            migrationBuilder.AddColumn<string>(
                name: "BankAddress",
                table: "Case",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Excess",
                table: "Case",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Case",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InsuranceType",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayTo",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryAddress",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryAddress",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VatRegistered",
                table: "Case",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAddress",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Excess",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "InsuranceType",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "PayTo",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "PrimaryAddress",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "SecondaryAddress",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "VatRegistered",
                table: "Case");

            migrationBuilder.RenameColumn(
                name: "VatRate",
                table: "Case",
                newName: "DamageTypeId");

            migrationBuilder.RenameColumn(
                name: "PostCode",
                table: "Case",
                newName: "Vessel");

            migrationBuilder.RenameColumn(
                name: "PaymentDetails",
                table: "Case",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "InceptionDate",
                table: "Case",
                newName: "CargoDeliveryDate");

            migrationBuilder.AddColumn<string>(
                name: "BillOfLadingNumber",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfContainers",
                table: "Case",
                maxLength: 3,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaseChangeReason",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaseId = table.Column<int>(nullable: false),
                    ChangedBy = table.Column<string>(nullable: true),
                    ChangedDate = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(type: "varchar(500)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Case_DamageTypeId",
                table: "Case",
                column: "DamageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseChangeReason_CaseId",
                table: "CaseChangeReason",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_DamageCategory_DamageTypeId",
                table: "Case",
                column: "DamageTypeId",
                principalTable: "DamageCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
