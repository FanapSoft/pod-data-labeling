using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanap.DataLabeling.Migrations
{
    public partial class MajorChange1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Datasets_DatasetId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "DatasetItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "DatasetId",
                table: "Labels",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnswerType",
                table: "Datasets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "Datasets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Datasets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QuestionSrc",
                table: "Datasets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionTemplate",
                table: "Datasets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionType",
                table: "Datasets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "DatasetItems",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "DatasetItems",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "DatasetItems",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "DatasetItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "FinalLabelId",
                table: "DatasetItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGoldenData",
                table: "DatasetItems",
                maxLength: 1000,
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LabelId",
                table: "DatasetItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnswerLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    Ignored = table.Column<bool>(nullable: false),
                    IgnoreReason = table.Column<string>(nullable: true),
                    DataSetId = table.Column<Guid>(nullable: false),
                    DataSetItemId = table.Column<Guid>(nullable: false),
                    Answer = table.Column<int>(nullable: false),
                    QuestionObject = table.Column<string>(nullable: true),
                    DeterminedLabelId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerLogs_Datasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "Datasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerLogs_DatasetItems_DataSetItemId",
                        column: x => x.DataSetItemId,
                        principalTable: "DatasetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerLogs_Labels_DeterminedLabelId",
                        column: x => x.DeterminedLabelId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
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
                    Type = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Src = table.Column<string>(nullable: true),
                    Index = table.Column<int>(nullable: false),
                    DataSetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Datasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "Datasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatasetItems_FinalLabelId",
                table: "DatasetItems",
                column: "FinalLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_DatasetItems_LabelId",
                table: "DatasetItems",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLogs_DataSetId",
                table: "AnswerLogs",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLogs_DataSetItemId",
                table: "AnswerLogs",
                column: "DataSetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLogs_DeterminedLabelId",
                table: "AnswerLogs",
                column: "DeterminedLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_DataSetId",
                table: "AnswerOptions",
                column: "DataSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_DatasetItems_Labels_FinalLabelId",
                table: "DatasetItems",
                column: "FinalLabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatasetItems_Labels_LabelId",
                table: "DatasetItems",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Datasets_DatasetId",
                table: "Labels",
                column: "DatasetId",
                principalTable: "Datasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DatasetItems_Labels_FinalLabelId",
                table: "DatasetItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DatasetItems_Labels_LabelId",
                table: "DatasetItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Datasets_DatasetId",
                table: "Labels");

            migrationBuilder.DropTable(
                name: "AnswerLogs");

            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropIndex(
                name: "IX_DatasetItems_FinalLabelId",
                table: "DatasetItems");

            migrationBuilder.DropIndex(
                name: "IX_DatasetItems_LabelId",
                table: "DatasetItems");

            migrationBuilder.DropColumn(
                name: "AnswerType",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "QuestionSrc",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "QuestionTemplate",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "QuestionType",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "DatasetItems");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "DatasetItems");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "DatasetItems");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "DatasetItems");

            migrationBuilder.DropColumn(
                name: "FinalLabelId",
                table: "DatasetItems");

            migrationBuilder.DropColumn(
                name: "IsGoldenData",
                table: "DatasetItems");

            migrationBuilder.DropColumn(
                name: "LabelId",
                table: "DatasetItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "DatasetId",
                table: "Labels",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "DatasetItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Datasets_DatasetId",
                table: "Labels",
                column: "DatasetId",
                principalTable: "Datasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
