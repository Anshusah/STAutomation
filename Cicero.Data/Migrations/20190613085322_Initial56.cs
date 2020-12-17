using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial56 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToPosY",
                table: "StateToState",
                newName: "StatePosY");

            migrationBuilder.RenameColumn(
                name: "ToPosX",
                table: "StateToState",
                newName: "StatePosX");

            migrationBuilder.RenameColumn(
                name: "FromPosY",
                table: "StateToState",
                newName: "LinePosY");

            migrationBuilder.RenameColumn(
                name: "FromPosX",
                table: "StateToState",
                newName: "LinePosX");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatePosY",
                table: "StateToState",
                newName: "ToPosY");

            migrationBuilder.RenameColumn(
                name: "StatePosX",
                table: "StateToState",
                newName: "ToPosX");

            migrationBuilder.RenameColumn(
                name: "LinePosY",
                table: "StateToState",
                newName: "FromPosY");

            migrationBuilder.RenameColumn(
                name: "LinePosX",
                table: "StateToState",
                newName: "FromPosX");
        }
    }
}
