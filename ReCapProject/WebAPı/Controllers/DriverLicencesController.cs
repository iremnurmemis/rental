using Business;
using Core.Interceptors.Utilities.Results;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverLicencesController : ControllerBase
    {
      private readonly IDriverLicenceService _driverLicenceService;
        public DriverLicencesController(IDriverLicenceService driverLicenceService)
        {
            _driverLicenceService = driverLicenceService;
        }

        [HttpPost("upload-driver-licence")]
        public IActionResult UploadDriverLicence(IFormFile file, [FromForm] int userId)
        {
            try
            {
               
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Geçerli bir dosya yükleyin.");
                }

               
                var result = _driverLicenceService.UploadDriverLicence(file, userId); 

               
                if (result is ErrorResult errorResult)
                {
                    return BadRequest(errorResult.Message); 
                }

                if (result is SuccessResult successResult)
                {
                    return Ok(successResult.Message); 
                }

              
                return StatusCode(500, "Bilinmeyen bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] DriverLicence driverLicence)
        {
            var result = _driverLicenceService.Add(driverLicence);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _driverLicenceService.GetAllDriverLicences();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        
        [HttpGet("getuserdriverlicenceinfo")]
        public IActionResult GetUserDriverLicenceInfo([FromQuery] int userId)
        {
            var result = _driverLicenceService.GetUserDriverLicenceInfo(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
