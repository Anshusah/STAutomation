using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class addCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiUser");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    TenantId = table.Column<string>(type: "varchar(200)", nullable: false),
                    Title = table.Column<string>(type: "varchar(50)", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    MiddleName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true),
                    CountryCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    MobileNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    Street = table.Column<string>(type: "varchar(50)", nullable: true),
                    City = table.Column<string>(type: "varchar(50)", nullable: true),
                    PostCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IdType = table.Column<int>(nullable: false),
                    IdNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    IdExpiryDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

        }
    }
}
