using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerseTemplate.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // Db tables
        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Sci-Fi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 7,
                    Title = "Whispers of the Night",
                    Author = "Gina Harper",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WHN8888888801",
                    ListPrice = 85,
                    Price = 80,
                    Price50 = 75,
                    Price100 = 70,
                    CategoryId = 3,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 8,
                    Title = "Journey to the Unknown",
                    Author = "Sam Shadow",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "JTU1111111101",
                    ListPrice = 60,
                    Price = 55,
                    Price50 = 50,
                    Price100 = 45,
                    CategoryId = 1,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 9,
                    Title = "Mystic River",
                    Author = "Claire Green",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "MR5555555501",
                    ListPrice = 50,
                    Price = 45,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 2,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 10,
                    Title = "Echoes of Eternity",
                    Author = "Dylan Frost",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "EE9999999901",
                    ListPrice = 75,
                    Price = 70,
                    Price50 = 65,
                    Price100 = 60,
                    CategoryId = 3,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 11,
                    Title = "Shadows and Silhouettes",
                    Author = "Nina Nocturne",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SS2222222201",
                    ListPrice = 35,
                    Price = 30,
                    Price50 = 28,
                    Price100 = 25,
                    CategoryId = 1,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 12,
                    Title = "Winds of Change",
                    Author = "Ella Breeze",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WC3333333301",
                    ListPrice = 65,
                    Price = 60,
                    Price50 = 55,
                    Price100 = 50,
                    CategoryId = 2,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 13,
                    Title = "The Lost Expedition",
                    Author = "Zachary Quill",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "TLE4444444401",
                    ListPrice = 45,
                    Price = 40,
                    Price50 = 38,
                    Price100 = 35,
                    CategoryId = 3,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 14,
                    Title = "Legends of the Wild",
                    Author = "Fiona Ranger",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "LW6666666601",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 48,
                    Price100 = 45,
                    CategoryId = 1,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 15,
                    Title = "Beneath the Stars",
                    Author = "Olivia Moon",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "BS7777777701",
                    ListPrice = 40,
                    Price = 35,
                    Price50 = 32,
                    Price100 = 30,
                    CategoryId = 2,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 16,
                    Title = "The Silent Forest",
                    Author = "Hazel Wood",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "TSF8888888801",
                    ListPrice = 95,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 3,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 17,
                    Title = "Waves of Time",
                    Author = "Leo Waters",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WOT1111111101",
                    ListPrice = 60,
                    Price = 55,
                    Price50 = 50,
                    Price100 = 45,
                    CategoryId = 2,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 18,
                    Title = "The Last Frontier",
                    Author = "Emily Quest",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "TLF9999999901",
                    ListPrice = 75,
                    Price = 70,
                    Price50 = 65,
                    Price100 = 60,
                    CategoryId = 1,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 19,
                    Title = "City of Secrets",
                    Author = "Ivy Shadow",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "COS2222222201",
                    ListPrice = 45,
                    Price = 40,
                    Price50 = 35,
                    Price100 = 30,
                    CategoryId = 3,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 20,
                    Title = "Mysteries of the Deep",
                    Author = "Oliver Tide",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "MOD5555555501",
                    ListPrice = 85,
                    Price = 80,
                    Price50 = 75,
                    Price100 = 70,
                    CategoryId = 2,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 21,
                    Title = "Echoes of the Past",
                    Author = "Sophia Ember",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "EOP6666666601",
                    ListPrice = 50,
                    Price = 45,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 1,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 22,
                    Title = "Windswept Plains",
                    Author = "George Gale",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WSP4444444401",
                    ListPrice = 40,
                    Price = 35,
                    Price50 = 30,
                    Price100 = 25,
                    CategoryId = 3,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 23,
                    Title = "Twilight Meadows",
                    Author = "Hannah Dawn",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "TM7777777701",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 45,
                    Price100 = 40,
                    CategoryId = 2,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 24,
                    Title = "Fires of Autumn",
                    Author = "Charles Blaze",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOA8888888801",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 1,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 25,
                    Title = "Chasing the Horizon",
                    Author = "Violet Ray",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CTH9999999901",
                    ListPrice = 65,
                    Price = 60,
                    Price50 = 55,
                    Price100 = 50,
                    CategoryId = 2,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 26,
                    Title = "Tales of the Unknown",
                    Author = "Isaac Story",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "TOU000000001",
                    ListPrice = 35,
                    Price = 30,
                    Price50 = 28,
                    Price100 = 25,
                    CategoryId = 3,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 27,
                    Title = "The Hidden Oasis",
                    Author = "Lily Sands",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "THO2222222201",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 45,
                    Price100 = 40,
                    CategoryId = 1,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 28,
                    Title = "Whispers in the Wind",
                    Author = "Daisy Field",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WIW3333333301",
                    ListPrice = 45,
                    Price = 40,
                    Price50 = 38,
                    Price100 = 35,
                    CategoryId = 3,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 29,
                    Title = "Moonlit Nights",
                    Author = "Sebastian Dark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "MN4444444401",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 2,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                },
                new Product
                {
                    Id = 30,
                    Title = "Wonders of the Sky",
                    Author = "Aurora Skies",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WOS5555555501",
                    ListPrice = 85,
                    Price = 80,
                    Price50 = 75,
                    Price100 = 70,
                    CategoryId = 1,
                    ImageUrl = "images\\product\\2f063d7d-daab-4a52-baf0-2eb02ff7b74b.jpg"
                }
            );
        }
    }
}
