using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kpk_telegram_bot.Migrations
{
    public partial class AddItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemPropertyTypeEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPropertyTypeEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypeEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypeEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TypeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemEntity_ItemTypeEntity_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ItemTypeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemPropertyEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    TypeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPropertyEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPropertyEntity_ItemEntity_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPropertyEntity_ItemPropertyTypeEntity_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ItemPropertyTypeEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemEntity_TypeId",
                table: "ItemEntity",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPropertyEntity_ItemId",
                table: "ItemPropertyEntity",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPropertyEntity_TypeId",
                table: "ItemPropertyEntity",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPropertyEntity");

            migrationBuilder.DropTable(
                name: "ItemEntity");

            migrationBuilder.DropTable(
                name: "ItemPropertyTypeEntity");

            migrationBuilder.DropTable(
                name: "ItemTypeEntity");
        }
    }
}
