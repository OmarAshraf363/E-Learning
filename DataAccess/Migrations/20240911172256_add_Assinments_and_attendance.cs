using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_Assinments_and_attendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCurriculumID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeadLine = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentID);
                    table.ForeignKey(
                        name: "FK_Assignments_CourseCurricula_CourseCurriculumID",
                        column: x => x.CourseCurriculumID,
                        principalTable: "CourseCurricula",
                        principalColumn: "CourseCurriculumID");
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseCurriculumID = table.Column<int>(type: "int", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendances_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attendances_CourseCurricula_CourseCurriculumID",
                        column: x => x.CourseCurriculumID,
                        principalTable: "CourseCurricula",
                        principalColumn: "CourseCurriculumID");
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSubmissions",
                columns: table => new
                {
                    AssignmentSubmissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentID = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmissions", x => x.AssignmentSubmissionID);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Assignments_AssignmentID",
                        column: x => x.AssignmentID,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentID");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0ddc8ea4-d2b9-4dfe-93ca-ce928b53b9a9", "806d0b05-8876-438b-a38c-0765acfd4df8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f46aea0e-600c-42f4-9d3b-3734bb2e64e5", "AQAAAAIAAYagAAAAEJwWCjBAGYAiHkw7uRdJmOj46kZZyC5NFA5h3YlOdex7DuNqOVkMl99dRqpxl9f95g==", "1304378e-878e-4df5-a7ed-4db1bd6ba547" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "dea352bd-0bbb-48dc-9e26-8b3d1197dc86", "44c2b576-7c2c-4a8f-bbbb-dc43a0dc7f1a" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 11, 20, 22, 52, 244, DateTimeKind.Local).AddTicks(754));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 11, 20, 22, 52, 244, DateTimeKind.Local).AddTicks(857));

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseCurriculumID",
                table: "Assignments",
                column: "CourseCurriculumID");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_ApplicationUserID",
                table: "AssignmentSubmissions",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_AssignmentID",
                table: "AssignmentSubmissions",
                column: "AssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ApplicationUserID",
                table: "Attendances",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CourseCurriculumID",
                table: "Attendances",
                column: "CourseCurriculumID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentSubmissions");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Assignments");

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
        }
    }
}
