
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IUserBalanceService
    {
        //balance yükleme ve balance harcama ve userın balance bilgisini getirme kodları
        IDataResult<UserBalance> GetUserBalance(int userId);

        Task<IResult> LoadBalance(int userId,int cardId,int packageId);

        Task<IResult> Add(UserBalance userBalance);
        IResult Update(UserBalance userBalance);
        IDataResult<UserDetailDto> GetDetail(int userId);

    }
}
