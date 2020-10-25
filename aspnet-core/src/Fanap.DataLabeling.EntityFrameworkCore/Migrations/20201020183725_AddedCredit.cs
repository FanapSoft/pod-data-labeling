using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class AddedCredit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerBudgetCountPerUser",
                table: "Datasets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CorrectGoldenAnswerIndex",
                table: "Datasets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "T",
                table: "Datasets",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UMax",
                table: "Datasets",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UMin",
                table: "Datasets",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerBudgetCountPerUser",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "CorrectGoldenAnswerIndex",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "T",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "UMax",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "UMin",
                table: "Datasets");
        }
    }
}
