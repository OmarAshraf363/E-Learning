using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class insert_rating_to_feedback_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "KeyWords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Feedback",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b4d2f881-ed81-48ad-9311-6de07e9c83d5", "ac4861f5-fc69-4a9e-87c6-9387998b5116" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5986ee7e-09e2-467c-82f9-332f78ac4699", "AQAAAAIAAYagAAAAENXQSXrYB6kwUiHYvNmVLfeM4VWqYxWH2sAC1vNqEk6TXNOFM+fbQfas60N45A0A8A==", "7f872a4e-f07a-4af6-b8a5-677318dd3931" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9f15e4e7-acdd-4830-9c1e-7b1446314a78", "cfff2cc5-240c-4946-8f12-7c6a06cd1aa3" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 9, 22, 54, 298, DateTimeKind.Utc).AddTicks(1444));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 9, 22, 54, 298, DateTimeKind.Utc).AddTicks(1449));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                columns: new[] { "FeedbackDate", "Rating" },
                values: new object[] { new DateTime(2025, 8, 1, 12, 22, 54, 298, DateTimeKind.Local).AddTicks(475), null });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                columns: new[] { "FeedbackDate", "Rating" },
                values: new object[] { new DateTime(2025, 8, 1, 12, 22, 54, 298, DateTimeKind.Local).AddTicks(600), null });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 9, 22, 54, 298, DateTimeKind.Utc).AddTicks(1370));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 9, 22, 54, 298, DateTimeKind.Utc).AddTicks(1374));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Feedback");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "KeyWords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }
    }
}
