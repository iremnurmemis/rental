
using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfRentalDal:EfEntityRepositoryBase<Rental,ReCapContext>,IRentalDal
    {
    }
}
