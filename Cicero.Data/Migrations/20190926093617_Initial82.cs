using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial82 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StateActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    StatetoStateId = table.Column<int>(nullable: false),
                    ActionsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateActions_Actions_ActionsId",
                        column: x => x.ActionsId,
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StateActions_StateToState_StatetoStateId",
                        column: x => x.StatetoStateId,
                        principalTable: "StateToState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateActions_ActionsId",
                table: "StateActions",
                column: "ActionsId");

            migrationBuilder.CreateIndex(
                name: "IX_StateActions_StatetoStateId",
                table: "StateActions",
                column: "StatetoStateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateActions");
        }
    }
}
