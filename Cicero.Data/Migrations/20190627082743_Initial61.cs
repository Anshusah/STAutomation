using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial61 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StateToState",
                table: "StateToState");

            migrationBuilder.DropColumn(
                name: "Ids",
                table: "StateToState");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StateToState",
                table: "StateToState",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StateToState",
                table: "StateToState");

            migrationBuilder.AddColumn<int>(
                name: "Ids",
                table: "StateToState",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StateToState",
                table: "StateToState",
                column: "Ids");
        }
    }
}
