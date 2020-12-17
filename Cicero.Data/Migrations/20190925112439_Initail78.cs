using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initail78 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actions_Tenant_TenantId",
                table: "Actions");

            migrationBuilder.DropIndex(
                name: "IX_Actions_TenantId",
                table: "Actions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Actions_TenantId",
                table: "Actions",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_Tenant_TenantId",
                table: "Actions",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
