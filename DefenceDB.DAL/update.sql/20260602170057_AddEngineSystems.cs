using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class AddEngineSystems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElectricNuclearPowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PowerOutputMw = table.Column<double>(type: "float", nullable: true),
                    SystemType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricNuclearPowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectricNuclearPowers_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasTurbineEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ThrustKn = table.Column<double>(type: "float", nullable: true),
                    HasAfterburner = table.Column<bool>(type: "bit", nullable: false),
                    BypassRatio = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasTurbineEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasTurbineEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PistonEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HorsePower = table.Column<double>(type: "float", nullable: true),
                    TorqueNm = table.Column<double>(type: "float", nullable: true),
                    Cylinders = table.Column<int>(type: "int", nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PistonEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PistonEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RocketMotors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ThrustKn = table.Column<double>(type: "float", nullable: true),
                    BurnTimeSeconds = table.Column<double>(type: "float", nullable: true),
                    PropellantType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RocketMotors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RocketMotors_DefenseProducts_Id",
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
                    { 30, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-gear-wide-connected", null, "Motor ve Güç Sistemleri", null, "motor-ve-guc-sistemleri", 7, null },
                    { 31, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.GasTurbineEngine", "Gaz Türbinli Motorlar", 30, "gaz-turbinli-motorlar", 0, null },
                    { 32, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.PistonEngine", "Pistonlu/İçten Yanmalı Motorlar", 30, "pistonlu-motorlar", 0, null },
                    { 33, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.RocketMotor", "Roket Motorları", 30, "roket-motorlari", 0, null },
                    { 34, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.ElectricNuclearPower", "Elektrik ve Nükleer Güç", 30, "elektrik-ve-nukleer-guc", 0, null }
                });

            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 301, 31, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TEI", "TEI-TF6000", null, "tei-tf6000", null, null, null, null, null },
                    { 302, 31, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "TEI", "TEI-TF10000", null, "tei-tf10000", null, null, null, null, null },
                    { 303, 31, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Pratt & Whitney", "F135-PW-100", null, "f135-pw-100", null, null, null, null, null },
                    { 304, 31, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Electric", "F110-GE-129", null, "f110-ge-129", null, null, null, null, null },
                    { 305, 31, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "NPO Saturn", "AL-31F", null, "al-31f", null, null, null, null, null },
                    { 311, 32, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "TEI", "TEI-PD170", null, "tei-pd170", null, null, null, null, null },
                    { 312, 32, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "BMC Power", "BATU", null, "batu", null, null, null, null, null },
                    { 313, 32, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "MTU", "MTU MB 873 Ka-501", null, "mtu-mb-873", null, null, null, null, null },
                    { 321, 33, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Roketsan", "Roketsan Katı Yakıtlı Roket Motoru", null, "roketsan-kati-yakitli-motor", null, null, null, null, null },
                    { 322, 33, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "SpaceX", "Raptor", null, "raptor-engine", null, null, null, null, null },
                    { 331, 34, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Electric", "S9G Nükleer Reaktör", null, "s9g-nuclear-reactor", null, null, null, null, null },
                    { 332, 34, "Almanya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Siemens", "Siemens PEM Yakıt Hücresi (AIP)", null, "siemens-pem-aip", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "ElectricNuclearPowers",
                columns: new[] { "Id", "PowerOutputMw", "SystemType" },
                values: new object[,]
                {
                    { 331, 30.0, "Nükleer Reaktör" },
                    { 332, 0.23999999999999999, "AIP (Hava Bağımsız Tahrik)" }
                });

            migrationBuilder.InsertData(
                table: "GasTurbineEngines",
                columns: new[] { "Id", "BypassRatio", "HasAfterburner", "ThrustKn" },
                values: new object[,]
                {
                    { 301, 1.0800000000000001, false, 26.699999999999999 },
                    { 302, 1.0800000000000001, true, 44.5 },
                    { 303, 0.56999999999999995, true, 191.0 },
                    { 304, 0.76000000000000001, true, 131.0 },
                    { 305, 0.58999999999999997, true, 123.0 }
                });

            migrationBuilder.InsertData(
                table: "PistonEngines",
                columns: new[] { "Id", "Cylinders", "FuelType", "HorsePower", "TorqueNm" },
                values: new object[,]
                {
                    { 311, 4, "Dizel / JP-8", 170.0, null },
                    { 312, 12, "Dizel", 1500.0, null },
                    { 313, 12, "Dizel", 1500.0, null }
                });

            migrationBuilder.InsertData(
                table: "RocketMotors",
                columns: new[] { "Id", "BurnTimeSeconds", "PropellantType", "ThrustKn" },
                values: new object[,]
                {
                    { 321, null, "Katı", null },
                    { 322, null, "Sıvı (Metan/LOX)", 2200.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricNuclearPowers");

            migrationBuilder.DropTable(
                name: "GasTurbineEngines");

            migrationBuilder.DropTable(
                name: "PistonEngines");

            migrationBuilder.DropTable(
                name: "RocketMotors");

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 30);
        }
    }
}
