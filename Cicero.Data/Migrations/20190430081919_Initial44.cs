using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "State");

            migrationBuilder.AddColumn<string>(
                name: "Extras",
                table: "State",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extras",
                table: "State");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "State",
                type: "varchar(50)",
                nullable: true);
        }
    }
}
