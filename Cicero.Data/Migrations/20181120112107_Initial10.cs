using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_CaseClaim_ClaimTypeId",
                table: "Case");

            migrationBuilder.DropTable(
                name: "CaseClaim");

            migrationBuilder.CreateTable(
                name: "CaseForm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Fields = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseForm_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseForm_TenantId",
                table: "CaseForm",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_CaseForm_ClaimTypeId",
                table: "Case",
                column: "ClaimTypeId",
                principalTable: "CaseForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_CaseForm_ClaimTypeId",
                table: "Case");

            migrationBuilder.DropTable(
                name: "CaseForm");

            migrationBuilder.CreateTable(
                name: "CaseClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Fields = table.Column<string>(nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseClaim_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseClaim_TenantId",
                table: "CaseClaim",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_CaseClaim_ClaimTypeId",
                table: "Case",
                column: "ClaimTypeId",
                principalTable: "CaseClaim",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
