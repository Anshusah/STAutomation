using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class adddatatocountrylist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 18,
                column: "CurrencyCode",
                value: "BDT");

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 167,
                column: "CurrencyCode",
                value: "PKR");

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 231,
                columns: new[] { "CountryPhoneCode", "CurrencyCode" },
                values: new object[] { "+44", "GBP" });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 5, 22, 13, 40, 28, 736, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2020, 5, 22, 13, 40, 28, 737, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2020, 5, 22, 13, 40, 28, 737, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "AspNetUsers",
                maxLength: 10,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 18,
                column: "CurrencyCode",
                value: "");

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 167,
                column: "CurrencyCode",
                value: "");

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 231,
                columns: new[] { "CountryPhoneCode", "CurrencyCode" },
                values: new object[] { null, "" });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 5, 19, 15, 18, 32, 144, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2020, 5, 19, 15, 18, 32, 144, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2020, 5, 19, 15, 18, 32, 144, DateTimeKind.Local));
        }
    }
}
