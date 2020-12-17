using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial94 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "MailMergeField",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "MailMergeField",
                keyColumn: "Id",
                keyValue: 7,
                column: "FieldName",
                value: "Role Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "MailMergeField");

            migrationBuilder.UpdateData(
                table: "MailMergeField",
                keyColumn: "Id",
                keyValue: 7,
                column: "FieldName",
                value: "RoleName");
        }
    }
}
