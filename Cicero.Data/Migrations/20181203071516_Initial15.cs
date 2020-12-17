using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 5,
                column: "FieldValue",
                value: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 5,
                column: "FieldValue",
                value: "London");
        }
    }
}
