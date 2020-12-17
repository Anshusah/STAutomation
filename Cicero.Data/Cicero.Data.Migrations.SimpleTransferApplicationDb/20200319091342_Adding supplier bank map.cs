using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class Addingsupplierbankmap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierBankMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransfastBankCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    NecMoneyBankCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierBankMap", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 664, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 193,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 194,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 195,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 196,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 197,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 213,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 214,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 215,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 216,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 223,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 224,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 225,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 226,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 227,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 228,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 229,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 230,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 231,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 232,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 233,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 234,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 235,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 236,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 237,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 238,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 239,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 240,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 241,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 242,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 243,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 244,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 245,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 246,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 247,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 665, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 662, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 660, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 663, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 663, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 19, 14, 58, 41, 663, DateTimeKind.Local), new DateTime(2020, 3, 19, 14, 58, 41, 663, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierBankMap");

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 193,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 194,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 195,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 196,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 197,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 213,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 214,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 215,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 216,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 223,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 224,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 225,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 226,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 227,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 228,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 229,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 230,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 231,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 232,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 233,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 234,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 235,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 236,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 237,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 238,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 239,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 240,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 241,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 242,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 243,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 244,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 245,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 246,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 247,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 169, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 167, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 166, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 167, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 167, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 3, 12, 14, 21, 46, 167, DateTimeKind.Local), new DateTime(2020, 3, 12, 14, 21, 46, 167, DateTimeKind.Local) });
        }
    }
}
