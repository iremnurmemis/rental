using Business;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService=customerService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users=_customerService.GetAll();
            if(users.Success)
                return Ok(users);
            return BadRequest(users);
        }
        
        [HttpGet("GetByUserId")]
        public IActionResult GetByUserId(int userId)
        {
            var users=_customerService.GetByUserId(userId);
            if(users.Success)
                return Ok(users);
            return BadRequest(users);
        }

        [HttpPost("Add")]
        public IActionResult Add(Customer customer)
        {
            var result = _customerService.Add(customer);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(Customer customer)
        {
            var result = _customerService.Delete(customer);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [HttpPost("Update")]
        public IActionResult Update(Customer customer)
        {
            var result = _customerService.Update(customer);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
