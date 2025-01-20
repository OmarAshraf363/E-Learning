using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class addRelationBetweenCourse_Department : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentID",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "Courses",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_DepartmentID",
                table: "Courses",
                newName: "IX_Courses_DepartmentId");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Courses",
                newName: "DepartmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                newName: "IX_Courses_DepartmentID");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentID",
                table: "Courses",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID");
        }
    }
}
