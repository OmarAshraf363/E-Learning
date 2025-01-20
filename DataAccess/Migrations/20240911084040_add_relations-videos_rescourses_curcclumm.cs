using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_relationsvideos_rescourses_curcclumm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseCurriculumID",
                table: "CourseVideos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LearningObjectives",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TopicsCovered",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseResources",
                columns: table => new
                {
                    CourseResourceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCurriculumID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseResources", x => x.CourseResourceID);
                    table.ForeignKey(
                        name: "FK_CourseResources_CourseCurricula_CourseCurriculumID",
                        column: x => x.CourseCurriculumID,
                        principalTable: "CourseCurricula",
                        principalColumn: "CourseCurriculumID");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "cfce45d9-7020-4d05-b12a-1825baff8d7d", "fd12cd63-e460-452f-bd38-588228dc0bbc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f62662ed-675a-4854-8f86-e4fab4ad6195", "AQAAAAIAAYagAAAAEKKX+tJDUYv6eRdeoPbJcVES00mSgUAElHhZRNgRqXmF/75Fq7ZnBYJL2dFONJeXLw==", "0f8a0713-f863-4e63-8485-32a7f6e36176" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4a65c6cc-1095-4157-bbde-6c6d9f6ef702", "1d8224f7-4b1d-4ac1-a629-be2df6bc3fe0" });

            migrationBuilder.UpdateData(
                table: "CourseVideos",
                keyColumn: "CourseVideoID",
                keyValue: 1,
                column: "CourseCurriculumID",
                value: null);

            migrationBuilder.UpdateData(
                table: "CourseVideos",
                keyColumn: "CourseVideoID",
                keyValue: 2,
                column: "CourseCurriculumID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 1,
                columns: new[] { "LearningObjectives", "TopicsCovered" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 2,
                columns: new[] { "LearningObjectives", "TopicsCovered" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 11, 11, 40, 35, 988, DateTimeKind.Local).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 11, 11, 40, 35, 988, DateTimeKind.Local).AddTicks(9538));

            migrationBuilder.CreateIndex(
                name: "IX_CourseVideos_CourseCurriculumID",
                table: "CourseVideos",
                column: "CourseCurriculumID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseResources_CourseCurriculumID",
                table: "CourseResources",
                column: "CourseCurriculumID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseVideos_CourseCurricula_CourseCurriculumID",
                table: "CourseVideos",
                column: "CourseCurriculumID",
                principalTable: "CourseCurricula",
                principalColumn: "CourseCurriculumID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseVideos_CourseCurricula_CourseCurriculumID",
                table: "CourseVideos");

            migrationBuilder.DropTable(
                name: "CourseResources");

            migrationBuilder.DropIndex(
                name: "IX_CourseVideos_CourseCurriculumID",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "CourseCurriculumID",
                table: "CourseVideos");

            migrationBuilder.DropColumn(
                name: "LearningObjectives",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TopicsCovered",
                table: "Courses");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7d975647-45b5-487e-8fad-e15285b82654", "81e62642-5848-4434-8a26-59705a87bb41" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c3adf023-24f9-4c06-8003-7268334949e4", "AQAAAAIAAYagAAAAECUv6F8bYaZgH3dBOZGL3wkdGqREXGB2/ij4nQuNKlz52vJZEwQp5wc/mOEkwtDLkw==", "cfed0ffa-4dcb-4819-b681-54bdda725866" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "daa203ab-8af9-44b9-a7ce-9a2eb6bb08f6", "6686df28-7056-4928-8727-88192c355965" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 10, 14, 24, 37, 199, DateTimeKind.Local).AddTicks(5947));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 10, 14, 24, 37, 199, DateTimeKind.Local).AddTicks(6068));
        }
    }
}
