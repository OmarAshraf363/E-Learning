using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class editToIdentituUserAndAppUaer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "80526a8e-9e0e-433e-b500-606a8c6f43f0", "8cb305f1-e7ba-40c8-8361-d56b27cb30d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c48770fe-857a-45df-8a1d-da27acdbcd8e", "f2e21d4b-8985-4c84-9414-7d0df48d0247" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 5, 15, 24, 19, 945, DateTimeKind.Local).AddTicks(9839));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 5, 15, 24, 19, 945, DateTimeKind.Local).AddTicks(9905));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "56ac5bf0-82ab-482b-adfb-8388ea8e93c6", "d101f0fd-5a12-48bd-96a0-e115cd3258f1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0ee82d09-6e19-4b89-9f9f-2742d78caf51", "5fbe98cc-a4ee-485e-9a72-af106416adcb" });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 5, 15, 1, 59, 440, DateTimeKind.Local).AddTicks(1759));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "FeedbackDate",
                value: new DateTime(2024, 9, 5, 15, 1, 59, 440, DateTimeKind.Local).AddTicks(1818));
        }
    }
}
