
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;
using System.Data;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;
using System.Transactions;

namespace Business
{
    public class CarRentalManager : ICarRentalService
    {
        ICarRentalDal _carRental;
        ICarDal _car;
        INotificationService _notificationService;
        IUserService _userService;

        public CarRentalManager(ICarRentalDal carRental, ICarDal car, INotificationService notificationService, IUserService userService)
        {
            _carRental = carRental;
            _car = car;
            _notificationService = notificationService;
            _userService = userService;
        }


        public IResult AddCarRental(int carId, int userId)
        {
            // Kullanıcının zaten aktif bir kiralaması var mı kontrol et
            var existingRental = _carRental.Get(cr => cr.UserId == userId && cr.RentalStatus == RentalStatus.Active);
            if (existingRental != null)
            {
                return new ErrorResult("Kullanıcının aktif bir kiralama işlemi zaten var.");
            }

            // Arabayı doğrudan Car tablosundan kontrol et
            var carAvailable = _car.Get(c => c.Id == carId); // carId'yi kullanmak daha mantıklı
            if (carAvailable == null)
            {
                return new ErrorResult("Araç şu anda uygun değil.");
            }

            // Kullanıcı bilgilerini al
            var user = _userService.GetById(userId);
            if (user == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı.");
            }

            // Tüm aracı JSON formatında yazdır
            Debug.WriteLine(JsonSerializer.Serialize(carAvailable, new JsonSerializerOptions { WriteIndented = true }));

            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    var newCarRental = new CarRental
                    {
                        CarId = carId,
                        UserId = userId,
                        StartDate = DateTime.UtcNow, // UTC zamanını kullanıyoruz
                        RentalStatus = RentalStatus.Active,
                        StartLatitude = carAvailable.Latitude,
                        StartLongitude = carAvailable.Longitude,
                    };

                    _carRental.Add(newCarRental);

                    carAvailable.IsAvailable = false;
                    _car.Update(carAvailable);

                    // E-posta bildirimini gönder
                    var subject = "Araba Kiralama Onayı";
                    var body = $"Merhaba {user.Data.FirstName} araba kiralama işleminiz başarılı.\n\nTeşekkür ederiz!";
                    _notificationService.SendNotification(user.Data.Email, subject, body);

                    transactionScope.Complete();
                    return new SuccessDataResult<CarRental>(newCarRental, "Araç başarıyla kiralandı.");
                }
                catch (Exception ex)
                {
                    return new ErrorResult($"Kiralama işlemi başarısız: {ex.Message}");
                }
            }
        }

        public IResult CompleteCarRental(int rentalId)
        {
            var rental = _carRental.GetRentalWithCarId(rentalId);
            Debug.WriteLine(rental);

            if (rental == null)
            {
                return new ErrorResult($"{rentalId} ID'sine ait kiralama bulunamadı.");
            }

            var user=_userService.GetById(rental.UserId);

            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    var carHourlyPrice = rental.Car.PricePerHour;

                    // End latitude, end longitude, end date ve total price ayarlanıyor
                    rental.EndLatitude = 41.043;
                    rental.EndLongitude = 29.0083;
                    rental.EndDate = DateTime.UtcNow;
                    rental.RentalStatus = RentalStatus.Completed;


                    if (rental.EndDate.HasValue)
                    {
                        var rentalDurationHours = (decimal)(rental.EndDate.Value - rental.StartDate).TotalHours;
                        rental.TotalPrice = rentalDurationHours * carHourlyPrice;
                    }
                    else
                    {
                        return new ErrorResult("Başlangıç veya bitiş tarihi eksik, işlem tamamlanamıyor.");
                    }

                    // Kiralama bilgilerini güncelle
                    _carRental.Update(rental);
                    rental.Car.IsAvailable = true;
                    rental.Car.Latitude = rental.EndLatitude.GetValueOrDefault();
                    rental.Car.Longitude=rental.EndLongitude.GetValueOrDefault();
                    _car.Update(rental.Car);

                    // E-posta bildirimini gönder
                    var subject = "Araba İadesi";
                    var body = $"Merhaba {user.Data.FirstName} araba iade işleminiz başarılı.\n\nTeşekkür ederiz!";
                    _notificationService.SendNotification(user.Data.Email, subject, body);

                    transactionScope.Complete();
                    return new SuccessResult("İade işlemi başarıyla tamamlandı.");
                }
                catch (Exception ex)
                {
                    return new ErrorResult($"İade işlemi başarısız: {ex.Message}");
                }
            }
        }

     

        public IResult GetAllCarRentals()
        {
            return new SuccessDataResult<List<CarRental>>(_carRental.GetAll());
        }

        //public IResult GetAllCarRentalsWithDetails()
        //{
        //    return new SuccessDataResult<List<CarRental>>(_carRental.GetRentalsWithCar());
        //}

        public IResult GetActiveCarRentals()
        {
            return new SuccessDataResult<List<CarRental>>(_carRental.GetAll(cr=>cr.RentalStatus==RentalStatus.Active));
        }

        public IResult GetCompletedCarRentals()
        {
            return new SuccessDataResult<List<CarRental>>(_carRental.GetAll(cr => cr.RentalStatus == RentalStatus.Completed));
        }

        public IResult GetUserCarRentals(int userId)
        {
            return new SuccessDataResult<List<CarRental>>(_carRental.GetAll(cr => cr.UserId == userId));
        }

        public IResult GetUserActiveCarRentals(int userId)
        {
            return new SuccessDataResult<List<CarRental>>(_carRental.GetAll(cr => cr.RentalStatus == RentalStatus.Active && cr.UserId==userId));
        }

        public IResult GetUserCompletedCarRentals(int userId)
        {
            return new SuccessDataResult<List<CarRental>>(_carRental.GetAll(cr => cr.RentalStatus == RentalStatus.Completed && cr.UserId == userId));
        }
    }
}
