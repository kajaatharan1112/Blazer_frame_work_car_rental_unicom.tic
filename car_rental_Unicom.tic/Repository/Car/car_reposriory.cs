using car_rental_Unicom.tic.Data;

namespace car_rental_Unicom.tic.Repository.Car
{
    public class car_reposriory:Car_repostiory_Interface
    {
        private readonly ApplicationDbContext dbontext;

        public car_reposriory(ApplicationDbContext Dbcontext)
        {
            this.dbontext = Dbcontext;
        }
    }
}
