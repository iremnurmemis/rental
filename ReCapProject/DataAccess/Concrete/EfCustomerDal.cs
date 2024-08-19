

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, ReCapContext>, ICustomerDal
    {

    }
}
