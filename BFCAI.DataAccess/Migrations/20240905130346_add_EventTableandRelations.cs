using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class add_EventTableandRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Event_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Event_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Event_CreatedById",
                table: "Event",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Event_DepartmentID",
                table: "Event",
                column: "DepartmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");

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
    }
}
