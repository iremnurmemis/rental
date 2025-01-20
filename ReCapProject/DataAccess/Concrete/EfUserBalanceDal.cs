

using Core.DataAccess.EntityFramework;
using Entities;

namespace DataAccess
{
    public class EfUserBalanceDal : EfEntityRepositoryBase<UserBalance, ReCapContext>, IUserBalanceDal
    {
        public async Task AddAsync(UserBalance userBalance)
        {
            using (var context = new ReCapContext())
            {
                await context.UserBalances.AddAsync(userBalance);
                await context.SaveChangesAsync();
            }
        }

      
    }
}
