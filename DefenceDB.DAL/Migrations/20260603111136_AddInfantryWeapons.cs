using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddInfantryWeapons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfantryWeapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Caliber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EffectiveRangeMeters = table.Column<int>(type: "int", nullable: true),
                    RateOfFireRpm = table.Column<int>(type: "int", nullable: true),
                    WeightKg = table.Column<double>(type: "float", nullable: true),
                    MagazineCapacity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfantryWeapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfantryWeapons_DefenseProducts_Id",
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
                    { 54, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-crosshair", null, "Piyade Silahları", null, "piyade-silahlari", 9, null },
                    { 55, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.InfantryWeapon", "Tabancalar", 54, "tabancalar", 0, null },
                    { 56, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.InfantryWeapon", "Piyade Tüfekleri", 54, "piyade-tufekleri", 0, null },
                    { 57, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.InfantryWeapon", "Makineli Tüfekler", 54, "makineli-tufekler", 0, null },
                    { 58, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.InfantryWeapon", "Keskin Nişancı Tüfekleri", 54, "keskin-nisanci-tufekleri", 0, null }
                });

            migrationBuilder.InsertData(
                table: "DefenseProducts",
                columns: new[] { "Id", "CategoryId", "Country", "CreatedAt", "Description", "IsActive", "IsShowcase", "Manufacturer", "Name", "NatoReportingName", "Slug", "Status", "ThumbnailUrl", "UpdatedAt", "VideoUrl", "YearIntroduced" },
                values: new object[,]
                {
                    { 601, 55, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Emniyet Genel Müdürlüğü ve Türk Silahlı Kuvvetleri'nin ana hizmet tabancası.", true, true, "Sarsılmaz", "SAR 9", null, "sar-9", null, null, null, null, null },
                    { 602, 56, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tamamen milli imkanlarla geliştirilen TSK'nın ana piyade tüfeği.", true, true, "MKE / Kale Kalıp / Sarsılmaz", "MPT-76", null, "mpt-76", null, null, null, null, null },
                    { 603, 56, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Özel kuvvetler ve emniyet birimleri için kısa namlulu ve hafif milli piyade tüfeği.", true, false, "MKE", "MPT-55", null, "mpt-55", null, null, null, null, null },
                    { 604, 57, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Araç üstü ve piyade kullanımı için geliştirilmiş yerli makineli tüfek.", true, true, "Sarsılmaz", "SAR 762 MT", null, "sar-762-mt", null, null, null, null, null },
                    { 605, 58, "Türkiye", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MPT-76 platformu üzerinden geliştirilen yarı otomatik manga tipi keskin nişancı tüfeği.", true, true, "MKE", "KNT-76", null, "knt-76", null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "InfantryWeapons",
                columns: new[] { "Id", "Caliber", "EffectiveRangeMeters", "MagazineCapacity", "RateOfFireRpm", "WeightKg" },
                values: new object[,]
                {
                    { 601, "9x19mm Parabellum", 50, 15, null, 0.79000000000000004 },
                    { 602, "7.62x51mm NATO", 600, 20, 700, 4.0999999999999996 },
                    { 603, "5.56x45mm NATO", 400, 30, 800, 3.2999999999999998 },
                    { 604, "7.62x51mm NATO", 1200, 100, 850, 12.0 },
                    { 605, "7.62x51mm NATO", 800, 20, null, 4.7000000000000002 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfantryWeapons");

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 601);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 602);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 603);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 604);

            migrationBuilder.DeleteData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 605);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 54);
        }
    }
}
