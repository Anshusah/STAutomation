using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class simpletransfer_addtableratesupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RateSupplier",
                columns: table => new
                {
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    SystemId = table.Column<string>(nullable: true),
                    RatePriority = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateSupplier", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RateSupplier",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Name", "Password", "RatePriority", "SystemId", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Tranfast", "P@ssw0rd", 0, "1AB804F0-9252-4A7E-885A-276B65540D84", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "transfast" });

            migrationBuilder.InsertData(
                table: "RateSupplier",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Name", "Password", "RatePriority", "SystemId", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "NecMoney", "P@ssw0rd", 0, "9D6D65E4-EFF1-4752-892D-3F637A56C159", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "necmoney" });

            migrationBuilder.InsertData(
                table: "RateSupplier",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Name", "Password", "RatePriority", "SystemId", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Safkhan", "P@ssw0rd", 0, "D93C65D4-7AA9-4A84-B23F-BEBB8D324476", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "safkhan" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RateSupplier");
        }
    }
}
