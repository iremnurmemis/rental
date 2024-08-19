using Business;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        ICarService _carService;
        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var cars = _carService.GetAll();
            if(cars.Success)
                return Ok(cars);
            return BadRequest(cars);
        }

        [HttpGet("GetCarsByBrandId")]
        public IActionResult GetCarsByBrandId(int brandId)
        {
            var cars = _carService.GetCarsByBrandId(brandId);
            if(cars.Success)
                return Ok(cars);
            return BadRequest(cars);
        }
        
        [HttpGet("GetCarsByColorId")]
        public IActionResult GetCarsByColorId(int colorId)
        {
            var cars = _carService.GetCarsByColorId(colorId);
            if(cars.Success)
                return Ok(cars);
            return BadRequest(cars);
        }
        
        [HttpGet("GetByCarId")]
        public IActionResult GetByCarId(int carId)
        {
            var cars = _carService.GetByCarId(carId);
            if(cars.Success)
                return Ok(cars);
            return BadRequest(cars);
        }

        [HttpPost("Add")]
        public IActionResult Add(Car car)
        {
            var result = _carService.Add(car);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(Car car)
        {
            var result = _carService.Delete(car);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Car car)
        {
            var result = _carService.Update(car);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetCarDetails")]
        public IActionResult GetCarDetails()
        {
            var cars= _carService.GetCarDetails();
            if(cars.Success)
                return Ok(cars);
            return BadRequest(cars);
        }

    }
}
