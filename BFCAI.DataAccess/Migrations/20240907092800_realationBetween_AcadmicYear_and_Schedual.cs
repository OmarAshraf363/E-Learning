using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class realationBetween_AcadmicYear_and_Schedual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcadmicYearId",
                table: "ClassSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AcademicYear",
                columns: new[] { "AcademicYearID", "YearName" },
                values: new object[,]
                {
                    { 1, "2020-2021" },
                    { 2, "2021-2022" },
                    { 3, "2022-2023" },
                    { 4, "2023-2024" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9117c69e-ed21-4141-b370-7aa217625127", "ca1628b9-d617-4186-86cb-45678ca4f402" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "367ba316-99b2-4c1e-b5b3-d2c199b60d6c", "a14b04ae-6166-4bc6-b3e6-b4005ed32039" });

            migrationBuilder.UpdateData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 1,
                column: "AcadmicYearId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 2,
                column: "AcadmicYearId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 7, 12, 27, 57, 155, DateTimeKind.Local).AddTicks(317));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 7, 12, 27, 57, 155, DateTimeKind.Local).AddTicks(383));

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedule_AcadmicYearId",
                table: "ClassSchedule",
                column: "AcadmicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_AcademicYear_AcadmicYearId",
                table: "ClassSchedule",
                column: "AcadmicYearId",
                principalTable: "AcademicYear",
                principalColumn: "AcademicYearID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_AcademicYear_AcadmicYearId",
                table: "ClassSchedule");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedule_AcadmicYearId",
                table: "ClassSchedule");

            migrationBuilder.DeleteData(
                table: "AcademicYear",
                keyColumn: "AcademicYearID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AcademicYear",
                keyColumn: "AcademicYearID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AcademicYear",
                keyColumn: "AcademicYearID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AcademicYear",
                keyColumn: "AcademicYearID",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "AcadmicYearId",
                table: "ClassSchedule");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "47f7b7de-9e84-45ca-ac52-4a56db83ed7e", "364e0e9f-05f7-4bb3-984a-f69c5f5ab94c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a2bf75ab-d16d-4dcc-899f-8e0e73a6fdef", "408740ad-5232-436a-80ba-063642e41286" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 6, 17, 36, 55, 860, DateTimeKind.Local).AddTicks(2172));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 6, 17, 36, 55, 860, DateTimeKind.Local).AddTicks(2216));
        }
    }
}
