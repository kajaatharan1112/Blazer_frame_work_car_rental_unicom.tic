using car_rental_Unicom.tic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace car_rental_Unicom.tic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            TestSacation();
        }
        public IActionResult TestSacation()
        {
            // Console log
            Console.WriteLine("=== Sacation Values ===");
            Console.WriteLine("Id: " + Sacation.id);
            Console.WriteLine("Name: " + Sacation.Name);
            Console.WriteLine("Role: " + Sacation.Role);

            return Content("Check Output Window in Visual Studio for Sacation values");
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
