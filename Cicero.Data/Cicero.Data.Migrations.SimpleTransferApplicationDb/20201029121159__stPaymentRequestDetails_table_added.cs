using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _stPaymentRequestDetails_table_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayeeCountry",
                table: "STPaymentRequest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerName",
                table: "STPaymentRequest",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "STPaymentRequestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    STPaymentRequestId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PurposeOfRequest = table.Column<int>(nullable: false),
                    Bank = table.Column<string>(nullable: true),
                    Branch = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    Invoice = table.Column<string>(nullable: true),
                    Reminder = table.Column<string>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STPaymentRequestDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STPaymentRequestDetails");

            migrationBuilder.DropColumn(
                name: "PayeeCountry",
                table: "STPaymentRequest");

            migrationBuilder.DropColumn(
                name: "PayerName",
                table: "STPaymentRequest");
        }
    }
}
