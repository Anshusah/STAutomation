using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                table: "ActivityLog",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_StateId",
                table: "ActivityLog",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_State_StateId",
                table: "ActivityLog",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_State_StateId",
                table: "ActivityLog");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLog_StateId",
                table: "ActivityLog");

            migrationBuilder.AlterColumn<int>(
                name: "StateId",
                table: "ActivityLog",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
