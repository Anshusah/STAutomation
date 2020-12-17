using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial67 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QueueToState",
                table: "QueueToState");

            migrationBuilder.DropColumn(
                name: "Ids",
                table: "QueueToState");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "QueueToState",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueueToState",
                table: "QueueToState",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QueueToState",
                table: "QueueToState");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "QueueToState");

            migrationBuilder.AddColumn<int>(
                name: "Ids",
                table: "QueueToState",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueueToState",
                table: "QueueToState",
                column: "Ids");
        }
    }
}
