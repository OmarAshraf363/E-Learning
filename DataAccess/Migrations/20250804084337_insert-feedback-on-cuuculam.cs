using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class insertfeedbackoncuuculam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseCurriculumId",
                table: "Feedback",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1ef031ae-ce6c-41df-8db3-70c19780350a", "a09e59de-af32-48fe-aaab-02da69763312" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "442b7053-a69a-45f0-b0c6-2928501bb299", "AQAAAAIAAYagAAAAEOKMejFTYeiJ5hgZ5smhv+Lh8ceqasjM9aKaTgN50y2M9lt+j74g2+MXuDkYxXTlYA==", "4e1cc11b-e68d-4d2b-9eb8-281c07b006e1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "aa14c0f9-f6ef-4db1-a40f-e2ec45fc2107", "8aa0c3dc-66a2-4470-9e78-32ed819bca24" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 8, 43, 32, 506, DateTimeKind.Utc).AddTicks(8064));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 8, 43, 32, 506, DateTimeKind.Utc).AddTicks(8068));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                columns: new[] { "CourseCurriculumId", "FeedbackDate" },
                values: new object[] { null, new DateTime(2025, 8, 4, 11, 43, 32, 506, DateTimeKind.Local).AddTicks(6933) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                columns: new[] { "CourseCurriculumId", "FeedbackDate" },
                values: new object[] { null, new DateTime(2025, 8, 4, 11, 43, 32, 506, DateTimeKind.Local).AddTicks(7073) });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 8, 43, 32, 506, DateTimeKind.Utc).AddTicks(7952));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 8, 43, 32, 506, DateTimeKind.Utc).AddTicks(7957));

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_CourseCurriculumId",
                table: "Feedback",
                column: "CourseCurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_CourseCurricula_CourseCurriculumId",
                table: "Feedback",
                column: "CourseCurriculumId",
                principalTable: "CourseCurricula",
                principalColumn: "CourseCurriculumID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_CourseCurricula_CourseCurriculumId",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_CourseCurriculumId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "CourseCurriculumId",
                table: "Feedback");

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
                column: "FeedbackDate",
                value: new DateTime(2025, 8, 1, 12, 22, 54, 298, DateTimeKind.Local).AddTicks(475));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2025, 8, 1, 12, 22, 54, 298, DateTimeKind.Local).AddTicks(600));

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
    }
}
