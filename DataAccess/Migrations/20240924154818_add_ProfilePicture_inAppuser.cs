using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_ProfilePicture_inAppuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Picture", "SecurityStamp" },
                values: new object[] { "ce189c14-40b8-4cea-a7c4-3b481f79c5f6", "", "2f96cfff-88a6-498e-8fdd-bf43ea8eafdd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Picture", "SecurityStamp" },
                values: new object[] { "2d088c52-f508-45c3-8868-abed8d91129c", "AQAAAAIAAYagAAAAEAL0/mPBcDh20oXm9AttIOH8/Tir7klOLE9iTdG1pFPQr6QCTQ0cjWWNU8NuaaYQlQ==", "", "29e0f1ce-cc51-4d76-9d5a-8694d01a441e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "Picture", "SecurityStamp" },
                values: new object[] { "d9636673-a99e-40e0-89be-f2ea17014a38", "", "f5000022-f172-43f0-b517-0388370b0503" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 24, 18, 48, 16, 366, DateTimeKind.Local).AddTicks(138));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 24, 18, 48, 16, 366, DateTimeKind.Local).AddTicks(560));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "50804db8-9226-40c8-aed1-f08a6765e4cc", "acd7688b-52cd-4d0f-843e-26e64f425205" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "483dedbf-fbdd-44f0-9383-c0a7b5102df6", "AQAAAAIAAYagAAAAEEZcLbGG2TMjEB9aCYnqGQ6gfJ6UHX4hjdYp/ZBaA8rTbP4WqsnDKx0STtbXSZk53g==", "f1598cbe-00e0-4c9c-aadd-546c7fd91f5e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "088283b0-8a58-4867-b47b-3bc8e999b385", "254b40f7-95f6-46ae-8fb0-53c8ff8ef4e6" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 24, 17, 7, 52, 328, DateTimeKind.Local).AddTicks(6434));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 24, 17, 7, 52, 328, DateTimeKind.Local).AddTicks(6548));
        }
    }
}
