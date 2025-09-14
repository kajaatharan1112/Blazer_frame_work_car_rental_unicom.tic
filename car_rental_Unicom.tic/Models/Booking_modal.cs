using System.ComponentModel.DataAnnotations;

namespace car_rental_Unicom.tic.Models
{
    public class Booking_modal
    {
        [Key]
        public Guid booking_id { get; set; }
        public int licence_number {  get; set; }//for the verification process
        public string car_id {  get; set; }
        public string user_id {  get; set; }
        public string name { get; set; }    
        public string start_date {  get; set; }
        public string dayes { get; set; }
        public string boking_status {  get; set; }//booking process status [active] or [booking_chackout] or [chack in]
    }
}
