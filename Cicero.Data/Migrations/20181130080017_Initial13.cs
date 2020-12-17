using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PolicyNumber",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 19,
                column: "Content",
                value: "<p>Hi, [user_name]!</p><h3> Please reset your password </h3><p> Click here to reset you password[reset_link]</p><p><br/> Regards,<br/> Cicero Team </p>");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 20,
                column: "Content",
                value: "<p>Hi, [user_name]!</p><br/> Regards,<br/> Cicero Team </p>");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 21,
                column: "Content",
                value: "<p>Hi, [user_name]!</p><br/> Regards,<br/> Cicero Team </p>");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 22,
                column: "Content",
                value: "<p>Hi, [user_name]!</p><br/> Regards,<br/> Cicero Team </p>");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyNumber",
                table: "Case");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 19,
                column: "Content",
                value: "<p>Hi, (user_name)!</p><h3> Please reset your password </h3><p> Click here to reset you password(reset_link)</p><p><br/> Regards,<br/> Cicero Team </p>");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 20,
                column: "Content",
                value: "<p>Hi, (user_name)!</p><br/> Regards,<br/> Cicero Team </p>");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 21,
                column: "Content",
                value: "<p>Hi, (user_name)!</p><br/> Regards,<br/> Cicero Team </p>");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 22,
                column: "Content",
                value: "<p>Hi, (user_name)!</p><br/> Regards,<br/> Cicero Team </p>");
        }
    }
}
