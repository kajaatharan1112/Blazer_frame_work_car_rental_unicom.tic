using System.ComponentModel.DataAnnotations;

namespace car_rental_Unicom.tic.Vew_modal
{
    public class admin_view_modal
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string ContactNo { get; set; }

        [Range(18, 70, ErrorMessage = "Age must be between 18 and 70")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        // Extra field for UI (optional)
        public string Role { get; set; } = "Admin";

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
