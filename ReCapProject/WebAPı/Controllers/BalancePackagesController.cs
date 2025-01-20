using Business;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalancePackagesController : ControllerBase
    {
        private readonly IBalancePackageService _balancePackageService;

        public BalancePackagesController(IBalancePackageService balancePackageService)
        {   
            _balancePackageService = balancePackageService;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var result=_balancePackageService.GetAll();
            if(result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);

        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var result = _balancePackageService.GetBalancePackageById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);

        }

        [HttpPost("add")]
        public IActionResult GetById([FromBody] BalancePackage balancePackage)
        {
            var result = _balancePackageService.Add(balancePackage);
            if (result.Success)
            {
                return Ok(result.Success);
            }

            return BadRequest(result.Message);

        }
    }
}
