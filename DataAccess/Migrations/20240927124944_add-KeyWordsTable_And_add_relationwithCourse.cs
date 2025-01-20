using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class addKeyWordsTable_And_add_relationwithCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyWord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyWord_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3397e8dd-1baf-4d3d-a0af-182f5019afa3", "02b51652-e3e4-4010-887d-a5470ece2a33" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "151ad79e-39d3-400a-8841-860a7bd62346", "AQAAAAIAAYagAAAAEHQdQRNZImowhvzE6b2YIwVtVvnYmafsXeDbXEZd5Z/OGmAJ1FZRgu2p3jR7rFFXZw==", "71cac8d1-e8f8-47f8-8c65-7661de4bfc49" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c9336337-5f33-4792-aa50-c5869584f511", "dc4b8d49-d383-4031-a929-4cc7fd93db6e" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 27, 15, 49, 39, 481, DateTimeKind.Local).AddTicks(9507));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 27, 15, 49, 39, 481, DateTimeKind.Local).AddTicks(9588));

            migrationBuilder.CreateIndex(
                name: "IX_KeyWord_CourseId",
                table: "KeyWord",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyWord");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ce189c14-40b8-4cea-a7c4-3b481f79c5f6", "2f96cfff-88a6-498e-8fdd-bf43ea8eafdd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2d088c52-f508-45c3-8868-abed8d91129c", "AQAAAAIAAYagAAAAEAL0/mPBcDh20oXm9AttIOH8/Tir7klOLE9iTdG1pFPQr6QCTQ0cjWWNU8NuaaYQlQ==", "29e0f1ce-cc51-4d76-9d5a-8694d01a441e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d9636673-a99e-40e0-89be-f2ea17014a38", "f5000022-f172-43f0-b517-0388370b0503" });

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
    }
}
