using Core.DataAccess.EntityFramework;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess
{
    public class EfCarDal : EfEntityRepositoryBase<Car, ReCapContext>, ICarDal
    {
      

        public List<CarDetailDto> GetCarDetails()
        {
            using (ReCapContext context = new ReCapContext())
            {
                // Veritabanından Car detaylarını alıyoruz
                var result = from car in context.Cars
                             join brand in context.Brands on car.BrandId equals brand.Id
                             join model in context.Models on car.ModelId equals model.Id
                             join color in context.Colors on car.ColorId equals color.Id
                             join category in context.Categories on car.CategoryId equals category.Id
                             select new CarDetailDto
                             {
                                 Id = car.Id,
                                 BrandId = brand.Id,
                                 BrandName = brand.Name,
                                 ModelId = model.Id,
                                 ModelName = model.Name,
                                 ColorId = color.Id,
                                 ColorName = color.Name,
                                 CategoryId = category.Id,
                                 CategoryName = category.Name,
                                 Year = car.Year,
                                 Plate = car.Plate,
                                 IsAvailable = car.IsAvailable,
                                 FuelType = car.FuelType.ToString(),
                                 Transmission = car.Transmission.ToString(),
                                 PricePerHour = car.PricePerHour,
                                 SeatCount = car.SeatCount,
                                 Latitude = car.Latitude,
                                 Longitude = car.Longitude,
                                 PricePerDay=car.PricePerDay,
                                 CarImages = (from carImage in context.CarImages
                                              where carImage.CarId == car.Id
                                              select new CarImageDto
                                              {
                                                  Id = carImage.Id,
                                                  CarId = carImage.CarId,
                                                  ImageUrl = carImage.ImagePath,
                                                  IsMain = carImage.IsMain
                                              }).ToList(),
                                 CarRentals = (from rental in context.CarRentals
                                               where rental.CarId == car.Id
                                               select new CarRentalDto
                                               {
                                                   Id = rental.Id,
                                                   CarId = rental.CarId,
                                                   UserId = rental.UserId,
                                                   StartDate = rental.StartDate,
                                                   StartLatitude = rental.StartLatitude,
                                                   StartLongitude = rental.StartLongitude,
                                                   EndDate = rental.EndDate,
                                                   EndLatitude = rental.EndLatitude,
                                                   EndLongitude = rental.EndLongitude,
                                                   
                 
                                               }).ToList(),
                                 MainImage =  (from carImage in context.CarImages
                                                where carImage.CarId == car.Id && carImage.IsMain == true
                                                 select carImage.ImagePath).FirstOrDefault()
                                                     
                             };

                return result.ToList(); // DTO listesi döndürüyoruz
            }
        }

        public CarDetailDto GetCarDetailById(int carId)
        {
            using (ReCapContext context = new ReCapContext())
            {
                // Veritabanından Car detaylarını alıyoruz
                var result = from car in context.Cars
                             join brand in context.Brands on car.BrandId equals brand.Id
                             join model in context.Models on car.ModelId equals model.Id
                             join color in context.Colors on car.ColorId equals color.Id
                             join category in context.Categories on car.CategoryId equals category.Id
                             where car.Id==carId
                             select new CarDetailDto
                             {
                                 Id = car.Id,
                                 BrandId = brand.Id,
                                 BrandName = brand.Name,
                                 ModelId = model.Id,
                                 ModelName = model.Name,
                                 ColorId = color.Id,
                                 ColorName = color.Name,
                                 CategoryId = category.Id,
                                 CategoryName = category.Name,
                                 Year = car.Year,
                                 Plate = car.Plate,
                                 IsAvailable = car.IsAvailable,
                                 FuelType = car.FuelType.ToString(),
                                 Transmission = car.Transmission.ToString(),
                                 PricePerHour = car.PricePerHour,
                                 SeatCount = car.SeatCount,
                                 Latitude = car.Latitude,
                                 Longitude = car.Longitude,
                                 PricePerDay = car.PricePerDay,
                                 CarImages = (from carImage in context.CarImages
                                              where carImage.CarId == car.Id
                                              select new CarImageDto
                                              {
                                                  Id = carImage.Id,
                                                  CarId = carImage.CarId,
                                                  ImageUrl = carImage.ImagePath,
                                                  IsMain = carImage.IsMain
                                              }).ToList(),
                                 CarRentals = (from rental in context.CarRentals
                                               where rental.CarId == car.Id
                                               select new CarRentalDto
                                               {
                                                   Id = rental.Id,
                                                   CarId = rental.CarId,
                                                   UserId = rental.UserId,
                                                   StartDate = rental.StartDate,
                                                   StartLatitude = rental.StartLatitude,
                                                   StartLongitude = rental.StartLongitude,
                                                   EndDate = rental.EndDate,
                                                   EndLatitude = rental.EndLatitude,
                                                   EndLongitude = rental.EndLongitude,


                                               }).ToList(),
                                 MainImage = (from carImage in context.CarImages
                                              where carImage.CarId == car.Id && carImage.IsMain == true
                                              select carImage.ImagePath).FirstOrDefault()

                             };

                return result.FirstOrDefault(); // DTO listesi döndürüyoruz
            }
        }
    }
}
