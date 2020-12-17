using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _sanctionPep_table_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           // migrationBuilder.AddColumn<string>(
             //   name: "TabEnable",
               // table: "CaseForm",
                //nullable: true);

            migrationBuilder.CreateTable(
                name: "LexisNexis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    LexisNexisId = table.Column<string>(nullable: true),
                    Ikey = table.Column<string>(nullable: true),
                    EquifaxUsername = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    ScoreCard = table.Column<int>(nullable: false),
                    ResultText = table.Column<string>(nullable: true),
                    ProfileUrl = table.Column<string>(nullable: true),
                    Credits = table.Column<int>(nullable: false),
                    UKLexIdField = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LexisNexis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SanctionPepBeneficiary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LexisNexisId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true),
                    TransactionType = table.Column<int>(nullable: false),
                    PepsType = table.Column<int>(nullable: false),
                    IsDeactivated = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsMatch = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanctionPepBeneficiary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SanctionPepCustomer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LexisNexisId = table.Column<int>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true),
                    TransactionType = table.Column<int>(nullable: false),
                    PepsType = table.Column<int>(nullable: false),
                    IsDeactivated = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsMatch = table.Column<bool>(nullable: false),
                    NewMatchVerified = table.Column<bool>(nullable: false),
                    PeriodicVerified = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanctionPepCustomer", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 12, 7, 14, 36, 33, 248, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LexisNexis");

            migrationBuilder.DropTable(
                name: "SanctionPepBeneficiary");

            migrationBuilder.DropTable(
                name: "SanctionPepCustomer");

            migrationBuilder.DropColumn(
                name: "TabEnable",
                table: "CaseForm");

            migrationBuilder.UpdateData(
                table: "RateSupplier",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2020, 11, 13, 11, 44, 38, 372, DateTimeKind.Local));
        }
    }
}
