using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.update.sql
{
    /// <inheritdoc />
    public partial class UpdateTankModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CrewCount",
                table: "Tanks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAutoloader",
                table: "Tanks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrewCount",
                table: "Tanks");

            migrationBuilder.DropColumn(
                name: "HasAutoloader",
                table: "Tanks");
        }
    }
}
