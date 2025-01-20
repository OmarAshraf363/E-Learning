using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_TopicCover_DemoVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LearningObjectives",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "TopicsCovered",
                table: "Courses",
                newName: "DemoVideoUrl");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LearningObjective",
                columns: table => new
                {
                    LearningObjectiveID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Objective = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningObjective", x => x.LearningObjectiveID);
                    table.ForeignKey(
                        name: "FK_LearningObjective_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID");
                });

            migrationBuilder.CreateTable(
                name: "TopicCovered",
                columns: table => new
                {
                    TopicCoveredID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicCovered", x => x.TopicCoveredID);
                    table.ForeignKey(
                        name: "FK_TopicCovered_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8c925266-7814-43fe-81db-d238af357976", "89ff8900-5d5d-49ef-bd5c-6e36f602cef9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "482a03b8-088e-427b-8c35-2f4691484af3", "AQAAAAIAAYagAAAAECeh55GitD1c22S/70nsmY8nasuEnV9RGxH21T+DVvfBuYLDrsIiWJ9H+0pPHpxIhg==", "2599f618-3294-4ee7-a185-72e7b06e51b3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "35c35a24-e945-4ef7-b3d3-fcf868a74834", "54af9bb4-d76b-48bf-a54c-da32b594158c" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 1,
                column: "Price",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 2,
                column: "Price",
                value: null);

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 15, 17, 5, 598, DateTimeKind.Local).AddTicks(9069));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 15, 17, 5, 598, DateTimeKind.Local).AddTicks(9161));

            migrationBuilder.CreateIndex(
                name: "IX_LearningObjective_CourseID",
                table: "LearningObjective",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_TopicCovered_CourseID",
                table: "TopicCovered",
                column: "CourseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningObjective");

            migrationBuilder.DropTable(
                name: "TopicCovered");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DemoVideoUrl",
                table: "Courses",
                newName: "TopicsCovered");

            migrationBuilder.AddColumn<string>(
                name: "LearningObjectives",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3b7bdeec-e95f-4855-847a-c0bc528acf1a", "ecfc1335-8134-4a29-89d4-587b3897e292" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c4b6acfc-ab3c-464e-8d7f-887aee7e19e8", "AQAAAAIAAYagAAAAEAjDRkeX3UX0ccbgEIL3nVvQhfkZu/NK7giZ8s/VaYMyRqmoBChXaIJYwb112z54iw==", "4e45a5d2-d38d-4085-9485-d723810e5fa2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "961c6663-d4cf-47d3-b5da-1a46670aaca8", "a1686777-58e1-4948-91d0-fc995cfdaf57" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 1,
                column: "LearningObjectives",
                value: "");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 2,
                column: "LearningObjectives",
                value: "");

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 14, 41, 46, 267, DateTimeKind.Local).AddTicks(4249));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 15, 14, 41, 46, 267, DateTimeKind.Local).AddTicks(4325));
        }
    }
}
