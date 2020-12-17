using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class simpletransfer_updateratesupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RatePriority", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 2, 5, 11, 1, 39, 110, DateTimeKind.Local), 1, new DateTime(2020, 2, 5, 11, 1, 39, 109, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RatePriority", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 2, 5, 11, 1, 39, 110, DateTimeKind.Local), 2, new DateTime(2020, 2, 5, 11, 1, 39, 110, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RatePriority", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 2, 5, 11, 1, 39, 110, DateTimeKind.Local), 3, new DateTime(2020, 2, 5, 11, 1, 39, 110, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RatePriority", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RatePriority", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RatePriority", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
