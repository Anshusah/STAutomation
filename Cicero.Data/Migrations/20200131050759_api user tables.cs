using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Migrations
{
    public partial class apiusertables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUser",
                columns: table => new
                {
                    ApiUserId = table.Column<string>(maxLength: 450, nullable: false),
                    SystemId = table.Column<string>(type: "varchar(200)", nullable: true),
                    Username = table.Column<string>(type: "varchar(50)", nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUser", x => x.ApiUserId);
                });

            migrationBuilder.CreateTable(
                name: "ApiUserToken",
                columns: table => new
                {
                    UserTokenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Token = table.Column<string>(type: "varchar(200)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(200)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    TokenCreatedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    TokenModifiedDate = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    TokenExpiryDatetime = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUserToken", x => x.UserTokenId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiUser");

            migrationBuilder.DropTable(
                name: "ApiUserToken");
        }
    }
}
