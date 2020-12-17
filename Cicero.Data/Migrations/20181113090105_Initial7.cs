using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "Content", "CreatedAt", "Excerpt", "ParentId", "Slug", "Status", "Template", "TenantId", "Title", "Type", "UpdatedAt", "UserId", "Version" },
                values: new object[] { 22, "<p>Hi, (user_name)!</p><br/> Regards,<br/> Cicero Team </p>", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, null, (short)0, null, null, "subrogation_letter", "template", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 22);
        }
    }
}
