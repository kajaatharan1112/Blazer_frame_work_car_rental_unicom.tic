using System;
using System.ComponentModel.DataAnnotations;

namespace car_rental_Unicom.tic.Vew_modal
{
    public class home_view_modal
    {
        public Guid CarId { get; set; }
        public string Car_modalName { get; set; }
        public int year { get; set; }
        public string image_path { get; set; }
        public string number_plact { get; set; }
        public string ac { get; set; }
        public string top_speed { get; set; }
        public string Gear_System { get; set; }
        public string milage { get; set; }
        public string car_status { get; set; }
        public string MaintenanceCharge { get; set; }
        public string RentPerDay { get; set; }



         public Guid booking_id { get; set; }
        public int licence_number {  get; set; }//for the verification process
        public string car_id {  get; set; }
        public string user_id {  get; set; }
        public string name { get; set; }    
        public string start_date {  get; set; }
        public string dayes { get; set; }
        public string boking_status {  get; set; }//booking process status [active] or [booking fnnisheg] or [chack in]
    }
    
}