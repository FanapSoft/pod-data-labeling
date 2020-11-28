using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class ChangesToTargets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TargetDefinitions_AbpUsers_OwnerId",
                table: "TargetDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_TargetDefinitions_OwnerId",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "T",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "UMax",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "UMin",
                table: "Datasets");

            migrationBuilder.AddColumn<double>(
                name: "BonusFalse",
                table: "TargetDefinitions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusTrue",
                table: "TargetDefinitions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "GoldenCount",
                table: "TargetDefinitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "T",
                table: "TargetDefinitions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UMax",
                table: "TargetDefinitions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UMin",
                table: "TargetDefinitions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalBudget",
                table: "Datasets",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusFalse",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "BonusTrue",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "GoldenCount",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "T",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "UMax",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "UMin",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "TotalBudget",
                table: "Datasets");

            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "TargetDefinitions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "T",
                table: "Datasets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UMax",
                table: "Datasets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UMin",
                table: "Datasets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TargetDefinitions_OwnerId",
                table: "TargetDefinitions",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TargetDefinitions_AbpUsers_OwnerId",
                table: "TargetDefinitions",
                column: "OwnerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
