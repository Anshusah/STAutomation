using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvolvedParties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    SurName = table.Column<string>(type: "varchar(50)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    Phone = table.Column<string>(type: "varchar(50)", nullable: true),
                    MartialStatus = table.Column<string>(type: "varchar(50)", nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    Address1 = table.Column<string>(type: "varchar(50)", nullable: true),
                    Address2 = table.Column<string>(type: "varchar(50)", nullable: true),
                    City = table.Column<string>(type: "varchar(50)", nullable: true),
                    PostCode = table.Column<string>(type: "varchar(20)", nullable: true),
                    IdNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    Country = table.Column<string>(type: "varchar(50)", nullable: true),
                    Nationality = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvolvedParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvolvedParties_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvolvedParties_UserId",
                table: "InvolvedParties",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvolvedParties");
        }
    }
}
