

using Entities;

namespace DataAccess
{
    public interface IUserBalanceDal:IEntityRepository<UserBalance>
    {
         Task AddAsync(UserBalance userBalance);
    }
}
