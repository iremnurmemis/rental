
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IBalancePackageService
    {
        IResult Add(BalancePackage balancePackage);
        IDataResult<List<BalancePackage>> GetAll();
        IDataResult<BalancePackage> GetBalancePackageById(int id);
    }
}
