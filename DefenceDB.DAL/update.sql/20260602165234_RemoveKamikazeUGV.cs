using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class RemoveKamikazeUGV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KamikazeUGVs");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 29);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KamikazeUGVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaxSpeedKmh = table.Column<double>(type: "float", nullable: true),
                    RangeKm = table.Column<double>(type: "float", nullable: true),
                    WarheadWeightKg = table.Column<double>(type: "float", nullable: true)
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "ModelTypeName", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[] { 29, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.KamikazeUGV", "Kamikaze İKA", 23, "kamikaze-ika", 0, null });
        }
    }
}
