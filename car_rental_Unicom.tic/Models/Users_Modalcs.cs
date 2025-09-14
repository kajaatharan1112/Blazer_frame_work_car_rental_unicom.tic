using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_rental_Unicom.tic.Models
{
    public class Users_Modalcs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]     // ✅ EF auto-generate panna koodathu
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
    }
}
