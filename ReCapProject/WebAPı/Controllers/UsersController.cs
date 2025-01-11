using Business;
using Core;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        IUserService _userService;
        public UsersController(IUserService userrService)
        {
            _userService = userrService;
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] User user)
        {
            _userService.Add(user);
            return Ok(new { message = "User added successfully" });
        }


        [HttpGet("get-by-id")]
        public IActionResult GetById(int userId)
        {
            var result = _userService.GetById(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(new { message = "User not found" });
        }

        [HttpGet("get-by-email")]
        public IActionResult GetByMail(string email)
        {
            var result = _userService.GetByMail(email);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(new { message = "User not found" });
        }





    }
}
