using Business;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormsController : ControllerBase
    {
        private readonly IContactFormService _contactFormService;
        public ContactFormsController(IContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
            
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] ContactForm contactForm)
        {
            var result = _contactFormService.Add(contactForm);
            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var result = _contactFormService.GetAll();
            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }


    }
}
