using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _payee_securityQuestion_table_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "Customer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.CreateTable(
                name: "Payee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayeeType = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    TypeOfBusinessEntity = table.Column<string>(nullable: true),
                    CompanyWebsite = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CountryCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", nullable: true),
                    City = table.Column<string>(type: "varchar(50)", nullable: true),
                    PostCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    MobileNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    IdType = table.Column<int>(nullable: false),
                    IdNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    IdExpiryDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SecurityQuestionId = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    IssuingCountry = table.Column<string>(nullable: true),
                    CompanyRegistrationNumber = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecurityQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityQuestion", x => x.Id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payee");

            migrationBuilder.DropTable(
                name: "SecurityQuestion");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "Customer",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(int));

        }
    }
}
