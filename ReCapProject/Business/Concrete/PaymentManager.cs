

using Core.Interceptors.Utilities.Results;
using DataAccess;
using Iyzipay.Model;
using Iyzipay.Request;
using Entities;
using DataAccess.Migrations;

namespace Business
{
    public class PaymentManager : IPaymentService
    {
        private readonly IPaymentDal _paymentDal;
        private readonly ICardDal _cardDal;
        private readonly ICarDal _carDal;
        private readonly IBrandDal _brandDal;
        private readonly IModelDal _modelDal;
        public PaymentManager(IPaymentDal paymentDal,ICarDal carDal,ICardDal cardDal,IBrandDal brandDal,IModelDal modelDal)
        {
            _paymentDal = paymentDal;
            _carDal = carDal;
            _cardDal = cardDal;
            _brandDal = brandDal;
            _modelDal = modelDal;
            
        }

        public IDataResult<Entities.Payment> Add(Entities.Payment payment)
        {
            _paymentDal.Add(payment);
            return new SuccessDataResult<Entities.Payment>(payment, "ödeme başarı ile eklendi");
        }

        public IResult Delete(Entities.Payment payment)
        {
            _paymentDal.Delete(payment);
            return new SuccessResult("payment silindi");
        }

        public IDataResult<List<Entities.Payment>> GetAll()
        {
            return new SuccessDataResult<List<Entities.Payment>>(_paymentDal.GetAll(),"Tüm ödemeler listelendi");
        }

        public IDataResult<List<UserPaymentsDto>> GetAllPaymentByUserId(int userId)
        {
            try
            {
                var payments = (from payment in _paymentDal.GetAll()
                                join card in _cardDal.GetAll() on payment.CardId equals card.Id
                                join car in _carDal.GetAll() on payment.CarId equals car.Id
                                join brand in _brandDal.GetAll() on car.BrandId equals brand.Id
                                join model in _modelDal.GetAll() on car.ModelId equals model.Id
                                where payment.UserId == userId
                                select new UserPaymentsDto
                                {
                                    Id = payment.Id,
                                    CarId = car.Id,
                                    RentalId = payment.RentalId,
                                    CardId = card.Id,
                                    CardHolderName = card.CardHolderName,
                                    CardNumber = card.CardNumber,
                                    RentalAmount = payment.TotalPrice,
                                    Status = payment.Status.ToString(),
                                    UserId = payment.UserId,
                                    Plate = car.Plate,
                                    Brand = brand.Name,
                                    Model = model.Name,
                                    Created=payment.CreatedTime,
                                    PaymentType= payment.Type.ToString(),
                                    balancePackageId=payment.BalancePackageId,
                                    totalPrice=payment.TotalPrice,
                                   

                                }).ToList();

                if (payments.Any())
                {
                    return new SuccessDataResult<List<UserPaymentsDto>>(payments);
                }
                else
                {
                    return new ErrorDataResult<List<UserPaymentsDto>>("Bu kullanıcıya ait ödeme bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                // Loglama işlemi yapılabilir (ex kullanılarak hata detayı kaydedilebilir)
                return new ErrorDataResult<List<UserPaymentsDto>>("Ödeme bilgileri alınırken bir hata oluştu.");
            }
        }

        public IDataResult<List<UserPaymentsDto>> GetAllPaymentsFront()
        {
            try
            {
                var payments = (from payment in _paymentDal.GetAll()
                               join card in _cardDal.GetAll() on payment.CardId equals card.Id
                               select new UserPaymentsDto
                               {
                                   Id = payment.Id,
                                   UserId = payment.UserId,
                                   totalPrice = payment.TotalPrice,
                                   CardNumber=card.CardNumber,
                                   Created= payment.CreatedTime,
                                   Status=payment.Status.ToString(),
                                   PaymentType= payment.Type.ToString(),
                                   RentalId=payment.RentalId,
                                   balancePackageId= payment.BalancePackageId,

                               }).ToList();

                if (payments.Any())
                {
                    return new SuccessDataResult<List<UserPaymentsDto>>(payments);
                }
                else
                {
                    return new ErrorDataResult<List<UserPaymentsDto>>(" ödeme bulunamadı.");
                }
            }
            catch (Exception ex) 
            {
                return new ErrorDataResult<List<UserPaymentsDto>>("Ödeme bilgileri alınırken bir hata oluştu.");
            }
        }       

           public IDataResult<List<UserPaymentsDto>> GetAllPaymentByUserIdFrontend(int userId)
        {
            try
            {
                var payments = (from payment in _paymentDal.GetAll()
                                where payment.UserId == userId
                                select new UserPaymentsDto
                                {
                                    Id = payment.Id,
                                    CarId = payment.CarId,
                                    RentalId = payment.RentalId,
                                    Status = payment.Status.ToString(),
                                    Created=payment.CreatedTime,
                                    PaymentType= payment.Type.ToString(),
                                    balancePackageId=payment.BalancePackageId,
                                    totalPrice=payment.TotalPrice,                        

                                }).ToList();

                if (payments.Any())
                {
                    return new SuccessDataResult<List<UserPaymentsDto>>(payments);
                }
                else
                {
                    return new ErrorDataResult<List<UserPaymentsDto>>("Bu kullanıcıya ait ödeme bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                // Loglama işlemi yapılabilir (ex kullanılarak hata detayı kaydedilebilir)
                return new ErrorDataResult<List<UserPaymentsDto>>("Ödeme bilgileri alınırken bir hata oluştu.");
            }
        }
    }
}




