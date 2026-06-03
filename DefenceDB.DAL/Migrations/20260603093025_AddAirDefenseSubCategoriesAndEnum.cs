using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAirDefenseSubCategoriesAndEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SystemType",
                table: "AirDefenseSystems",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 401,
                column: "SystemType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 402,
                column: "SystemType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 403,
                column: "SystemType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 404,
                column: "SystemType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 405,
                column: "SystemType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 406,
                column: "SystemType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 408,
                column: "SystemType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 409,
                column: "SystemType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 411,
                column: "SystemType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 412,
                column: "SystemType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 413,
                column: "SystemType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 414,
                column: "SystemType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 415,
                column: "SystemType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 416,
                column: "SystemType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 417,
                column: "SystemType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "AirDefenseSystems",
                keyColumn: "Id",
                keyValue: 418,
                column: "SystemType",
                value: 4);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconClass", "ModelTypeName", "Name", "ParentCategoryId", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 44, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AirDefenseSystem", "Taşınabilir Hava Savunma Sistemleri (MANPADS)", 39, "manpads", 0, null },
                    { 45, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AirDefenseSystem", "Yakın Savunma Silah Sistemleri (CIWS)", 39, "ciws", 0, null },
                    { 46, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AirDefenseSystem", "Kundağı Motorlu Uçaksavar Topları (SPAAG)", 39, "spaag", 0, null },
                    { 47, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DefenceDB.EL.Models.Products.AirDefenseSystem", "Hava ve Füze Savunma Sistemleri", 39, "hava-ve-fuze-savunma-sistemleri", 0, null }
                });

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 401,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 402,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 403,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 404,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 405,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 406,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 408,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 409,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 411,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 412,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 413,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 414,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 415,
                column: "CategoryId",
                value: 44);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 416,
                column: "CategoryId",
                value: 46);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 417,
                column: "CategoryId",
                value: 47);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 418,
                column: "CategoryId",
                value: 47);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DropColumn(
                name: "SystemType",
                table: "AirDefenseSystems");

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 401,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 402,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 403,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 404,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 405,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 406,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 408,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 409,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 411,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 412,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 413,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 414,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 415,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 416,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 417,
                column: "CategoryId",
                value: 39);

            migrationBuilder.UpdateData(
                table: "DefenseProducts",
                keyColumn: "Id",
                keyValue: 418,
                column: "CategoryId",
                value: 39);
        }
    }
}
