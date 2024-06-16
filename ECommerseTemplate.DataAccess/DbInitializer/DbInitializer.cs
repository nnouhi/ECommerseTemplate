using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerseTemplate.DataAccess.DbInitializer
{
	public class DbInitializer : IDbInitializer
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _db;

		public DbInitializer(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			ApplicationDbContext db)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_db = db;
		}

		public void Initialize()
		{
			// Init migrations if they are not applied
			if (_db.Database.GetPendingMigrations().Count() > 0)
			{
				_db.Database.Migrate();
			}

			// Init roles if thet are not created
			// NOTE: Can also use if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult()) {}
			bool roleExists = _roleManager.RoleExistsAsync(SD.Roles.Admin).GetAwaiter().GetResult();
			if (!roleExists)
			{
				_roleManager.CreateAsync(new IdentityRole(SD.Roles.Admin)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(SD.Roles.Employee)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(SD.Roles.Company)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(SD.Roles.Customer)).GetAwaiter().GetResult();

				// Init default admin user
				_userManager.CreateAsync(new ApplicationUser
				{
					UserName = "admin@template.com",
					Email = "admin@template.com",
					Name = "Nicolas Test",
					PhoneNumber = "96891108",
					StreetAddress = "Test Street Address 5",
					City = "Paphos",
					Country = "Cyprus",
					PostalCode = "1234"
				}, "Admin123!").GetAwaiter().GetResult();

				// Fetch and add admin role to our user
				ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@template.com");
				if (user != null)
				{
					_userManager.AddToRoleAsync(user, SD.Roles.Admin).GetAwaiter().GetResult();
				}
			}
		}
	}
}
