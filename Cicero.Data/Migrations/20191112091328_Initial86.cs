using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial86 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LWObjectId",
                table: "WorkflowPoint",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FWObjectId",
                table: "WorkflowPoint",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "WorkFlowState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FromStateId = table.Column<int>(nullable: false),
                    ToStateId = table.Column<int>(nullable: false),
                    CaseFormId = table.Column<int>(nullable: false),
                    BeforeChangeActionsId = table.Column<string>(nullable: true),
                    ActionsId = table.Column<string>(nullable: true),
                    AfterChangeActionsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkFlowState_WorkflowObject_ActionsId",
                        column: x => x.ActionsId,
                        principalTable: "WorkflowObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowState_CaseForm_CaseFormId",
                        column: x => x.CaseFormId,
                        principalTable: "CaseForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowPoint_CaseFormId",
                table: "WorkflowPoint",
                column: "CaseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowPoint_FWObjectId",
                table: "WorkflowPoint",
                column: "FWObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowPoint_LWObjectId",
                table: "WorkflowPoint",
                column: "LWObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowObject_CaseFormId",
                table: "WorkflowObject",
                column: "CaseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowState_ActionsId",
                table: "WorkFlowState",
                column: "ActionsId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowState_CaseFormId",
                table: "WorkFlowState",
                column: "CaseFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowObject_CaseForm_CaseFormId",
                table: "WorkflowObject",
                column: "CaseFormId",
                principalTable: "CaseForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowPoint_CaseForm_CaseFormId",
                table: "WorkflowPoint",
                column: "CaseFormId",
                principalTable: "CaseForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowPoint_WorkflowObject_FWObjectId",
                table: "WorkflowPoint",
                column: "FWObjectId",
                principalTable: "WorkflowObject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowPoint_WorkflowObject_LWObjectId",
                table: "WorkflowPoint",
                column: "LWObjectId",
                principalTable: "WorkflowObject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowObject_CaseForm_CaseFormId",
                table: "WorkflowObject");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowPoint_CaseForm_CaseFormId",
                table: "WorkflowPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowPoint_WorkflowObject_FWObjectId",
                table: "WorkflowPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowPoint_WorkflowObject_LWObjectId",
                table: "WorkflowPoint");

            migrationBuilder.DropTable(
                name: "WorkFlowState");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowPoint_CaseFormId",
                table: "WorkflowPoint");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowPoint_FWObjectId",
                table: "WorkflowPoint");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowPoint_LWObjectId",
                table: "WorkflowPoint");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowObject_CaseFormId",
                table: "WorkflowObject");

            migrationBuilder.AlterColumn<string>(
                name: "LWObjectId",
                table: "WorkflowPoint",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FWObjectId",
                table: "WorkflowPoint",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
