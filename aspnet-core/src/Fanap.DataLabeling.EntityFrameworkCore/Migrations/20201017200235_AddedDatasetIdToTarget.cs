using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class AddedDatasetIdToTarget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DataSetId",
                table: "TargetDefinitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TargetDefinitions_DataSetId",
                table: "TargetDefinitions",
                column: "DataSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_TargetDefinitions_Datasets_DataSetId",
                table: "TargetDefinitions",
                column: "DataSetId",
                principalTable: "Datasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TargetDefinitions_Datasets_DataSetId",
                table: "TargetDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_TargetDefinitions_DataSetId",
                table: "TargetDefinitions");

            migrationBuilder.DropColumn(
                name: "DataSetId",
                table: "TargetDefinitions");
        }
    }
}
