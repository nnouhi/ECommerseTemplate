using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerseTemplate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changedFieldInProductReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "ProductReviews",
                newName: "IsAdminApproved");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAdminApproved",
                table: "ProductReviews",
                newName: "IsVerified");
        }
    }
}
