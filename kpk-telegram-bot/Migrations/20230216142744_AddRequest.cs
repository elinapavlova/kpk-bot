using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kpk_telegram_bot.Migrations
{
    public partial class AddRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    StudentName = table.Column<string>(type: "TEXT", nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestEntity_ItemEntity_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ItemEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestEntity_GroupId",
                table: "RequestEntity",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestEntity");
        }
    }
}
