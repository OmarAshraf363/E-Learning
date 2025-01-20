using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class insertJopDescriptionFeild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JopDescription",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "JopDescription", "SecurityStamp" },
                values: new object[] { "14e95052-310b-4dad-9ada-c596dc9b4353", "", "c76fb197-88c4-4e88-94ba-8653cfa01f85" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "JopDescription", "PasswordHash", "SecurityStamp" },
                values: new object[] { "370de259-7db8-4276-a1c7-cc5a21d97f1e", "", "AQAAAAIAAYagAAAAEEzF5MawgDsGklyhtKQlm1kEt2n3QKZwGQ5Do7aQGZAWi0dajtUHr4cGMLYaommC2g==", "5db23939-dc36-4b2b-b1d3-ea210af4c7a0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "JopDescription", "SecurityStamp" },
                values: new object[] { "3ecc001e-7c3d-4720-9971-5e97df6d883b", "", "619fadc1-0464-44bc-ba8c-4cbebbefdf25" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 49, 6, 65, DateTimeKind.Utc).AddTicks(7494));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 49, 6, 65, DateTimeKind.Utc).AddTicks(7497));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 9, 17, 49, 6, 65, DateTimeKind.Local).AddTicks(6623));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 9, 17, 49, 6, 65, DateTimeKind.Local).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 49, 6, 65, DateTimeKind.Utc).AddTicks(7435));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 15, 49, 6, 65, DateTimeKind.Utc).AddTicks(7439));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JopDescription",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d29c41fd-064b-4d0b-bb5c-f8b15d389e0f", "8ad23b77-dc7c-4edf-8678-1ea1a70700f0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd490c9e-580e-4d3f-96eb-01b2d9e53a46", "AQAAAAIAAYagAAAAEO7cHLxJsrCPKFPM7LBETwzPsE157fJfWdtsSNOuiVyP7zdqUjpeh6li0x/z19PL5g==", "789b66be-2202-4ce7-bc50-68c7f62cb3d7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "446ac065-6852-420e-a9f0-be9138f7987b", "2fad0b89-a5da-4060-8f8a-7bc18736c46a" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 7, 9, 44, 0, 433, DateTimeKind.Utc).AddTicks(1590));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 7, 9, 44, 0, 433, DateTimeKind.Utc).AddTicks(1594));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 7, 11, 44, 0, 432, DateTimeKind.Local).AddTicks(8011));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 7, 11, 44, 0, 432, DateTimeKind.Local).AddTicks(8641));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 7, 9, 44, 0, 433, DateTimeKind.Utc).AddTicks(1381));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 7, 9, 44, 0, 433, DateTimeKind.Utc).AddTicks(1385));
        }
    }
}
