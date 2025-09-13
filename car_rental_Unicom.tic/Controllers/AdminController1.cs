using car_rental_Unicom.tic.Data;
using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using car_rental_Unicom.tic.View_modal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace car_rental_Unicom.tic.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // ✅ View all admins
        public IActionResult Index()
        {
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
            admin.ContactNo = int.Parse(vm.ContactNo);
            admin.Age = vm.Age;
            admin.Gender = vm.Gender;
            admin.Email = vm.Email;

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
