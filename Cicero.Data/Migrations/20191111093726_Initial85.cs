using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial85 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workflow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaseFormId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    JsonData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowObject",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowObject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowPoint",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    FWObjectId = table.Column<string>(nullable: true),
                    LWObjectId = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowPoint", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workflow");

            migrationBuilder.DropTable(
                name: "WorkflowObject");

            migrationBuilder.DropTable(
                name: "WorkflowPoint");
        }
    }
}
