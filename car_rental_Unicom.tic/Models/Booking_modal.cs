namespace car_rental_Unicom.tic.Models
{
    public class Booking_modal
    {
        public Guid booking_id { get; set; }
        public int licence_number {  get; set; }
        public string car_id {  get; set; }
        public string user_id {  get; set; }
        public string name { get; set; }    
        public string start_date {  get; set; }
        public string end_date { get; set; }
        public string boking_status {  get; set; }
    }
}
