namespace car_rental_Unicom.tic.Models
{
    public class Car_modal
    {
        public Guid CarId { get; set; } 
        public string Car_modalName { get; set; }
        public int year {  get; set; }
        public string image_path {  get; set; }
        public string number_plact {  get; set; }
        public string ac {  get; set; }
        public string top_speed {  get; set; }
        public string Gear_System {  get; set; }
        public string milage {  get; set; }
        public string car_status {  get; set; }
    }
}
