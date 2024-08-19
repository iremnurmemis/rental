

using Core;

namespace DataAccess
{
    public interface IUserDal:IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user); //join operasyonu
    }
}
