using Business;
using Core.Interceptors.Utilities.Results;
using Entities;
using Entitiesü;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

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
        public async Task<IActionResult> AddCarRental([FromBody] CarRentalRequest request)
        {
            
            var result = await _carRentalService.AddCarRental(request.CarId,request.UserId,request.CardId,request.RentalType,request.durationInDays);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("CompleteCarRental")]
        public async Task<IActionResult> CompleteCarRental([FromBody] İadeRequest request)
        {
            var result = await _carRentalService.CompleteCarRental(request.rentalId);  // await kullanarak sonucu bekliyoruz
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("updatecardıd")]
        public IActionResult UpdateCardId([FromBody] UpdateCardIdRequest request)
        {
            var result = _carRentalService.UpdateCardId(request.cardId, request.rentalId);

            if (result is ErrorResult)
            {
                return BadRequest(result.Message);  // Return bad request for error
            }

            return Ok(result.Message);  // Return success message
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
        [HttpGet("GetUserCarRentals")]
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
        [HttpGet("GetUserActiveCarRentals")]
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
        [HttpGet("GetUserCompletedCarRentals")]
        public IActionResult GetUserCompletedCarRentals(int userId)
        {
            var result = _carRentalService.GetUserCompletedCarRentals(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getUserActiveCarRentalsDetails")]
        public IActionResult GetUserActiveCarRentalsDetails(int userId)
        {
            // Servisten gelen yanıtı alıyoruz
            IDataResult<RentalDto> result = _carRentalService.GetUserActiveCarRentalsDetails(userId);

            // Sonuç başarılıysa (Success)
            if (result.Success)
            {
                return Ok(result.Data);  // 200 OK ve rental bilgileri ile dönüş yapılır
            }
            else
            {
                // Eğer bir hata oluşursa (Error)
                return BadRequest(result.Message);  // 400 BadRequest ve hata mesajı döndürülür
            }
        }


        [HttpGet("getUserAllCarRentalsDetails")]
        public IActionResult GetUserAllCarRentalsDetails(int userId)
        {
            // Servisten gelen yanıtı alıyoruz
            IDataResult<List<RentalDto>> result = _carRentalService.GetUserAllCarRentalsDetails(userId);

          
            if (result.Success)
            {
                return Ok(result.Data); 
            }
            else
            {
                // Eğer bir hata oluşursa (Error)
                return BadRequest(result.Message);  
            }
        }


    }
}
