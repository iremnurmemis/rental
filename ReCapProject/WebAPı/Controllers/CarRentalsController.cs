using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarRentalsController : ControllerBase
    {
        ICarRentalService _carRentalService;
        public CarRentalsController(ICarRentalService carRentalService)
        {
            _carRentalService = carRentalService;
        }

        [HttpPost("AddCarRental")]
        public IActionResult AddCarRental(int carId,int userId)
        {
            var result = _carRentalService.AddCarRental(carId, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("CompleteCarRental")]
        public IActionResult CompleteCarRental(int rentalId)
        {
            var result = _carRentalService.CompleteCarRental(rentalId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //tüm kiralamlar
        [HttpGet("GetAllRentals")]
        public IActionResult GetAllRentals()
        {
            var result = _carRentalService.GetAllCarRentals();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        ////tüm kiralamlar araba bilgisi ile 
        //[HttpGet("GetAllCarRentalsWithDetails")]
        //public IActionResult GetAllCarRentalsWithDetails()
        //{
        //    var result = _carRentalService.GetAllCarRentalsWithDetails();
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}


        //aktif kiralamlar
        [HttpGet("GetAciveCarRentals")]
        public IActionResult GetActiveCarRentals()
        {
            var result = _carRentalService.GetActiveCarRentals();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //tamamlanmış kiralamlar
        [HttpGet("GetCompletedCarRentals")]
        public IActionResult GetCompletedCarRentals()
        {
            var result = _carRentalService.GetCompletedCarRentals();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //usera ait kiralamlar
        [HttpPost("GetUserCarRentals")]
        public IActionResult GetUserCarRentals(int userId)
        {
            var result = _carRentalService.GetUserCarRentals(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //usera ait aktif kiralamlar
        [HttpPost("GetUserActiveCarRentals")]
        public IActionResult GetUserActiveCarRentals(int userId)
        {
            var result = _carRentalService.GetUserActiveCarRentals(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //usera ait tamamlanmış kiralamlar
        [HttpPost("GetUserCompletedCarRentals")]
        public IActionResult GetUserCompletedCarRentals(int userId)
        {
            var result = _carRentalService.GetUserCompletedCarRentals(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }



    }
}
