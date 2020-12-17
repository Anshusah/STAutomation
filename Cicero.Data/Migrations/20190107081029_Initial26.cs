using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ActivityLog",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClaimId",
                table: "ActivityLog",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "ActivityLog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_ClaimId",
                table: "ActivityLog",
                column: "ClaimId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_Case_ClaimId",
                table: "ActivityLog",
                column: "ClaimId",
                principalTable: "Case",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_Case_ClaimId",
                table: "ActivityLog");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLog_ClaimId",
                table: "ActivityLog");

            migrationBuilder.DropColumn(
                name: "ClaimId",
                table: "ActivityLog");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "ActivityLog");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ActivityLog",
                type: "varchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);
        }
    }
}
