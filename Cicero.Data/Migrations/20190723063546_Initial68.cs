using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial68 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QueuePosY",
                table: "QueueToState",
                newName: "PosY");

            migrationBuilder.RenameColumn(
                name: "QueuePosX",
                table: "QueueToState",
                newName: "PosX");

            migrationBuilder.AddColumn<bool>(
                name: "IsQueue",
                table: "QueueToState",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsQueue",
                table: "QueueToState");

            migrationBuilder.RenameColumn(
                name: "PosY",
                table: "QueueToState",
                newName: "QueuePosY");

            migrationBuilder.RenameColumn(
                name: "PosX",
                table: "QueueToState",
                newName: "QueuePosX");
        }
    }
}
