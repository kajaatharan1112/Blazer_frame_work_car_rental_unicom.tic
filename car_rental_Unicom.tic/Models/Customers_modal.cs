﻿using System.ComponentModel.DataAnnotations;

namespace car_rental_Unicom.tic.Models
{
    public class Customers_modal
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int contactNo {  get; set; }
        public int Age {  get; set; }
        public string Gender {  get; set; }
        public string Email {  get; set; }
    }
}
