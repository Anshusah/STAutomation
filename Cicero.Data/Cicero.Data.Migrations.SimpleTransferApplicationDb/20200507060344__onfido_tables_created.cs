using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _onfido_tables_created : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnfidoApplicant",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 450, nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: true),
                    sandbox = table.Column<bool>(nullable: false),
                    first_name = table.Column<string>(nullable: false),
                    last_name = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    dob = table.Column<string>(nullable: true),
                    delete_at = table.Column<DateTime>(nullable: true),
                    href = table.Column<string>(nullable: true),
                    flat_number = table.Column<string>(nullable: true),
                    building_number = table.Column<string>(nullable: true),
                    building_name = table.Column<string>(nullable: true),
                    street = table.Column<string>(nullable: true),
                    sub_street = table.Column<string>(nullable: true),
                    town = table.Column<string>(nullable: true),
                    state = table.Column<string>(nullable: true),
                    postcode = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    line1 = table.Column<string>(nullable: true),
                    line2 = table.Column<string>(nullable: true),
                    line3 = table.Column<string>(nullable: true),
                    id_numbers = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnfidoApplicant", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OnfidoApplicantDocument",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 450, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: true),
                    file_name = table.Column<string>(nullable: true),
                    file_type = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    side = table.Column<string>(nullable: true),
                    issuing_country = table.Column<string>(nullable: true),
                    applicant_id = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnfidoApplicantDocument", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OnfidoApplicantLivePhoto",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 450, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: true),
                    file_name = table.Column<string>(nullable: true),
                    file_type = table.Column<string>(nullable: true),
                    applicant_id = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnfidoApplicantLivePhoto", x => x.id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnfidoApplicant");

            migrationBuilder.DropTable(
                name: "OnfidoApplicantDocument");

            migrationBuilder.DropTable(
                name: "OnfidoApplicantLivePhoto");

        }
    }
}
