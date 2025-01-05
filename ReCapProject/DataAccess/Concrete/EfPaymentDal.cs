

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfPaymentDal:EfEntityRepositoryBase<Payment,ReCapContext>,IPaymentDal
    {
    }
}
