namespace car_rental_Unicom.tic.View_modal
{
    public class car_add_view_modal
    {
        // CarId -> Database auto generate ஆகும், அதனால் user input தேவையில்லை
        public Guid CarId { get; set; }

        // Car modal name (ex: Toyota, Nissan)
        public string Car_modalName { get; set; }

        public int year { get; set; }
        public string image_path { get; set; }
        public string number_plact { get; set; }
        public string ac { get; set; }
        public string top_speed { get; set; }
        public string Gear_System { get; set; }
        public string milage { get; set; }

        // car_status default = "available"
        public string car_status { get; set; } = "available";
    }
}
