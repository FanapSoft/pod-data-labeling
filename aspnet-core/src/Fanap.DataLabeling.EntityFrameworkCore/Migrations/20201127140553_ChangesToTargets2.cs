using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class ChangesToTargets2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusFalse",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "BonusTrue",
                table: "TargetDefinitions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BonusFalse",
                table: "TargetDefinitions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusTrue",
                table: "TargetDefinitions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
