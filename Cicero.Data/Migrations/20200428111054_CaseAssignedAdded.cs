using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class CaseAssignedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "Case",
                type: "datetime2(3)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForAssignment",
                table: "Case",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnEligibleAt",
                table: "Case",
                type: "datetime2(3)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "ForAssignment",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "UnEligibleAt",
                table: "Case");
        }
    }
}
