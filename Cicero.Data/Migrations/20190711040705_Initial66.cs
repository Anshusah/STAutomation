using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial66 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Queue");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Queue");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "QueueToState",
                newName: "Ids");

            migrationBuilder.AddColumn<bool>(
                name: "AllUser",
                table: "StateForForm",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CaseFormId",
                table: "QueueToState",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LinePosX",
                table: "QueueToState",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinePosY",
                table: "QueueToState",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueuePosX",
                table: "QueueToState",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueuePosY",
                table: "QueueToState",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QueueForForm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateId = table.Column<int>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    AllUser = table.Column<bool>(nullable: false),
                    QueueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueForForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueueForForm_CaseForm_CaseFormId",
                        column: x => x.CaseFormId,
                        principalTable: "CaseForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueueForForm_Queue_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QueuePermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QueueForFormId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    RoleForQueueId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueuePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueuePermission_QueueForForm_QueueForFormId",
                        column: x => x.QueueForFormId,
                        principalTable: "QueueForForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueuePermission_Role_RoleForQueueId",
                        column: x => x.RoleForQueueId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueueForForm_CaseFormId",
                table: "QueueForForm",
                column: "CaseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_QueueForForm_QueueId",
                table: "QueueForForm",
                column: "QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_QueuePermission_QueueForFormId",
                table: "QueuePermission",
                column: "QueueForFormId");

            migrationBuilder.CreateIndex(
                name: "IX_QueuePermission_RoleForQueueId",
                table: "QueuePermission",
                column: "RoleForQueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueuePermission");

            migrationBuilder.DropTable(
                name: "QueueForForm");

            migrationBuilder.DropColumn(
                name: "AllUser",
                table: "StateForForm");

            migrationBuilder.DropColumn(
                name: "CaseFormId",
                table: "QueueToState");

            migrationBuilder.DropColumn(
                name: "LinePosX",
                table: "QueueToState");

            migrationBuilder.DropColumn(
                name: "LinePosY",
                table: "QueueToState");

            migrationBuilder.DropColumn(
                name: "QueuePosX",
                table: "QueueToState");

            migrationBuilder.DropColumn(
                name: "QueuePosY",
                table: "QueueToState");

            migrationBuilder.RenameColumn(
                name: "Ids",
                table: "QueueToState",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Queue",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "Queue",
                nullable: true);
        }
    }
}
