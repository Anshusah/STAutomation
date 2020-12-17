using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _kycManualPass_added_customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KycFailedCount",
                table: "Customer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "KycManualPass",
                table: "Customer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "KycManualPassDate",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KycVerifiedDate",
                table: "Customer",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KycFailedCount",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "KycManualPass",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "KycManualPassDate",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "KycVerifiedDate",
                table: "Customer");

        }
    }
}
