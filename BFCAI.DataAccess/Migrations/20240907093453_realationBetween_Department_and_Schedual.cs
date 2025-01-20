using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class realationBetween_Department_and_Schedual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "ClassSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0af3073e-9959-41ea-96a0-7b365d43e38a", "118d016e-0a05-4303-8feb-de958ff7c400" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "66339bf3-9159-4484-afef-fcf8d0b6ce14", "0e23b6cb-cfaa-4e68-b4de-90cd07caf997" });

            migrationBuilder.UpdateData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 1,
                column: "DepartmentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 2,
                column: "DepartmentId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 7, 12, 34, 52, 380, DateTimeKind.Local).AddTicks(858));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 7, 12, 34, 52, 380, DateTimeKind.Local).AddTicks(923));

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedule_DepartmentId",
                table: "ClassSchedule",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_Departments_DepartmentId",
                table: "ClassSchedule",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_Departments_DepartmentId",
                table: "ClassSchedule");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedule_DepartmentId",
                table: "ClassSchedule");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ClassSchedule");

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
        }
    }
}
