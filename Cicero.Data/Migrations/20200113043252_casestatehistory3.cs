using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class casestatehistory3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseStateHistory_User_UpdatedBy",
                table: "CaseStateHistory");

            migrationBuilder.DropIndex(
                name: "IX_CaseStateHistory_UpdatedBy",
                table: "CaseStateHistory");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CaseStateHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "CaseStateHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseStateHistory_UserId",
                table: "CaseStateHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseStateHistory_User_UserId",
                table: "CaseStateHistory",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseStateHistory_User_UserId",
                table: "CaseStateHistory");

            migrationBuilder.DropIndex(
                name: "IX_CaseStateHistory_UserId",
                table: "CaseStateHistory");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CaseStateHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "CaseStateHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseStateHistory_UpdatedBy",
                table: "CaseStateHistory",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseStateHistory_User_UpdatedBy",
                table: "CaseStateHistory",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
