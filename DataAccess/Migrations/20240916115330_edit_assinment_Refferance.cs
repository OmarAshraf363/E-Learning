using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class edit_assinment_Refferance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSubmissions_Assignments_AssignmentID",
                table: "AssignmentSubmissions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f5929a36-bb58-48b1-bc8c-64de317692c4", "53a2018d-3b85-4af8-a6ff-660cb3526b01" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31f8414b-8f7c-4ed4-807a-57bd6796d159", "AQAAAAIAAYagAAAAENewa5C7HMqfX9ZazaYqREmvWosFfupPHqrWAdfWKNdWCwTt2G/M/6/gBqMl4hVX4g==", "4609ea27-dc94-486c-8c4a-de3ef99c5479" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "022c1ebf-d5ed-43b5-a02f-30d914584877", "17dcedea-2303-47c0-8b0b-13212e55ceca" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 16, 14, 53, 25, 442, DateTimeKind.Local).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 16, 14, 53, 25, 442, DateTimeKind.Local).AddTicks(9504));

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSubmissions_Assignments_AssignmentID",
                table: "AssignmentSubmissions",
                column: "AssignmentID",
                principalTable: "Assignments",
                principalColumn: "AssignmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSubmissions_Assignments_AssignmentID",
                table: "AssignmentSubmissions");

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
                name: "FK_AssignmentSubmissions_Assignments_AssignmentID",
                table: "AssignmentSubmissions",
                column: "AssignmentID",
                principalTable: "Assignments",
                principalColumn: "AssignmentID");
        }
    }
}
