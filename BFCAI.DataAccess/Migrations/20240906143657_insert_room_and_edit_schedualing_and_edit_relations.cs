using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class insert_room_and_edit_schedualing_and_edit_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "ClassSchedule",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "ClassSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomID);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "47f7b7de-9e84-45ca-ac52-4a56db83ed7e", "364e0e9f-05f7-4bb3-984a-f69c5f5ab94c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a2bf75ab-d16d-4dcc-899f-8e0e73a6fdef", "408740ad-5232-436a-80ba-063642e41286" });

            migrationBuilder.UpdateData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 1,
                columns: new[] { "EndTime", "InstructorId", "RoomId" },
                values: new object[] { new DateTime(2024, 10, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "1", 1 });

            migrationBuilder.UpdateData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 2,
                columns: new[] { "DayOfWeek", "EndTime", "InstructorId", "RoomId", "StartTime" },
                values: new object[] { "Tuesday", new DateTime(2024, 10, 1, 10, 2, 0, 0, DateTimeKind.Unspecified), "2", 2, new DateTime(2024, 10, 1, 10, 2, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 6, 17, 36, 55, 860, DateTimeKind.Local).AddTicks(2172));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 6, 17, 36, 55, 860, DateTimeKind.Local).AddTicks(2216));

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "RoomID", "Capacity", "Name" },
                values: new object[,]
                {
                    { 1, 30, "Room 101" },
                    { 2, 20, "Lab 202" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedule_InstructorId",
                table: "ClassSchedule",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSchedule_RoomId",
                table: "ClassSchedule",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_AspNetUsers_InstructorId",
                table: "ClassSchedule",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_Room_RoomId",
                table: "ClassSchedule",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_AspNetUsers_InstructorId",
                table: "ClassSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_Room_RoomId",
                table: "ClassSchedule");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedule_InstructorId",
                table: "ClassSchedule");

            migrationBuilder.DropIndex(
                name: "IX_ClassSchedule_RoomId",
                table: "ClassSchedule");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "ClassSchedule");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "ClassSchedule");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0d16bc8c-ce61-4471-bc3f-cc40df7067af", "1d0dc1b7-f4d1-438b-b771-efef6fd3af51" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a9d0850e-8317-42b0-a316-bc12de80f698", "ce6cb7d4-2585-4946-b88e-33541271b42d" });

            migrationBuilder.UpdateData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 1,
                column: "EndTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ClassSchedule",
                keyColumn: "ClassScheduleId",
                keyValue: 2,
                columns: new[] { "DayOfWeek", "EndTime", "StartTime" },
                values: new object[] { "Monday", new DateTime(2024, 10, 1, 12, 1, 1, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) });

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
        }
    }
}
