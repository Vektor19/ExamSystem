using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExamAnswerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExamId",
                table: "Answers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ExamId",
                table: "Answers",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Exams_ExamId",
                table: "Answers",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Exams_ExamId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_ExamId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "Answers");
        }
    }
}
