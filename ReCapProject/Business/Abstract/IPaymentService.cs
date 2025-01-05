


using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
   public interface IPaymentService
    {
        IDataResult<Payment> Add(Payment payment);
        IResult Delete(Payment payment);

        IDataResult<List<Payment>> GetAll();
        IDataResult<List<Payment>> GetAllPaymentByUserId(int userId);



    }
}
