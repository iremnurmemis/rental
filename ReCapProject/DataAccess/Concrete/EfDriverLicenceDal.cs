

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfDriverLicenceDal:EfEntityRepositoryBase<DriverLicence,ReCapContext>,IDriverLicenceDal
    {
    }
}
