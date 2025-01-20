using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class insert_Progress_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentCourseProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    StudentId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourseProgress_AspNetUsers_StudentId1",
                        column: x => x.StudentId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentCourseProgress_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseID");
                });

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
                name: "IX_StudentCourseProgress_CourseId",
                table: "StudentCourseProgress",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseProgress_StudentId1",
                table: "StudentCourseProgress",
                column: "StudentId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCourseProgress");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a190eecf-d7b2-4949-ab75-19f17f6befc5", "d1f98af7-6ba7-4aa2-9783-d11571fb0cbd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1961222e-51a1-4346-9edd-f0dd424215ab", "AQAAAAIAAYagAAAAEJgyePLdPdVo3iJcHNAXJr8IQahyX8k9q2hVfu6JrEbd8tyYdl2W0+84oZxtDpNWOQ==", "8907c61e-e0ea-49d9-8661-6c6ec21aa2e9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "27b98628-b7d1-4a73-b365-e6f885e643fe", "efd45d77-d222-4ef6-8d12-22d334ee2b56" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 12, 10, 13, 13, 0, 588, DateTimeKind.Local).AddTicks(856));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 12, 10, 13, 13, 0, 588, DateTimeKind.Local).AddTicks(908));
        }
    }
}
