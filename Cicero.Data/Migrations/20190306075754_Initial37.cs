using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "InvolvedParties");

            migrationBuilder.DropColumn(
                name: "AdditionalFirstName",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "AdditionalLastName",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "AdditionalPolicyHolder",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Address3",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Agent",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Case");

            migrationBuilder.RenameColumn(
                name: "RelationshipToPolicyHolder",
                table: "Case",
                newName: "ContactDay");

            migrationBuilder.RenameColumn(
                name: "PaymentDetails",
                table: "Case",
                newName: "BankSortCode");

            migrationBuilder.RenameColumn(
                name: "PayTo",
                table: "Case",
                newName: "BankAccountName");

            migrationBuilder.RenameColumn(
                name: "InceptionDate",
                table: "Case",
                newName: "PolicyStartDate");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "Case",
                newName: "PolicyEndDate");

            migrationBuilder.RenameColumn(
                name: "BankAddress",
                table: "Case",
                newName: "BankAccountNumber");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "InvolvedParties",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "InvolvedParties");

            migrationBuilder.RenameColumn(
                name: "PolicyStartDate",
                table: "Case",
                newName: "InceptionDate");

            migrationBuilder.RenameColumn(
                name: "PolicyEndDate",
                table: "Case",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "ContactDay",
                table: "Case",
                newName: "RelationshipToPolicyHolder");

            migrationBuilder.RenameColumn(
                name: "BankSortCode",
                table: "Case",
                newName: "PaymentDetails");

            migrationBuilder.RenameColumn(
                name: "BankAccountNumber",
                table: "Case",
                newName: "BankAddress");

            migrationBuilder.RenameColumn(
                name: "BankAccountName",
                table: "Case",
                newName: "PayTo");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "InvolvedParties",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "InvolvedParties",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdNumber",
                table: "InvolvedParties",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "InvolvedParties",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalFirstName",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalLastName",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalPolicyHolder",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address3",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Agent",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "Case",
                type: "datetime2(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Case",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
