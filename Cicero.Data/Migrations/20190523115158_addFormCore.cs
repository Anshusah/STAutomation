using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class addFormCore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoreCaseTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Fields = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    CaseFormId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreCaseTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreCaseTable_CaseForm_CaseFormId",
                        column: x => x.CaseFormId,
                        principalTable: "CaseForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoreCaseTable_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoreCaseTable_CaseFormId",
                table: "CoreCaseTable",
                column: "CaseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreCaseTable_TenantId",
                table: "CoreCaseTable",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoreCaseTable");
        }
    }
}
