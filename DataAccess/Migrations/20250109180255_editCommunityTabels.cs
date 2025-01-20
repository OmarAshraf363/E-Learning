using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class editCommunityTabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reaction_Comment",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reaction_Post",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_TargetId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "Reactions");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Reactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Reactions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "cbcf02ac-677f-4aab-8913-f426b36f914a", "e0493558-4db9-4c1f-a607-4a7ee57c5778" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "960fe410-599b-4dd1-993b-7e6fc644ffdc", "AQAAAAIAAYagAAAAEJwFVZIAJJc+Manv7dGvZNRK9vlweq2SWtWm0H8iloI50vQAYar830VFp1Ih7H222Q==", "ae4e54a3-c0a1-40a9-8184-758ad0ea710b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "38ff035e-6f0c-4106-b044-35c72d51e7b3", "d9a738b3-325f-4339-a789-3e75e032c5e7" });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 18, 2, 54, 372, DateTimeKind.Utc).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 18, 2, 54, 372, DateTimeKind.Utc).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 9, 20, 2, 54, 372, DateTimeKind.Local).AddTicks(2655));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2025, 1, 9, 20, 2, 54, 372, DateTimeKind.Local).AddTicks(2776));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 18, 2, 54, 372, DateTimeKind.Utc).AddTicks(3595));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 9, 18, 2, 54, 372, DateTimeKind.Utc).AddTicks(3598));

            migrationBuilder.UpdateData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CommentId", "PostId" },
                values: new object[] { null, 1 });

            migrationBuilder.UpdateData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CommentId", "PostId" },
                values: new object[] { null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId",
                table: "Reactions",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Comments_CommentId",
                table: "Reactions",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Posts_PostId",
                table: "Reactions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Comments_CommentId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Posts_PostId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_PostId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Reactions");

            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "Reactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                table: "Reactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "14e95052-310b-4dad-9ada-c596dc9b4353", "c76fb197-88c4-4e88-94ba-8653cfa01f85" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "123548458",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "370de259-7db8-4276-a1c7-cc5a21d97f1e", "AQAAAAIAAYagAAAAEEzF5MawgDsGklyhtKQlm1kEt2n3QKZwGQ5Do7aQGZAWi0dajtUHr4cGMLYaommC2g==", "5db23939-dc36-4b2b-b1d3-ea210af4c7a0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3ecc001e-7c3d-4720-9971-5e97df6d883b", "619fadc1-0464-44bc-ba8c-4cbebbefdf25" });

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

            migrationBuilder.UpdateData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "TargetId", "TargetType" },
                values: new object[] { 1, "Post" });

            migrationBuilder.UpdateData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "TargetId", "TargetType" },
                values: new object[] { 1, "Comment" });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_TargetId",
                table: "Reactions",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reaction_Comment",
                table: "Reactions",
                column: "TargetId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reaction_Post",
                table: "Reactions",
                column: "TargetId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
