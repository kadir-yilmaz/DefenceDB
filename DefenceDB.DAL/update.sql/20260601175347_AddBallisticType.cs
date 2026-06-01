using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class AddBallisticType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BallisticType",
                table: "BallisticMissiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BallisticMissiles",
                keyColumn: "Id",
                keyValue: 33,
                column: "BallisticType",
                value: null);

            migrationBuilder.UpdateData(
                table: "BallisticMissiles",
                keyColumn: "Id",
                keyValue: 34,
                column: "BallisticType",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BallisticType",
                table: "BallisticMissiles");
        }
    }
}
