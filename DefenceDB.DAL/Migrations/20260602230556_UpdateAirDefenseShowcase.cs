using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAirDefenseShowcase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 401,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 402,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 403,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 404,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 408,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 409,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 413,
                column: "IsShowcase",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 401,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 402,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 403,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 404,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 408,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 409,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 413,
                column: "IsShowcase",
                value: true);
        }
    }
}
