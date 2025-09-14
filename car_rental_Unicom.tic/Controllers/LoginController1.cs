using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
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
    }
}
