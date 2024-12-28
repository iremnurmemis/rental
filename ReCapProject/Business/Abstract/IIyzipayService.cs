
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IIyzipayService
    {
        Task<string> CreateCardToken(CreatCardTokenDto creatCardTokenDto);

    }
}
