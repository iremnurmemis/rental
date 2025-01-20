
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;
using System.Reflection.Metadata.Ecma335;

namespace Business
{   
    public class BalancePackageManager : IBalancePackageService
    {
        private readonly IBalancePackageDal _balancePackageDal;
        public BalancePackageManager(IBalancePackageDal balancePackageDal)
        {
            _balancePackageDal = balancePackageDal;
        }
        public IResult Add(BalancePackage balancePackage)
        {
            _balancePackageDal.Add(balancePackage);
            return new SuccessResult("Bakiye paketi başarıyla eklendi");
        }

        public IDataResult<List<BalancePackage>> GetAll()
        {
            return new SuccessDataResult<List<BalancePackage>>(_balancePackageDal.GetAll(),"Paketler Listelendi.");   
        }

        public IDataResult<BalancePackage> GetBalancePackageById(int id)
        {
            return new SuccessDataResult<BalancePackage>(_balancePackageDal.Get(p => p.Id == id), "paket bilgisi getirildi");
        }
    }
}
