using Business;
using Core;
using DataAccess;
using DataAccess.Migrations;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPı.Controllers
{
    //[Authorize] // Bu controller’daki tüm action’lar authorization gerektirir
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        ICarService _carService;
        ICarDal _carDal;
        IFileHelper _fileHelper;
        ICarImageDal _carImageDal;
        public CarsController(ICarService carService,ICarImageDal carImageDal,ICarDal carDal,IFileHelper fileHelper)
        {
            _carService = carService;
            _carDal = carDal;
            _fileHelper = fileHelper;
            _carImageDal = carImageDal;
        }

        //[HttpGet]
        //public string GenerateToken(string username, string password)
        //{
        //    var claims = new[] {
        //        new Claim(ClaimTypes.Name, username),
        //        new Claim(JwtRegisteredClaimNames.Email,username)
        //    };
        //    string signinkey = "BuBenimSigningKeyBuBenimSigningKey";

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinkey));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var jwtSecurityToken = new JwtSecurityToken(
        //        issuer: "admin.admin@gmail.com",
        //        audience: "BuBenimKullabdıgımAudienceDegeri",
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddDays(15),
        //        notBefore: DateTime.UtcNow,
        //        signingCredentials: credentials
        //        );


        //    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        //    return token;
        //}


        //[HttpGet("ValidateToken")]
        //public bool ValidateToken(string token)
        //{
        //    string signinkey = "BuBenimSigningKeyBuBenimSigningKey";

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinkey));

        //    try
        //    {
        //        JwtSecurityTokenHandler handler = new();
        //        handler.ValidateToken(token, new TokenValidationParameters()
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = securityKey,
        //            ValidateLifetime = true,
        //            ValidateAudience = false,
        //            ValidateIssuer = false,

        //        }, out SecurityToken validatedToken);

        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        var claims = jwtToken.Claims.ToList();
        //        return true;



        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);    
        //        return false;
        //    }

        //}

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var cars = _carService.GetAll();
           
            return BadRequest(cars);
        }

        [HttpGet("GetCarByBrandId")]
        public IActionResult GetCarByBrandId(int brandId)
        {
            var cars = _carService.GetByBrandId(brandId);
            if (cars.Success)
                return Ok(cars);
            return BadRequest(cars);
        }

        [HttpGet("GetCarByCategoryId")]
        public IActionResult GetCarByCategoryId(int categoryId)
        {
            var cars = _carService.GetByCategoryId(categoryId);
            if (cars.Success)
                return Ok(cars);
            return BadRequest(cars);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllDetails")]
        public IActionResult GetAllDetails()
        {
            var cars = _carService.GetAllDetail();
            if (cars.Success)
                return Ok(cars);
            return BadRequest(cars);
        }


        [HttpGet("GetCarDetail")]
        public IActionResult GetCarDetail(int carId)
        {
            var cars = _carService.GetCarDetail(carId);
            if (cars.Success)
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
        public IActionResult Delete(int carId)
        {
            var result = _carService.Delete(carId);
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

        [HttpGet("GetAllDetaylı")]
        public async Task<IActionResult> GetAllDeneme()

        {
            var _context = new ReCapContext();
            var cars = await _context.Cars
                .Select(c => new
                {
                    c.Id,
                    c.BrandId,
                    c.ModelId,
                    c.ColorId,
                    c.CategoryId,
                    c.Year,
                    c.Plate,
                    c.IsAvailable,
                    c.FuelType,
                    c.Transmission,
                    c.PricePerHour,
                    c.SeatCount,
                    c.Latitude,
                    c.Longitude,
                    CarImages = c.CarImages.Select(img => new
                    {
                        img.Id,
                        img.ImagePath,
                        img.IsMain
                    }).ToList(), 
                    CarRentals = c.CarRentals.Select(r => new
                    {
                        r.Id,
                        r.StartDate,
                        r.EndDate,
                        r.StartLongitude,
                        r.StartLatitude,
                        r.EndLatitude,
                        r.EndLongitude,
                        r.UserId,
                        r.TotalPrice
                    }).ToList(),
                    MainImage =c.CarImages.Where(d=> d.IsMain && d.CarId==c.Id).FirstOrDefault().ImagePath,
                })
                .ToListAsync();

            return Ok(new { data = cars, success = true, message = "Arabalar listelendi" });
        }


        [HttpGet("getAvailableCarsLocations")]
        public IActionResult GetAvailableCarsLocations()
        {
            var result = _carService.GetAllCarsLocation();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("GetAllCarsLocationByCategoryId")]
        public IActionResult GetAllCarsLocationByCategoryId(int categoryId)
        {
            var result = _carService.GetAllCarsLocationByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("GetAllCarsLocationByBrandId")]
        public IActionResult GetAllCarsLocationByBrandId(int brandId)
        {
            var result = _carService.GetAllCarsLocationByBrandId(@brandId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }



        [HttpGet("getCarListPage")]
        public IActionResult GetCarListPage()
        {
            var result = _carService.GetAllCarsForListPage();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("GetAllAvailableBrand")]
        public IActionResult GetAllAvailableBrand()
        {
            var result = _carService.GetAllAvailableBrand();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("add-with-images")]
        public async Task<IActionResult> AddCarWithImages([FromForm] Car car, [FromForm] List<IFormFile> images)
        {
            _carDal.Add(car);

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    string imagePath = _fileHelper.Add(image);
                    var carImage = new CarImage
                    {
                        CarId = car.Id,
                        ImagePath = imagePath,
                        Date = DateTime.Now
                    };
                    _carImageDal.Add(carImage);
                }
            }

            return Ok(new { message = "Car added successfully" });
        }


    }
}
