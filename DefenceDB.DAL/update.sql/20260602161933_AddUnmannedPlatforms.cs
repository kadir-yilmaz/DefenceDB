using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class AddUnmannedPlatforms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KamikazeUAVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WarheadWeightKg = table.Column<double>(type: "float", nullable: true),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKmh = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KamikazeUAVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KamikazeUAVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KamikazeUGVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WarheadWeightKg = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKmh = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KamikazeUGVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KamikazeUGVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KamikazeUSVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WarheadWeightKg = table.Column<double>(type: "float", nullable: true),
                    RangeNauticalMiles = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKnots = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KamikazeUSVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KamikazeUSVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UAVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    MaxAltitudeFeet = table.Column<int>(type: "int", nullable: true),
                    PayloadCapacityKg = table.Column<double>(type: "float", nullable: true),
                    WingSpanMeters = table.Column<double>(type: "float", nullable: true),
                    CruisingSpeedKmh = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UAVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UAVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UGVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WeightKg = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKmh = table.Column<double>(type: "float", nullable: true),
                    OperationalRangeKm = table.Column<double>(type: "float", nullable: true),
                    DriveType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UGVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UGVs_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EnduranceHours = table.Column<double>(type: "float", nullable: true),
                    MaxSpeedKnots = table.Column<double>(type: "float", nullable: true),
                    DisplacementTons = table.Column<double>(type: "float", nullable: true),
                    OperationalRangeNauticalMiles = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_USVs_DefenseProducts_Id",
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
                    { 23, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-robot", null, "İnsansız Platformlar", null, "insansiz-platformlar", 6, null },
                    { 24, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.UAV", "İHA (UAV)", 23, "iha-uav", 0, null },
                    { 25, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.USV", "İDA (USV)", 23, "ida-usv", 0, null },
                    { 26, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.UGV", "İKA (UGV)", 23, "ika-ugv", 0, null },
                    { 27, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.KamikazeUAV", "Kamikaze İHA", 23, "kamikaze-iha", 0, null },
                    { 28, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.KamikazeUSV", "Kamikaze İDA", 23, "kamikaze-ida", 0, null },
                    { 29, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.KamikazeUGV", "Kamikaze İKA", 23, "kamikaze-ika", 0, null }
                });

            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 201, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Baykar", "Bayraktar TB2", null, "bayraktar-tb2", null, null, null, null, null },
                    { 202, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Baykar", "Bayraktar TB3", null, "bayraktar-tb3", null, null, null, null, null },
                    { 203, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Baykar", "Bayraktar Akıncı", null, "bayraktar-akinci", null, null, null, null, null },
                    { 204, 24, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, true, "Baykar", "Bayraktar Kızılelma", null, "bayraktar-kizilelma", null, null, null, null, null },
                    { 205, 24, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "General Atomics", "MQ-9 Reaper", null, "mq-9-reaper", null, null, null, null, null },
                    { 206, 24, "ABD", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Northrop Grumman", "RQ-4 Global Hawk", null, "rq-4-global-hawk", null, null, null, null, null },
                    { 207, 24, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Chengdu", "Wing Loong II", null, "wing-loong-ii", null, null, null, null, null },
                    { 208, 24, "Çin", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "CASC", "CH-4 Rainbow", null, "ch-4-rainbow", null, null, null, null, null },
                    { 209, 24, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Kronshtadt Group", "Orion", null, "orion", null, null, null, null, null },
                    { 210, 24, "Rusya", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Sukhoi", "S-70 Okhotnik", null, "s-70-okhotnik", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "UAVs",
                columns: new[] { "Id", "CruisingSpeedKmh", "EnduranceHours", "MaxAltitudeFeet", "PayloadCapacityKg", "WingSpanMeters" },
                values: new object[,]
                {
                    { 201, 130.0, 27.0, 25000, 150.0, 12.0 },
                    { 202, 160.0, 24.0, 25000, 280.0, 14.0 },
                    { 203, 277.0, 24.0, 40000, 1500.0, 20.0 },
                    { 204, 735.0, 5.0, 45000, 1500.0, 10.0 },
                    { 205, 313.0, 27.0, 50000, 1700.0, 20.0 },
                    { 206, 575.0, 34.0, 60000, 1360.0, 39.899999999999999 },
                    { 207, 370.0, 32.0, 32500, 480.0, 20.5 },
                    { 208, 235.0, 40.0, 23600, 345.0, 18.0 },
                    { 209, 120.0, 24.0, 24600, 250.0, 16.300000000000001 },
                    { 210, 1000.0, 12.0, 34400, 2800.0, 20.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KamikazeUAVs");

            migrationBuilder.DropTable(
                name: "KamikazeUGVs");

            migrationBuilder.DropTable(
                name: "KamikazeUSVs");

            migrationBuilder.DropTable(
                name: "UAVs");

            migrationBuilder.DropTable(
                name: "UGVs");

            migrationBuilder.DropTable(
                name: "USVs");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 23);
        }
    }
}
