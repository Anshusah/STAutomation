using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondaryAddress",
                table: "Case",
                newName: "Occupation");

            migrationBuilder.RenameColumn(
                name: "PrimaryAddress",
                table: "Case",
                newName: "Address3");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Case",
                newName: "SurName");

            migrationBuilder.AlterColumn<string>(
                name: "VatRegistered",
                table: "Case",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

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

            migrationBuilder.AddColumn<bool>(
                name: "AdditionalPolicyHolder",
                table: "Case",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Case",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
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
                name: "FirstName",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasChildren",
                table: "Case",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MaritialStatus",
                table: "Case",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfChildren",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RelationshipToPolicyHolder",
                table: "Case",
                type: "varchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Address1",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Agent",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "HasChildren",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "MaritialStatus",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "NumberOfChildren",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "RelationshipToPolicyHolder",
                table: "Case");

            migrationBuilder.RenameColumn(
                name: "SurName",
                table: "Case",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Occupation",
                table: "Case",
                newName: "SecondaryAddress");

            migrationBuilder.RenameColumn(
                name: "Address3",
                table: "Case",
                newName: "PrimaryAddress");

            migrationBuilder.AlterColumn<string>(
                name: "VatRegistered",
                table: "Case",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
