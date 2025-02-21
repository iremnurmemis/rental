
using Core;
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

namespace Business
{
    public class AuthManager : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private IEmailHelper _emailService;
        private IUserDal _userDal;
        private IUserService _userService;
        private ITokenHelper _tokenHelper; //token üreticez register ve loginde 
        private ITokenBlackListService _tokenBlackListService;
        INotificationService _notificationService;

        public AuthManager(IConfiguration configuration, IUserService userService, ITokenHelper tokenHelper, ITokenBlackListService tokenBlackListService, IEmailHelper emailService, IUserDal userDal, IHttpContextAccessor httpContextAccessor,INotificationService notificationService)
        {
            _tokenHelper = tokenHelper;
            _userService = userService;
            _tokenBlackListService = tokenBlackListService;
            _emailService = emailService;
            _userDal = userDal;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
        }



        public IDataResult<User> AdminLogin(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email).Data;
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>("Bu emaile sahip kullanıcı bulunamadı.");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>("Girdiğiniz şifre hatalı.");
            }

            if (userToCheck.OperationClaimId !=2)
            {
                return new ErrorDataResult<User>("yetkisiz erişim");
            }

            // Kullanıcı bilgilerini içeren Claims oluşturma
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userToCheck.Email),

            };

            // OperationClaimId değerine göre rol belirleme
            if (userToCheck.OperationClaimId == 0)
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            else if (userToCheck.OperationClaimId == 2)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }


            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            // Cookie kimlik doğrulama bilgisi
            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                IsPersistent = true, // Kalıcı oturum
                ExpiresUtc = DateTime.UtcNow.AddHours(56) // 2 saat geçerlilik
            };

            // Kullanıcı oturumu başlatma
            _httpContextAccessor.HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties).Wait();

            return new SuccessDataResult<User>(userToCheck, "Login successful.");
        }


        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email).Data;
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>("Bu emaile sahip kullanıcı bulunamadı.");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>("Girdiğiniz şifre hatalı.");
            }

            if (userToCheck.OperationClaimId == 2)
            {
                return new ErrorDataResult<User>("yetkisiz erişim");
            }

            // Kullanıcı bilgilerini içeren Claims oluşturma
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userToCheck.Email),

            };

            // OperationClaimId değerine göre rol belirleme
            if (userToCheck.OperationClaimId == 0)
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            else if (userToCheck.OperationClaimId == 2)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }


            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            // Cookie kimlik doğrulama bilgisi
            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                IsPersistent = true, // Kalıcı oturum
                ExpiresUtc = DateTime.UtcNow.AddHours(56) // 2 saat geçerlilik
            };

            // Kullanıcı oturumu başlatma
            _httpContextAccessor.HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties).Wait();

            return new SuccessDataResult<User>(userToCheck, "Login successful.");
        }


        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var userToCheck = _userService.GetByMail(userForRegisterDto.Email).Data;
            if (userToCheck == null)
            {
                var user = new User
                {
                    FirstName = userForRegisterDto.FirstName,
                    LastName = userForRegisterDto.LastName,
                    Email = userForRegisterDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    PhoneNumber = userForRegisterDto.PhoneNumber,
                    Status = false, // false yap, ilk maille onaylasın
                   // TCKN=userForRegisterDto.TCKN,
                };

                _userService.Add(user);

                user.Token = Guid.NewGuid().ToString();
                _userDal.Update(user);

                var confirmationLink = $"http://localhost:5153/api/Auths/confirm-email?email={user.Email}&token={user.Token}";

                // NotificationService ile e-posta gönderme
                var subject = "E-posta Doğrulama";
                var body = $"Merhaba {user.FirstName}, lütfen hesabınızı doğrulamak için <a href='{confirmationLink}'>buraya tıklayın</a>.";

                // NotificationService kullanarak e-posta gönderimi
                _notificationService.SendNotification(user.Email, subject, body);

                return new SuccessDataResult<User>(user, Messages.UserRegistered);
            }

            return new ErrorDataResult<User>("Girmiş olduğunuz mail adresine sahip kullanıcı bulunmaktadır.");
        }


        public IResult Logout()
        {
            _httpContextAccessor.HttpContext.SignOutAsync("CookieAuth").Wait();
            return new SuccessResult("Logout successful.");
        }


        public IResult UserExist(string email)
        {
            if (_userService.GetByMail(email).Data != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }

            return new SuccessResult();
        }


        //ŞİFREMİ UNUTTUM
        // Şifre sıfırlama talebi
        public IResult RequestPasswordReset(string email)
        {
            var emailValidation = new EmailAddressAttribute();
            if (!emailValidation.IsValid(email))
            {
                return new ErrorResult("Geçerli bir e-posta adresi giriniz.");
            }

            var user = _userService.GetByMail(email).Data;
            if (user == null)
            {
                return new ErrorResult("Bu e-posta adresine ait bir kullanıcı bulunamadı.");
            }

            try
            {
                user.Token = Guid.NewGuid().ToString();  // Eşsiz token oluşturuluyor
                _userDal.Update(user);  // Kullanıcıyı güncelle

                var resetLink = $"http://localhost:3000/reset-password?email={user.Email}&token={user.Token}";
                var emailBody = $"Merhaba {user.FirstName}, şifrenizi sıfırlamak için <a href='{resetLink}'>buraya tıklayın</a>.";

                _notificationService.SendNotification(user.Email, "Şifre Sıfırlama", emailBody);  // E-posta gönderimi

                return new SuccessResult("Şifre sıfırlama bağlantısı e-posta ile gönderildi.");  // Başarı durumu
            }
            catch (Exception ex)
            {
                return new ErrorResult($"E-posta gönderimi sırasında bir hata oluştu: {ex.Message}");  // Hata durumu
            }
        }


        // Şifre değiştirme işlemi
        public IResult ResetPassword(string email, string token, string newPassword)
        {
            var user = _userService.GetByMail(email).Data;
            if (user == null || user.Token != token)
            {
                return new ErrorResult("Geçersiz veya süresi dolmuş token");
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);  // Şifre hash'leme
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Token = null;  // Token'ı sıfırlıyoruz
            _userDal.Update(user);  // Kullanıcıyı güncelliyoruz

            return new SuccessResult("Şifre başarıyla değiştirildi.");
        }


        //ŞİİFREMİ DEĞİŞTİRMEK İSTİYORUM
        public IResult ChangePassword(string email, string currentPassword, string newPassword)
        {
            var user = _userService.GetByMail(email).Data;
            if (user == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı.");
            }

            if (!HashingHelper.VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                return new ErrorResult("Mevcut şifre yanlış.");
            }

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userDal.Update(user);
            return new SuccessResult("Şifre başarıyla güncellendi.");

        }




    }


}
