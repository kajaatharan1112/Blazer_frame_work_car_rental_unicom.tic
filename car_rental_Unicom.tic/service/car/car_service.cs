using car_rental_Unicom.tic.Repository.Car;

namespace car_rental_Unicom.tic.service.car
{

    public class car_service:car_service_Interface
    {
        private readonly object _Car_repostiory_Interface;

        public car_service(Car_repostiory_Interface rcar)
        {
            _Car_repostiory_Interface= rcar;
        }
    }
}
