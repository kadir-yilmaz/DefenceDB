using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-rocket-takeoff", "Füzeler", null, "fuzeler", 1, null },
                    { 2, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-airplane-engines", "Savaş Uçakları", null, "savas-ucaklari", 2, null },
                    { 3, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-tsunami", "Savaş Gemileri", null, "savas-gemileri", 3, null },
                    { 4, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-radar", "Radarlar", null, "radarlar", 4, null },
                    { 5, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hava-Hava Füzeleri", 1, "hava-hava-fuzeleri", 0, null },
                    { 6, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Balistik Füzeler", 1, "balistik-fuzeler", 0, null },
                    { 7, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Anti-Gemi Füzeleri", 1, "anti-gemi-fuzeleri", 0, null },
                    { 8, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Seyir Füzeleri", 1, "seyir-fuzeleri", 0, null },
                    { 9, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Anti-Radyasyon Füzeleri", 1, "anti-radyasyon", 0, null },
                    { 10, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hipersonik Süzülme Araçları", 1, "hgv", 0, null },
                    { 11, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Avcı (Fighter)", 2, "avci-ucaklari", 0, null },
                    { 12, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Bombardıman", 2, "bombardiman", 0, null },
                    { 13, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Eğitim", 2, "egitim", 0, null },
                    { 14, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hücumbot", 3, "hucumbot", 0, null },
                    { 15, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Korvet", 3, "korvet", 0, null },
                    { 16, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Fırkateyn", 3, "firkateyn", 0, null },
                    { 17, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Muhrip (Destroyer)", 3, "muhrip", 0, null },
                    { 18, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Denizaltı", 3, "denizalti", 0, null },
                    { 19, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hava Savunma Radarları", 4, "hava-savunma-radarlari", 0, null },
                    { 20, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Hava Radarları (Airborne)", 4, "airborne-radarlar", 0, null },
                    { 21, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Deniz Radarları (Naval)", 4, "deniz-radarlari", 0, null }
                });

            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "YearIntroduced" },
                values: new object[,]
                {
                    { 1, 11, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Çok amaçlı 4. nesil savaş uçağı.", true, "Lockheed Martin", "F-16 Fighting Falcon", null, "", null, null, null, null },
                    { 2, 11, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5. nesil çok amaçlı hayalet savaş uçağı.", true, "Lockheed Martin", "F-35 Lightning II", null, "", null, null, null, null },
                    { 3, 11, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hava üstünlüğü sağlayan 5. nesil savaş uçağı.", true, "Lockheed Martin", "F-22 Raptor", null, "", null, null, null, null },
                    { 4, 11, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Milli Muharip Uçak (MMU). 5. nesil çok rollü savaş uçağı.", true, "TUSAŞ", "KAAN", null, "", null, null, null, null },
                    { 5, 11, "Avrupa Birliği", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Çift motorlu, delta kanatlı çok rollü savaş uçağı.", true, "Eurofighter Jagdflugzeug", "Eurofighter Typhoon", null, "", null, null, null, null },
                    { 6, 11, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Omnirole savaş uçağı.", true, "Dassault Aviation", "Dassault Rafale", null, "", null, null, null, null },
                    { 7, 11, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rusya'nın 5. nesil savaş uçağı.", true, "Sukhoi", "Su-57", null, "", null, null, null, null },
                    { 8, 11, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gelişmiş 4++ nesil çok amaçlı savaş uçağı.", true, "Sukhoi", "Su-35", null, "", null, null, null, null },
                    { 9, 11, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Çin'in 5. nesil ağır savaş uçağı.", true, "Chengdu", "J-20 Mighty Dragon", null, "", null, null, null, null },
                    { 10, 11, "İsveç", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hafif, tek motorlu çok amaçlı uçak.", true, "Saab", "JAS 39 Gripen", null, "", null, null, null, null },
                    { 11, 11, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "F-15'in modernize edilmiş en gelişmiş versiyonu.", true, "Boeing", "F-15EX Eagle II", null, "", null, null, null, null },
                    { 12, 5, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Raytheon", "AIM-9 Sidewinder", null, "", null, null, null, null },
                    { 13, 5, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Raytheon", "AIM-120 AMRAAM", null, "", null, null, null, null },
                    { 14, 5, "Avrupa Birliği", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "MBDA", "Meteor", null, "", null, null, null, null },
                    { 15, 5, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Diehl Defence", "IRIS-T", null, "", null, null, null, null },
                    { 16, 5, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "TÜBİTAK SAGE", "GÖKDOĞAN", null, "", null, null, null, null },
                    { 17, 5, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "TÜBİTAK SAGE", "BOZDOĞAN", null, "", null, null, null, null },
                    { 18, 5, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Vympel", "R-77 (AA-12 Adder)", null, "", null, null, null, null },
                    { 19, 5, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Vympel", "R-73 (AA-11 Archer)", null, "", null, null, null, null },
                    { 20, 5, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "CASC", "PL-15", null, "", null, null, null, null },
                    { 21, 5, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "MBDA", "MICA", null, "", null, null, null, null },
                    { 22, 20, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Northrop Grumman", "AN/APG-81", null, "", null, null, null, null },
                    { 23, 20, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Northrop Grumman", "AN/APG-77", null, "", null, null, null, null },
                    { 24, 20, "Avrupa Birliği", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Euroradar", "Captor-E", null, "", null, null, null, null },
                    { 25, 20, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "NIIP", "Irbis-E", null, "", null, null, null, null },
                    { 26, 20, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Aselsan", "MURAD", null, "", null, null, null, null },
                    { 27, 19, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Lockheed Martin", "AN/SPY-1", null, "", null, null, null, null },
                    { 28, 19, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Aselsan", "Ers-Int (Erken İhbar)", null, "", null, null, null, null },
                    { 29, 19, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Raytheon", "Patriot AN/MPQ-65", null, "", null, null, null, null },
                    { 30, 19, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Almaz-Antey", "S-400 91N6E", null, "", null, null, null, null },
                    { 31, 7, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Boeing", "Harpoon", null, "", null, null, null, null },
                    { 32, 7, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Roketsan", "ATMACA", null, "", null, null, null, null },
                    { 33, 6, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Lockheed Martin", "Trident II D5", null, "", null, null, null, null },
                    { 34, 6, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Roketsan", "TAYFUN", null, "", null, null, null, null },
                    { 35, 16, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "STM", "İstif Sınıfı (TCG İstanbul)", null, "", null, null, null, null },
                    { 36, 16, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Naval Group", "FREMM Sınıfı", null, "", null, null, null, null },
                    { 37, 18, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "General Dynamics", "Virginia Sınıfı", null, "", null, null, null, null },
                    { 38, 18, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Gölcük Tersanesi", "Reis Sınıfı (Tip 214TN)", null, "", null, null, null, null },
                    { 39, 8, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Raytheon", "Tomahawk", null, "", null, null, null, null },
                    { 40, 8, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "TÜBİTAK SAGE / Roketsan", "SOM", null, "", null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "AirDefenseRadars",
                columns: new[] { "Id", "MaxRangeKm", "RadarType", "TargetTrackingCapacity" },
                values: new object[,]
                {
                    { 27, 310.0, "PESA", 100 },
                    { 28, 600.0, "AESA", 200 },
                    { 29, 150.0, "AESA", 100 },
                    { 30, 600.0, "AESA", 300 }
                });

            migrationBuilder.InsertData(
                table: "AirToAirMissiles",
                columns: new[] { "Id", "GuidanceType", "MaxSpeedMach", "RangeClass" },
                values: new object[,]
                {
                    { 12, "Infrared (IR)", 2.5, "Kısa" },
                    { 13, "Aktif Radar", 4.0, "Görüş Ötesi (BVR)" },
                    { 14, "Aktif Radar", 4.0, "Görüş Ötesi (BVR)" },
                    { 15, "Infrared (IR)", 3.0, "Kısa" },
                    { 16, "Aktif Radar", 4.0, "Görüş Ötesi (BVR)" },
                    { 17, "Infrared (IR)", 4.0, "Kısa" },
                    { 18, "Aktif Radar", 4.0, "Görüş Ötesi (BVR)" },
                    { 19, "Infrared (IR)", 2.5, "Kısa" },
                    { 20, "Aktif Radar", 4.5, "Görüş Ötesi (BVR)" },
                    { 21, "IR/RF", 3.0, "Orta" }
                });

            migrationBuilder.InsertData(
                table: "AirborneRadars",
                columns: new[] { "Id", "MaxRangeKm", "RadarType", "TargetTrackingCapacity" },
                values: new object[,]
                {
                    { 22, 160.0, "AESA", 23 },
                    { 23, 240.0, "AESA", 100 },
                    { 24, 200.0, "AESA", 60 },
                    { 25, 400.0, "PESA", 30 },
                    { 26, 150.0, "AESA", 40 }
                });

            migrationBuilder.InsertData(
                table: "AntiShipMissiles",
                columns: new[] { "Id", "RangeKm", "SeaSkimming", "SpeedClass" },
                values: new object[,]
                {
                    { 31, 140.0, true, "Subsonic" },
                    { 32, 220.0, true, "Subsonic" }
                });

            migrationBuilder.InsertData(
                table: "BallisticMissiles",
                columns: new[] { "Id", "HasMirv", "IsNuclearCapable", "PayloadKg", "RangeKm" },
                values: new object[,]
                {
                    { 33, true, true, 2800.0, 12000.0 },
                    { 34, false, false, 500.0, 560.0 }
                });

            migrationBuilder.InsertData(
                table: "CruiseMissiles",
                columns: new[] { "Id", "CepMeters", "RangeKm" },
                values: new object[,]
                {
                    { 39, 10.0, 1600.0 },
                    { 40, 5.0, 250.0 }
                });

            migrationBuilder.InsertData(
                table: "FighterAircrafts",
                columns: new[] { "Id", "CombatRadiusKm", "Generation", "HasAesaRadar", "HasStealth" },
                values: new object[,]
                {
                    { 1, 550.0, "4", true, false },
                    { 2, 1090.0, "5", true, true },
                    { 3, 850.0, "5", true, true },
                    { 4, 1100.0, "5", true, true },
                    { 5, 1390.0, "4.5", true, false },
                    { 6, 1850.0, "4.5", true, false },
                    { 7, 1500.0, "5", true, true },
                    { 8, 1600.0, "4.5", true, false },
                    { 9, 2000.0, "5", true, true },
                    { 10, 800.0, "4.5", true, false },
                    { 11, 1270.0, "4.5", true, false }
                });

            migrationBuilder.InsertData(
                table: "Frigates",
                columns: new[] { "Id", "DisplacementTons", "VlsCellsCount" },
                values: new object[,]
                {
                    { 35, 3000.0, 16 },
                    { 36, 6000.0, 32 }
                });

            migrationBuilder.InsertData(
                table: "Submarines",
                columns: new[] { "Id", "DisplacementTons", "MaxDepthMeters", "PropulsionType", "TorpedoTubesCount" },
                values: new object[,]
                {
                    { 37, 7900.0, 240.0, "Nükleer", 4 },
                    { 38, 2010.0, 400.0, "AIP (Hava Bağımsız)", 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AirDefenseRadars",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AirDefenseRadars",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AirDefenseRadars",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AirDefenseRadars",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AirborneRadars",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AntiShipMissiles",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "AntiShipMissiles",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "BallisticMissiles",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "BallisticMissiles",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CruiseMissiles",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "CruiseMissiles",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "FighterAircrafts",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Frigates",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Frigates",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Submarines",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Submarines",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
