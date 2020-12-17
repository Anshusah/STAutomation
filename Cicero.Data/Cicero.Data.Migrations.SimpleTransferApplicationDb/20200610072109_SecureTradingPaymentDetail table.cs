using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class SecureTradingPaymentDetailtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecureTradingPaymentDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    requestreference = table.Column<string>(nullable: true),
                    version = table.Column<string>(nullable: true),
                    secrand = table.Column<string>(nullable: true),
                    transactionstartedtimestamp = table.Column<string>(nullable: true),
                    livestatus = table.Column<string>(nullable: true),
                    issuer = table.Column<string>(nullable: true),
                    splitfinalnumber = table.Column<string>(nullable: true),
                    dccenabled = table.Column<string>(nullable: true),
                    settleduedate = table.Column<string>(nullable: true),
                    errorcode = table.Column<string>(nullable: true),
                    baseamount = table.Column<string>(nullable: true),
                    tid = table.Column<string>(nullable: true),
                    merchantnumber = table.Column<string>(nullable: true),
                    securityresponsepostcode = table.Column<string>(nullable: true),
                    transactionreference = table.Column<string>(nullable: true),
                    merchantname = table.Column<string>(nullable: true),
                    paymenttypedescription = table.Column<string>(nullable: true),
                    orderreference = table.Column<string>(nullable: true),
                    accounttypedescription = table.Column<string>(nullable: true),
                    acquirerresponsecode = table.Column<string>(nullable: true),
                    requesttypedescription = table.Column<string>(nullable: true),
                    securityresponsesecuritycode = table.Column<string>(nullable: true),
                    currencyiso3a = table.Column<string>(nullable: true),
                    authcode = table.Column<string>(nullable: true),
                    errormessage = table.Column<string>(nullable: true),
                    operatorname = table.Column<string>(nullable: true),
                    merchantcountryiso2a = table.Column<string>(nullable: true),
                    maskedpan = table.Column<string>(nullable: true),
                    securityresponseaddress = table.Column<string>(nullable: true),
                    issuercountryiso2a = table.Column<string>(nullable: true),
                    settlestatus = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecureTradingPaymentDetail", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 10, 13, 6, 8, 825, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 10, 13, 6, 8, 827, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 10, 13, 6, 8, 827, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecureTradingPaymentDetail");

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 10, 11, 23, 46, 388, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 10, 11, 23, 46, 390, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 10, 11, 23, 46, 390, DateTimeKind.Local));
        }
    }
}
