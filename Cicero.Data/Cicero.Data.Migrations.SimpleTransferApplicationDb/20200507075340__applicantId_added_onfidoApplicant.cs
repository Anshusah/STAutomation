using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _applicantId_added_onfidoApplicant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "ApplicantId",
                table: "OnfidoApplicant",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "OnfidoApplicant");

            migrationBuilder.AddColumn<string>(
                name: "ApplicantId",
                table: "Customer",
                nullable: true);

        }
    }
}
