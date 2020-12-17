using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class transfastsoftable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransfastSourceOfFund",
                table: "TransfastSourceOfFund");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransfastRemittancePurpose",
                table: "TransfastRemittancePurpose");

            migrationBuilder.DropColumn(
                name: "TransfastId",
                table: "TransfastSourceOfFund");

            migrationBuilder.DropColumn(
                name: "TransfastId",
                table: "TransfastRemittancePurpose");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TransfastSourceOfFund",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TransfastRemittancePurpose",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransfastSourceOfFund",
                table: "TransfastSourceOfFund",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransfastRemittancePurpose",
                table: "TransfastRemittancePurpose",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 9, 14, 16, 36, 50, 238, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2020, 9, 14, 16, 36, 50, 239, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2020, 9, 14, 16, 36, 50, 239, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransfastSourceOfFund",
                table: "TransfastSourceOfFund");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransfastRemittancePurpose",
                table: "TransfastRemittancePurpose");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TransfastSourceOfFund");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TransfastRemittancePurpose");

            migrationBuilder.AddColumn<int>(
                name: "TransfastId",
                table: "TransfastSourceOfFund",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "TransfastId",
                table: "TransfastRemittancePurpose",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransfastSourceOfFund",
                table: "TransfastSourceOfFund",
                column: "TransfastId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransfastRemittancePurpose",
                table: "TransfastRemittancePurpose",
                column: "TransfastId");

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 9, 14, 16, 34, 53, 96, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2020, 9, 14, 16, 34, 53, 97, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2020, 9, 14, 16, 34, 53, 97, DateTimeKind.Local));
        }
    }
}
