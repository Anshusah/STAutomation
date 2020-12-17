using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _onfido_url_id_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoId",
                table: "OnfidoApplicantLivePhoto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "OnfidoApplicantLivePhoto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentId",
                table: "OnfidoApplicantDocument",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "OnfidoApplicantDocument",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "OnfidoApplicantLivePhoto");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "OnfidoApplicantLivePhoto");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "OnfidoApplicantDocument");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "OnfidoApplicantDocument");

        }
    }
}
