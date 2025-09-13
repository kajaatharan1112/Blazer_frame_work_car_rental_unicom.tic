using car_rental_Unicom.tic.Data;
using car_rental_Unicom.tic.Data.YourNamespace;
using car_rental_Unicom.tic.Models;
using car_rental_Unicom.tic.View_modal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace car_rental_Unicom.tic.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CarController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Car Page
        public IActionResult Car()
        {

            var cars = dbContext.Cars.ToList();

            List<car_add_view_modal> carListViewModel = new List<car_add_view_modal>();

            foreach (var car in cars)
            {
                carListViewModel.Add(new car_add_view_modal
                {
                    CarId = car.CarId,
                    Car_modalName = car.Car_modalName,
                    year = car.year,
                    number_plact = car.number_plact,
                    ac = car.ac,
                    Gear_System = car.Gear_System,
                    milage = car.milage,
                    top_speed = car.top_speed,
                    image_path = car.image_path,                  // multi-images
                    MaintenanceCharge = car.MaintenanceCharge,    // new property
                    RentPerDay = car.RentPerDay,                  // new property
                    ImageFiles = null,
                    car_status = car.car_status
                });
            }

            return View(carListViewModel);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult UpdateCar(Guid carId)
        {
            var car = dbContext.Cars.FirstOrDefault(c => c.CarId == carId);

            if (car == null)
            {
                return NotFound();
            }

            var carViewModel = new car_add_view_modal
            {
                CarId = car.CarId,
                Car_modalName = car.Car_modalName,
                year = car.year,
                number_plact = car.number_plact,
                ac = car.ac,
                top_speed = car.top_speed,
                Gear_System = car.Gear_System,
                milage = car.milage,
                car_status = car.car_status,
                MaintenanceCharge = car.MaintenanceCharge,
                RentPerDay = car.RentPerDay,
                image_path = car.image_path
            };

            return View(carViewModel);
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> UpdateCar(car_add_view_modal car)
        {
            var existingCar = dbContext.Cars.FirstOrDefault(c => c.CarId == car.CarId);
            if (existingCar == null)
            {
                return NotFound();
            }

            // Optional: handle new image uploads like in Add method
            string imagePath = existingCar.image_path;

            if (car.ImageFiles != null && car.ImageFiles.Count > 0)
            {
                List<string> savedPaths = new List<string>();
                foreach (var file in car.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        var fileName = Guid.NewGuid().ToString() + fileExt;
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        savedPaths.Add("/uploads/" + fileName);
                    }
                }

                imagePath = string.Join("#", savedPaths);
            }

            // Update fields
            existingCar.Car_modalName = car.Car_modalName;
            existingCar.year = car.year;
            existingCar.number_plact = car.number_plact;
            existingCar.ac = car.ac;
            existingCar.top_speed = car.top_speed;
            existingCar.Gear_System = car.Gear_System;
            existingCar.milage = car.milage;
            existingCar.car_status = car.car_status;
            existingCar.image_path = imagePath;
            existingCar.MaintenanceCharge = car.MaintenanceCharge;
            existingCar.RentPerDay = car.RentPerDay;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Car", "Car");
        }
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

       [HttpPost]
        public IActionResult delet_car(string carId)
        {
            // Delete logic here
            return View("Car");
        }
        public IActionResult add_car(string id)
        {
            return View();
        }
 //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Add Car - POST
        [HttpPost]
        public async Task<IActionResult> Add(car_add_view_modal car)
        {
            string imagePath = null;

            // Multi-image upload
            if (car.ImageFiles != null && car.ImageFiles.Count > 0)
            {
                List<string> savedPaths = new List<string>();

                foreach (var file in car.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        var fileName = Guid.NewGuid().ToString() + fileExt;
                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        savedPaths.Add("/uploads/" + fileName);
                    }
                }

                // Join all paths with #
                imagePath = string.Join("#", savedPaths);
            }

            // Create new Car_modal object
            var CAR = new Car_modal
            {
                Car_modalName = car.Car_modalName,
                year = car.year,
                image_path = imagePath,
                number_plact = car.number_plact,
                ac = car.ac,
                top_speed = car.top_speed,
                Gear_System = car.Gear_System,
                milage = car.milage,
                car_status = "available",
                MaintenanceCharge = car.MaintenanceCharge,
                RentPerDay = car.RentPerDay
            };

            dbContext.Cars.Add(CAR);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Car", "Car");
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public IActionResult GetCar(Guid id)
        {
            var car = dbContext.Cars.FirstOrDefault(c => c.CarId == id);

            if (car == null)
            {
                return NotFound();
            }

            var carViewModel = new car_add_view_modal
            {
                CarId = car.CarId,
                Car_modalName = car.Car_modalName,
                year = car.year,
                number_plact = car.number_plact,
                ac = car.ac,
                Gear_System = car.Gear_System,
                milage = car.milage,
                top_speed = car.top_speed,
                image_path = car.image_path,
                MaintenanceCharge = car.MaintenanceCharge,
                RentPerDay = car.RentPerDay,
                car_status = car.car_status
            };

            return View(carViewModel);
        



        }
        [HttpPost]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            var car = dbContext.Cars.FirstOrDefault(c => c.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            dbContext.Cars.Remove(car);
            await dbContext.SaveChangesAsync();

            // Delete பண்ணிட்டு car list page-க்கு redirect
            return RedirectToAction("Car");
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
