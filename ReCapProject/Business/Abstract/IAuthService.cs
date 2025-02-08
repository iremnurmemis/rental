

using Core;
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto,string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult Logout();
        IResult UserExist(string email);
        IResult RequestPasswordReset(string email); //emaile şifre sıfırlama bağlantısı gönderilir ŞİFREMİ UNUTTUM
        IResult ResetPassword(string email,string token,string newPassword);//şifre sıfırlama ekranı
        IResult ChangePassword(string email,string currentPassword,string newPassword);//şifre değiştirme 
    }
}
