using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _remarks_sanctionpep_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "SanctionPepCustomer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "SanctionPepBeneficiary",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PepMatch",
                table: "LexisNexis",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SanctionMatch",
                table: "LexisNexis",
                nullable: false,
                defaultValue: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "SanctionPepCustomer");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "SanctionPepBeneficiary");

            migrationBuilder.DropColumn(
                name: "PepMatch",
                table: "LexisNexis");

            migrationBuilder.DropColumn(
                name: "SanctionMatch",
                table: "LexisNexis");

        }
    }
}
