using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsStorage.Data.Migrations
{
    public partial class Last72HoursCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Classifieds",
                newName: "Yesterday");

            migrationBuilder.AddColumn<int>(
                name: "Before_Yesterday",
                table: "Classifieds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Today",
                table: "Classifieds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Before_Yesterday",
                table: "Classifieds");

            migrationBuilder.DropColumn(
                name: "Today",
                table: "Classifieds");

            migrationBuilder.RenameColumn(
                name: "Yesterday",
                table: "Classifieds",
                newName: "Count");
        }
    }
}
