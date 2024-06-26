using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerseTemplate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedDateAddedToProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2846));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2886));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2890));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2892));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2894));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2897));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2899));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2902));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2904));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2907));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2910));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2912));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2915));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2917));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2920));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2922));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2925));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2927));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2930));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2932));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2934));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2937));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2939));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2991));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2994));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2997));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(2999));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(3002));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(3004));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30,
                column: "DateAdded",
                value: new DateTime(2024, 6, 26, 21, 57, 9, 18, DateTimeKind.Local).AddTicks(3006));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Products");
        }
    }
}
