using Microsoft.EntityFrameworkCore.Migrations;

namespace Prediction.Migrations.PhoneProperties
{
    public partial class Hardwareselectable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isSelected",
                table: "PhoneProperties",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSelected",
                table: "PhoneProperties");
        }
    }
}
