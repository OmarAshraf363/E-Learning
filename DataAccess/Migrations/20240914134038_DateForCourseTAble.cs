using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class DateForCourseTAble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Courses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Courses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fcf9e3cc-5495-4d54-922e-cdb4be1b6640", "1702b8b4-a6fa-4d20-8ed8-59928ac02b6c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fcac1edf-4abf-4876-b179-b735e702ed2c", "AQAAAAIAAYagAAAAEOfq1j2zf4UUzJnBldxiAwTNhXMVRf8DAG+lulhiH/GXVsipbROyjrGuxNeVStFq6A==", "e4fc99c4-15c2-4b66-bc0c-4ff6b4434d11" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "dc8f340b-4665-401c-ad79-7dfb9f187618", "a143839f-7f0d-411d-8d7e-da3eb4279176" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 14, 16, 40, 34, 188, DateTimeKind.Local).AddTicks(3180));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 14, 16, 40, 34, 188, DateTimeKind.Local).AddTicks(3269));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Courses");

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
        }
    }
}
