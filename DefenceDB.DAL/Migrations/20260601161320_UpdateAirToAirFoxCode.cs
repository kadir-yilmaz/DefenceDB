using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAirToAirFoxCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RangeClass",
                table: "AirToAirMissiles",
                newName: "FoxCode");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 12,
                column: "FoxCode",
                value: "Fox 2");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 13,
                column: "FoxCode",
                value: "Fox 3");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 14,
                column: "FoxCode",
                value: "Fox 3");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 15,
                column: "FoxCode",
                value: "Fox 2");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 16,
                column: "FoxCode",
                value: "Fox 3");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 17,
                column: "FoxCode",
                value: "Fox 2");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 18,
                column: "FoxCode",
                value: "Fox 3");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 19,
                column: "FoxCode",
                value: "Fox 2");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 20,
                column: "FoxCode",
                value: "Fox 3");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 21,
                column: "FoxCode",
                value: "Fox 2 / Fox 3");

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsShowcase",
                value: true);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 32,
                column: "IsShowcase",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FoxCode",
                table: "AirToAirMissiles",
                newName: "RangeClass");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 12,
                column: "RangeClass",
                value: "Kısa");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 13,
                column: "RangeClass",
                value: "Görüş Ötesi (BVR)");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 14,
                column: "RangeClass",
                value: "Görüş Ötesi (BVR)");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 15,
                column: "RangeClass",
                value: "Kısa");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 16,
                column: "RangeClass",
                value: "Görüş Ötesi (BVR)");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 17,
                column: "RangeClass",
                value: "Kısa");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 18,
                column: "RangeClass",
                value: "Görüş Ötesi (BVR)");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 19,
                column: "RangeClass",
                value: "Kısa");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 20,
                column: "RangeClass",
                value: "Görüş Ötesi (BVR)");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 21,
                column: "RangeClass",
                value: "Orta");

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsShowcase",
                value: false);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 32,
                column: "IsShowcase",
                value: false);
        }
    }
}
