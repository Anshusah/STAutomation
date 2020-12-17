using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class Initial24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_To",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_To",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Message");

            migrationBuilder.AlterColumn<int>(
                name: "ClaimId",
                table: "Message",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotice",
                table: "Message",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MessageUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageUser_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_ClaimId",
                table: "Message",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageUser_MessageId",
                table: "MessageUser",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageUser_UserId",
                table: "MessageUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Case_ClaimId",
                table: "Message",
                column: "ClaimId",
                principalTable: "Case",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Case_ClaimId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "MessageUser");

            migrationBuilder.DropIndex(
                name: "IX_Message_ClaimId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsNotice",
                table: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimId",
                table: "Message",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Message",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_To",
                table: "Message",
                column: "To");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_To",
                table: "Message",
                column: "To",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
