using System.ComponentModel.DataAnnotations;

namespace car_rental_Unicom.tic.Models
{
    public class Admin_modalcs
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ContactNo {  get; set; }
        public int Age {  get; set; }
        public string Gender {  get; set; }
        public string Email { get; set; }

    }
}
