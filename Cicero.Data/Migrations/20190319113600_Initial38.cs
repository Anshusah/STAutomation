using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "InvolvedParties");

            migrationBuilder.CreateTable(
                name: "PolicyManagement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Fields = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyManagement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyManagement_CaseForm_CaseFormId",
                        column: x => x.CaseFormId,
                        principalTable: "CaseForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PolicyManagement_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "FieldDisplay", "FieldKey" },
                values: new object[] { "Starting Claim for Claimant", "app_claim_front" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldType" },
                values: new object[] { "Starting Claim for Back office", "app_claim_back", "TENANTCLAIM" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldType", "FieldValue" },
                values: new object[] { "Role for register User in front office", "app_user_role", "USERROLE", "" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldValue" },
                values: new object[] { "Url", "app_url", "http://52.228.24.65/" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldValue" },
                values: new object[] { "Facebook Url", "app_facebook", "http://facebook.com" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "FieldDisplay", "FieldGridSize", "FieldKey", "FieldValue", "FieldVisiblity" },
                values: new object[] { "Twitter Url", "6", "app_twitter", "http://twitter.com", 1 });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldValue" },
                values: new object[] { "Navigation - Primary", "Primary", "[{\"index\":0,\"menu\":\"Home\",\"type\":\"custom\",\"url\":\"/\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]}]" });

            migrationBuilder.InsertData(
                table: "Setting",
                columns: new[] { "Id", "FieldDisplay", "FieldGridSize", "FieldKey", "FieldOptions", "FieldType", "FieldValue", "FieldVisiblity", "TenantId" },
                values: new object[,]
                {
                    { 13, "Navigation - Bottom", null, "Bottom", null, "TEXTBOX", "[{\"index\":0,\"menu\":\"About Cicero\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Claim Process\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":3,\"menu\":\"Blogs\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":4,\"menu\":\"Careers\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":1,\"menu\":\"Legals\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Terms and Conditions\",\"type\":\"article\",\"url\":\"1\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Terms and Conditions\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":2,\"menu\":\"Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Contact Us\",\"type\":\"article\",\"url\":\"18\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Contact Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Feedback\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Help & Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]}]", 0, null },
                    { 14, null, null, "app_themes", null, null, "[{\"index\":0,\"menu\":\"About Cicero\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Claim Process\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":3,\"menu\":\"Blogs\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":4,\"menu\":\"Careers\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":1,\"menu\":\"Legals\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Terms and Conditions\",\"type\":\"article\",\"url\":\"1\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Terms and Conditions\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":2,\"menu\":\"Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Contact Us\",\"type\":\"article\",\"url\":\"18\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Contact Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Feedback\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Help & Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]}]", 0, null },
                    { 15, "Theme", "6", "app_theme", null, "TENANTTHEME", "Test", 1, null },
                    { 16, "Sync Case", "12", "app_case_synchronization", null, "CASESYNCHRONIZATION", null, 1, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyManagement_CaseFormId",
                table: "PolicyManagement",
                column: "CaseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyManagement_TenantId",
                table: "PolicyManagement",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyManagement");

            migrationBuilder.DeleteData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "InvolvedParties",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "FieldDisplay", "FieldKey" },
                values: new object[] { "Starting Claim", "app_claim" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldType" },
                values: new object[] { "Role for register User", "app_user_role", "USERROLE" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldType", "FieldValue" },
                values: new object[] { "Url", "app_url", "TEXTBOX", "http://52.228.24.65/" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldValue" },
                values: new object[] { "Facebook Url", "app_facebook", "http://facebook.com" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldValue" },
                values: new object[] { "Twitter Url", "app_twitter", "http://twitter.com" });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "FieldDisplay", "FieldGridSize", "FieldKey", "FieldValue", "FieldVisiblity" },
                values: new object[] { "Navigation - Primary", null, "Primary", "[{\"index\":0,\"menu\":\"Home\",\"type\":\"custom\",\"url\":\"/\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]}]", 0 });

            migrationBuilder.UpdateData(
                table: "Setting",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "FieldDisplay", "FieldKey", "FieldValue" },
                values: new object[] { "Navigation - Bottom", "Bottom", "[{\"index\":0,\"menu\":\"About Cicero\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"About Us\",\"type\":\"article\",\"url\":\"2\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"About Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Claim Process\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":3,\"menu\":\"Blogs\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":4,\"menu\":\"Careers\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":1,\"menu\":\"Legals\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Terms and Conditions\",\"type\":\"article\",\"url\":\"1\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Terms and Conditions\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Privacy Policy\",\"type\":\"article\",\"url\":\"3\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Privacy Policy\",\"target\":\"off\",\"childrens\":[]}]},{\"index\":2,\"menu\":\"Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[{\"index\":0,\"menu\":\"Contact Us\",\"type\":\"article\",\"url\":\"18\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"Contact Us\",\"target\":\"off\",\"childrens\":[]},{\"index\":1,\"menu\":\"Feedback\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]},{\"index\":2,\"menu\":\"Help & Support\",\"type\":\"custom\",\"url\":\"#\",\"desc\":\"\",\"css_class\":\"\",\"url_title\":\"\",\"target\":\"off\",\"childrens\":[]}]}]" });
        }
    }
}
