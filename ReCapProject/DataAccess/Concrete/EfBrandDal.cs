

using Core.DataAccess.EntityFramework;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EfBrandDal : EfEntityRepositoryBase<Brand,ReCapContext>,IBrandDal
    {

    }   
        
 
}
