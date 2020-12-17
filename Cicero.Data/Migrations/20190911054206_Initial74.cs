using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial74 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FieldKey = table.Column<string>(type: "varchar(50)", nullable: true),
                    FieldValue = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    FieldDisplay = table.Column<string>(type: "varchar(200)", nullable: true),
                    FieldVisiblity = table.Column<int>(nullable: false),
                    ComponentType = table.Column<string>(type: "varchar(50)", nullable: true),
                    FieldOptions = table.Column<string>(type: "varchar(500)", nullable: true),
                    FieldGridSize = table.Column<string>(type: "varchar(200)", nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Component_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Component_TenantId",
                table: "Component",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Component");
        }
    }
}
