
using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfCardDal:EfEntityRepositoryBase<Card,ReCapContext>,ICardDal
    {
    }
}
