using ASP.NET_MVC_Application.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_MVC_Application.Migrations;
using System.Net;

namespace ASP.NET_MVC_Application.Controllers
{
    [CustomAuthorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Role/List
        public ActionResult List()
        {
             var model = new List<RoleUserViewModel>();

                // Retrieve all roles
                var roles = RoleManager.Roles.ToList();

                // Retrieve all users
                var users = UserManager.Users.ToList();

                // Loop through each role
                foreach (var role in roles)
                {
                    // Loop through each user
                    foreach (var user in users)
                    {
                        // Check if user is in the role
                        if (UserManager.IsInRole(user.Id, role.Name))
                        {
                            if(role.Name != "User")
                        {
                            model.Add(new RoleUserViewModel
                            {
                                RoleName = role.Name,
                                UserName = user.UserName
                            });
                        }
                        }
                    }
                }

                return View(model);
           

        }

        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(RegisterRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check role limits
                if (model.RoleName.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase))
                {
                    if (RoleManager.Roles.Any(r => r.Name.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase)))
                    {
                        ModelState.AddModelError("", "SuperAdmin role already exists.");
                        return View(model);
                    }
                }
                else if (model.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    if (RoleManager.Roles.Count(r => r.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase)) >= 2)
                    {
                        ModelState.AddModelError("", "Maximum number of Admins reached.");
                        return View(model);
                    }
                }

                IdentityRole role = new IdentityRole { Name = model.RoleName };
                IdentityResult result = RoleManager.Create(role);
                if (result.Succeeded)
                {
                    string RoleId = role.Id;
                    return RedirectToAction("Index", "Home", new { Id = RoleId });
                }
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }

        public ActionResult UpdateRole()
        {
            var model = new EditRoleViewModel
            {
                Roles = RoleManager.Roles.Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the role by Id
                IdentityRole roleToUpdate = RoleManager.FindByName(model.SelectedRoleName);
                if (roleToUpdate == null)
                {
                    return HttpNotFound("Role not found.");
                }

                // Validation to prevent updating SuperAdmin, Admin, and User roles
                if (roleToUpdate.Name.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase) ||
                    roleToUpdate.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    roleToUpdate.Name.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("", $"{roleToUpdate.Name} role cannot be updated because it is essential for the system.");
                    RepopulateRoles(model); // Repopulate dropdown after validation
                    return View(model);
                }

                // Proceed with the update if validation passes
                roleToUpdate.Name = model.NewRoleName;
                IdentityResult result = RoleManager.Update(roleToUpdate);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"{roleToUpdate.Name} role was updated successfully.";
                    return RedirectToAction("Index", "Home");
                }

                // Handle errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            // Repopulate dropdowns in case of an error
            RepopulateRoles(model);
            return View(model);
        }


        // Helper method to repopulate roles dropdown list
        private void RepopulateRoles(EditRoleViewModel model)
        {
            model.Roles = RoleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();
        }


        public ActionResult DeleteRole()
        {
            var model = new DeleteRoleViewModel
            {
                Roles = RoleManager.Roles.Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult DeleteRole(DeleteRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the role by Id
                IdentityRole roleToDelete = RoleManager.FindByName(model.SelectedRoleName);
                if (roleToDelete == null)
                {
                    return HttpNotFound("Role not found.");
                }

                // Validation to prevent deletion of SuperAdmin, Admin, and User roles
                if (roleToDelete.Name.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase) ||
                    roleToDelete.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    roleToDelete.Name.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("", $"{roleToDelete.Name} role cannot be deleted because it is essential for the system.");
                    RepopulateRoles(model); // Repopulate dropdown after validation
                    return View(model);
                }

                // Proceed with deletion if validation passes
                IdentityResult result = RoleManager.Delete(roleToDelete);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"{roleToDelete.Name} role was deleted successfully.";
                    return RedirectToAction("Index", "Home");
                }

                // Handle errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            // Repopulate dropdowns in case of an error
            RepopulateRoles(model);
            return View(model);
        }


        // Helper method to repopulate roles dropdown list
        private void RepopulateRoles(DeleteRoleViewModel model)
        {
            model.Roles = RoleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();
        }

        public ActionResult AssignRole()
        {
            var model = new AssignRoleViewModel
            {
                Users = UserManager.Users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                }).ToList(),

                Roles = RoleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AssignRole(AssignRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by Id
                var user = UserManager.FindById(model.SelectedUserId);
                if (user == null)
                {
                    return HttpNotFound("User not found");
                }

                // Get the roles currently assigned to the user
                var roles = UserManager.GetRoles(user.Id);

                // Validation for SuperAdmin and Admin conflict
                if (model.SelectedRoleName.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase) && roles.Contains("Admin"))
                {
                    ModelState.AddModelError("", "User cannot be both SuperAdmin and Admin.");
                    RepopulateUsersAndRoles(model); // Ensure dropdown lists are repopulated
                    return View(model);
                }
                else if (model.SelectedRoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase) && roles.Contains("SuperAdmin"))
                {
                    ModelState.AddModelError("", "User cannot be both SuperAdmin and Admin.");
                    RepopulateUsersAndRoles(model); // Ensure dropdown lists are repopulated
                    return View(model);
                }

                // Assign the role to the user
                var result = UserManager.AddToRole(user.Id, model.SelectedRoleName);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Handle errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            // Always repopulate the dropdown lists if model is invalid or assignment fails
            RepopulateUsersAndRoles(model);
            return View(model);
        }

        // Helper method to repopulate Users and Roles dropdown lists
        private void RepopulateUsersAndRoles(AssignRoleViewModel model)
        {
            model.Users = UserManager.Users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            }).ToList();

            model.Roles = RoleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
        }

    }
}