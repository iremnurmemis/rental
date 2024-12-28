

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Entities;
using DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Migrations;

public class EfCarRentalDal : EfEntityRepositoryBase<CarRental, ReCapContext>, ICarRentalDal
{
    public List<CarRental> GetRentalsWithCar()
    {
        using (var context = new ReCapContext())
        {
            var result = from carRental in context.CarRentals
                         join car in context.Cars on carRental.CarId equals car.Id
                         join user in context.Users on carRental.UserId equals user.Id
                         select new CarRental
                         {
                             Id = carRental.Id,
                             CarId = carRental.CarId,
                             UserId = carRental.UserId,
                             StartDate = carRental.StartDate,
                             EndDate = carRental.EndDate,
                             RentalStatus = carRental.RentalStatus,
                             TotalPrice = carRental.TotalPrice,
                             StartLatitude = carRental.StartLatitude,
                             StartLongitude = carRental.StartLongitude,
                             EndLatitude = carRental.EndLatitude,
                             EndLongitude = carRental.EndLongitude,
                             Car = new Car
                             {
                                 Id = car.Id,
                                 BrandId = car.BrandId,
                                 ModelId = car.ModelId,
                                 CategoryId = car.CategoryId,
                                 ColorId = car.ColorId,
                                 //Brand = new Brand { Name = carRental.Car.Brand.Name, Id = carRental.Car.BrandId },
                                 //Model = new Model { Name = carRental.Car.Model.Name, Id = carRental.Car.ModelId },
                                 //Category = new Category { Name = carRental.Car.Category.Name, Id = carRental.Car.CategoryId },
                                 //Color = new Color { Name = carRental.Car.Color.Name, Id = carRental.Car.ColorId },
                                 FuelType = car.FuelType,
                                 Transmission = car.Transmission,
                                 IsAvailable = car.IsAvailable,
                                 Plate = car.Plate,
                                 SeatCount = car.SeatCount,
                                 Year = car.Year,
                                 PricePerHour = car.PricePerHour,
                                 Latitude = car.Latitude,
                                 Longitude = car.Longitude,
                             },
                             User = new Core.User
                             {
                                 Id = user.Id,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Email = user.Email,

                             }

                         };

            return result.ToList();
        }


    }

    public CarRental GetRentalWithCarId(int rentalId)
    {
        using (var context = new ReCapContext())
        {
            return context.CarRentals
                .Include(r => r.Car)
                .FirstOrDefault(r => r.Id == rentalId);
        }
    }
}
