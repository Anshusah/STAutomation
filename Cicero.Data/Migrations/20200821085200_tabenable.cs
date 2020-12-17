using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class tabenable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TabEnable",
                table: "CaseForm",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(type: "varchar(100)", nullable: true),
                    KeyId = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductConfig_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scoreing",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PoolName = table.Column<string>(type: "varchar(500)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(500)", nullable: true),
                    Parameter = table.Column<string>(nullable: true),
                    Expression = table.Column<string>(nullable: true),
                    CaseFormId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scoreing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scoreing_ProductConfig_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 1,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 2,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 3,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 4,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 5,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 6,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 7,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 8,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 9,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 10,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 11,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 12,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 13,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 14,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 15,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 16,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 17,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 18,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 19,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 20,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 21,
                column: "RecipientType",
                value: "0");

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 22,
                column: "RecipientType",
                value: "0");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConfig_TenantId",
                table: "ProductConfig",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Scoreing_ProductId",
                table: "Scoreing",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scoreing");

            migrationBuilder.DropTable(
                name: "ProductConfig");

          

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 1,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 2,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 3,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 4,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 5,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 6,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 7,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 8,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 9,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 10,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 11,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 12,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 13,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 14,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 15,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 16,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 17,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 18,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 19,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 20,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 21,
                column: "RecipientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 22,
                column: "RecipientType",
                value: null);
        }
    }
}
