using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "Media",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvolvementType",
                table: "InvolvedParties",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SignatureDate",
                table: "Case",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Media_CaseId",
                table: "Media",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Case_CaseId",
                table: "Media",
                column: "CaseId",
                principalTable: "Case",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Case_CaseId",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_CaseId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "InvolvementType",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "SignatureDate",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "Case");
        }
    }
}
