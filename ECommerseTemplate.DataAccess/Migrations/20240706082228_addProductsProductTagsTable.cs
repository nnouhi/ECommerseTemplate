using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerseTemplate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addProductsProductTagsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsProductTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsProductTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsProductTags_ProductTags_ProductTagId",
                        column: x => x.ProductTagId,
                        principalTable: "ProductTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProductTags_ProductId",
                table: "ProductsProductTags",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsProductTags_ProductTagId",
                table: "ProductsProductTags",
                column: "ProductTagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsProductTags");
        }
    }
}
