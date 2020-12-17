using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _sanctionPep_scorecard_modified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ScoreCard",
                table: "LexisNexis",
                nullable: true,
                oldClrType: typeof(int));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ScoreCard",
                table: "LexisNexis",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

        }
    }
}
