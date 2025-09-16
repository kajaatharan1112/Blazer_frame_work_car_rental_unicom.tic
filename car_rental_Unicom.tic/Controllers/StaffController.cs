using car_rental_Unicom.tic.Data;
using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace car_rental_Unicom.tic.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StaffController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // 🟢 View Staff Details by Session ID
        public async Task<IActionResult> profil()
        {
            var id = Sacation.id; // Get the staff ID from the static session class

            var staff = await dbContext.Staffs.FindAsync(id);
            if (staff == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(id);

            var vm = new Staff_vew_modal
            {
                Id = staff.id,
                Name = staff.name,
                ContactNo = staff.ContactNo,
                Age = staff.Age,
                Gender = staff.Gender,
                Email = staff.Email,
                UserName = user?.UserName,
                Password = user?.password
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit_profil(Guid id)
        {
            var staff = await dbContext.Staffs.FindAsync(id);
            if (staff == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(id);

            var vm = new Staff_vew_modal
            {
                Id = staff.id,
                Name = staff.name,
                ContactNo = staff.ContactNo,
                Age = staff.Age,
                Gender = staff.Gender,
                Email = staff.Email,
                UserName = user?.UserName,
                Password = user?.password
            };

            return View(vm);
        }

        // 🟡 Edit Staff (POST)
        [HttpPost]
        public async Task<IActionResult> Edit_profil(Staff_vew_modal vm, string NewPassword, string ConfirmPassword)
        {
            var staff = await dbContext.Staffs.FindAsync(vm.Id);
            if (staff == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(vm.Id);
            if (user == null)
                return NotFound();

            if (!string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmPassword)
            {
                user.password = NewPassword;
            }

            staff.name = vm.Name;
            staff.ContactNo = vm.ContactNo;
            staff.Age = vm.Age;
            staff.Gender = vm.Gender;
            staff.Email = vm.Email;

            user.Name = vm.Name;
            user.UserName = vm.UserName;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("profil");
        }
        [HttpPost]
        public async Task<IActionResult> Delete_profil(Guid id)
        {
            var staff = await dbContext.Staffs.FindAsync(id);
            var user = await dbContext.Users.FindAsync(id);

            if (staff != null)
                dbContext.Staffs.Remove(staff);

            if (user != null)
                dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // 🟢 View All Staff
        public async Task<IActionResult> Index()
        {
            var staffList = await dbContext.Staffs.ToListAsync();
            var users = await dbContext.Users.ToListAsync();

            var viewModels = staffList.Select(s =>
            {
                var user = users.FirstOrDefault(u => u.Id == s.id);
                return new Staff_vew_modal
                {
                    Id = s.id,
                    Name = s.name,
                    ContactNo = s.ContactNo,
                    Age = s.Age,
                    Gender = s.Gender,
                    Email = s.Email,
                    UserName = user?.UserName,
                    Password = user?.password
                };
            }).ToList();

            return View(viewModels);
        }

       

        // 🟢 Add Staff (GET)
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // 🟢 Add Staff (POST)
        [HttpPost]
        public async Task<IActionResult> Add(Staff_vew_modal vm)
        {
            if (dbContext.Users.Any(u => u.UserName == vm.UserName))
            {
                ModelState.AddModelError("UserName", "This username is already taken.");
                return View(vm);
            }

            var staff = new Staff_modal
            {
                id = Guid.NewGuid(),
                name = vm.Name,
                ContactNo = vm.ContactNo,
                Age = vm.Age,
                Gender = vm.Gender,
                Email = vm.Email
            };

            dbContext.Staffs.Add(staff);
            await dbContext.SaveChangesAsync();

            var user = new Users_Modalcs
            {
                Id = staff.id,
                Name = vm.Name,
                Role = "Staff",
                UserName = vm.UserName,
                password = vm.Password
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // 🟡 Edit Staff (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var staff = await dbContext.Staffs.FindAsync(id);
            if (staff == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(id);

            var vm = new Staff_vew_modal
            {
                Id = staff.id,
                Name = staff.name,
                ContactNo = staff.ContactNo,
                Age = staff.Age,
                Gender = staff.Gender,
                Email = staff.Email,
                UserName = user?.UserName,
                Password = user?.password
            };

            return View(vm);
        }

        // 🟡 Edit Staff (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Staff_vew_modal vm, string NewPassword, string ConfirmPassword)
        {
            var staff = await dbContext.Staffs.FindAsync(vm.Id);
            if (staff == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(vm.Id);
            if (user == null)
                return NotFound();

            if (!string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmPassword)
            {
                user.password = NewPassword;
            }

            staff.name = vm.Name;
            staff.ContactNo = vm.ContactNo;
            staff.Age = vm.Age;
            staff.Gender = vm.Gender;
            staff.Email = vm.Email;

            user.Name = vm.Name;
            user.UserName = vm.UserName;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // 🔴 Delete Staff
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var staff = await dbContext.Staffs.FindAsync(id);
            var user = await dbContext.Users.FindAsync(id);

            if (staff != null)
                dbContext.Staffs.Remove(staff);

            if (user != null)
                dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // 🟢 Check Unique Username (AJAX)
        [HttpGet]
        public JsonResult IsUserNameUnique(string username)
        {
            bool exists = dbContext.Users.Any(u => u.UserName == username);
            return Json(!exists);
        }
    }
}