using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceDB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddProductReadModelIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_CategoryId",
                table: "ProductReadModels",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_Country",
                table: "ProductReadModels",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_CreatedAt",
                table: "ProductReadModels",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_IsActive_IsShowcase",
                table: "ProductReadModels",
                columns: new[] { "IsActive", "IsShowcase" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_ProductType",
                table: "ProductReadModels",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReadModels_Slug",
                table: "ProductReadModels",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductReadModels_CategoryId",
                table: "ProductReadModels");

            migrationBuilder.DropIndex(
                name: "IX_ProductReadModels_Country",
                table: "ProductReadModels");

            migrationBuilder.DropIndex(
                name: "IX_ProductReadModels_CreatedAt",
                table: "ProductReadModels");

            migrationBuilder.DropIndex(
                name: "IX_ProductReadModels_IsActive_IsShowcase",
                table: "ProductReadModels");

            migrationBuilder.DropIndex(
                name: "IX_ProductReadModels_ProductType",
                table: "ProductReadModels");

            migrationBuilder.DropIndex(
                name: "IX_ProductReadModels_Slug",
                table: "ProductReadModels");
        }
    }
}
