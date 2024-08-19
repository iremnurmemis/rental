

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfCarImageDal: EfEntityRepositoryBase<CarImage, ReCapContext>, ICarImageDal
    {
    }
}
