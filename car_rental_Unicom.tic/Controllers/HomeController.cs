using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Globalization;

namespace car_rental_Unicom.tic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext dbContext, ILogger<HomeController> logger)
        {
            this.dbContext = dbContext;
            _logger = logger;
        }

        // View cars for booking
        public IActionResult Index()
        {
            var cars = dbContext.Cars
                .Where(c => c.car_status == "Available")
                .Select(c => new home_view_modal
                {
                    CarId = c.CarId,
                    Car_modalName = c.Car_modalName,
                    year = c.year,
                    image_path = c.image_path,
                    number_plact = c.number_plact,
                    ac = c.ac,
                    top_speed = c.top_speed,
                    Gear_System = c.Gear_System,
                    milage = c.milage,
                    car_status = c.car_status,
                    MaintenanceCharge = c.MaintenanceCharge,
                    RentPerDay = c.RentPerDay
                })
                .ToList();

            return View(cars);
        }

        // Show booking form by CarId (GET)
        [HttpGet]
        public IActionResult AddBooking(Guid carId)
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

        // Save booking (POST)
        [HttpPost]
        public IActionResult AddBooking(home_view_modal vm)
        {
            // Parse start_date and dayes safely
            DateTime startDate;
            int days;
            if (!DateTime.TryParse(vm.start_date, out startDate) || !int.TryParse(vm.dayes, out days))
            {
                ModelState.AddModelError("", "Invalid date or days.");
                return View(vm);
            }

            var booking = new Booking_modal
            {
                booking_id = Guid.NewGuid(),
                licence_number = vm.licence_number,
                car_id = vm.CarId.ToString(),
                user_id = Sacation.id.ToString(),
                name = Sacation.Name,
                start_date = startDate.ToString("yyyy-MM-dd"),
                dayes = days.ToString(),
                boking_status = "active"
            };
            dbContext.Bookings.Add(booking);
            dbContext.SaveChanges();

            TempData["Success"] = "Booking successful!";
            return RedirectToAction("Index");
        }

        // View bookings for session user
        public IActionResult ViewBooking()
        {
            var userId = Sacation.id.ToString();
            var bookings = dbContext.Bookings
                .Where(b => b.user_id == userId &&
                            (b.boking_status == "active" || b.boking_status == "booking_chackout"))
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

            return View("ViewBookingPage", bookings);
        }

        // View bookings for session user (AJAX partial)
        public IActionResult ViewBookingPartial()
        {
            var userId = Sacation.id.ToString();
            var bookings = dbContext.Bookings
                .Where(b => b.user_id == userId &&
                            (b.boking_status == "active" || b.boking_status == "booking_chackout"))
                .Select(b => new home_view_modal
                {
                    booking_id = b.booking_id,
                    car_id = b.car_id,
                    start_date = b.start_date,
                    dayes = b.dayes,
                    boking_status = b.boking_status
                })
                .ToList();

            return PartialView(bookings);
        }

        // View bookings by car id
        public IActionResult ViewCarBookings(Guid carId)
        {
            var bookings = dbContext.Bookings
                .Where(b => b.car_id == carId.ToString())
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

            ViewData["CarId"] = carId;
            return View(bookings);
        }

        // View all bookings
        public IActionResult Privacy()
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

        // Delete booking
        [HttpPost]
        public IActionResult DeleteBooking(Guid bookingId)
        {
            var booking = dbContext.Bookings.Find(bookingId);
            if (booking != null)
            {
                dbContext.Bookings.Remove(booking);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ViewBooking"); 
        }

        // Get booked dates for a car (AJAX)
        [HttpGet]
        public JsonResult GetBookedDates(Guid carId)
        {
            var bookings = dbContext.Bookings
                .Where(b => b.car_id == carId.ToString())
                .Select(b => new { b.start_date, b.dayes })
                .ToList();

            var bookedDates = new List<string>();
            foreach (var booking in bookings)
            {
                DateTime startDate;
                int days;
                if (DateTime.TryParse(booking.start_date, out startDate) && int.TryParse(booking.dayes, out days))
                {
                    for (int i = 0; i < days; i++)
                    {
                        var date = startDate.AddDays(i).ToString("yyyy-MM-dd");
                        bookedDates.Add(date);
                    }
                }
            }
            return Json(bookedDates);
        }
    }
}
