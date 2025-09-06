using Microsoft.EntityFrameworkCore;

namespace car_rental_Unicom.tic.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
