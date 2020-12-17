using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class addsmslogtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsCodeCustomerRegistraion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    MobileNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    SmsSuccess = table.Column<bool>(nullable: false),
                    CustomerSuccess = table.Column<bool>(nullable: false),
                    RetryCount = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    ExpiryMinute = table.Column<int>(nullable: false),
                    RequestedDateTime = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ExpiryDateTime = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsCodeCustomerRegistraion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsCodeCustomerRegistraionLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    MobileNumber = table.Column<string>(nullable: true),
                    SmsCode = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SmsSuccess = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsCodeCustomerRegistraionLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    StatusMessage = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    ResponseMessage = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsLog", x => x.Id);
                });

          }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsCodeCustomerRegistraion");

            migrationBuilder.DropTable(
                name: "SmsCodeCustomerRegistraionLog");

            migrationBuilder.DropTable(
                name: "SmsLog");

           }
    }
}
