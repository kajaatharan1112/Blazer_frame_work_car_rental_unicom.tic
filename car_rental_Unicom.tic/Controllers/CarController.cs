using car_rental_Unicom.tic.DTO;
using car_rental_Unicom.tic.service.car;
using car_rental_Unicom.tic.View_modal;
using Microsoft.AspNetCore.Mvc;

namespace car_rental_Unicom.tic.Controllers
{
    public class CarController : Controller
    {
        private readonly object _car_service_Interface;

        public CarController(car_service_Interface icar)
        {
            _car_service_Interface= icar;
        }
        public IActionResult Car()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(car_add_view_modal car)
        {
            var car_modal = new Car_DTO
            {
                Car_modalName = car.Car_modalName,     // modal name map
                year = car.year,                       // year map
                image_path = car.image_path,           // image path map
                number_plact = car.number_plact,       // number plate map
                ac = car.ac,                           // AC details map
                top_speed = car.top_speed,             // top speed map
                Gear_System = car.Gear_System,         // gear system map
                milage = car.milage,                   // mileage map
                car_status = "available"

            };

            return RedirectToAction("car", "car");
        }
    }
}
