using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "Content", "CreatedAt", "Excerpt", "ParentId", "Slug", "Status", "Template", "TenantId", "Title", "Type", "UpdatedAt", "UserId", "Version" },
                values: new object[] { 19, "<p>Hi, (user_name)!</p><h3> Please reset your password </h3><p> Click here to reset you password(reset_link)</p><p><br/> Regards,<br/> Cicero Team </p>", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, null, (short)0, null, null, "forgot-password-email", "template", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "Content", "CreatedAt", "Excerpt", "ParentId", "Slug", "Status", "Template", "TenantId", "Title", "Type", "UpdatedAt", "UserId", "Version" },
                values: new object[] { 20, "<p>Hi, (user_name)!</p><br/> Regards,<br/> Cicero Team </p>", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, null, (short)0, null, null, "claim-email-notification", "template", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "Content", "CreatedAt", "Excerpt", "ParentId", "Slug", "Status", "Template", "TenantId", "Title", "Type", "UpdatedAt", "UserId", "Version" },
                values: new object[] { 21, "<p>Hi, (user_name)!</p><br/> Regards,<br/> Cicero Team </p>", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 0, null, (short)0, null, null, "claim-filed-against-email", "template", new DateTime(2018, 9, 5, 16, 20, 30, 0, DateTimeKind.Unspecified), null, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 21);
        }
    }
}
