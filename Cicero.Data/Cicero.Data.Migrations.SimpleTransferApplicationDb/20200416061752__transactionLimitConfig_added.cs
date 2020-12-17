using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _transactionLimitConfig_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionLimitConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    CountryCode = table.Column<string>(nullable: true),
                    LimitAmountPerTxn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LimitAmountPerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LimitAmountPerMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LimitNoPerDay = table.Column<int>(nullable: false),
                    LimitNoPerMonth = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(500)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(500)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLimitConfig", x => x.Id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionLimitConfig");

        }
    }
}
