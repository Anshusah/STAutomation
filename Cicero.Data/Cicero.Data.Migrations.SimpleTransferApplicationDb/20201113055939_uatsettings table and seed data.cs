using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class uatsettingstableandseeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.CreateTable(
                name: "UatSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UatSetting", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 11, 13, 11, 44, 38, 372, DateTimeKind.Local));

            migrationBuilder.InsertData(
                table: "UatSetting",
                columns: new[] { "Id", "PhoneNumber", "Status" },
                values: new object[] { 1, "", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UatSetting");

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 11, 10, 19, 47, 49, 779, DateTimeKind.Local));

            migrationBuilder.InsertData(
                table: "RateSupplier",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Name", "Password", "RatePriority", "SystemId", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "NecMoney", "P@ssw0rd", 2, "9D6D65E4-EFF1-4752-892D-3F637A56C159", new DateTime(2020, 11, 10, 19, 47, 49, 781, DateTimeKind.Local), null, "necmoney" });

            migrationBuilder.InsertData(
                table: "RateSupplier",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Name", "Password", "RatePriority", "SystemId", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { 3, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Safkhan", "P@ssw0rd", 3, "D93C65D4-7AA9-4A84-B23F-BEBB8D324476", new DateTime(2020, 11, 10, 19, 47, 49, 781, DateTimeKind.Local), null, "safkhan" });
        }
    }
}
