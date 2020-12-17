using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial48 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermissionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    PermissionIds = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroup", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PermissionGroup",
                columns: new[] { "Id", "Name", "PermissionIds" },
                values: new object[,]
                {
                    { 1, "Claim", "1,2,3,4" },
                    { 2, "User", "5,6,7,8,9" },
                    { 3, "Article", "10,11,12,13" },
                    { 4, "Media", "14,15,16,17" },
                    { 5, "Menu", "18,19,20,21" },
                    { 6, "Role", "22,23,24,25" },
                    { 7, "Setting", "26,27,28,29" },
                    { 8, "Message", "30,31,32,33" },
                    { 9, "Queue", "34,35,36,37" },
                    { 10, "Form", "38,39,40,41" },
                    { 11, "Tenant", "42" },
                    { 12, "Dashboard Layout", "43,44" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionGroup");
        }
    }
}
