using System.ComponentModel.DataAnnotations;

namespace car_rental_Unicom.tic.Models
{
    public class Bills_modal
    {
        [Key]
        public Guid bill_id { get; set; }
        public string staff_id { get; set; }    
        public string CarID { get; set; }
        public string Booking_id { get; set; }
        public string total_amount { get; set; }
        public string RentalDuration { get; set; }
    }
}