using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_rel_bet_exam_and_Curriculum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurriculumId",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseID",
                table: "Enrollments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a39da4de-1e7b-470d-9780-e5d0fc322b3f", "cbec06a3-3bf5-4d0e-bbb5-8d0af94a0b4b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a689c92-35d5-42a6-aaff-e10d547ebd28", "AQAAAAIAAYagAAAAEEP/nLDECFBsfm9AADnFNThNtXQwIOOBSmcAzGOsi/Lqg6MaNmODb8o1ToFtrRPjCA==", "f3dee2b7-9200-45e3-a98e-3d979213c1cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "222ce834-fb3d-4786-aa26-4c5ac7565c85", "e0e71337-dd87-434d-a1c9-3949f39983cc" });

            migrationBuilder.UpdateData(
                table: "Exams",
                keyColumn: "ExamID",
                keyValue: 1,
                column: "CurriculumId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exams",
                keyColumn: "ExamID",
                keyValue: 2,
                column: "CurriculumId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 30, 11, 4, 35, 776, DateTimeKind.Local).AddTicks(5757));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 30, 11, 4, 35, 776, DateTimeKind.Local).AddTicks(5839));

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CurriculumId",
                table: "Exams",
                column: "CurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_CourseCurricula_CurriculumId",
                table: "Exams",
                column: "CurriculumId",
                principalTable: "CourseCurricula",
                principalColumn: "CourseCurriculumID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_CourseCurricula_CurriculumId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_CurriculumId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "CurriculumId",
                table: "Exams");

            migrationBuilder.AlterColumn<int>(
                name: "CourseID",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "aad000b1-4dbc-4249-b933-2eb5d5fd95ef", "9cc85cb6-8586-4091-90ff-7bbb8ae22791" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f4482bd9-7fa1-4ca9-8514-bc81248e89fa", "AQAAAAIAAYagAAAAEJUDEfh5GQQnFBy28fPNQhjY9CtV0QkzYmiCBrYjQZ97pVmJPV5bpB3OY4CBnd2Wiw==", "f8578bf7-fbc4-40d6-bd8d-00fdc8926ed9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "db49d20b-6b55-4b1d-b88f-25f834fde92e", "2ce43abb-2c11-4a9d-8ab2-41e5f756233b" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 29, 14, 12, 39, 783, DateTimeKind.Local).AddTicks(3536));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 29, 14, 12, 39, 783, DateTimeKind.Local).AddTicks(3654));
        }
    }
}
