using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace car_rental_Unicom.tic.Controllers
{
    public class Chack_outController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Chack_outController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE: Add a new booking
        [HttpPost]
        public IActionResult Create(Booking_modal booking)
        {
            if (ModelState.IsValid)
            {
                booking.booking_id = Guid.NewGuid();
                booking.boking_status = "active";
                _context.Bookings.Add(booking);
                _context.SaveChanges();
                TempData["Success"] = "Booking created successfully!";
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        // READ: View booking details (with car details)
        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.booking_id == id);
            if (booking == null)
                return NotFound();

            var car = _context.Cars.FirstOrDefault(c => c.CarId.ToString() == booking.car_id);
            var viewModel = new home_view_modal
            {
                booking_id = booking.booking_id,

                car_id = booking.car_id,
                user_id = booking.user_id,
                name = booking.name,
                start_date = booking.start_date,
                dayes = booking.dayes,
                boking_status = booking.boking_status,
                Car_modalName = car?.Car_modalName,
                year = car?.year ?? 0,
                number_plact = car?.number_plact,
                ac = car?.ac,
                top_speed = car?.top_speed,
                Gear_System = car?.Gear_System,
                milage = car?.milage,
                car_status = car?.car_status,
                image_path = car?.image_path,
                MaintenanceCharge = car?.MaintenanceCharge,
                RentPerDay = car?.RentPerDay
            };

            return View(viewModel);
        }

        // UPDATE: Update booking details (e.g., status, license number)
        [HttpPost]
        public IActionResult Update(Guid id, Booking_modal updatedBooking)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.booking_id == id);
            if (booking == null)
                return NotFound();

            booking.licence_number = updatedBooking.licence_number;
            booking.start_date = updatedBooking.start_date;
            booking.dayes = updatedBooking.dayes;
            booking.boking_status = updatedBooking.boking_status;

            _context.Bookings.Update(booking);
            _context.SaveChanges();
            TempData["Success"] = "Booking updated successfully!";
            return RedirectToAction("Details", new { id = booking.booking_id });
        }

        // DELETE: Delete a booking
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.booking_id == id);
            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            TempData["Success"] = "Booking deleted successfully!";
            return RedirectToAction("chack_out", "booking");
        }

        // CHECKOUT: Confirm checkout and update statuses
        [HttpPost]
        public IActionResult Checkout(Guid id, int licence_number)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.booking_id == id);
            if (booking == null)
                return NotFound();

            if (booking.licence_number != licence_number)
            {
                ModelState.AddModelError("licence_number", "License number does not match.");
                return RedirectToAction("Details", new { id });
            }

            // Update booking status
            booking.boking_status = "booking_fnnisheg";

            // Update car status
            var car = _context.Cars.FirstOrDefault(c => c.CarId.ToString() == booking.car_id);
            if (car != null)
                car.car_status = "not available";

            _context.SaveChanges();
            TempData["Success"] = "Checkout completed successfully!";
            return RedirectToAction("chack_out", "booking");
        }

        
    }
}