using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class SkillSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSkillSet",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    SkillSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillSet", x => new { x.SkillSetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserSkillSet_SkillSet_SkillSetId",
                        column: x => x.SkillSetId,
                        principalTable: "SkillSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillSet_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SkillSet",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Title", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified), null, true, "Expert", new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SkillSet",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Title", "UpdatedAt" },
                values: new object[] { 2, new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified), null, true, "Intermediate", new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SkillSet",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsActive", "Title", "UpdatedAt" },
                values: new object[] { 3, new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified), null, true, "Beginner", new DateTime(2020, 1, 29, 16, 20, 30, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillSet_UserId",
                table: "UserSkillSet",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSkillSet");

            migrationBuilder.DropTable(
                name: "SkillSet");
        }
    }
}
