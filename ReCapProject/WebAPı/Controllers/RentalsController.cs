using Business;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        IRentalService _rentalService;
        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var rentals= _rentalService.GetAll();
            if(rentals.Success) 
                return Ok(rentals);
            return BadRequest(rentals);
        }

        [HttpGet("GetRentalsByCarId")]
        public IActionResult GetRentalsByCarId(int carId)
        {
            var rentals=_rentalService.GetByCarId(carId);
            if(rentals.Success)
                return Ok(rentals);
            return BadRequest(rentals);
        }

        [HttpGet("GetRentalsByCustomerId")]
        public IActionResult GetRentalsByCustomerId(int customerId)
        {
            var rentals = _rentalService.GetRentalsByCustomerId(customerId);
            if (rentals.Success)
                return Ok(rentals);
            return BadRequest(rentals);
        }

        [HttpPost("Add")]
        public IActionResult Add(Rental rental)
        {
            var result = _rentalService.Add(rental);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);

        }
        [HttpPost("Update")]
        public IActionResult Update(Rental rental)
        {
            var result = _rentalService.Update(rental);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);

        }
        [HttpPost("Delete")]
        public IActionResult Delete(Rental rental)
        {
            var result = _rentalService.Delete(rental);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);

        }

    }
}
