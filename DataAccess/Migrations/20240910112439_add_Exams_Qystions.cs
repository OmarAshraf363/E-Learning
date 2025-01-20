using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_Exams_Qystions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_Room_RoomId",
                table: "ClassSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "RoomID");

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    ExamID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.ExamID);
                    table.ForeignKey(
                        name: "FK_Exams_AspNetUsers_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSubmissions",
                columns: table => new
                {
                    ExamSubmissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamID = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSubmissions", x => x.ExamSubmissionID);
                    table.ForeignKey(
                        name: "FK_ExamSubmissions_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamSubmissions_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "ExamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionID);
                    table.ForeignKey(
                        name: "FK_Questions_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "ExamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionChoices",
                columns: table => new
                {
                    QuestionChoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChoiceText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionChoices", x => x.QuestionChoiceID);
                    table.ForeignKey(
                        name: "FK_QuestionChoices_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7d975647-45b5-487e-8fad-e15285b82654", "81e62642-5848-4434-8a26-59705a87bb41" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c3adf023-24f9-4c06-8003-7268334949e4", "AQAAAAIAAYagAAAAECUv6F8bYaZgH3dBOZGL3wkdGqREXGB2/ij4nQuNKlz52vJZEwQp5wc/mOEkwtDLkw==", "cfed0ffa-4dcb-4819-b681-54bdda725866" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "daa203ab-8af9-44b9-a7ce-9a2eb6bb08f6", "6686df28-7056-4928-8727-88192c355965" });

            migrationBuilder.InsertData(
                table: "Exams",
                columns: new[] { "ExamID", "CourseID", "ExamDate", "InstructorId", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "Math Final Exam" },
                    { 2, 2, new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "Physics Final Exam" }
                });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 10, 14, 24, 37, 199, DateTimeKind.Local).AddTicks(5947));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 10, 14, 24, 37, 199, DateTimeKind.Local).AddTicks(6068));

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "QuestionID", "ExamID", "QuestionText" },
                values: new object[,]
                {
                    { 1, 1, "What is 2 + 2?" },
                    { 2, 2, "What is the speed of light?" }
                });

            migrationBuilder.InsertData(
                table: "QuestionChoices",
                columns: new[] { "QuestionChoiceID", "ChoiceText", "IsCorrect", "QuestionID" },
                values: new object[,]
                {
                    { 1, "4", true, 1 },
                    { 2, "3", false, 1 },
                    { 3, "299,792 km/s", true, 2 },
                    { 4, "150,000 km/s", false, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseID",
                table: "Exams",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_InstructorId",
                table: "Exams",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubmissions_ExamID",
                table: "ExamSubmissions",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubmissions_StudentId",
                table: "ExamSubmissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionChoices_QuestionID",
                table: "QuestionChoices",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamID",
                table: "Questions",
                column: "ExamID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_Rooms_RoomId",
                table: "ClassSchedule",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_Rooms_RoomId",
                table: "ClassSchedule");

            migrationBuilder.DropTable(
                name: "ExamSubmissions");

            migrationBuilder.DropTable(
                name: "QuestionChoices");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "RoomID");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "72776944-292c-4959-995f-a58bac2998fd", "51b6ec5c-3e7c-4ef6-a855-dc120a15ad8b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fe91fe19-9bbc-4fed-b8f8-f5a4cb3b6649", "AQAAAAIAAYagAAAAECRtHmFcj/Pxu13+P7jwDQ2K09fJJfpsTmCLai2COodS25NgUxVjrw/lXi5hrqIu+Q==", "c934d21a-ca33-4770-a1d1-72ae97e5b36c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "53f21dbe-9721-49a8-9c54-e90c792fcd6c", "aa11a6b5-1533-4848-86c9-a37f24d4de71" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 7, 21, 24, 47, 176, DateTimeKind.Local).AddTicks(4506));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 7, 21, 24, 47, 176, DateTimeKind.Local).AddTicks(4642));

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_Room_RoomId",
                table: "ClassSchedule",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
