using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class AddTankModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EngineHorsePower = table.Column<int>(type: "int", nullable: true),
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[] { 22, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "bi bi-shield-shaded", "Tanklar", null, "tanklar", 5, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tanks");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 22);
        }
    }
}
