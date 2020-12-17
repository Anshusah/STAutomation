using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initail80 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionsReceiver_Actions_ActionsId",
                table: "ActionsReceiver");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionsSender_Actions_ActionsId",
                table: "ActionsSender");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Actions",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionsReceiver_Actions_ActionsId",
                table: "ActionsReceiver",
                column: "ActionsId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionsSender_Actions_ActionsId",
                table: "ActionsSender",
                column: "ActionsId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionsReceiver_Actions_ActionsId",
                table: "ActionsReceiver");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionsSender_Actions_ActionsId",
                table: "ActionsSender");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Actions");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionsReceiver_Actions_ActionsId",
                table: "ActionsReceiver",
                column: "ActionsId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionsSender_Actions_ActionsId",
                table: "ActionsSender",
                column: "ActionsId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
