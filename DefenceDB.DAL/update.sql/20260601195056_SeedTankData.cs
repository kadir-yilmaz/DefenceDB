using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class SeedTankData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 101, 22, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "KMW / Rheinmetall", "Leopard 2A7", null, "leopard-2a7", null, null, null, null, 2014 },
                    { 102, 22, "Güney Kore", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Hyundai Rotem", "K2 Black Panther", null, "k2-black-panther", null, null, null, null, 2014 },
                    { 103, 22, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "BMC / Otokar", "Altay", null, "altay", null, null, null, null, 2025 },
                    { 104, 22, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Dynamics", "M1A2 SEPv3 Abrams", null, "m1a2-abrams", null, null, null, null, 2020 },
                    { 105, 22, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Uralvagonzavod", "T-72B3", null, "t-72b3", null, null, null, null, 2013 },
                    { 106, 22, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Omsktransmash", "T-80BVM", null, "t-80bvm", null, null, null, null, 2017 },
                    { 107, 22, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Uralvagonzavod", "T-90M Proryv", null, "t-90m-proryv", null, null, null, null, 2020 },
                    { 108, 22, "Birleşik Krallık", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "BAE Systems", "Challenger 2", null, "challenger-2", null, null, null, null, 1998 },
                    { 109, 22, "İsrail", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "MANTAK", "Merkava Mk.4", null, "merkava-mk4", null, null, null, null, 2004 },
                    { 110, 22, "Fransa", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Nexter", "Leclerc", null, "leclerc", null, null, null, null, 1992 },
                    { 111, 22, "Japonya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Mitsubishi Heavy Industries", "Type 10", null, "type-10", null, null, null, null, 2012 },
                    { 112, 22, "İtalya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Iveco-Oto Melara", "C1 Ariete", null, "c1-ariete", null, null, null, null, 1995 }
                });

            migrationBuilder.InsertData(
                table: "Tanks",
                columns: new[] { "Id", "CrewCount", "EngineHorsePower", "HasAutoloader", "MainGunCaliberMm", "WeightTons" },
                values: new object[,]
                {
                    { 101, 4, 1500, false, 120.0, 66.5 },
                    { 102, 3, 1500, true, 120.0, 55.0 },
                    { 103, 4, 1500, false, 120.0, 65.0 },
                    { 104, 4, 1500, false, 120.0, 66.799999999999997 },
                    { 105, 3, 1130, true, 125.0, 46.0 },
                    { 106, 3, 1250, true, 125.0, 46.0 },
                    { 107, 3, 1130, true, 125.0, 48.0 },
                    { 108, 4, 1200, false, 120.0, 64.0 },
                    { 109, 4, 1500, false, 120.0, 65.0 },
                    { 110, 3, 1500, true, 120.0, 57.399999999999999 },
                    { 111, 3, 1200, true, 120.0, 44.0 },
                    { 112, 4, 1247, false, 120.0, 54.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 112);
        }
    }
}
