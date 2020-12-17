using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class AuditlogFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTimeStamp",
                table: "AuditLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuditLog",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManual",
                table: "AuditLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTimeStamp",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "IsManual",
                table: "AuditLog");
        }
    }
}
