using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initail76 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UrlIdentifier = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    TemplateId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actions_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionsReceiver",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActionsId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionsReceiver", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionsReceiver_Actions_ActionsId",
                        column: x => x.ActionsId,
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionsSender",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActionsId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionsSender", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionsSender_Actions_ActionsId",
                        column: x => x.ActionsId,
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_TenantId",
                table: "Actions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsReceiver_ActionsId",
                table: "ActionsReceiver",
                column: "ActionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsSender_ActionsId",
                table: "ActionsSender",
                column: "ActionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionsReceiver");

            migrationBuilder.DropTable(
                name: "ActionsSender");

            migrationBuilder.DropTable(
                name: "Actions");
        }
    }
}
