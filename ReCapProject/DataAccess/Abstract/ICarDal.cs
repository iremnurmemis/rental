
using Entities;

namespace DataAccess
{
    public interface ICarDal:IEntityRepository<Car>
    {
        List<CarDetailDto> GetCarDetails();
       


    }
}
