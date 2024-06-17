using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerseTemplate.Areas.Admin.Controllers
{
	[Area(SD.Roles.Admin)]
	[Authorize(Roles = SD.Roles.Admin)]
	public class UserController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ApplicationDbContext _db;

		public UserController(IUnitOfWork unitOfWork, ApplicationDbContext db)
		{
			_unitOfWork = unitOfWork;
			_db = db;
		}

		public IActionResult Index()
		{
			return View();
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();

			List<IdentityUserRole<string>> userRoles = _db.UserRoles.ToList();
			Dictionary<string, string> roles = _db.Roles.ToDictionary(role => role.Id, role => role.Name);
			// Create a dictionary to map UserIds to their Role names
			Dictionary<string, string> userIdToRoleNameMap = userRoles
				.Where(ur => roles.ContainsKey(ur.RoleId))
				.ToDictionary(ur => ur.UserId, ur => roles[ur.RoleId]);


			// Populate Company and Role fields for all users
			foreach (ApplicationUser user in users)
			{
				// If the user is not assigned to a company, set the company field to a new Company with "N/A" as the name
				user.Company ??= new Company { Name = "N/A" };

				// If the user is not assigned to a role, set the role field to "N/A"
				user.Role = userIdToRoleNameMap.TryGetValue(user.Id, out string roleName) ? roleName : "N/A";
			}

			return Json(new { data = users });
		}

		[HttpPost]
		public IActionResult LockUnlock([FromBody] string? id)
		{
			ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
			if (applicationUser == null)
			{
				return Json(new { success = true, message = "Error while locking/unlocking user" });
			}

			if (applicationUser.LockoutEnd > DateTime.Now)
			{
				// User is locked, proceed to unlock
				applicationUser.LockoutEnd = DateTime.Now;
			}
			else
			{
				// User is not locked, proceed to lock
				applicationUser.LockoutEnd = DateTime.Now.AddYears(1000);
			}

			_db.SaveChanges();
			return Json(new { success = true, message = "Operation Successful" });
		}
		#endregion
	}
}
