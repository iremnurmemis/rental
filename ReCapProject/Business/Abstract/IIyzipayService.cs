
using Core;
using Core.Interceptors.Utilities.Results;
using Entities;
using Iyzipay.Model;

namespace Business
{
    public interface IIyzipayService
    {
        Task<string> CreateCardToken(CreatCardTokenDto creatCardTokenDto);
        Task<Iyzipay.Model.Payment> CreatePayment(Entities.Card card,User user,int processId,decimal totalPrice);



    }
}
