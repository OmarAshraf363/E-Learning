using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class edit_studentId_in_progress_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourseProgress_AspNetUsers_StudentId1",
                table: "StudentCourseProgress");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourseProgress_StudentId1",
                table: "StudentCourseProgress");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "StudentCourseProgress");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "StudentCourseProgress",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a7ecdf03-cbe1-480f-a194-988e659b776a", "aa1e1f0f-fa87-4abb-841b-f26214a7be8e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1c964e96-1c3a-445f-90e1-42b8b7e3c6a4", "AQAAAAIAAYagAAAAEFC7XsB3YM72DE2ZMA8gLtMj26nKA0aNNtzeeW9VYAvp5ylFcK6LWoDKOHJhErp1Sw==", "2c73e84f-1701-4a01-9ad6-ec5fa0e6f16c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fe687040-8687-48a8-b730-827f0639c40c", "6e681bf7-0d94-45bc-b335-1b254b99a336" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 12, 12, 14, 1, 48, 190, DateTimeKind.Local).AddTicks(7149));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 12, 12, 14, 1, 48, 190, DateTimeKind.Local).AddTicks(7231));

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseProgress_StudentId",
                table: "StudentCourseProgress",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourseProgress_AspNetUsers_StudentId",
                table: "StudentCourseProgress",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourseProgress_AspNetUsers_StudentId",
                table: "StudentCourseProgress");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourseProgress_StudentId",
                table: "StudentCourseProgress");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "StudentCourseProgress",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId1",
                table: "StudentCourseProgress",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6ada3209-2499-419c-ac1f-47671705eab8", "3cab9e9f-d150-4bde-b7d0-6794ca24e486" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a329e0d-b300-42f6-b6ab-4cc455cccb12", "AQAAAAIAAYagAAAAEKBcV4ux2Jmb9Sewkg6Jk10q5gBfVx/jPAFWWDLadISD29CrScpK3jyK1kqH8Rb6hw==", "4af861de-c59d-4c02-bbcd-eae1db280167" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "17d55a8d-d49a-4266-847b-86440c709e84", "76e030c0-5d33-4062-8d6e-ba49dc038074" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 12, 11, 14, 39, 16, 169, DateTimeKind.Local).AddTicks(9762));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 12, 11, 14, 39, 16, 169, DateTimeKind.Local).AddTicks(9851));

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseProgress_StudentId1",
                table: "StudentCourseProgress",
                column: "StudentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourseProgress_AspNetUsers_StudentId1",
                table: "StudentCourseProgress",
                column: "StudentId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
