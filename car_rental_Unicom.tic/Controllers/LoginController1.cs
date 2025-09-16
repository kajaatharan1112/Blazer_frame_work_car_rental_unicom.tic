using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using car_rental_Unicom.tic.View_modal; // ✅ spelling correct
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_rental_Unicom.tic.Controllers
{
    public class LoginController1 : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public LoginController1(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Login()
        {
            if (!dbContext.Users.Any())
            {
                var adminUser = new Users_Modalcs
                {
                    Id = Guid.NewGuid(),
                    Name = "kajaa",
                    Role = "Admin",
                    UserName = "Admin",
                    password = "1234"
                };

                dbContext.Users.Add(adminUser);
                dbContext.SaveChanges();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(login_vew_modal model)
        {
            if (ModelState.IsValid)
            {
                var user = dbContext.Users
                    .FirstOrDefault(u => u.UserName == model.username && u.password == model.password);

                if (user != null)
                {
                    // ✅ Static class
                    Sacation.Name = user.UserName;
                    Sacation.Role = user.Role;
                    Sacation.id=user.Id;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Invalid Username or Password";
                    return View(model);
                }
            }

            return View(model);
        }
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

            return RedirectToAction("Login");
        }
    }
}
