

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfContactFormDal:EfEntityRepositoryBase<ContactForm,ReCapContext>,IContactFormDal
    {
    }
}
