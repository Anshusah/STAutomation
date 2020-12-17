using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_CaseForm_ClaimTypeId",
                table: "Case");

            migrationBuilder.RenameColumn(
                name: "ClaimTypeId",
                table: "Case",
                newName: "CaseFormId");

            migrationBuilder.RenameIndex(
                name: "IX_Case_ClaimTypeId",
                table: "Case",
                newName: "IX_Case_CaseFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_CaseForm_CaseFormId",
                table: "Case",
                column: "CaseFormId",
                principalTable: "CaseForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_CaseForm_CaseFormId",
                table: "Case");

            migrationBuilder.RenameColumn(
                name: "CaseFormId",
                table: "Case",
                newName: "ClaimTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Case_CaseFormId",
                table: "Case",
                newName: "IX_Case_ClaimTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_CaseForm_ClaimTypeId",
                table: "Case",
                column: "ClaimTypeId",
                principalTable: "CaseForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
