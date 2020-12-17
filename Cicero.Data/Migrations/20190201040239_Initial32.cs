using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HasChildren",
                table: "Case",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HasChildren",
                table: "Case",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
