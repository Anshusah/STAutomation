using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial69 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QueueToState",
                table: "QueueToState");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "QueueToState",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Ids",
                table: "QueueToState",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueueToState",
                table: "QueueToState",
                column: "Ids");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QueueToState",
                table: "QueueToState");

            migrationBuilder.DropColumn(
                name: "Ids",
                table: "QueueToState");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "QueueToState",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueueToState",
                table: "QueueToState",
                column: "Id");
        }
    }
}
