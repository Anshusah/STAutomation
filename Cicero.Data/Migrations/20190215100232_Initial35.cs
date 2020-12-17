using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "InvolvedParties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedParties_CaseId",
                table: "InvolvedParties",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvolvedParties_Case_CaseId",
                table: "InvolvedParties",
                column: "CaseId",
                principalTable: "Case",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvolvedParties_Case_CaseId",
                table: "InvolvedParties");

            migrationBuilder.DropIndex(
                name: "IX_InvolvedParties_CaseId",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "InvolvedParties");
        }
    }
}
