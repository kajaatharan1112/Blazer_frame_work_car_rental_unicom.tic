using Microsoft.AspNetCore.Mvc;

namespace car_rental_Unicom.tic.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Car()
        {
            return View();
        }
    }
}
