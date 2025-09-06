using System.ComponentModel.DataAnnotations;

namespace car_rental_Unicom.tic.Models
{
    public class rental_payment_modal
    {
        [Key]
        public Guid payment_id { get; set; }
        public string car_id { get; set; }
        public string MaintenanceCharge { get; set; }
        public string RentPerDay { get; set; }

    }
}
