using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Side",
                table: "Queue");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "State",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Queue",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "State");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Queue");

            migrationBuilder.AddColumn<string>(
                name: "Side",
                table: "Queue",
                nullable: true);
        }
    }
}
