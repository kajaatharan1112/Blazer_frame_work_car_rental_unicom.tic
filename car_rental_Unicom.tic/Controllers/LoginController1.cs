using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using car_rental_Unicom.tic.View_modal; // ✅ spelling correct
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace car_rental_Unicom.tic.Controllers
{
    public class LoginController1 : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public LoginController1(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpPost]
        public IActionResult SendOtp(string email)
        {
            try
            {
                Random rnd = new Random();
                int otp = rnd.Next(100000, 999999); // 6 digit OTP

                // Save OTP in static session class
                Sacation.Email_otp = otp;

                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("carrental2025unicom@gmail.com", "niksan2004");

                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("carrental2025unicom@gmail.com");
                    mailMessage.To.Add(email);
                    mailMessage.Subject = "Your Car Rental OTP Code";
                    mailMessage.Body = $"Hello,\n\nYour OTP code is: {otp}\n\nUse this code to verify your email.\n\nCar Rental System";

                    client.Send(mailMessage);
                }

                return Json(new { success = true, message = "OTP sent successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            if (Sacation.Email_otp.ToString() == otp)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Invalid OTP. Try again." });
            }
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
