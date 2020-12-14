using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class addedcreditcalculated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CreditCalculated",
                table: "AnswerLogs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditCalculated",
                table: "AnswerLogs");
        }
    }
}
