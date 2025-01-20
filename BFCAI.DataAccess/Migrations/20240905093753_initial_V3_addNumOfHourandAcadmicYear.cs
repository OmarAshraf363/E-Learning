using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banha_UniverCity.Migrations
{
    /// <inheritdoc />
    public partial class initial_V3_addNumOfHourandAcadmicYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicYearID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailableCreditHours",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcademicYear",
                columns: table => new
                {
                    AcademicYearID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYear", x => x.AcademicYearID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AcademicYearID",
                table: "AspNetUsers",
                column: "AcademicYearID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AcademicYear_AcademicYearID",
                table: "AspNetUsers",
                column: "AcademicYearID",
                principalTable: "AcademicYear",
                principalColumn: "AcademicYearID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AcademicYear_AcademicYearID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AcademicYear");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AcademicYearID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AcademicYearID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvailableCreditHours",
                table: "AspNetUsers");
        }
    }
}
