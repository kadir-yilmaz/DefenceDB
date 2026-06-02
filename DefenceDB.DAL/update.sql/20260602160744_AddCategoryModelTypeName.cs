using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class AddCategoryModelTypeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelTypeName",
                table: "Categories",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModelTypeName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirToAirMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.BallisticMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AntiShipMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.CruiseMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AntiRadiationMissile");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.HypersonicGlideVehicle");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.FighterAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.BomberAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.TrainerAircraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 14,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.FastAttackCraft");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 15,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Corvette");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 16,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Frigate");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 17,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Destroyer");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 18,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Submarine");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 19,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirDefenseRadar");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 20,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.AirborneRadar");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 21,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.NavalRadar");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 22,
                column: "ModelTypeName",
                value: "DefenceDB.EL.Models.Products.Tank");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelTypeName",
                table: "Categories");
        }
    }
}
