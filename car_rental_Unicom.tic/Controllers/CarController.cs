using car_rental_Unicom.tic.Data;
using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.View_modal;
using Microsoft.AspNetCore.Mvc;

namespace car_rental_Unicom.tic.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public CarController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Car()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(car_add_view_modal car)
        {
            var CAR = new Car_modal
            {
                Car_modalName = car.Car_modalName,
                car_status = "Available",
                year = car.year,
                image_path = car.image_path,
                number_plact = car.number_plact,
                ac = car.ac,
                top_speed = car.top_speed,
                Gear_System = car.Gear_System,
                milage = car.milage,

            };

            return RedirectToAction("car", "car");
        }
    }
}
