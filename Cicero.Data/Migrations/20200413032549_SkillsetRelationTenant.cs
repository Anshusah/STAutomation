using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class SkillsetRelationTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SkillSet",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SkillSet",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SkillSet",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "CaseLimit",
                table: "SkillSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "SkillSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SkillSet_TenantId",
                table: "SkillSet",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillSet_Tenant_TenantId",
                table: "SkillSet",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillSet_Tenant_TenantId",
                table: "SkillSet");

            migrationBuilder.DropIndex(
                name: "IX_SkillSet_TenantId",
                table: "SkillSet");

            migrationBuilder.DropColumn(
                name: "CaseLimit",
                table: "SkillSet");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SkillSet");

            migrationBuilder.InsertData(
                table: "SkillSet",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Title", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified), null, true, "Expert", new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SkillSet",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Title", "UpdatedAt" },
                values: new object[] { 2, new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified), null, true, "Intermediate", new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SkillSet",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Title", "UpdatedAt" },
                values: new object[] { 3, new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified), null, true, "Beginner", new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified) });
        }
    }
}
