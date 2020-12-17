using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
             name: "IX_Queue_RoleId",
             table: "Queue");
            migrationBuilder.DropForeignKey(
                name: "FK_Queue_Role_RoleId",
                table: "Queue");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "Queue",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "Queue",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
