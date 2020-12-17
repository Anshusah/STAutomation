using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class updatepaymentdetailtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isRefund",
                table: "SecureTradingPaymentDetail",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 15, 13, 3, 12, 752, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 15, 13, 3, 12, 753, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2020, 6, 15, 13, 3, 12, 753, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isRefund",
                table: "SecureTradingPaymentDetail");

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
    }
}
