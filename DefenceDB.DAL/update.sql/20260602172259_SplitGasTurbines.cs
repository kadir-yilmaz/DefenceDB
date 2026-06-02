using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class SplitGasTurbines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GasTurbineEngines");

            migrationBuilder.CreateTable(
                name: "MarineGasTurbines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShaftHorsePowerHp = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarineGasTurbines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarineGasTurbines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurbofanEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaxThrustLbf = table.Column<double>(type: "float", nullable: true),
                    DryThrustLbf = table.Column<double>(type: "float", nullable: true),
                    HasAfterburner = table.Column<bool>(type: "bit", nullable: false),
                    BypassRatio = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurbofanEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurbofanEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurbojetEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaxThrustLbf = table.Column<double>(type: "float", nullable: true),
                    DryThrustLbf = table.Column<double>(type: "float", nullable: true),
                    HasAfterburner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurbojetEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurbojetEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurbopropEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShaftHorsePowerHp = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurbopropEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurbopropEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurboshaftEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShaftHorsePowerHp = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurboshaftEngines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurboshaftEngines_DefenseProducts_Id",
                        column: x => x.Id,
                        principalTable: "DefenseProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 31,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.TurbofanEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 35,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.TurbojetEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "ModelTypeName", "Name", "Slug" },
                values: new object[] { "DefenceDB.EL.Models.Products.TurbopropEngine", "Turboprop Motorlar", "turboprop-motorlar" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 37,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.MarineGasTurbine");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "ModelTypeName", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[] { 38, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.TurboshaftEngine", "Turboshaft Motorlar", 30, "turboshaft-motorlar", 0, null });

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 307,
                column: "CategoryId",
                value: 38);

            migrationBuilder.InsertData(
                table: "MarineGasTurbines",
                columns: new[] { "Id", "ShaftHorsePowerHp" },
                values: new object[] { 309, 33600.0 });

            migrationBuilder.InsertData(
                table: "TurbofanEngines",
                columns: new[] { "Id", "BypassRatio", "DryThrustLbf", "HasAfterburner", "MaxThrustLbf" },
                values: new object[,]
                {
                    { 301, 1.0800000000000001, 6000.0, false, 6000.0 },
                    { 302, 1.0800000000000001, 6000.0, true, 10000.0 },
                    { 303, 0.56999999999999995, 28000.0, true, 43000.0 },
                    { 304, 0.76000000000000001, 17155.0, true, 29500.0 },
                    { 305, 0.58999999999999997, 17130.0, true, 27560.0 }
                });

            migrationBuilder.InsertData(
                table: "TurbojetEngines",
                columns: new[] { "Id", "DryThrustLbf", "HasAfterburner", "MaxThrustLbf" },
                values: new object[] { 306, 720.0, false, 720.0 });

            migrationBuilder.InsertData(
                table: "TurbopropEngines",
                columns: new[] { "Id", "ShaftHorsePowerHp" },
                values: new object[] { 308, 1200.0 });

            migrationBuilder.InsertData(
                table: "TurboshaftEngines",
                columns: new[] { "Id", "ShaftHorsePowerHp" },
                values: new object[] { 307, 1400.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarineGasTurbines");

            migrationBuilder.DropTable(
                name: "TurbofanEngines");

            migrationBuilder.DropTable(
                name: "TurbojetEngines");

            migrationBuilder.DropTable(
                name: "TurbopropEngines");

            migrationBuilder.DropTable(
                name: "TurboshaftEngines");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.CreateTable(
                name: "GasTurbineEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BypassRatio = table.Column<double>(type: "float", nullable: true),
                    DryThrustLbf = table.Column<double>(type: "float", nullable: true),
                    HasAfterburner = table.Column<bool>(type: "bit", nullable: false),
                    MaxThrustLbf = table.Column<double>(type: "float", nullable: true),
                    ShaftHorsePowerHp = table.Column<double>(type: "float", nullable: true)
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

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 31,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.GasTurbineEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 35,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.GasTurbineEngine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "ModelTypeName", "Name", "Slug" },
                values: new object[] { "DefenceDB.EL.Models.Products.GasTurbineEngine", "Turboprop ve Turboshaft Motorlar", "turboprop-turboshaft-motorlar" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 37,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.GasTurbineEngine");

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 307,
                column: "CategoryId",
                value: 36);

            migrationBuilder.InsertData(
                table: "GasTurbineEngines",
                columns: new[] { "Id", "BypassRatio", "DryThrustLbf", "HasAfterburner", "MaxThrustLbf", "ShaftHorsePowerHp" },
                values: new object[,]
                {
                    { 301, 1.0800000000000001, 6000.0, false, 6000.0, null },
                    { 302, 1.0800000000000001, 6000.0, true, 10000.0, null },
                    { 303, 0.56999999999999995, 28000.0, true, 43000.0, null },
                    { 304, 0.76000000000000001, 17155.0, true, 29500.0, null },
                    { 305, 0.58999999999999997, 17130.0, true, 27560.0, null },
                    { 306, 0.0, 720.0, false, 720.0, null },
                    { 307, 0.0, null, false, null, 1400.0 },
                    { 308, 0.0, null, false, null, 1200.0 },
                    { 309, 0.0, null, false, null, 33600.0 }
                });
        }
    }
}
