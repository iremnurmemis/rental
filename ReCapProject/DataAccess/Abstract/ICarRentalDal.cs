
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public interface ICarRentalDal:IEntityRepository<CarRental>
    {
        CarRental GetRentalWithCarId(int rentalId);
        List<CarRental> GetRentalsWithCar();
    }
}
