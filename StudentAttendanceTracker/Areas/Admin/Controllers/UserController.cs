﻿//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 https://github.com/vgc12
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceTracker.Models;
using System.Linq;
using System.Security.Claims;

namespace StudentAttendanceTracker.Areas.Admin.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    [Route("[area]/{action}")]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AttendanceTrackerContext _context;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AttendanceTrackerContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View(new RegisterViewModel { Roles = _roleManager.Roles });
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            UserViewModel model = new()
            {
                Users = _userManager.Users,

            };
            foreach (User user in model.Users)
            {
                user.RoleNames = await _userManager.GetRolesAsync(user);
            }
            model.Users = model.Users.OrderBy(u => u.RoleNames[0]).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            User? u = await _userManager.FindByIdAsync(id);
            if (u is null)
            {
                return NotFound();
            }
            var model = new UpdateUserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user is null)
                {
                    return NotFound();
                }
                user.FirstName = model.FirstName.ToLower();
                user.LastName = model.LastName.ToLower();
                user.UserName = model.UserName.ToLower();
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                        if (!passwordResult.Succeeded)
                        {
                            foreach (var error in passwordResult.Errors)
                            {
                                ModelState.AddModelError("NewPassword", error.Description);
                            }
                            return View(model);
                        }
                    }

                    var success = await UpdateOtherTables(user);
                    if (!success)
                    {
                        ModelState.AddModelError("", "An error has occured");
                        return View(model);
                    }

                    return RedirectToAction("Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [NonAction]
        private async Task<bool> UpdateOtherTables(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles[0] == null)
            {
                return false;
            }

            switch (roles[0])
            {
                case "Admin":
                    var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == user.Id);
                    if (admin is null)
                    {
                        return false;
                    }
                    admin.FirstName = user.FirstName;
                    admin.LastName = user.LastName;
                    admin.AdministratorEmail = user.UserName!;
                    _context.Admins.Update(admin);
                    break;
                case "Student":
                    var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
                    if (student is null)
                    {
                        return false;
                    }
                    student.FirstName = user.FirstName;
                    student.LastName = user.LastName;
                    student.StudentEmail = user.UserName!;
                    _context.Students.Update(student);
                    break;
                case "Instructor":
                    var instructor = await _context.Instructors.FirstOrDefaultAsync(i => i.UserId == user.Id);
                    if (instructor is null)
                    {
                        return false;
                    }
                    instructor.FirstName = user.FirstName;
                    instructor.LastName = user.LastName;
                    instructor.InstructorEmail = user.UserName!;
                    _context.Instructors.Update(instructor);
                    break;
                case "QualifiedStaff":
                    var qualifiedStaff = await _context.QualifiedStaff.FirstOrDefaultAsync(q => q.UserId == user.Id);
                    if (qualifiedStaff is null)
                    {
                        return false;
                    }
                    qualifiedStaff.FirstName = user.FirstName;
                    qualifiedStaff.LastName = user.LastName;
                    qualifiedStaff.QualifiedStaffEmail = user.UserName!;
                    _context.QualifiedStaff.Update(qualifiedStaff);
                    break;
                default:
                    return false;
            }

            return true;

        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            model.Roles = _roleManager.Roles;
            if (ModelState.IsValid)
            {
                model.Username = model.Username.ToLower();
                model.FirstName = model.FirstName.ToLower();
                model.LastName = model.LastName.ToLower();


                if (await _userManager.FindByNameAsync(model.Username) is not null)
                {
                    ModelState.AddModelError("Username", "Email already in use");
                    ViewBag.Roles = _roleManager.Roles;
                    return View(model);
                }

                if (await _roleManager.FindByNameAsync(model.Role) is null || model.Role == "0")
                {
                    ModelState.AddModelError("Role", "Invalid Role");
                    ViewBag.Roles = _roleManager.Roles;
                    return View(model);
                }



                User user = new()
                {
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);

                    switch (model.Role)
                    {
                        case "Admin":
                            _context.Admins.Add(new Administrator { UserId = user.Id, AdministratorEmail = model.Username, FirstName = model.FirstName, LastName = model.LastName });
                            break;
                        case "Student":
                            _context.Students.Add(new Models.Student { UserId = user.Id, StudentEmail = model.Username, FirstName = model.FirstName, LastName = model.LastName });
                            break;
                        case "Instructor":
                            _context.Instructors.Add(new Models.Instructor { UserId = user.Id, InstructorEmail = model.Username, FirstName = model.FirstName, LastName = model.LastName });
                            break;
                        case "QualifiedStaff":
                            _context.QualifiedStaff.Add(new Models.QualifiedStaff { UserId = user.Id, QualifiedStaffEmail = model.Username, FirstName = model.FirstName, LastName = model.LastName });
                            break;
                        default:
                            ModelState.AddModelError("", "An error has occured");
                            break;
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Home", "Admin");
                }
                else
                {
                    ModelState.AddModelError("Password", $"{result.Errors.First()} An error has occured please try again");
                }


            }

            return View(model);
        }

        /// <summary>
        /// Deletes a user entirely, removes them from all tables
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles[0] == "Admin" && _context.Admins.Count() <= 1)
                {
                    TempData["ErrorMessage"] = "Cannot delete the last admin";
                    return RedirectToAction("Users");
                }
                else if (roles[0] == "Admin" && user.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    TempData["ErrorMessage"] = "Error, you cannot delete yourself";
                    return RedirectToAction("Users");
                }
                await DeleteFromDatabaseTables(user, roles[0]);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.DeleteAsync(user);
                TempData["SuccessMessage"] = $"{user.FirstName.FirstCharToUpper()} {user.LastName.FirstCharToUpper()} was deleted successfully";
            }
            return await Task.Run(() =>
            {
                return RedirectToAction("Users");
            });
        }

        [NonAction]
        /// <summary>
        /// This method delets a identity user from the database tables, such as Admin, Student, Instructor, and QualifiedStaff
        /// </summary>
        /// <param name="user">The user to wipe from the database</param>
        /// <returns></returns>
        private async Task DeleteFromDatabaseTables(User? user, string role)
        {
            switch (role)
            {
                case "Admin":
                    var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == user.Id);
                    if (admin is not null)
                    {
                        _context.Admins.Remove(admin);
                    }
                    break;
                case "Student":
                    var student = await _context.Students.Include(s => s.Courses).FirstOrDefaultAsync(s => s.UserId == user.Id);
                    if (student is not null)
                    {
                        var courses = _context.Courses.Include(c => c.Students).Where(c => c.Students.Contains(student));
                        foreach (Course c in courses)
                        {
                            c.Students.Remove(student);
                            _context.Courses.Update(c);
                        }
                        _context.Students.Remove(student);
                    }
                    break;
                case "Instructor":
                    var instructor = await _context.Instructors.Include(I => I.Courses).FirstOrDefaultAsync(i => i.UserId == user.Id);
                    if (instructor is not null)
                    {
                        foreach (Course c in instructor.Courses)
                        {
                            c.InstructorId = null;
                            _context.Courses.Update(c);
                        }
                        _context.Instructors.Remove(instructor);
                        
                    }

                    break;
                case "QualifiedStaff":
                    var qualifiedStaff = await _context.QualifiedStaff.FirstOrDefaultAsync(q => q.UserId == user.Id);
                    if (qualifiedStaff is not null)
                    {
                        _context.QualifiedStaff.Remove(qualifiedStaff);
                    }
                    break;
            }
            await _context.SaveChangesAsync();

        }
    }
}
