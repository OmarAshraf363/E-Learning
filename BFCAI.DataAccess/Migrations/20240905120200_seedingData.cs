using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class seedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AcademicYearID", "AccessFailedCount", "AvailableCreditHours", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserType" },
                values: new object[,]
                {
                    { "1", null, 0, 0, "56ac5bf0-82ab-482b-adfb-8388ea8e93c6", "ApplicationUser", "student1@example.com", false, "Student One", false, null, "STUDENT1@EXAMPLE.COM", "STUDENT1", null, null, false, "d101f0fd-5a12-48bd-96a0-e115cd3258f1", false, "student1", "Student" },
                    { "2", null, 0, 0, "0ee82d09-6e19-4b89-9f9f-2742d78caf51", "ApplicationUser", "instructor1@example.com", false, "Instructor One", false, null, "INSTRUCTOR1@EXAMPLE.COM", "INSTRUCTOR1", null, null, false, "5fbe98cc-a4ee-485e-9a72-af106416adcb", false, "instructor1", "Instructor" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentID", "DepartmentName" },
                values: new object[,]
                {
                    { 1, "Computer Science" },
                    { 2, "Electrical Engineering" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseID", "CourseName", "Credits", "DepartmentId", "Description", "InstructorId" },
                values: new object[,]
                {
                    { 1, "Introduction to Programming", 0, 1, "", "2" },
                    { 2, "Digital Circuits", 0, 2, "", "2" }
                });

            migrationBuilder.InsertData(
                table: "ClassSchedule",
                columns: new[] { "ClassScheduleId", "CourseId", "DayOfWeek", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, 1, "Monday", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, "Monday", new DateTime(2024, 10, 1, 12, 1, 1, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "CourseCurricula",
                columns: new[] { "CourseCurriculumID", "Content", "CourseID", "Title" },
                values: new object[,]
                {
                    { 1, "Intro Of Cs", 1, "Introduction" },
                    { 2, "Intro Of C++", 2, "Basic Programming Concepts" }
                });

            migrationBuilder.InsertData(
                table: "CourseVideos",
                columns: new[] { "CourseVideoID", "CourseID", "VideoTitle", "VideoURL" },
                values: new object[,]
                {
                    { 1, 1, "Course Overview", "http://example.com/video1" },
                    { 2, 1, "Getting Started", "http://example.com/video2" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "EnrollmentID", "CourseID", "Grade", "StudentId" },
                values: new object[,]
                {
                    { 1, 1, null, "1" },
                    { 2, 2, null, "1" }
                });

            migrationBuilder.InsertData(
                table: "Feedback",
                columns: new[] { "FeedbackID", "Content", "CourseId", "FeedbackDate", "ProviderUserId", "TargetStudentUserId" },
                values: new object[,]
                {
                    { 1, "Great course!", 1, new DateTime(2024, 9, 5, 15, 1, 59, 440, DateTimeKind.Local).AddTicks(1759), "1", "1" },
                    { 2, "Need improvement on some topics.", 2, new DateTime(2024, 9, 5, 15, 1, 59, 440, DateTimeKind.Local).AddTicks(1818), "2", "1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CourseCurricula",
                keyColumn: "CourseCurriculumID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CourseCurricula",
                keyColumn: "CourseCurriculumID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CourseVideos",
                keyColumn: "CourseVideoID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CourseVideos",
                keyColumn: "CourseVideoID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "EnrollmentID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "EnrollmentID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentID",
                keyValue: 2);
        }
    }
}
