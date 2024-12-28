

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfModelDal:EfEntityRepositoryBase<Model,ReCapContext>,IModelDal
    {
    }
}
