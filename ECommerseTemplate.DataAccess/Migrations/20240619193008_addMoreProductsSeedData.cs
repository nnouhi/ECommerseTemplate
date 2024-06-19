using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerseTemplate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addMoreProductsSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 7, "Gina Harper", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "WHN8888888801", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 85f, 80f, 70f, 75f, "Whispers of the Night" },
                    { 8, "Sam Shadow", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "JTU1111111101", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 60f, 55f, 45f, 50f, "Journey to the Unknown" },
                    { 9, "Claire Green", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "MR5555555501", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 50f, 45f, 35f, 40f, "Mystic River" },
                    { 10, "Dylan Frost", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "EE9999999901", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 75f, 70f, 60f, 65f, "Echoes of Eternity" },
                    { 11, "Nina Nocturne", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "SS2222222201", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 35f, 30f, 25f, 28f, "Shadows and Silhouettes" },
                    { 12, "Ella Breeze", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "WC3333333301", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 65f, 60f, 50f, 55f, "Winds of Change" },
                    { 13, "Zachary Quill", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "TLE4444444401", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 45f, 40f, 35f, 38f, "The Lost Expedition" },
                    { 14, "Fiona Ranger", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "LW6666666601", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 55f, 50f, 45f, 48f, "Legends of the Wild" },
                    { 15, "Olivia Moon", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "BS7777777701", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 40f, 35f, 30f, 32f, "Beneath the Stars" },
                    { 16, "Hazel Wood", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "TSF8888888801", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 95f, 90f, 80f, 85f, "The Silent Forest" },
                    { 17, "Leo Waters", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "WOT1111111101", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 60f, 55f, 45f, 50f, "Waves of Time" },
                    { 18, "Emily Quest", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "TLF9999999901", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 75f, 70f, 60f, 65f, "The Last Frontier" },
                    { 19, "Ivy Shadow", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "COS2222222201", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 45f, 40f, 30f, 35f, "City of Secrets" },
                    { 20, "Oliver Tide", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "MOD5555555501", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 85f, 80f, 70f, 75f, "Mysteries of the Deep" },
                    { 21, "Sophia Ember", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "EOP6666666601", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 50f, 45f, 35f, 40f, "Echoes of the Past" },
                    { 22, "George Gale", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "WSP4444444401", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 40f, 35f, 25f, 30f, "Windswept Plains" },
                    { 23, "Hannah Dawn", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "TM7777777701", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 55f, 50f, 40f, 45f, "Twilight Meadows" },
                    { 24, "Charles Blaze", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "FOA8888888801", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 70f, 65f, 55f, 60f, "Fires of Autumn" },
                    { 25, "Violet Ray", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "CTH9999999901", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 65f, 60f, 50f, 55f, "Chasing the Horizon" },
                    { 26, "Isaac Story", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "TOU000000001", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 35f, 30f, 25f, 28f, "Tales of the Unknown" },
                    { 27, "Lily Sands", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "THO2222222201", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 55f, 50f, 40f, 45f, "The Hidden Oasis" },
                    { 28, "Daisy Field", 3, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "WIW3333333301", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 45f, 40f, 35f, 38f, "Whispers in the Wind" },
                    { 29, "Sebastian Dark", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "MN4444444401", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 70f, 65f, 55f, 60f, "Moonlit Nights" },
                    { 30, "Aurora Skies", 1, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "WOS5555555501", "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg", 85f, 80f, 70f, 75f, "Wonders of the Sky" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);
        }
    }
}
