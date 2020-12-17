using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameFrontend",
                table: "State",
                newName: "SystemName");

            migrationBuilder.RenameColumn(
                name: "NameBackend",
                table: "State",
                newName: "ActionName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SystemName",
                table: "State",
                newName: "NameFrontend");

            migrationBuilder.RenameColumn(
                name: "ActionName",
                table: "State",
                newName: "NameBackend");
        }
    }
}
