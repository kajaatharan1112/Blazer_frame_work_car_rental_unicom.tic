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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CustomersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult profil()
        {
            // ✅ Sacation.id கொண்டு customer data எடுத்துக்கிறோம்
            var customer = dbContext.Customers
                              .Where(c => c.Id == Sacation.id)
                              .Select(c => new Customers_vew_modal
                              {
                                  Id = c.Id,
                                  Name = c.Name,
                                  contactNo = c.contactNo,
                                  Age = c.Age,
                                  Gender = c.Gender,
                                  Email = c.Email,
                              })
                              .FirstOrDefault();

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }



        [HttpGet]
        public async Task<IActionResult> Edit_profil(Guid id)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(id);

            var vm = new Customers_vew_modal
            {
                Id = customer.Id,
                Name = customer.Name,
                contactNo = customer.contactNo,
                Age = customer.Age,
                Gender = customer.Gender,
                Email = customer.Email,
                UserName = user?.UserName,
                password = user?.password,

            };

            return View(vm);
        }

        // 🟡 Customer-ஐ Edit செய்யும் POST function
        [HttpPost]
        public async Task<IActionResult> Edit_profil(Customers_vew_modal vm, string NewPassword, string ConfirmPassword)
        {
            var customer = await dbContext.Customers.FindAsync(vm.Id);
            if (customer == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(vm.Id);
            if (user == null)
                return NotFound();

            // Check if the new password matches the confirm password
            if (!string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmPassword)
            {
                user.password = NewPassword; // Update the password
            }

            // Update customer details
            customer.Name = vm.Name;
            customer.contactNo = vm.contactNo;
            customer.Age = vm.Age;
            customer.Gender = vm.Gender;
            customer.Email = vm.Email;

            // Update user details
            user.Name = vm.Name;
            user.UserName = vm.UserName;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("profil");
        }
        [HttpPost]
        public async Task<IActionResult> Delete_profil(Guid id)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            var user = await dbContext.Users.FindAsync(id);

            if (customer != null)
                dbContext.Customers.Remove(customer);

            if (user != null)
                dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Login", "LoginController1");
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // 🟢 அனைத்து Customers-ஐ பார்க்கும் function
        public async Task<IActionResult> Index()
        {
            var customers = await dbContext.Customers.ToListAsync();
            var users = await dbContext.Users.ToListAsync();

            // Customer + User info-ஐ ViewModel-ல் சேர்க்கிறது
            var viewModels = customers.Select(c =>
            {
                var user = users.FirstOrDefault(u => u.Id == c.Id);
                return new Customers_vew_modal
                {
                    Id = c.Id,
                    Name = c.Name,
                    contactNo = c.contactNo,
                    Age = c.Age,
                    Gender = c.Gender,
                    Email = c.Email,
                    UserName = user?.UserName,
                    password = user?.password,

                };
            }).ToList();

            return View(viewModels);
        }

        // 🟢 Customer-ஐ Id மூலம் பார்க்கும் function
        public async Task<IActionResult> Details(Guid id)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(id);

            var vm = new Customers_vew_modal
            {
                Id = customer.Id,
                Name = customer.Name,
                contactNo = customer.contactNo,
                Age = customer.Age,
                Gender = customer.Gender,
                Email = customer.Email,
                UserName = user?.UserName,
                password = user?.password,
            };

            return View(vm);
        }

        // 🟢 Customer-ஐ Add செய்யும் GET function
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // 🟢 Customer-ஐ Add செய்யும் POST function
        [HttpPost]
        public async Task<IActionResult> Add(Customers_vew_modal vm)
        {
            // Check if the username is unique
            if (dbContext.Users.Any(u => u.UserName == vm.UserName))
            {
                ModelState.AddModelError("UserName", "This username is already taken.");
                return View(vm);
            }

            // Create a new customer and save it to the database
            var customer = new Customers_modal
            {
                Id = Guid.NewGuid(), // Generate a new unique ID for the customer
                Name = vm.Name,
                contactNo = vm.contactNo,
                Age = vm.Age,
                Gender = vm.Gender,
                Email = vm.Email
            };

            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync(); // Save the customer to generate the ID

            // Use the generated customer ID to create a user
            var user = new Users_Modalcs
            {
                Id = customer.Id, // Use the same ID as the customer
                Name = vm.Name,
                Role = "Customer",
                UserName = vm.UserName,
                password = vm.password
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(); // Save the user

            return RedirectToAction("Index");
        }

        // 🟡 Customer-ஐ Edit செய்யும் GET function
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(id);

            var vm = new Customers_vew_modal
            {
                Id = customer.Id,
                Name = customer.Name,
                contactNo = customer.contactNo,
                Age = customer.Age,
                Gender = customer.Gender,
                Email = customer.Email,
                UserName = user?.UserName,
                password = user?.password,
          
            };

            return View(vm);
        }

        // 🟡 Customer-ஐ Edit செய்யும் POST function
        [HttpPost]
        public async Task<IActionResult> Edit(Customers_vew_modal vm, string NewPassword, string ConfirmPassword)
        {
            var customer = await dbContext.Customers.FindAsync(vm.Id);
            if (customer == null)
                return NotFound();

            var user = await dbContext.Users.FindAsync(vm.Id);
            if (user == null)
                return NotFound();

            // Check if the new password matches the confirm password
            if (!string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmPassword)
            {
                user.password = NewPassword; // Update the password
            }

            // Update customer details
            customer.Name = vm.Name;
            customer.contactNo = vm.contactNo;
            customer.Age = vm.Age;
            customer.Gender = vm.Gender;
            customer.Email = vm.Email;

            // Update user details
            user.Name = vm.Name;
            user.UserName = vm.UserName;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // 🔴 Customer-ஐ Delete செய்யும் function
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            var user = await dbContext.Users.FindAsync(id);

            if (customer != null)
                dbContext.Customers.Remove(customer);

            if (user != null)
                dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // 🟢 Unique username-ஐ check செய்யும் function (AJAX)
        [HttpGet]
        public JsonResult IsUserNameUnique(string username)
        {
            bool exists = dbContext.Users.Any(u => u.UserName == username);
            return Json(!exists);
        }
    }
}
