using Microsoft.AspNetCore.Mvc;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.Vew_modal;
using System;
using System.Linq;
using System.Collections.Generic;
using car_rental_Unicom.tic.Data.YourNamespace;

namespace car_rental_Unicom.tic.Controllers
{
    /// <summary>
    /// Controller for handling car check-in, booking, and billing operations.
    /// </summary>
    public class chackinController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor for dependency injection of the database context.
        /// </summary>
        public chackinController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays all bookings with status "booking fnnisheg".
        /// </summary>
        public IActionResult Bookings()
        {
            // Get all bookings with status "booking fnnisheg"
            var bookings = _context.Bookings
/*                .Where(b => b.boking_status == "booking fnnisheg")*/
                .ToList()
                .Select(booking =>
                {
                    var car = _context.Cars.FirstOrDefault(c => c.CarId.ToString() == booking.car_id);
                    return new chackin_vew_modal
                    {
                        booking_id = booking.booking_id,
                        licence_number = booking.licence_number,
                        car_id = booking.car_id,
                        user_id = booking.user_id,
                        name = booking.name,
                        start_date = booking.start_date,
                        dayes = booking.dayes,
                        boking_status = booking.boking_status,
                        CarId = car?.CarId ?? Guid.Empty,
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
                })
                .ToList();

            return View(bookings);
        }

        /// <summary>
        /// Shows details for a specific booking and its car.
        /// </summary>
        public IActionResult BookingDetails(Guid id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.booking_id == id);
            if (booking == null) return NotFound();

            var car = _context.Cars.FirstOrDefault(c => c.CarId.ToString() == booking.car_id);
            var viewModel = new chackin_vew_modal
            {
                booking_id = booking.booking_id,
                licence_number = booking.licence_number,
                car_id = booking.car_id,
                user_id = booking.user_id,
                name = booking.name,
                start_date = booking.start_date,
                dayes = booking.dayes,
                boking_status = booking.boking_status,
                CarId = car?.CarId ?? Guid.Empty,
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

        /// <summary>
        /// GET: Show the Add Bill form for a booking.
        /// </summary>
        [HttpGet]
        public IActionResult AddBill(Guid bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.booking_id == bookingId);
            if (booking == null) return NotFound();

            var car = _context.Cars.FirstOrDefault(c => c.CarId.ToString() == booking.car_id);
            var viewModel = new chackin_vew_modal
            {
                booking_id = booking.booking_id,
                car_id = booking.car_id,
                user_id = booking.user_id,
                name = booking.name,
                start_date = booking.start_date,
                dayes = booking.dayes,
                CarId = car?.CarId ?? Guid.Empty,
                Car_modalName = car?.Car_modalName,
                RentPerDay = car?.RentPerDay
            };
            return View(viewModel);
        }

        /// <summary>
        /// POST: Add a new bill, update car and booking status.
        /// </summary>
        [HttpPost]
        public IActionResult AddBill(chackin_vew_modal model)
        {
            if (ModelState.IsValid)
            {
                // Add the bill
                var bill = new Bills_modal
                {
                    bill_id = Guid.NewGuid(),
                    staff_id = model.staff_id,
                    CarID = model.CarId.ToString(),
                    Booking_id = model.booking_id.ToString(),
                    total_amount = model.total_amount,
                    RentalDuration = model.RentalDuration
                };
                _context.Bills.Add(bill);

                // Update car status to "available"
                var car = _context.Cars.FirstOrDefault(c => c.CarId == model.CarId);
                if (car != null)
                {
                    car.car_status = "available";
                }

                // Update booking status to "chack in"
                var booking = _context.Bookings.FirstOrDefault(b => b.booking_id == model.booking_id);
                if (booking != null)
                {
                    booking.boking_status = "chack in";
                }

                _context.SaveChanges();
                return RedirectToAction("Bills");
            }
            return View(model);
        }

        /// <summary>
        /// GET: Show the Edit Bill form.
        /// </summary>
        [HttpGet]
        public IActionResult EditBill(Guid id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.bill_id == id);
            if (bill == null) return NotFound();

            var model = new chackin_vew_modal
            {
                bill_id = bill.bill_id,
                staff_id = bill.staff_id,
                CarID = bill.CarID,
                Booking_id = bill.Booking_id,
                total_amount = bill.total_amount,
                RentalDuration = bill.RentalDuration
            };
            return View(model);
        }

        /// <summary>
        /// POST: Edit an existing bill.
        /// </summary>
        [HttpPost]
        public IActionResult EditBill(chackin_vew_modal model)
        {
            if (ModelState.IsValid)
            {
                var bill = _context.Bills.FirstOrDefault(b => b.bill_id == model.bill_id);
                if (bill == null) return NotFound();

                bill.staff_id = model.staff_id;
                bill.CarID = model.CarID;
                bill.Booking_id = model.Booking_id;
                bill.total_amount = model.total_amount;
                bill.RentalDuration = model.RentalDuration;

                _context.SaveChanges();
                return RedirectToAction("Bills");
            }
            return View(model);
        }

        /// <summary>
        /// POST: Delete a bill by its ID.
        /// </summary>
        [HttpPost]
        public IActionResult DeleteBill(Guid id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.bill_id == id);
            if (bill == null) return NotFound();

            _context.Bills.Remove(bill);
            _context.SaveChanges();
            return RedirectToAction("Bills");
        }

        /// <summary>
        /// Displays all bills as chackin_vew_modal list.
        /// </summary>
        public IActionResult Bills()
        {
            // Get all bills and project to chackin_vew_modal
            var bills = _context.Bills
                .ToList()
                .Select(bill =>
                {
                    var booking = _context.Bookings.FirstOrDefault(bk => bk.booking_id.ToString() == bill.Booking_id);
                    var car = _context.Cars.FirstOrDefault(c => c.CarId.ToString() == bill.CarID);

                    return new chackin_vew_modal
                    {
                        bill_id = bill.bill_id,
                        staff_id = bill.staff_id,
                        CarID = bill.CarID,
                        Booking_id = bill.Booking_id,
                        total_amount = bill.total_amount,
                        RentalDuration = bill.RentalDuration,
                        booking_id = booking?.booking_id ?? Guid.Empty,
                        car_id = booking?.car_id,
                        user_id = booking?.user_id,
                        name = booking?.name,
                        start_date = booking?.start_date,
                        dayes = booking?.dayes,
                        boking_status = booking?.boking_status,
                        CarId = car?.CarId ?? Guid.Empty,
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
                })
                .ToList();

            return View(bills);
        }

        /// <summary>
        /// Shows details for a specific bill, including related booking and car.
        /// </summary>
        public IActionResult BillDetails(Guid id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.bill_id == id);
            if (bill == null) return NotFound();

            var booking = _context.Bookings.FirstOrDefault(bk => bk.booking_id.ToString() == bill.Booking_id);
            var car = _context.Cars.FirstOrDefault(c => c.CarId.ToString() == bill.CarID);

            var viewModel = new chackin_vew_modal
            {
                bill_id = bill.bill_id,
                staff_id = bill.staff_id,
                CarID = bill.CarID,
                Booking_id = bill.Booking_id,
                total_amount = bill.total_amount,
                RentalDuration = bill.RentalDuration,
                booking_id = booking?.booking_id ?? Guid.Empty,
                car_id = booking?.car_id,
                user_id = booking?.user_id,
                name = booking?.name,
                start_date = booking?.start_date,
                dayes = booking?.dayes,
                boking_status = booking?.boking_status,
                CarId = car?.CarId ?? Guid.Empty,
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
    }
}