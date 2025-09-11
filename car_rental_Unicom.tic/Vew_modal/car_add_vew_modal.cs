using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace car_rental_Unicom.tic.View_modal
{
    public class car_add_view_modal
    {
        public Guid CarId { get; set; }          // ✅ Car ID சேர்க்கப்பட்டது
        public string Car_modalName { get; set; }
        public int year { get; set; }
        public string number_plact { get; set; }
        public string ac { get; set; }
        public string top_speed { get; set; }
        public string Gear_System { get; set; }
        public string milage { get; set; }
        public string car_status { get; set; }

        public List<IFormFile> ImageFiles { get; set; } // ✅ Multiple images

        public string image_path { get; set; }          // Database-ல் சேமிக்கப்பட்ட path
        public List<string> ImagePaths                  // Razor View-ல் gallery க்கு
        {
            get
            {
                if (!string.IsNullOrEmpty(image_path))
                    return new List<string>(image_path.Split('#'));
                return new List<string>();
            }
        }
        public string MaintenanceCharge { get; set; }
        public string RentPerDay { get; set; }
    }
}
