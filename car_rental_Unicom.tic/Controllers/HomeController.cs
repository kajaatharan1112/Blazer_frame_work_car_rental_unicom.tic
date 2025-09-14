using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
            var booking = new Booking_modal
            {
                booking_id = Guid.NewGuid(),
                licence_number = vm.licence_number,
                car_id = vm.CarId.ToString(),
                user_id = Sacation.id.ToString(),
                name = Sacation.Name,
                start_date = vm.start_date,
                dayes = vm.dayes,
                boking_status = "active"
            };
            dbContext.Bookings.Add(booking);
            dbContext.SaveChanges();

            TempData["Success"] = "Booking successful!";
            return RedirectToAction("Index");
        }

        // View bookings for session user (active, booking_chackout)
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

            return View("ViewBookingPage", bookings); // Calls the new Razor page
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
            // If called via AJAX, return partial, else redirect
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return ViewBookingPartial();
            else
                return RedirectToAction("ViewBooking");
        }
    }
}
