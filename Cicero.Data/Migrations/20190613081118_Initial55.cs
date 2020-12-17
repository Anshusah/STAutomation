using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial55 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaseFormId",
                table: "StateToState",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FromPosX",
                table: "StateToState",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromPosY",
                table: "StateToState",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToPosX",
                table: "StateToState",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToPosY",
                table: "StateToState",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseFormId",
                table: "StateToState");

            migrationBuilder.DropColumn(
                name: "FromPosX",
                table: "StateToState");

            migrationBuilder.DropColumn(
                name: "FromPosY",
                table: "StateToState");

            migrationBuilder.DropColumn(
                name: "ToPosX",
                table: "StateToState");

            migrationBuilder.DropColumn(
                name: "ToPosY",
                table: "StateToState");
        }
    }
}
