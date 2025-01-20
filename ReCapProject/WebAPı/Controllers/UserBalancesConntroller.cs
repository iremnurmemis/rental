using Business;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBalancesController : ControllerBase
    {
        private readonly IUserBalanceService _userBalanceService;

        public UserBalancesController(IUserBalanceService userBalanceService)
        {
            _userBalanceService = userBalanceService;
        }

        [HttpGet("get-user-balance")]
        public IActionResult GetUserBalance(int userId)
        {
            var result = _userBalanceService.GetUserBalance(userId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] UserBalance userBalance)
        {
            var result = await _userBalanceService.Add(userBalance);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("load-balance")]
        public async Task<IActionResult> LoadBalance([FromBody] LoadBalanceDto loadBalanceDto)
        {
            var result = await _userBalanceService.LoadBalance( loadBalanceDto.UserId,loadBalanceDto.CardId,loadBalanceDto.PackageId);

            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

    }
}
