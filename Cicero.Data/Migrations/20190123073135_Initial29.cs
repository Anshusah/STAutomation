using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "Queue",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Queue_RoleId",
                table: "Queue",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queue_Role_RoleId",
                table: "Queue",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queue_Role_RoleId",
                table: "Queue");

            migrationBuilder.DropIndex(
                name: "IX_Queue_RoleId",
                table: "Queue");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Queue");
        }
    }
}
