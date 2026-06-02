using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class EngineUpdatesFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThrustKn",
                table: "GasTurbineEngines",
                newName: "ShaftHorsePowerHp");

            migrationBuilder.AddColumn<double>(
                name: "DryThrustLbf",
                table: "GasTurbineEngines",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxThrustLbf",
                table: "GasTurbineEngines",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Name", "Slug" },
                values: new object[] { "Turbofan Motorlar", "turbofan-motorlar" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "ModelTypeName", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 35, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.GasTurbineEngine", "Turbojet Motorlar", 30, "turbojet-motorlar", 0, null },
                    { 36, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.GasTurbineEngine", "Turboprop ve Turboshaft Motorlar", 30, "turboprop-turboshaft-motorlar", 0, null },
                    { 37, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.GasTurbineEngine", "Deniz Gaz Türbinleri", 30, "deniz-gaz-turbinleri", 0, null }
                });

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 301,
                columns: new[] { "DryThrustLbf", "MaxThrustLbf", "ShaftHorsePowerHp" },
                values: new object[] { 6000.0, 6000.0, null });

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 302,
                columns: new[] { "DryThrustLbf", "MaxThrustLbf", "ShaftHorsePowerHp" },
                values: new object[] { 6000.0, 10000.0, null });

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 303,
                columns: new[] { "DryThrustLbf", "MaxThrustLbf", "ShaftHorsePowerHp" },
                values: new object[] { 28000.0, 43000.0, null });

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 304,
                columns: new[] { "DryThrustLbf", "MaxThrustLbf", "ShaftHorsePowerHp" },
                values: new object[] { 17155.0, 29500.0, null });

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 305,
                columns: new[] { "DryThrustLbf", "MaxThrustLbf", "ShaftHorsePowerHp" },
                values: new object[] { 17130.0, 27560.0, null });

            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 306, 35, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Kale Arge", "Kale KTJ-3200", null, "kale-ktj-3200", null, null, null, null, null },
                    { 307, 36, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TEI", "TEI-TS1400", null, "tei-ts1400", null, null, null, null, null },
                    { 308, 36, "Kanada", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Pratt & Whitney Canada", "PT6A-67A", null, "pt6a-67a", null, null, null, null, null },
                    { 309, 37, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Electric", "GE LM2500", null, "ge-lm2500", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "GasTurbineEngines",
                columns: new[] { "Id", "BypassRatio", "DryThrustLbf", "HasAfterburner", "MaxThrustLbf", "ShaftHorsePowerHp" },
                values: new object[,]
                {
                    { 306, 0.0, 720.0, false, 720.0, null },
                    { 307, 0.0, null, false, null, 1400.0 },
                    { 308, 0.0, null, false, null, 1200.0 },
                    { 309, 0.0, null, false, null, 33600.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DropColumn(
                name: "DryThrustLbf",
                table: "GasTurbineEngines");

            migrationBuilder.DropColumn(
                name: "MaxThrustLbf",
                table: "GasTurbineEngines");

            migrationBuilder.RenameColumn(
                name: "ShaftHorsePowerHp",
                table: "GasTurbineEngines",
                newName: "ThrustKn");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Name", "Slug" },
                values: new object[] { "Gaz Türbinli Motorlar", "gaz-turbinli-motorlar" });

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 301,
                column: "ThrustKn",
                value: 26.699999999999999);

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 302,
                column: "ThrustKn",
                value: 44.5);

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 303,
                column: "ThrustKn",
                value: 191.0);

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 304,
                column: "ThrustKn",
                value: 131.0);

            migrationBuilder.UpdateData(
                table: "GasTurbineEngines",
                keyColumn: "Id",
                keyValue: 305,
                column: "ThrustKn",
                value: 123.0);
        }
    }
}
