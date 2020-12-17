using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_Claim_ClaimTypeId",
                table: "Case");

            migrationBuilder.DropTable(
                name: "Claim");

            migrationBuilder.DropIndex(
                name: "IX_Case_ClaimTypeId",
                table: "Case");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Setting",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaseClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "varchar(100)", nullable: true),
                    ClaimFields = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseClaim", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Setting_TenantId",
                table: "Setting",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Tenant_TenantId",
                table: "Setting",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Tenant_TenantId",
                table: "Setting");

            migrationBuilder.DropTable(
                name: "CaseClaim");

            migrationBuilder.DropIndex(
                name: "IX_Setting_TenantId",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Setting");

            migrationBuilder.CreateTable(
                name: "Claim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimFields = table.Column<string>(nullable: true),
                    ClaimType = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claim", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Case_ClaimTypeId",
                table: "Case",
                column: "ClaimTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_Claim_ClaimTypeId",
                table: "Case",
                column: "ClaimTypeId",
                principalTable: "Claim",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
