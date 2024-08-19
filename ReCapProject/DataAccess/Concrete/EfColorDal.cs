

using Core.DataAccess.EntityFramework;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess
{
    public class EfColorDal : EfEntityRepositoryBase<Color, ReCapContext>,IColorDal
    {
        
    }
}
