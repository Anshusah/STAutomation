using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissionGroupId",
                table: "RolePermission",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PermissionGroup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PermissionGroup",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "PermissionGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PermissionGroup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PermissionGroup",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionGroupId",
                table: "RolePermission");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PermissionGroup");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PermissionGroup");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PermissionGroup");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PermissionGroup");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PermissionGroup");
        }
    }
}
