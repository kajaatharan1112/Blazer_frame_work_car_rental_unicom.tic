using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
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
            // Users table empty ahh check pannum
            if (!dbContext.Users.Any())
            {
                var adminUser = new Users_Modalcs
                {
                    Id = Guid.NewGuid(),
                    Name = "kajaa",
                    Role = "Admin",
                    UserName = "Admin",
                    password = "1234" // ⚠ plain text password ahh save panna koodathu, hashing better
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
                    // ✅ Static class பயன்படுத்தி login info save பண்ணும்
                    sacation.name = user.UserName;
                    sacation.roll = user.Role;

                   /* // அல்லது session-ல் save பண்ணனும் (recommended)
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("Role", user.Role);*/

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

    }
}
