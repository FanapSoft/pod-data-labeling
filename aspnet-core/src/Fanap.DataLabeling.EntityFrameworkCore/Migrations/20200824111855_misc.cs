using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class misc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemsSourcePath",
                table: "Datasets",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DatasetID",
                table: "DatasetItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DatasetItems_DatasetID",
                table: "DatasetItems",
                column: "DatasetID");

            migrationBuilder.AddForeignKey(
                name: "FK_DatasetItems_Datasets_DatasetID",
                table: "DatasetItems",
                column: "DatasetID",
                principalTable: "Datasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatasetItems_Datasets_DatasetID",
                table: "DatasetItems");

            migrationBuilder.DropIndex(
                name: "IX_DatasetItems_DatasetID",
                table: "DatasetItems");

            migrationBuilder.DropColumn(
                name: "ItemsSourcePath",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "DatasetID",
                table: "DatasetItems");
        }
    }
}
