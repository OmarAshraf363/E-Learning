using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class make_relation_Post_keyWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "KeyWords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "KeyWords",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e609e3f6-909e-4877-827d-220b2789f38f", "966510bf-86a9-4a6f-8f7c-a162a9e1b83b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "03461d48-1cff-44e6-9956-e9c00a3956d1", "AQAAAAIAAYagAAAAEAimo7E6kFMuqEHQHYT6BfLsF8F4x0TnEN+4xMurCVdicW8XnFcQLVm4AqTjFrKObQ==", "251a5884-2a62-4927-b638-e65168023f09" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8f4f0e6d-5cfb-4421-824c-0474e9f11fa7", "b580e67b-adc8-4579-bcc7-e5660109de09" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 19, 12, 52, 25, 795, DateTimeKind.Utc).AddTicks(8097));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 19, 12, 52, 25, 795, DateTimeKind.Utc).AddTicks(8100));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 19, 14, 52, 25, 795, DateTimeKind.Local).AddTicks(6714));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 19, 14, 52, 25, 795, DateTimeKind.Local).AddTicks(6804));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 19, 12, 52, 25, 795, DateTimeKind.Utc).AddTicks(7820));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 19, 12, 52, 25, 795, DateTimeKind.Utc).AddTicks(7824));

            migrationBuilder.CreateIndex(
                name: "IX_KeyWords_PostId",
                table: "KeyWords",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyWords_Posts_PostId",
                table: "KeyWords",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyWords_Posts_PostId",
                table: "KeyWords");

            migrationBuilder.DropIndex(
                name: "IX_KeyWords_PostId",
                table: "KeyWords");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "KeyWords");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "KeyWords",
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
                values: new object[] { "cbcf02ac-677f-4aab-8913-f426b36f914a", "e0493558-4db9-4c1f-a607-4a7ee57c5778" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "960fe410-599b-4dd1-993b-7e6fc644ffdc", "AQAAAAIAAYagAAAAEJwFVZIAJJc+Manv7dGvZNRK9vlweq2SWtWm0H8iloI50vQAYar830VFp1Ih7H222Q==", "ae4e54a3-c0a1-40a9-8184-758ad0ea710b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "38ff035e-6f0c-4106-b044-35c72d51e7b3", "d9a738b3-325f-4339-a789-3e75e032c5e7" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 18, 2, 54, 372, DateTimeKind.Utc).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 18, 2, 54, 372, DateTimeKind.Utc).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 9, 20, 2, 54, 372, DateTimeKind.Local).AddTicks(2655));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 9, 20, 2, 54, 372, DateTimeKind.Local).AddTicks(2776));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 18, 2, 54, 372, DateTimeKind.Utc).AddTicks(3595));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 18, 2, 54, 372, DateTimeKind.Utc).AddTicks(3598));
        }
    }
}
