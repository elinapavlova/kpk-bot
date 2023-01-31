using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kpk_telegram_bot.Migrations
{
    public partial class AddItemParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ItemEntity",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemEntity_ParentId",
                table: "ItemEntity",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemEntity_ItemEntity_ParentId",
                table: "ItemEntity",
                column: "ParentId",
                principalTable: "ItemEntity",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemEntity_ItemEntity_ParentId",
                table: "ItemEntity");

            migrationBuilder.DropIndex(
                name: "IX_ItemEntity_ParentId",
                table: "ItemEntity");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ItemEntity");
        }
    }
}
