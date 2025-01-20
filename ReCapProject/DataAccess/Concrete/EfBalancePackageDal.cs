

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    
    public class EfBalancePackageDal:EfEntityRepositoryBase<BalancePackage,ReCapContext>,IBalancePackageDal
    {
    }
}
