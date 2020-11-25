using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class ChangedAnswerNumberRequiredPerLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerNumberRequiredPerLabel",
                table: "Datasets");

            migrationBuilder.AddColumn<int>(
                name: "AnswerReplicationCount",
                table: "Datasets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerReplicationCount",
                table: "Datasets");

            migrationBuilder.AddColumn<int>(
                name: "AnswerNumberRequiredPerLabel",
                table: "Datasets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
