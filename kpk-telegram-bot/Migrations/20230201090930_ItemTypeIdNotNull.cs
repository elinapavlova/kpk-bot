using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kpk_telegram_bot.Migrations
{
    public partial class ItemTypeIdNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPropertyTypeEntity_ItemTypeEntity_ItemTypeId",
                table: "ItemPropertyTypeEntity");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemTypeId",
                table: "ItemPropertyTypeEntity",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPropertyTypeEntity_ItemTypeEntity_ItemTypeId",
                table: "ItemPropertyTypeEntity",
                column: "ItemTypeId",
                principalTable: "ItemTypeEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPropertyTypeEntity_ItemTypeEntity_ItemTypeId",
                table: "ItemPropertyTypeEntity");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemTypeId",
                table: "ItemPropertyTypeEntity",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPropertyTypeEntity_ItemTypeEntity_ItemTypeId",
                table: "ItemPropertyTypeEntity",
                column: "ItemTypeId",
                principalTable: "ItemTypeEntity",
                principalColumn: "Id");
        }
    }
}
