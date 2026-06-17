using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFoxCodeToByte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "FoxCodeNew",
                table: "AirToAirMissiles",
                type: "tinyint",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE AirToAirMissiles SET FoxCodeNew = 2 WHERE FoxCode = 'Fox 2';
                UPDATE AirToAirMissiles SET FoxCodeNew = 3 WHERE FoxCode = 'Fox 3';
                """);

            migrationBuilder.DropColumn(
                name: "FoxCode",
                table: "AirToAirMissiles");

            migrationBuilder.RenameColumn(
                name: "FoxCodeNew",
                table: "AirToAirMissiles",
                newName: "FoxCode");

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 12,
                column: "FoxCode",
                value: (byte)2);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 13,
                column: "FoxCode",
                value: (byte)3);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 14,
                column: "FoxCode",
                value: (byte)3);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 15,
                column: "FoxCode",
                value: (byte)2);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 16,
                column: "FoxCode",
                value: (byte)3);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 17,
                column: "FoxCode",
                value: (byte)2);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 18,
                column: "FoxCode",
                value: (byte)3);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 19,
                column: "FoxCode",
                value: (byte)2);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 20,
                column: "FoxCode",
                value: (byte)3);

            migrationBuilder.UpdateData(
                table: "AirToAirMissiles",
                keyColumn: "Id",
                keyValue: 21,
                column: "FoxCode",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FoxCode",
                table: "AirToAirMissiles",
                newName: "FoxCodeNew");

            migrationBuilder.AddColumn<string>(
                name: "FoxCode",
                table: "AirToAirMissiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE AirToAirMissiles SET FoxCode = 'Fox 2 / Fox 3' WHERE Id = 21;
                UPDATE AirToAirMissiles SET FoxCode = 'Fox 2' WHERE FoxCodeNew = 2 AND Id <> 21;
                UPDATE AirToAirMissiles SET FoxCode = 'Fox 3' WHERE FoxCodeNew = 3 AND Id <> 21;
                """);

            migrationBuilder.DropColumn(
                name: "FoxCodeNew",
                table: "AirToAirMissiles");
        }
    }
}
