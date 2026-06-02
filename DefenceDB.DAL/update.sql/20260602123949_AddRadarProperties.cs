using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class AddRadarProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoolingSystem",
                table: "NavalRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrequencyBand",
                table: "NavalRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScanCoverage",
                table: "NavalRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetTrackingCapacity",
                table: "NavalRadars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrModuleCount",
                table: "NavalRadars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoolingSystem",
                table: "AirDefenseRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrequencyBand",
                table: "AirDefenseRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScanCoverage",
                table: "AirDefenseRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrModuleCount",
                table: "AirDefenseRadars",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoolingSystem",
                table: "AirborneRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrequencyBand",
                table: "AirborneRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScanCoverage",
                table: "AirborneRadars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrModuleCount",
                table: "AirborneRadars",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AirDefenseRadars",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AirDefenseRadars",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AirDefenseRadars",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AirDefenseRadars",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "CoolingSystem", "FrequencyBand", "ScanCoverage", "TrModuleCount" },
                values: new object[] { null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoolingSystem",
                table: "NavalRadars");

            migrationBuilder.DropColumn(
                name: "FrequencyBand",
                table: "NavalRadars");

            migrationBuilder.DropColumn(
                name: "ScanCoverage",
                table: "NavalRadars");

            migrationBuilder.DropColumn(
                name: "TargetTrackingCapacity",
                table: "NavalRadars");

            migrationBuilder.DropColumn(
                name: "TrModuleCount",
                table: "NavalRadars");

            migrationBuilder.DropColumn(
                name: "CoolingSystem",
                table: "AirDefenseRadars");

            migrationBuilder.DropColumn(
                name: "FrequencyBand",
                table: "AirDefenseRadars");

            migrationBuilder.DropColumn(
                name: "ScanCoverage",
                table: "AirDefenseRadars");

            migrationBuilder.DropColumn(
                name: "TrModuleCount",
                table: "AirDefenseRadars");

            migrationBuilder.DropColumn(
                name: "CoolingSystem",
                table: "AirborneRadars");

            migrationBuilder.DropColumn(
                name: "FrequencyBand",
                table: "AirborneRadars");

            migrationBuilder.DropColumn(
                name: "ScanCoverage",
                table: "AirborneRadars");

            migrationBuilder.DropColumn(
                name: "TrModuleCount",
                table: "AirborneRadars");
        }
    }
}
