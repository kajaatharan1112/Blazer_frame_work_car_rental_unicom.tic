using Microsoft.AspNetCore.Http;

namespace car_rental_Unicom.tic.View_modal
{
    public class car_add_view_modal
    {
        public string Car_modalName { get; set; }
        public int year { get; set; }

        // Removed this: public string image_path { get; set; }

        public string number_plact { get; set; }
        public string ac { get; set; }
        public string top_speed { get; set; }
        public string Gear_System { get; set; }
        public string milage { get; set; }

        public IFormFile ImageFile { get; set; }  // ✅ Add this for image upload
    }
}
