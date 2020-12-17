using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DamageCategory");

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "State",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoledId",
                table: "State",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_State_RoleId",
                table: "State",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_State_Role_RoleId",
                table: "State",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_State_Role_RoleId",
                table: "State");

            migrationBuilder.DropIndex(
                name: "IX_State_RoleId",
                table: "State");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "State");

            migrationBuilder.DropColumn(
                name: "RoledId",
                table: "State");

            migrationBuilder.CreateTable(
                name: "DamageCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageCategory", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DamageCategory",
                columns: new[] { "Id", "Name", "ParentId" },
                values: new object[,]
                {
                    { 1, "Contamination", 0 },
                    { 31, "Water ingress Ripped Tarpaulin", 5 },
                    { 30, "Flooding", 5 },
                    { 29, "Other", 4 },
                    { 28, "Reefer physical damage", 4 },
                    { 27, "Reefer malfunction", 4 },
                    { 26, "Pilferage", 4 },
                    { 25, "Flooding", 4 },
                    { 24, "Off power", 4 },
                    { 23, "Delay", 4 },
                    { 22, "Other", 3 },
                    { 21, "Cargo shifting", 3 },
                    { 20, "Cargo leakage", 3 },
                    { 19, "Freezing damage", 3 },
                    { 18, "Heating damage", 3 },
                    { 32, "Water ingress Damaged/Holed Container", 5 },
                    { 17, "Impact", 3 },
                    { 15, "other", 2 },
                    { 14, "container lost overboard", 2 },
                    { 13, "container disappearance", 2 },
                    { 12, "cargo pilferage", 2 },
                    { 11, "Other", 1 },
                    { 10, "Vermin", 1 },
                    { 9, "Smelling", 1 },
                    { 8, "Gas", 1 },
                    { 7, "Solid", 1 },
                    { 6, "Liquid", 1 },
                    { 5, "Wetting Damage", 0 },
                    { 4, "Reefer Damage", 0 },
                    { 3, "Physical Damage", 0 },
                    { 2, "Loss", 0 },
                    { 16, "Fire", 3 },
                    { 33, "Water ingress Corroded/wears & tears", 5 }
                });
        }
    }
}
