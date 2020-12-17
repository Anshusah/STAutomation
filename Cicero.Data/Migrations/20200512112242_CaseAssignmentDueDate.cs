using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class CaseAssignmentDueDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForAssignment",
                table: "Case");

            migrationBuilder.RenameColumn(
                name: "UnEligibleAt",
                table: "Case",
                newName: "DueDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Case",
                newName: "UnEligibleAt");

            migrationBuilder.AddColumn<bool>(
                name: "ForAssignment",
                table: "Case",
                nullable: false,
                defaultValue: false);
        }
    }
}
