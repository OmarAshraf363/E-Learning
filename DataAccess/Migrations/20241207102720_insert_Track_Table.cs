using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class insert_Track_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrackID",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Track", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6791b24d-a9b8-4c5b-acf9-4178efce58fa", "9910387b-1743-42a4-91f2-433fbc066019" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "77550852-eace-4f21-971c-c69cab1300f5", "AQAAAAIAAYagAAAAEMuRTzyk+fjuWjRm2M+RUCaJjsqrHGMoOEIriRjhbMfPRjFSkcPZyoWoqsP414Kwnw==", "769abdbf-09a0-4c9e-94c6-85655036e0cd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d6442f7d-ac10-4123-b883-94a364ac1a85", "6c9a5499-c0f6-4e35-9a1e-241d449f80d3" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 1,
                column: "TrackID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "CourseID",
                keyValue: 2,
                column: "TrackID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 12, 7, 12, 27, 9, 537, DateTimeKind.Local).AddTicks(1563));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 12, 7, 12, 27, 9, 537, DateTimeKind.Local).AddTicks(1693));

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TrackID",
                table: "Courses",
                column: "TrackID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Track_TrackID",
                table: "Courses",
                column: "TrackID",
                principalTable: "Track",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Track_TrackID",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TrackID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TrackID",
                table: "Courses");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a39da4de-1e7b-470d-9780-e5d0fc322b3f", "cbec06a3-3bf5-4d0e-bbb5-8d0af94a0b4b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a689c92-35d5-42a6-aaff-e10d547ebd28", "AQAAAAIAAYagAAAAEEP/nLDECFBsfm9AADnFNThNtXQwIOOBSmcAzGOsi/Lqg6MaNmODb8o1ToFtrRPjCA==", "f3dee2b7-9200-45e3-a98e-3d979213c1cb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "222ce834-fb3d-4786-aa26-4c5ac7565c85", "e0e71337-dd87-434d-a1c9-3949f39983cc" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 30, 11, 4, 35, 776, DateTimeKind.Local).AddTicks(5757));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 30, 11, 4, 35, 776, DateTimeKind.Local).AddTicks(5839));
        }
    }
}
