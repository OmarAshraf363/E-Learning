using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_Rate_imgCover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgCover",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Courses",
                type: "int",
                nullable: true);

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
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 1,
                columns: new[] { "ImgCover", "Rate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 2,
                columns: new[] { "ImgCover", "Rate" },
                values: new object[] { null, null });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgCover",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Courses");

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
        }
    }
}
