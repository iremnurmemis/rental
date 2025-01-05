

using Core.Interceptors.Utilities.Results;
using DataAccess;
using Iyzipay.Model;
using Iyzipay.Request;
using Entities;

namespace Business
{
    public class PaymentManager : IPaymentService
    {
        private readonly IPaymentDal _paymentDal;
        public PaymentManager(IPaymentDal paymentDal)
        {
            _paymentDal = paymentDal;
            
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

        public IDataResult<List<Entities.Payment>> GetAllPaymentByUserId(int userId)
        {
            return new SuccessDataResult<List<Entities.Payment>>(_paymentDal.GetAll(p=>p.UserId==userId), "Userın tüm ödemeleri listelendi");
        }
    }
}
