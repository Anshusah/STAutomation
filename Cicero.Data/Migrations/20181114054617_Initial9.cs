using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClaimType",
                table: "CaseClaim",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ClaimFields",
                table: "CaseClaim",
                newName: "UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CaseClaim",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Fields",
                table: "CaseClaim",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "CaseClaim",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CaseClaim",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_CaseClaim_TenantId",
                table: "CaseClaim",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseClaim_Tenant_TenantId",
                table: "CaseClaim",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseClaim_Tenant_TenantId",
                table: "CaseClaim");

            migrationBuilder.DropIndex(
                name: "IX_CaseClaim_TenantId",
                table: "CaseClaim");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CaseClaim");

            migrationBuilder.DropColumn(
                name: "Fields",
                table: "CaseClaim");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CaseClaim");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CaseClaim");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CaseClaim",
                newName: "ClaimFields");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CaseClaim",
                newName: "ClaimType");
        }
    }
}
