using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult RoleManagement(string? userId)
        {
            ApplicationUser user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperties: "Company");
            user.Role = _db.UserRoles.FirstOrDefault(ur => ur.UserId == userId).RoleId;
            IEnumerable<SelectListItem> roleList = _db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            });
            IEnumerable<SelectListItem> companyList = _unitOfWork.Company.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            RoleManagementVM roleManagementVM = new RoleManagementVM()
            {
                User = user,
                RoleList = roleList,
                CompanyList = companyList
            };

            return View(roleManagementVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model state is not valid";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagementVM.User.Id);
                IdentityUserRole<string> oldUserRole = _db.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);

                // Delete the current user role if exists
                if (oldUserRole != null)
                {
                    _db.UserRoles.Remove(oldUserRole);
                }

                // Add a new user role
                IdentityUserRole<string> newUserRole = new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = roleManagementVM.User.Role
                };
                _db.UserRoles.Add(newUserRole);

                // If current role is company, update the user's company id
                string newRoleName = _db.Roles.FirstOrDefault(r => r.Id == roleManagementVM.User.Role)?.Name;
                if (newRoleName == "Company")
                {
                    user.CompanyId = roleManagementVM.User.CompanyId;
                }
                else
                {
                    // If previous role was company, update the user's company id to null
                    string oldRoleName = _db.Roles.FirstOrDefault(r => r.Id == oldUserRole.RoleId)?.Name;
                    if (oldRoleName == "Company")
                    {
                        user.CompanyId = null;
                    }
                }

                _db.ApplicationUsers.Update(user);
                _db.SaveChanges();
                TempData["success"] = "User role updated successfully";
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it accordingly
                TempData["error"] = "Failed to update user role: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
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
