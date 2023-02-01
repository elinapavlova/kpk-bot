using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kpk_telegram_bot.Migrations
{
    public partial class AddFKItemTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ItemTypeId",
                table: "ItemPropertyTypeEntity",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemPropertyTypeEntity_ItemTypeId",
                table: "ItemPropertyTypeEntity",
                column: "ItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPropertyTypeEntity_ItemTypeEntity_ItemTypeId",
                table: "ItemPropertyTypeEntity",
                column: "ItemTypeId",
                principalTable: "ItemTypeEntity",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPropertyTypeEntity_ItemTypeEntity_ItemTypeId",
                table: "ItemPropertyTypeEntity");

            migrationBuilder.DropIndex(
                name: "IX_ItemPropertyTypeEntity_ItemTypeId",
                table: "ItemPropertyTypeEntity");

            migrationBuilder.DropColumn(
                name: "ItemTypeId",
                table: "ItemPropertyTypeEntity");
        }
    }
}
