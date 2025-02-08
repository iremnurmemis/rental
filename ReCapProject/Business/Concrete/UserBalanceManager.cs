
using Core.Interceptors.Utilities.Results;
using DataAccess;
using DataAccess.Migrations;
using Entities;
using Iyzipay.Model;

namespace Business
{
    public class UserBalanceManager : IUserBalanceService
    {
        private readonly IUserBalanceDal _userBalanceDal;
        private readonly IUserDal _userDal;
        private readonly ICardDal _cardDal;
        private readonly IBalancePackageDal _balancePackageDal;
        private  readonly IIyzipayService _iyzipayService;
        private readonly IPaymentDal _paymentDal;
        private readonly IDriverLicenceDal _driverLicenceDal;

        public UserBalanceManager(IUserBalanceDal userBalanceDal,IUserDal userDal,ICardDal cardDal,IBalancePackageDal balancePackageDal,IIyzipayService ıyzipayService,IPaymentDal paymentDal,IDriverLicenceDal driverLicenceDal)
        {
            _userBalanceDal = userBalanceDal;
            _userDal = userDal; 
            _balancePackageDal = balancePackageDal;
            _cardDal = cardDal;
            _iyzipayService=ıyzipayService;
            _paymentDal= paymentDal;
            _driverLicenceDal= driverLicenceDal;
        }

        

        public async Task<IResult> Add(UserBalance userBalance)
        {
            if (userBalance == null || userBalance.Balance <= 0)
            {
                return new ErrorResult("Invalid user balance data.");
            }

            await _userBalanceDal.AddAsync(userBalance);
            return new SuccessResult("Bakiye başarıyla yüklendi");
        }

        public IDataResult<UserBalance> GetUserBalance(int userId)
        {
           return new SuccessDataResult<UserBalance>(_userBalanceDal.Get(ub=>ub.UserId==userId),"Kullanıcının bakiye bilgisi getirildi");
        }


        public IResult Update(UserBalance userBalance)
        {
           _userBalanceDal.Update(userBalance);
            return new SuccessResult("kullanıcı bakiyesi güncellendi");

        }

        public async Task<IResult> LoadBalance(int userId, int cardId, int packageId)
        {
            //card user package bilgilerini al user balance kaydı yoksa olustur varsa ekle package.CreditAmount

            var user=_userDal.Get(u=>u.Id==userId); 
            if (user == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı");
            }

            var card = _cardDal.Get(c => c.Id == cardId);
            if (card == null)
            {
                return new ErrorResult("Kart bilgisi bulunamadı");
            }

            var balancePackage = _balancePackageDal.Get(bp => bp.Id == packageId);
            if (balancePackage == null)
            {
                return new ErrorResult("Bakiye paketi bulunamadı");
            }

           
            var totalPrice = balancePackage.Price;

            // Ödeme kaydı oluştur
            // Ödeme kaydını oluştur
            var paymentRecord = new Entities.Payment
            {
                CardId = cardId,
                UserId = userId,
                TotalPrice = totalPrice,
                Status = PaymentStatus.Pending, 
                CreatedTime = DateTime.UtcNow,
                Type = PaymentType.Balance,
                BalancePackageId = balancePackage.Id,
                 
            };

            _paymentDal.Add(paymentRecord);

            var payment = await _iyzipayService.CreatePayment(card,user,packageId,totalPrice);

            if (payment.Status != "success")
            {
                paymentRecord.Status = PaymentStatus.Failed;
                _paymentDal.Update(paymentRecord);
                return new ErrorResult($"Ödeme başarısız: {payment.ErrorMessage}");
               
            }

            paymentRecord.Status=PaymentStatus.Success;
            _paymentDal.Update(paymentRecord);

            var userBalance= GetUserBalance(userId);
            if (userBalance.Data == null)
            {
                var newUserBalance = new UserBalance
                {
                    UserId = userId,
                    Balance = 0,
                    LastUpdated = DateTime.UtcNow,
                };

                await _userBalanceDal.AddAsync(newUserBalance);
                userBalance = GetUserBalance(userId);

            }

            userBalance.Data.Balance += balancePackage.CreditAmount; 
            userBalance.Data.LastUpdated = DateTime.UtcNow;
            _userBalanceDal.Update(userBalance.Data);

            return new SuccessResult("Bakiye başarıyla yüklendi.");

        }

        public IDataResult<UserDetailDto> GetDetail(int userId)
        {
            
            var user = _userDal.Get(u => u.Id == userId);
            if (user == null)
            {
                return new ErrorDataResult<UserDetailDto>("User not found.");
            }

            var userBalance = _userBalanceDal.Get(ub => ub.UserId == userId);
            var driverLicence = _driverLicenceDal.Get(dl => dl.UserId == userId);

  
            if (userBalance == null)
            {
                return new ErrorDataResult<UserDetailDto>("User balance not found.");
            }

            var userDetailDto = new UserDetailDto
            {
                userId = user.Id,
                fullname = user.FirstName + ' '+  user.LastName,
                email = user.Email,
                phone = user.PhoneNumber,
                status = user.Status,
                balance = userBalance.Balance,
                ısLicenceVerified = user.IsDrivingLicenseVerified,
                LicenceNo = driverLicence?.LicenceNo,
                ValidUntil = driverLicence?.ValidUntil,
                UploadDate = driverLicence?.UploadDate
            };

            return new SuccessDataResult<UserDetailDto>(userDetailDto, "User details fetched successfully.");
        }

    }
}
