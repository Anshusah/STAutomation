using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "State",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserAccess",
                table: "State",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Role",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "InvolvedParties",
                type: "varchar(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "InvolvedParties",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdDocument",
                table: "InvolvedParties",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdNumber",
                table: "InvolvedParties",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccessId",
                table: "Case",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "State");

            migrationBuilder.DropColumn(
                name: "UserAccess",
                table: "State");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "IdDocument",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "UserAccessId",
                table: "Case");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "InvolvedParties",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldNullable: true);
        }
    }
}
