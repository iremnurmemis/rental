using Core;
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IUserService
    {
        void Add(User user);
        IDataResult<User> GetByMail(string email);
        IDataResult<List<OperationClaim>> GetClaims(User user);
    }
}
