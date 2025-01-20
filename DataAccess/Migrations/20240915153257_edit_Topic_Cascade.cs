using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class edit_Topic_Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningObjective_Courses_CourseID",
                table: "LearningObjective");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicCovered_Courses_CourseID",
                table: "TopicCovered");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d2faf069-d301-4cb6-b6a3-79abfe1d4d99", "e25453af-0483-4db3-a0a1-cc0a6031b216" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "188c95bc-332d-4617-8266-e003c340cca5", "AQAAAAIAAYagAAAAEOoINuXRo8CVQE98UostUp00lXbwFxsEv1yEkzsTjhTMTIVqIHksf4MlToKbgujnew==", "ac0fbd62-b0e3-4d57-9f7c-8fce7bbbb30d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9e906b2d-ed30-4591-be17-f219517fd10d", "7c8fbfa4-8f68-4bbe-a85d-f08b5e734a08" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 18, 32, 52, 426, DateTimeKind.Local).AddTicks(9530));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 18, 32, 52, 426, DateTimeKind.Local).AddTicks(9672));

            migrationBuilder.AddForeignKey(
                name: "FK_LearningObjective_Courses_CourseID",
                table: "LearningObjective",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicCovered_Courses_CourseID",
                table: "TopicCovered",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningObjective_Courses_CourseID",
                table: "LearningObjective");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicCovered_Courses_CourseID",
                table: "TopicCovered");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8c925266-7814-43fe-81db-d238af357976", "89ff8900-5d5d-49ef-bd5c-6e36f602cef9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "482a03b8-088e-427b-8c35-2f4691484af3", "AQAAAAIAAYagAAAAECeh55GitD1c22S/70nsmY8nasuEnV9RGxH21T+DVvfBuYLDrsIiWJ9H+0pPHpxIhg==", "2599f618-3294-4ee7-a185-72e7b06e51b3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "35c35a24-e945-4ef7-b3d3-fcf868a74834", "54af9bb4-d76b-48bf-a54c-da32b594158c" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 15, 17, 5, 598, DateTimeKind.Local).AddTicks(9069));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 15, 17, 5, 598, DateTimeKind.Local).AddTicks(9161));

            migrationBuilder.AddForeignKey(
                name: "FK_LearningObjective_Courses_CourseID",
                table: "LearningObjective",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicCovered_Courses_CourseID",
                table: "TopicCovered",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID");
        }
    }
}
