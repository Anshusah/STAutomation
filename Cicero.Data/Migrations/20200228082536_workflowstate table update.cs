using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class workflowstatetableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_WorkFlowState", "WorkFlowState");
            migrationBuilder.RenameColumn("Id", "WorkFlowState", "Id_Old"); // Manually Added
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "WorkFlowState",
                nullable: false); // Manually Added


            migrationBuilder.DropColumn("Id_Old","WorkFlowState");    // Manually Added
            migrationBuilder.AddPrimaryKey("PK_WorkFlowState", "WorkFlowState","Id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
