using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial73 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Default",
                table: "CaseForm",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Case",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "CaseForm");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Case");
        }
    }
}
