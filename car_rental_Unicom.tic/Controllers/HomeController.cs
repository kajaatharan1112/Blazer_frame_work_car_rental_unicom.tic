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
