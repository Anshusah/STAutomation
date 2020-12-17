using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _onfidoChecks_table_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnfidoCheck",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 450, nullable: false),
                    ChecksId = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    redirect_uri = table.Column<string>(nullable: true),
                    result = table.Column<string>(nullable: true),
                    sandbox = table.Column<bool>(nullable: false),
                    tags = table.Column<string>(nullable: true),
                    results_uri = table.Column<string>(nullable: true),
                    form_uri = table.Column<string>(nullable: true),
                    paused = table.Column<bool>(nullable: false),
                    version = table.Column<string>(nullable: true),
                    report_ids = table.Column<string>(nullable: true),
                    href = table.Column<string>(nullable: true),
                    applicant_id = table.Column<string>(nullable: true),
                    applicant_provides_data = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnfidoCheck", x => x.id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnfidoCheck");

        }
    }
}
