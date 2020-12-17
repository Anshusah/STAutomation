using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class seedpermissiondata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {          
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 63, "Delete Bank Mapper" },
                    { 62, "Update Bank Mapper" },
                    { 61, "Create Bank Mapper" },
                    { 60, "View Bank Mapper" },
                    { 58, "Delete Country Payout Mode" },
                    { 52, "Create Country" },
                    { 56, "Create Country Payout Mode" },
                    { 55, "View Country Payout Mode" },
                    { 54, "Delete Country" },
                    { 53, "Update Country" },
                    { 64, "View Branch Mapper" },
                    { 57, "Update Country Payout Mode" },
                    { 65, "Create Branch Mapper" },
                    { 71, "Delete City Config" },
                    { 67, "Delete Branch Mapper" },
                    { 68, "View City Config" },
                    { 69, "Create City Config" },
                    { 70, "Update City Config " },
                    { 51, "View Country" },
                    { 73, "Create Rate Supplier" },
                    { 74, "Update Rate Supplier" },
                    { 75, "Delete Rate Supplier" },
                    { 76, "View ExchangeRates" },
                    { 77, "Create ExchangeRates" },
                    { 78, "Update ExchangeRates" },
                    { 79, "Delete ExchangeRates" },
                    { 66, "Update Branch Mapper" },
                    { 72, "View Rate Supplier" },
                    { 88, "View Marital Status Config" },
                    { 83, "Delete Correspondent Bank" },
                    { 111, "Delete Txn Limit Config" },
                    { 110, "Update Txn Limit Config" },
                    { 109, "Create Txn Limit Config" },
                    { 108, "View Txn Limit Config" },
                    { 107, "Delete Rate Supplier Fee Config" },
                    { 106, "Update Rate Supplier Fee Config" },
                    { 105, "Create Rate Supplier Fee Config" },
                    { 104, "View Rate Supplier Fee Config" },
                    { 103, "Delete Payment Purpose Config" },
                    { 102, "Update Payment Purpose Config" },
                    { 101, "Create Payment Purpose Config" },
                    { 100, "View Payment Purpose Config" },
                    { 99, "Delete IdType Config" },
                    { 82, "Update Correspondent Bank" },
                    { 98, "Update IdType Config" },
                    { 96, "View IdType Config" },
                    { 95, "Delete Gender Config" },
                    { 94, "Update Gender Config" },
                    { 93, "Create Gender Config" },
                    { 92, "View Gender Config" },
                    { 91, "Delete Marital Status Config" },
                    { 90, "Update Marital Status Config" },
                    { 89, "Create Marital Status Config" },
                    { 81, "Create Correspondent Bank" },
                    { 87, "Delete Relationship Config" },
                    { 86, "Update Relationship Config" },
                    { 85, "Create Relationship Config" },
                    { 84, "View Relationship Config" },
                    { 97, "Create IdType Config" },
                    { 80, "View Correspondent Bank" }
                });

            migrationBuilder.InsertData(
                table: "PermissionGroup",
                columns: new[] { "Id", "CaseFormId", "CreatedAt", "CreatedBy", "Name", "PermissionIds", "TenantId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 27, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "RateSupplierFeeConfig", "104,105,106,107", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 21, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "CorrespondentBank", "80,81,82,83", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 14, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "CountryConfig", "51,52,53,54", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 15, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "CountryPayoutMode", "55,56,57,58", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 16, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "BankMapper", "60,61,62,63", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 17, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "BranchMapper", "64,65,66,67", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 18, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "CityConfig", "68,69,70,71", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 19, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "RateSupplier", "72,73,74,75", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 20, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "ExchangeRates", "76,77,78,79", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 28, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "TxnLimitConfig", "108,109,110,111", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 22, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "RelationshipConfig", "84,85,86,87", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 23, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "MaritalStatusConfig", "88,89,90,91", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 24, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "GenderConfig", "92,93,94,95", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 25, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "IdTypeConfig", "96,97,98,99", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 26, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PaymentPurposeConfig", "100,101,102,103", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
                }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionGroup");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17d59ab8-a192-4537-a625-3227c8e57ef7",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "0ed60e11-8c45-4934-b0c3-86207c874046", new DateTime(2020, 4, 17, 18, 5, 41, 753, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 753, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca4de2af-8d15-4c49-bc12-0a22d7cdf43d",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "0ed60e11-8c45-4934-b0c3-86207c874046", new DateTime(2020, 4, 17, 18, 5, 41, 753, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 753, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 750, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 107,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 108,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 109,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 110,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 111,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 112,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 113,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 114,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 115,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 116,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 117,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 118,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 119,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 120,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 121,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 122,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 123,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 124,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 125,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 126,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 127,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 128,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 129,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 130,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 131,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 132,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 133,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 134,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 135,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 136,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 137,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 138,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 139,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 140,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 141,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 142,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 143,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 144,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 145,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 146,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 147,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 148,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 149,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 150,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 151,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 152,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 153,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 154,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 155,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 156,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 157,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 158,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 159,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 161,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 162,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 163,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 164,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 166,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 167,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 168,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 169,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 170,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 171,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 172,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 173,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 174,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 175,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 176,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 177,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 178,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 179,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 180,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 181,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 182,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 191,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 192,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 193,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 194,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 195,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 196,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 197,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 213,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 214,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 215,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 216,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 223,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 224,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 225,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 226,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 227,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 228,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 229,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 230,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 231,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 232,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 233,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 234,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 235,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 236,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 237,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 238,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 239,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 240,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 241,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 242,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 243,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 244,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 245,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 246,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "CountryList",
                keyColumn: "Id",
                keyValue: 247,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 751, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Gender",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 752, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 752, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Gender",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 752, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 752, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 750, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 749, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 750, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 750, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2020, 4, 17, 18, 5, 41, 750, DateTimeKind.Local), new DateTime(2020, 4, 17, 18, 5, 41, 750, DateTimeKind.Local) });
        }
    }
}
