using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class EditedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImported",
                table: "AbpUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "PodUserId",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ExternalTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    UsageTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalTokens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalTokens");

            migrationBuilder.DropColumn(
                name: "IsImported",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "PodUserId",
                table: "AbpUsers");
        }
    }
}
