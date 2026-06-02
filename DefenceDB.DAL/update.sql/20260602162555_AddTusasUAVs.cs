using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class AddTusasUAVs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 211, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "TUSAŞ", "Anka-S", null, "anka-s", null, null, null, null, null },
                    { 212, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TUSAŞ", "Aksungur", null, "aksungur", null, null, null, null, null },
                    { 213, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TUSAŞ", "Anka-3", null, "anka-3", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "UAVs",
                columns: new[] { "Id", "CruisingSpeedKmh", "EnduranceHours", "MaxAltitudeFeet", "PayloadCapacityKg", "WingSpanMeters" },
                values: new object[,]
                {
                    { 211, 200.0, 30.0, 30000, 350.0, 17.5 },
                    { 212, 250.0, 50.0, 40000, 750.0, 24.0 },
                    { 213, 800.0, 10.0, 40000, 1200.0, 12.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UAVs",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "UAVs",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "UAVs",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 213);
        }
    }
}
