using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kpk_telegram_bot.Migrations
{
    public partial class DeleteItemPropertyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ItemPropertyEntity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ItemPropertyEntity",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }
    }
}
