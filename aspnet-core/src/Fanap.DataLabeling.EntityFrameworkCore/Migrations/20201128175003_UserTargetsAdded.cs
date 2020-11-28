using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class UserTargetsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    OwnerId = table.Column<long>(nullable: false),
                    TargetDefinitionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTargets_AbpUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTargets_TargetDefinitions_TargetDefinitionId",
                        column: x => x.TargetDefinitionId,
                        principalTable: "TargetDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTargets_OwnerId",
                table: "UserTargets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTargets_TargetDefinitionId",
                table: "UserTargets",
                column: "TargetDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTargets");
        }
    }
}
