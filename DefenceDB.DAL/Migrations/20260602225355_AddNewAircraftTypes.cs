using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNewAircraftTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirSojAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    JammerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrequencyRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxRangeKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirSojAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirSojAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AwacsAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RadarType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetectionRangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxTrackedTargets = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwacsAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AwacsAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CargoAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PayloadCapacityTons = table.Column<double>(type: "float", nullable: true),
                    CargoVolumeCubicMeters = table.Column<double>(type: "float", nullable: true),
                    RangeWithMaxPayloadKm = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CargoAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaritimePatrolAircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    SonarType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasTorpedoTubes = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaritimePatrolAircrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaritimePatrolAircrafts_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "ModelTypeName", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 40, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AirSojAircraft", "Hava SOJ", 2, "hava-soj-ucaklari", 0, null },
                    { 41, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.CargoAircraft", "Askeri Kargo", 2, "askeri-kargo-ucaklari", 0, null },
                    { 42, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.MaritimePatrolAircraft", "Deniz Karakol", 2, "deniz-karakol-ucaklari", 0, null },
                    { 43, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AwacsAircraft", "AWACS", 2, "awacs-ucaklari", 0, null }
                });

            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 501, 40, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gelişmiş hava stand-off jammer karıştırma uçağı.", true, true, "TUSAŞ / Bombardier", "HAVA SOJ (Global 6000)", null, "hava-soj-global-6000", null, null, null, null, null },
                    { 502, 41, "Avrupa Birliği", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dört motorlu askeri kargo ve nakliye uçağı.", true, true, "Airbus Defence and Space", "A400M Atlas", null, "a400m-atlas", null, null, null, null, null },
                    { 503, 42, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Denizaltı savunma harbi, su üstü harbi ve istihbarat uçağı.", true, true, "Boeing", "P-8A Poseidon", null, "p-8a-poseidon", null, null, null, null, null },
                    { 504, 42, "Türkiye / İtalya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Denizaltı savunma harbi ve deniz karakol uçağı.", true, true, "Alenia Aermacchi / TUSAŞ", "ATR 72 Meltem III", null, "atr-72-meltem-iii", null, null, null, null, null },
                    { 505, 43, "Türkiye / ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Havadan erken ihbar ve kontrol uçağı (HİK).", true, true, "Boeing / TUSAŞ", "E-7T Barış Kartalı", null, "e-7t-baris-kartali", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "AirSojAircrafts",
                columns: new[] { "Id", "FrequencyRange", "JammerType", "MaxRangeKm" },
                values: new object[] { 501, "Multi-band HF/VHF/UHF/SHF", "Stand-off Jammer (SOJ)", 3000.0 });

            migrationBuilder.InsertData(
                table: "AwacsAircrafts",
                columns: new[] { "Id", "DetectionRangeKm", "MaxTrackedTargets", "RadarType" },
                values: new object[] { 505, 400.0, 180, "MESA (Çok Rollü Elektronik Taramalı Dizi)" });

            migrationBuilder.InsertData(
                table: "CargoAircrafts",
                columns: new[] { "Id", "CargoVolumeCubicMeters", "PayloadCapacityTons", "RangeWithMaxPayloadKm" },
                values: new object[] { 502, 340.0, 37.0, 3300.0 });

            migrationBuilder.InsertData(
                table: "MaritimePatrolAircrafts",
                columns: new[] { "Id", "EnduranceHours", "HasTorpedoTubes", "SonarType" },
                values: new object[,]
                {
                    { 503, 10.5, true, "AN/APY-10" },
                    { 504, 9.0, true, "AMASCOS" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirSojAircrafts");

            migrationBuilder.DropTable(
                name: "AwacsAircrafts");

            migrationBuilder.DropTable(
                name: "CargoAircrafts");

            migrationBuilder.DropTable(
                name: "MaritimePatrolAircrafts");

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 43);
        }
    }
}
