using car_rental_Unicom.tic.Data;
using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace car_rental_Unicom.tic.Controllers
{
    public class AdminController1 : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController1(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // ✅ View all admins
        public IActionResult Index()
        {
            if (!dbContext.Admins.Any())
            {
                var dummyAdmin = new Admin_modalcs
                {
                    Name = "Default Admin",
                    ContactNo = 771234567,
                    Age = 30,
                    Gender = "Male",
                    Email = "admin@demo.com"
                };

                dbContext.Admins.Add(dummyAdmin);
                dbContext.SaveChanges();

                var adminId = dummyAdmin.Id;

                var adminUser = new Users_Modalcs
                {
                    Id = adminId,
                    Name = dummyAdmin.Name,
                    Role = "admin",
                    UserName = "admin",
                    password = "0920"
                };

                dbContext.Users.Add(adminUser);
                dbContext.SaveChanges();
            }

            var admins = dbContext.Admins
                .Select(a => new admin_view_modal
                {
                    Id = a.Id,
                    Name = a.Name,
                    ContactNo = a.ContactNo.ToString(),
                    Age = a.Age,
                    Gender = a.Gender,
                    Email = a.Email
                })
                .ToList();

            return View(admins);
        }

        // ✅ Edit (GET)
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var admin = dbContext.Admins.Find(id);

            if (admin == null)
                return NotFound();

            var vm = new admin_view_modal
            {
                Id = admin.Id,
                Name = admin.Name,
                ContactNo = admin.ContactNo.ToString(),
                Age = admin.Age,
                Gender = admin.Gender,
                Email = admin.Email
            };

            return View(vm);
        }

        // ✅ Edit (POST)
        [HttpPost]
        public IActionResult Edit(admin_view_modal vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var admin = dbContext.Admins.Find(vm.Id);
            if (admin == null)
                return NotFound();

            admin.Name = vm.Name;
            if (!int.TryParse(vm.ContactNo, out int contactNo))
            {
                ModelState.AddModelError("ContactNo", "Contact number must be numeric.");
                return View(vm);
            }
            admin.ContactNo = contactNo;
            admin.Age = vm.Age;
            admin.Gender = vm.Gender;
            admin.Email = vm.Email;

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        // Change Password (GET)
        [HttpGet]
        public IActionResult ChangePassword(Guid id)
        {
            var vm = new admin_view_modal { Id = id };
            return View(vm);
        }

        // Change Password (POST)
        [HttpPost]
        public IActionResult ChangePassword(admin_view_modal vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (vm.NewPassword != vm.ConfirmPassword)
            {
                ModelState.AddModelError("", "New Password and Confirm Password do not match");
                return View(vm);
            }

            var user = dbContext.Users.Find(vm.Id);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(vm);
            }

            if (!string.IsNullOrEmpty(vm.CurrentPassword) && vm.CurrentPassword != user.password)
            {
                ModelState.AddModelError("", "Current password is incorrect");
                return View(vm);
            }

            user.password = vm.NewPassword;
            dbContext.SaveChanges();

            TempData["Success"] = "Password changed successfully!";
            return RedirectToAction("Index");
        }
    }
}