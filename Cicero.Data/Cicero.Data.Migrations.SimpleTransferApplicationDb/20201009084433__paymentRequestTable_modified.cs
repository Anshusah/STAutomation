using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _paymentRequestTable_modified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "PaymentRequest",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "PaymentRequest",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PayerName",
                table: "PaymentRequest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReferenceNumber",
                table: "PaymentRequest",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "PaymentRequest",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "PaymentRequest");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "PaymentRequest");

            migrationBuilder.DropColumn(
                name: "PayerName",
                table: "PaymentRequest");

            migrationBuilder.DropColumn(
                name: "PaymentReferenceNumber",
                table: "PaymentRequest");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "PaymentRequest");

        }
    }
}
