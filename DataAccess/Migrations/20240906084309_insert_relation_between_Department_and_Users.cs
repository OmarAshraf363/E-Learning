using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class insert_relation_between_Department_and_Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "DepartmentId", "SecurityStamp" },
                values: new object[] { "0d16bc8c-ce61-4471-bc3f-cc40df7067af", null, "1d0dc1b7-f4d1-438b-b771-efef6fd3af51" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "DepartmentId", "SecurityStamp" },
                values: new object[] { "a9d0850e-8317-42b0-a316-bc12de80f698", null, "ce6cb7d4-2585-4946-b88e-33541271b42d" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 6, 11, 43, 5, 382, DateTimeKind.Local).AddTicks(4288));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 6, 11, 43, 5, 382, DateTimeKind.Local).AddTicks(4346));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3e907534-08c4-4116-acad-94b80086e878", "03f235e3-0033-4b57-a124-1414e4c21cc8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a36f146a-9b8c-4b65-8b96-b03886d3b971", "7cb1b45d-27e3-4b5e-8f54-b5f3aa319f09" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 5, 16, 3, 42, 273, DateTimeKind.Local).AddTicks(2260));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 5, 16, 3, 42, 273, DateTimeKind.Local).AddTicks(2335));
        }
    }
}
