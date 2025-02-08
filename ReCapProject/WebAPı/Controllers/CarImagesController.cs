using Business;
using Core.Interceptors.Utilities.Results;
using DataAccess.Migrations;
using Entities;
using Entitiesü;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tesseract;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImagesController : ControllerBase
    {
        ICarImageService _carImageService;
       

        public CarImagesController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
         
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var carImages = _carImageService.GetAll();
            if (carImages.Success)
                return Ok(carImages);
            return BadRequest(carImages);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int Id)
        {
            var carImage = _carImageService.GetById(Id);
            if (carImage.Success)
                return Ok(carImage);
            return BadRequest(carImage);
        }

        [HttpGet("GetByCarId")]
        public IActionResult GetByCarId(int carId)
        {
            var carImages = _carImageService.GetByCarId(carId);
            if (carImages.Success)
                return Ok(carImages);
            return BadRequest(carImages);
        }


        [HttpPost("add")]
        public IActionResult Add(IFormFile file, [FromForm] int carId, [FromForm] bool IsMain)
        { //file ve carId lazım kullanıcı tarafından.
            CarImage carImage = new() { CarId = carId, IsMain = IsMain };
            var result = _carImageService.Add(file, carImage);


            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("AddRentalImages")]
        public async Task<IActionResult> AddRentalImages([FromForm] int rentalId, [FromForm] List<IFormFile> images)
        {
            var result = await _carImageService.AddRentalImages(rentalId, images);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("Update")]
        public IActionResult Update(IFormFile file, [FromForm] int Id)
        {
            CarImage oldCarImage = _carImageService.GetById(Id).Data;
            var result = _carImageService.Update(file, oldCarImage);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("Delete")]
        public IActionResult Delete(CarImage carImage)
        {

            var result = _carImageService.Delete(carImage);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

       
    }
}