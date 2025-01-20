using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_cover_in_deept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentDescription",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                table: "Departments",
                keyColumn: "DepartmentID",
                keyValue: 1,
                columns: new[] { "Cover", "DepartmentDescription" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentID",
                keyValue: 2,
                columns: new[] { "Cover", "DepartmentDescription" },
                values: new object[] { "", "" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentDescription",
                table: "Departments");

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
        }
    }
}
