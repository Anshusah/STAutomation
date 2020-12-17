﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cicero.Data.Cicero.Data.Migrations.SimpleTransferApplicationDb
{
    public partial class _userId_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Customer");
        }
    }
}
