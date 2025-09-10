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
        public async Task<IActionResult> Add(car_add_view_modal car)
        {
            // படத்தின் பாதையை சேமிக்க imagePath எனும் மாறி
            string imagePath = null;

            // பயனர் ஒரு படம் பதிவேற்றியுள்ளாரா என்பதை சரிபார்க்கின்றது
            if (car.ImageFile != null && car.ImageFile.Length > 0)
            {
                // படத்தின் விரிவாக்கம் (extension) எடுக்கப்படுகிறது (உதாரணம்: .jpg, .png)
                var fileExt = Path.GetExtension(car.ImageFile.FileName);

                // unique file name உருவாக்கப்படுகிறது (duplicate தவிர்க்க)
                var fileName = Guid.NewGuid().ToString() + fileExt;

                // படம் சேமிக்க வேண்டிய பாதை (wwwroot/uploads/filename.jpg)
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                // upload செய்யும் folder இல்லையெனில், அதை உருவாக்கும்
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                // படத்தை அந்த folder-க்கு சேமிக்கிறது
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await car.ImageFile.CopyToAsync(stream);
                }

                // சேமிக்கப்பட்ட படம் எங்கு இருக்கிறது என்பதை string-ஆக path-ல் வைத்துக்கொள்கிறது
                imagePath = "/uploads/" + fileName;
            }

            // புதிய கார் object உருவாக்கப்படுகிறது
            var CAR = new Car_modal
            {
                Car_modalName = car.Car_modalName, // கார் பெயர்
                year = car.year, // கார் தயாரிப்பு வருடம்
                image_path = imagePath, // படத்தின் பாதை
                number_plact = car.number_plact, // பதிவேற்ற எண்
                ac = car.ac, // ஏ.சி. உள்ளது என்பதை
                top_speed = car.top_speed, // அதிகபட்ச வேகம்
                Gear_System = car.Gear_System, // கியர் அமைப்பு
                milage = car.milage, // mileage
                car_status = "Available" // நிலைமை 'Available' என நிர்ணயிக்கப்படுகிறது
            };

            // கார் விவரங்களை தரவுத்தளத்தில் சேர்க்கிறது
            dbContext.Cars.Add(CAR);
            await dbContext.SaveChangesAsync(); // தரவுகளை save செய்கிறது

            // Car action-க்கு redirect செய்யப்படுகிறது (refresh)
            return RedirectToAction("Car", "Car");
        }

    }
}
