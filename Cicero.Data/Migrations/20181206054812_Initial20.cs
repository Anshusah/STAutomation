using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoledId",
                table: "State");

            migrationBuilder.AlterColumn<float>(
                name: "Excess",
                table: "Case",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoledId",
                table: "State",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Excess",
                table: "Case",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
