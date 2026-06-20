using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastVisitDate",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "VisitCount",
                table: "Visitors");

            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "Visitors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystem",
                table: "Visitors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "Visitors");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVisitDate",
                table: "Visitors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VisitCount",
                table: "Visitors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
