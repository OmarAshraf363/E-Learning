using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class edit_On_Delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_StudentId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseID",
                table: "Enrollments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "86530e6d-a8b5-422f-a437-91faf0ea2d1f", "25e3621c-8b4c-40a5-b724-863f8fa3c5d4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0deba64a-dd4b-4082-a0fe-c871bfc6820c", "AQAAAAIAAYagAAAAEHO4cLeizsiBrNKIlusk8gaua/YUFqD6ys4KK3ePq4g4snStEZgRNrGB1G/iiY6Pqw==", "3a69263b-51dc-4163-a542-b50b83e557b3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "03b53f13-5a42-4521-b75e-3cb68e96c233", "b8d29c6f-0676-4134-9e91-ea81a7f706fe" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 21, 17, 56, 43, 708, DateTimeKind.Local).AddTicks(9518));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 21, 17, 56, 43, 708, DateTimeKind.Local).AddTicks(9590));

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_StudentId",
                table: "Enrollments",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseID",
                table: "Enrollments",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_StudentId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_CourseID",
                table: "Enrollments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4a9e8bdb-96cd-4f6d-ba81-b634534dad26", "dbe1e130-4bcf-43c0-9e59-f433a00a73db" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8894b2b-bd29-4467-ac9a-d43bcfbd64ea", "AQAAAAIAAYagAAAAEEmU4zp7TXfnmN3HcSSxyIzkeNIjeWKTpuF2Mi5WUr+MNF4ecvfMp40HzFVCbEAECQ==", "54235fef-47b9-40db-80c9-35acf95342ce" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fa3beabb-7878-4c4b-b2ee-e25cb91d873a", "9ad26059-9984-422b-931e-ea9fb767fb5a" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 20, 19, 20, 26, 667, DateTimeKind.Local).AddTicks(3807));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 20, 19, 20, 26, 667, DateTimeKind.Local).AddTicks(3891));

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_StudentId",
                table: "Enrollments",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_CourseID",
                table: "Enrollments",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID");
        }
    }
}
