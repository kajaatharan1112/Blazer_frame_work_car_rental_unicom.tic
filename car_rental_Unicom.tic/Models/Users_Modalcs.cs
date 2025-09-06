using System.ComponentModel.DataAnnotations;

namespace car_rental_Unicom.tic.Models
{
    public class Users_Modalcs
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
    }
}
