using Microsoft.EntityFrameworkCore;

namespace car_rental_Unicom.tic.Data
{
    using car_rental_Unicom.tic.Models;
    using Microsoft.EntityFrameworkCore;

    namespace YourNamespace
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            // 👇 Tables (DbSets) create பண்ண
            public DbSet<Admin_modalcs> Admins { get; set; }
            public DbSet<Bills_modal> Bills { get; set; }
            public DbSet<Booking_modal> Bookings { get; set; }
            public DbSet<Car_modal> Cars { get; set; }
            public DbSet<Customers_modal> Customers { get; set; }
            public DbSet<rental_payment_modal> RentalPayments { get; set; }
            public DbSet<Staff_modal> Staffs { get; set; }
            public DbSet<Users_Modalcs> Users { get; set; }
        }
    }

}
