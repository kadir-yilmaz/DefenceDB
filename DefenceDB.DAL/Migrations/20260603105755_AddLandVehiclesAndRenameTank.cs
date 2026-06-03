using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddLandVehiclesAndRenameTank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tanks");

            migrationBuilder.CreateTable(
                name: "LandVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EngineHorsePower = table.Column<int>(type: "int", nullable: true),
                    MainGunCaliberMm = table.Column<double>(type: "float", nullable: true),
                    WeightTons = table.Column<double>(type: "float", nullable: true),
                    CrewCount = table.Column<int>(type: "int", nullable: true),
                    HasAutoloader = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandVehicles_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "ModelTypeName", "Name", "Slug" },
                values: new object[] { null, "Kara Araçları", "kara-araclari" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "ModelTypeName", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 48, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.LandVehicle", "Tanklar", 22, "tanklar", 0, null },
                    { 49, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.LandVehicle", "Obüs Sistemleri", 22, "obusler", 0, null },
                    { 50, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.LandVehicle", "Havan Sistemleri", 22, "havan-sistemleri", 0, null },
                    { 51, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.LandVehicle", "Zırhlı Personel Taşıyıcılar (ZPT)", 22, "zpt", 0, null },
                    { 52, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.LandVehicle", "Zırhlı Muharebe Araçları (ZMA)", 22, "zma", 0, null },
                    { 53, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.LandVehicle", "Çok Namlulu Roketatar Sistemleri (ÇNRA)", 22, "cnra", 0, null }
                });

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 101,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 102,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 103,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 104,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 105,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 106,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 107,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 108,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 109,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 110,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 111,
                column: "CategoryId",
                value: 48);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 112,
                column: "CategoryId",
                value: 48);

            migrationBuilder.InsertData(
                table: "LandVehicles",
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
            migrationBuilder.DropTable(
                name: "LandVehicles");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.CreateTable(
                name: "Tanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CrewCount = table.Column<int>(type: "int", nullable: true),
                    EngineHorsePower = table.Column<int>(type: "int", nullable: true),
                    HasAutoloader = table.Column<bool>(type: "bit", nullable: false),
                    MainGunCaliberMm = table.Column<double>(type: "float", nullable: true),
                    WeightTons = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tanks_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "ModelTypeName", "Name", "Slug" },
                values: new object[] { "DefenceDB.EL.Models.Products.Tank", "Tanklar", "tanklar" });

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 101,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 102,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 103,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 104,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 105,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 106,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 107,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 108,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 109,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 110,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 111,
                column: "CategoryId",
                value: 22);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 112,
                column: "CategoryId",
                value: 22);

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
    }
}
