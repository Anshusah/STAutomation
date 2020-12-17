using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial87 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowState_WorkflowObject_ActionsId",
                table: "WorkFlowState");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowState_ActionsId",
                table: "WorkFlowState");

            migrationBuilder.DropColumn(
                name: "ActionsId",
                table: "WorkFlowState");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "WorkFlowState",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "WorkFlowState");

            migrationBuilder.AddColumn<string>(
                name: "ActionsId",
                table: "WorkFlowState",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowState_ActionsId",
                table: "WorkFlowState",
                column: "ActionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowState_WorkflowObject_ActionsId",
                table: "WorkFlowState",
                column: "ActionsId",
                principalTable: "WorkflowObject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
