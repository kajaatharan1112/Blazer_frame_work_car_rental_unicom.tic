using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Vew_modal;
using Microsoft.AspNetCore.Mvc;

namespace car_rental_Unicom.tic.Controllers
{
    public class bookingController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public bookingController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult chack_out()
        {
            var bookings = dbContext.Bookings
                .Select(b => new home_view_modal
                {
                    booking_id = b.booking_id,
                    licence_number = b.licence_number,
                    car_id = b.car_id,
                    user_id = b.user_id,
                    name = b.name,
                    start_date = b.start_date,
                    dayes = b.dayes,
                    boking_status = b.boking_status
                })
                .ToList();

            return View(bookings);
        }   
        public IActionResult booking()
        {
            var bookings = dbContext.Bookings
                .Select(b => new home_view_modal
                {
                    booking_id = b.booking_id,
                    licence_number = b.licence_number,
                    car_id = b.car_id,
                    user_id = b.user_id,
                    name = b.name,
                    start_date = b.start_date,
                    dayes = b.dayes,
                    boking_status = b.boking_status
                })
                .ToList();

            return View(bookings);
        }

        [HttpGet]
        public IActionResult ViewCar(Guid carId)
        {
            var car = dbContext.Cars.Find(carId);
            if (car == null) return NotFound();

            var vm = new home_view_modal
            {
                CarId = car.CarId,
                Car_modalName = car.Car_modalName,
                year = car.year,
                image_path = car.image_path,
                number_plact = car.number_plact,
                ac = car.ac,
                top_speed = car.top_speed,
                Gear_System = car.Gear_System,
                milage = car.milage,
                car_status = car.car_status,
                MaintenanceCharge = car.MaintenanceCharge,
                RentPerDay = car.RentPerDay
            };
            return View(vm);
        }
    }
}
