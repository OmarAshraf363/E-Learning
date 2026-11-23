using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BFCAI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCurriculumSortOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "CourseCurricula",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CourseCurricula",
                keyColumn: "CourseCurriculumID",
                keyValue: 1,
                column: "SortOrder",
                value: 0);

            migrationBuilder.UpdateData(
                table: "CourseCurricula",
                keyColumn: "CourseCurriculumID",
                keyValue: 2,
                column: "SortOrder",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "CourseCurricula");
        }
    }
}
