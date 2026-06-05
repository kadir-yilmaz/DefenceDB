using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FirstVisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastVisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_FirstVisitDate",
                table: "Visitors",
                column: "FirstVisitDate");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_VisitorHash",
                table: "Visitors",
                column: "VisitorHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visitors");
        }
    }
}
