using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class ArticleEmailDetailsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientDatabaseTable",
                table: "Article",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientField",
                table: "Article",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientForm",
                table: "Article",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientType",
                table: "Article",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Article",
                type: "varchar(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientDatabaseTable",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "RecipientField",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "RecipientForm",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "RecipientType",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Article");
        }
    }
}
