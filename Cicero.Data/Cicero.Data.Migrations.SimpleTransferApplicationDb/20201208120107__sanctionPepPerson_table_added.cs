using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _sanctionPepPerson_table_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SanctionPepPerson",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LexisNexisId = table.Column<int>(nullable: false),
                    NameField = table.Column<string>(nullable: true),
                    RecencyField = table.Column<string>(nullable: true),
                    MatchScoreField = table.Column<int>(nullable: false),
                    SourceField = table.Column<string>(nullable: true),
                    TypeField = table.Column<string>(nullable: true),
                    CountryField = table.Column<string>(nullable: true),
                    AddressesField = table.Column<string>(nullable: true),
                    AliasesField = table.Column<string>(nullable: true),
                    ExceptionsField = table.Column<string>(nullable: true),
                    PositionsField = table.Column<string>(nullable: true),
                    DOBField = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanctionPepPerson", x => x.Id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SanctionPepPerson");

        }
    }
}
