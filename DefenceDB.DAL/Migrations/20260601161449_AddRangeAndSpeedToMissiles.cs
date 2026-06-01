using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRangeAndSpeedToMissiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RangeKm",
                table: "HypersonicGlideVehicles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxSpeedMach",
                table: "CruiseMissiles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxSpeedMach",
                table: "BallisticMissiles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxSpeedMach",
                table: "AntiShipMissiles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxSpeedMach",
                table: "AntiRadiationMissiles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RangeKm",
                table: "AirToAirMissiles",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 12,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 13,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 14,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 15,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 16,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 17,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 18,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 19,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 20,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 21,
                column: "RangeKm",
                value: null);

            migrationBuilder.UpdateData(
                table: "AntiShipMissiles",
                keyColumn: "Id",
                keyValue: 31,
                column: "MaxSpeedMach",
                value: null);

            migrationBuilder.UpdateData(
                table: "AntiShipMissiles",
                keyColumn: "Id",
                keyValue: 32,
                column: "MaxSpeedMach",
                value: null);

            migrationBuilder.UpdateData(
                table: "BallisticMissiles",
                keyColumn: "Id",
                keyValue: 33,
                column: "MaxSpeedMach",
                value: null);

            migrationBuilder.UpdateData(
                table: "BallisticMissiles",
                keyColumn: "Id",
                keyValue: 34,
                column: "MaxSpeedMach",
                value: null);

            migrationBuilder.UpdateData(
                table: "CruiseMissiles",
                keyColumn: "Id",
                keyValue: 39,
                column: "MaxSpeedMach",
                value: null);

            migrationBuilder.UpdateData(
                table: "CruiseMissiles",
                keyColumn: "Id",
                keyValue: 40,
                column: "MaxSpeedMach",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RangeKm",
                table: "HypersonicGlideVehicles");

            migrationBuilder.DropColumn(
                name: "MaxSpeedMach",
                table: "CruiseMissiles");

            migrationBuilder.DropColumn(
                name: "MaxSpeedMach",
                table: "BallisticMissiles");

            migrationBuilder.DropColumn(
                name: "MaxSpeedMach",
                table: "AntiShipMissiles");

            migrationBuilder.DropColumn(
                name: "MaxSpeedMach",
                table: "AntiRadiationMissiles");

            migrationBuilder.DropColumn(
                name: "RangeKm",
                table: "AirToAirMissiles");
        }
    }
}
