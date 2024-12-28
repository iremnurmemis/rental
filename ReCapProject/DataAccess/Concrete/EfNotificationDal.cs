

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfNotificationDal : EfEntityRepositoryBase<Notification, ReCapContext>, INotificationDal
    {
    }
}
