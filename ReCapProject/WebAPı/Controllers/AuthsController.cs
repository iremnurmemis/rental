using Business;
using Core;
using DataAccess;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private IAuthService _authService;
        private IUserDal _userDal;
        IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthsController(IAuthService authService, IUserDal userDal, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _userDal = userDal;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var result = _authService.Login(userForLoginDto);
            if (!result.Success)
            {

                return Unauthorized(new { message = result.Message });
            }

            return Ok(result.Data);
        }

        [HttpPost("admin/login")]
        public IActionResult AdminLogin([FromBody] UserForLoginDto userForLoginDto)
        {
            var result = _authService.AdminLogin(userForLoginDto);
            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(result.Data);
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _authService.Logout();
            return Ok("Logout successful.");
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            if (!registerResult.Success)
            {
                return BadRequest(new { message = registerResult.Message });
            }

            return Ok(registerResult.Data);
        }



        [HttpGet("confirm-email")]
        public IActionResult ConfirmEmail(string email, string token)
        {
            var user = _userDal.Get(u => u.Email == email);
            if (user == null || user.Token != token)
            {
                return BadRequest("Geçersiz token veya e-posta.");
            }

            user.Status = true; // Kullanıcıyı aktif hale getiriyoruz
            _userDal.Update(user);

            return Ok("E-posta doğrulandı. Hesabınız artık aktif!");
        }

        [HttpPost("request-password-reset")]
        public IActionResult RequestPasswordReset(string email)
        {
            var result = _authService.RequestPasswordReset(email);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = _authService.ResetPassword(resetPasswordDto.Email, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = _authService.ChangePassword(changePasswordDto.Email, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }





       


    }
}
