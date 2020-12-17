using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial92 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailMergeField",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FieldName = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(nullable: true),
                    DbSourceTable = table.Column<string>(nullable: true),
                    DbSourceField = table.Column<string>(nullable: true),
                    TemplateType = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailMergeField", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MailMergeField",
                columns: new[] { "Id", "Alias", "DbSourceField", "DbSourceTable", "FieldName", "TemplateType", "TenantId" },
                values: new object[,]
                {
                    { 1, "[First_Name]", "FirstName", "User", "First Name", 1, 1 },
                    { 2, "[Last_Name]", "LastName", "User", "Last Name", 1, 1 },
                    { 3, "[Email]", "Email", "User", "Email", 1, 1 },
                    { 4, "[Updated_Date]", "UpdatedAt", "User", "Updated Date", 1, 1 },
                    { 5, "[Address]", "Address", "User", "Address", 1, 1 },
                    { 6, "[Phone_Number]", "PhoneNumber", "User", "Phone Number", 1, 1 },
                    { 7, "[Role_Name]", "DisplayName", "Role", "RoleName", 1, 1 },
                    { 8, "[Form_Name]", "Name", "CaseForm", "Form Name", 2, 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailMergeField");
        }
    }
}
