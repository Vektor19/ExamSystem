using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class QuestionModifyPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaxPoints",
                table: "Questions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "ExamUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "ExamUsers");
        }
    }
}
