using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_NumOfEnrollments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumOfEnrollments",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3b7bdeec-e95f-4855-847a-c0bc528acf1a", "ecfc1335-8134-4a29-89d4-587b3897e292" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c4b6acfc-ab3c-464e-8d7f-887aee7e19e8", "AQAAAAIAAYagAAAAEAjDRkeX3UX0ccbgEIL3nVvQhfkZu/NK7giZ8s/VaYMyRqmoBChXaIJYwb112z54iw==", "4e45a5d2-d38d-4085-9485-d723810e5fa2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "961c6663-d4cf-47d3-b5da-1a46670aaca8", "a1686777-58e1-4948-91d0-fc995cfdaf57" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 1,
                column: "NumOfEnrollments",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 2,
                column: "NumOfEnrollments",
                value: null);

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 14, 41, 46, 267, DateTimeKind.Local).AddTicks(4249));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 14, 41, 46, 267, DateTimeKind.Local).AddTicks(4325));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfEnrollments",
                table: "Courses");

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
    }
}
