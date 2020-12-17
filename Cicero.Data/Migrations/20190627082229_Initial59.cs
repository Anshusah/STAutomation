using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial59 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
             name: "IX_State_RoleId",
             table: "State"); 
            migrationBuilder.DropForeignKey(
            name: "FK_State_Role_RoleId",
            table: "State");

            //migrationBuilder.DropIndex(
            //    name: "IX_State_RoleId",
            //    table: "State");

            migrationBuilder.DropColumn(
                name: "Extras",
                table: "State");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "State");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StateToState",
                newName: "Ids");

            migrationBuilder.CreateTable(
                name: "StateForForm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateId = table.Column<int>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateForForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateForForm_CaseForm_CaseFormId",
                        column: x => x.CaseFormId,
                        principalTable: "CaseForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StateForForm_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatePermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateForFormId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    CanEdit = table.Column<bool>(nullable: false),
                    ViewMode = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatePermission_StateForForm_StateForFormId",
                        column: x => x.StateForFormId,
                        principalTable: "StateForForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateForForm_CaseFormId",
                table: "StateForForm",
                column: "CaseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_StateForForm_StateId",
                table: "StateForForm",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_StatePermission_RoleId",
                table: "StatePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_StatePermission_StateForFormId",
                table: "StatePermission",
                column: "StateForFormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatePermission");

            migrationBuilder.DropTable(
                name: "StateForForm");

            migrationBuilder.RenameColumn(
                name: "Ids",
                table: "StateToState",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Extras",
                table: "State",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "State",
                nullable: true);

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
    }
}
