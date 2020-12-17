using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Case_ClaimTypeId",
                table: "Case",
                column: "ClaimTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_CaseClaim_ClaimTypeId",
                table: "Case",
                column: "ClaimTypeId",
                principalTable: "CaseClaim",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_CaseClaim_ClaimTypeId",
                table: "Case");

            migrationBuilder.DropIndex(
                name: "IX_Case_ClaimTypeId",
                table: "Case");
        }
    }
}
