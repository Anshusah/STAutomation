using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class casestatehistory1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CaseStateHistory_CurrentStateId",
                table: "CaseStateHistory",
                column: "CurrentStateId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseStateHistory_PreviousStateId",
                table: "CaseStateHistory",
                column: "PreviousStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseStateHistory_State_CurrentStateId",
                table: "CaseStateHistory",
                column: "CurrentStateId",
                principalTable: "State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseStateHistory_State_PreviousStateId",
                table: "CaseStateHistory",
                column: "PreviousStateId",
                principalTable: "State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseStateHistory_State_CurrentStateId",
                table: "CaseStateHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseStateHistory_State_PreviousStateId",
                table: "CaseStateHistory");

            migrationBuilder.DropIndex(
                name: "IX_CaseStateHistory_CurrentStateId",
                table: "CaseStateHistory");

            migrationBuilder.DropIndex(
                name: "IX_CaseStateHistory_PreviousStateId",
                table: "CaseStateHistory");
        }
    }
}
