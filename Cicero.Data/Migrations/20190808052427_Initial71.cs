using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial71 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueueForForm_Queue_QueueId",
                table: "QueueForForm");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "QueueForForm");

            migrationBuilder.AlterColumn<int>(
                name: "QueueId",
                table: "QueueForForm",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QueueForForm_Queue_QueueId",
                table: "QueueForForm",
                column: "QueueId",
                principalTable: "Queue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueueForForm_Queue_QueueId",
                table: "QueueForForm");

            migrationBuilder.AlterColumn<int>(
                name: "QueueId",
                table: "QueueForForm",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "QueueForForm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_QueueForForm_Queue_QueueId",
                table: "QueueForForm",
                column: "QueueId",
                principalTable: "Queue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
