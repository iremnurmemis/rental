
using Core;
using Core.Interceptors.Utilities.Results;
using DataAccess;
using DataAccess.Migrations;
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
        ICardDal _cardDal;
        IBrandDal _brandDal;
        IModelDal _modelDal;
        ICategoryDal _categoryDal;
       IIyzipayService _iyzipayService;
        IPaymentDal _paymentDal;

        public CarRentalManager(ICarRentalDal carRental, ICarDal car, INotificationService notificationService, IUserService userService,ICardDal cardDal,ICategoryDal categoryDal,IBrandDal brandDal,IModelDal modelDal,IIyzipayService ıyzipayService,IPaymentDal paymentDal)
        {
            _carRental = carRental;
            _car = car;
            _notificationService = notificationService;
            _userService = userService;
            _cardDal = cardDal;
            _brandDal = brandDal;
            _modelDal = modelDal;
            _categoryDal = categoryDal;
            _paymentDal = paymentDal;
           _iyzipayService=ıyzipayService;
        }


        public IResult AddCarRental(int carId, int userId,int cardId)
        {
            // Kullanıcı bilgilerini al
            var user = _userService.GetById(userId);
            if (user == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı.");
            }

            if (user.Data.IsDrivingLicenseVerified == false)
            {
                return new ErrorResult("Kiralamaya başlamadan önce Sürücü belgenizi sisteme yüklemelisiniz"); 
            }


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

           

            // Tüm aracı JSON formatında yazdır
            Debug.WriteLine(JsonSerializer.Serialize(carAvailable, new JsonSerializerOptions { WriteIndented = true }));

            //var preAuthResult = _paymentService.PreAuthorize(carAvailable.PricePerHour, cardId);
            //if (!preAuthResult.Success)
            //    return new ErrorResult(preAuthResult.Message);


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
                        CardId = cardId,
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

        public async Task<IResult> CompleteCarRental(int rentalId)
        {
            var rental = _carRental.GetRentalWithCarId(rentalId);

            if (rental == null)
            {
                return new ErrorResult($"{rentalId} ID'sine ait kiralama bulunamadı.");
            }

            var user = _userService.GetById(rental.UserId);

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

                var totalPrice = Convert.ToInt32(rental.TotalPrice.Value);
                var card = _cardDal.Get(c => c.Id == rental.CardId);
                var userBilgi = _userService.GetById(rental.UserId).Data;

                // Ödeme kaydını hemen ekliyoruz
                var paymentRecord = new Payment
                {
                    CardId = card.Id,
                    UserId = rental.UserId,
                    CarId = rental.CarId,
                    RentalId = rental.Id,
                    TotalPrice = totalPrice,
                    Status = PaymentStatus.Failed, // Başlangıçta durum 'Failed' olarak atanabilir
                    CreatedTime = DateTime.UtcNow,
                };

                _paymentDal.Add(paymentRecord);

                // Ödeme işlemini yapıyoruz
                var payment = await _iyzipayService.CreatePayment(card, userBilgi, rental, totalPrice);

                // Eğer ödeme başarısızsa ödeme kaydını güncelliyoruz
                if (payment.Status != "success")
                {
                    paymentRecord.Status = PaymentStatus.Failed;
                    _paymentDal.Update(paymentRecord); // Durumu güncelliyoruz
                    return new ErrorResult($"Ödeme işlemi başarısız: {payment.ErrorMessage}");
                }

                // Ödeme başarılıysa işlem devam eder
                using (var transactionScope = new TransactionScope())
                {
                    try
                    {
                        paymentRecord.Status = PaymentStatus.Success; // Durum başarılı olarak güncelleniyor
                        paymentRecord.CreatedTime = DateTime.UtcNow;
                        _paymentDal.Update(paymentRecord);

                        _carRental.Update(rental);
                        rental.Car.IsAvailable = true;
                        rental.Car.Latitude = rental.EndLatitude.GetValueOrDefault();
                        rental.Car.Longitude = rental.EndLongitude.GetValueOrDefault();
                        _car.Update(rental.Car);

                        // E-posta bildirimini gönder
                        var subject = "Araba İadesi";
                        var body = $"Merhaba {user.Data.FirstName}, araba iade işleminiz başarılı.\n\nTeşekkür ederiz!";
                        _notificationService.SendNotification(user.Data.Email, subject, body);

                        transactionScope.Complete();
                        return new SuccessResult("İade işlemi başarıyla tamamlandı.");
                    }
                    catch (Exception ex)
                    {
                        return new ErrorResult($"İade işlemi sırasında bir hata oluştu: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult($"İade işlemi başarısız: {ex.Message}");
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
            var carRental = _carRental.Get(cr => cr.RentalStatus == RentalStatus.Active && cr.UserId == userId);
            if (carRental != null)
            {
                carRental.Car = _car.Get(c=>c.Id==carRental.CarId);
            }

            return new SuccessDataResult<CarRental>(carRental);

        }

        public IResult GetUserCompletedCarRentals(int userId)
        {
            return new SuccessDataResult<List<CarRental>>(_carRental.GetAll(cr => cr.RentalStatus == RentalStatus.Completed && cr.UserId == userId));
        }

        public IDataResult<RentalDto> GetUserActiveCarRentalsDetails(int userId)
        {
            try
            {
                var rental = (from cr in _carRental.GetAll()
                              join c in _car.GetAll() on cr.CarId equals c.Id
                              join b in _brandDal.GetAll() on c.BrandId equals b.Id
                              join m in _modelDal.GetAll() on c.ModelId equals m.Id
                              join cat in _categoryDal.GetAll() on c.CategoryId equals cat.Id
                              where cr.UserId == userId && cr.RentalStatus == RentalStatus.Active
                              select new RentalDto
                              {
                                  Id = cr.Id,
                                  CarId = cr.CarId,
                                  CardId=cr.CardId,
                                  UserId = cr.UserId,
                                  StartDate = cr.StartDate,
                                  EndDate = cr.EndDate,
                                  TotalPrice = cr.TotalPrice,
                                  StartLatitude = cr.StartLatitude,
                                  StartLongitude = cr.StartLongitude,
                                  EndLatitude = cr.EndLatitude,
                                  EndLongitude = cr.EndLongitude,
                                  RentalStatus = cr.RentalStatus,
                                  // Car bilgileri
                                  Car = new Car
                                  {
                                      Id = c.Id,
                                      Plate = c.Plate,
                                      Year = c.Year,
                                      IsAvailable = c.IsAvailable,
                                      FuelType = c.FuelType,
                                      Transmission = c.Transmission,
                                      PricePerHour = c.PricePerHour,
                                      SeatCount = c.SeatCount,
                                      Latitude = c.Latitude,
                                      Longitude = c.Longitude,
                                      ModelId = c.ModelId,
                                      BrandId = c.BrandId,
                                      CategoryId = c.CategoryId,
                                  },
                                  // User bilgileri
                                
                                  CategoryName = cat.Name,
                                  BrandName = b.Name,
                                  ModelName = m.Name,
                                  FuelTypeName=c.FuelType.ToString(),
                                  TranssmissionName=c.Transmission.ToString(),
                              }).FirstOrDefault();

                if (rental != null)
                {
                    return new SuccessDataResult<RentalDto>(rental);  
                }
                else
                {
                    return new ErrorDataResult<RentalDto>("No active rental found for this user.");
                }
            }
            catch (Exception ex)
            {
               
                return new ErrorDataResult<RentalDto>(ex.Message);
            }
        }

        public IDataResult<List<RentalDto>> GetUserAllCarRentalsDetails(int userId)
        {
            try
            {
                var rentals = (from cr in _carRental.GetAll()
                               join c in _car.GetAll() on cr.CarId equals c.Id
                               join b in _brandDal.GetAll() on c.BrandId equals b.Id
                               join m in _modelDal.GetAll() on c.ModelId equals m.Id
                               join cat in _categoryDal.GetAll() on c.CategoryId equals cat.Id
                               where cr.UserId == userId 
                               select new RentalDto
                               {
                                   Id = cr.Id,
                                   CarId = cr.CarId,
                                   UserId = cr.UserId,
                                   StartDate = cr.StartDate,
                                   EndDate = cr.EndDate,
                                   TotalPrice = cr.TotalPrice,
                                   StartLatitude = cr.StartLatitude,
                                   StartLongitude = cr.StartLongitude,
                                   EndLatitude = cr.EndLatitude,
                                   EndLongitude = cr.EndLongitude,
                                   RentalStatus = cr.RentalStatus,
                                   Car = new Car
                                   {
                                       Id = c.Id,
                                       Plate = c.Plate,
                                       Year = c.Year,
                                       IsAvailable = c.IsAvailable,
                                       FuelType = c.FuelType,
                                       Transmission = c.Transmission,
                                       PricePerHour = c.PricePerHour,
                                       SeatCount = c.SeatCount,
                                       Latitude = c.Latitude,
                                       Longitude = c.Longitude,
                                       ModelId = c.ModelId,
                                       BrandId = c.BrandId,
                                       CategoryId = c.CategoryId,
                                   },
                                   CategoryName = cat.Name,
                                   BrandName = b.Name,
                                   ModelName = m.Name,
                                   FuelTypeName = c.FuelType.ToString(),
                                   TranssmissionName = c.Transmission.ToString(),
                               }).ToList();

                if (rentals.Any())
                {
                    return new SuccessDataResult<List<RentalDto>>(rentals);
                }
                else
                {
                    return new ErrorDataResult<List<RentalDto>>("No active rentals found for this user.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary.
                return new ErrorDataResult<List<RentalDto>>("An error occurred while retrieving rental details.");
            }
        }

        public IResult UpdateCardId(int cardId, int rentalId)
        {
            
            var rental = _carRental.Get(cr => cr.Id == rentalId);
      
            if (rental == null)
            {
                return new ErrorResult("Rental not found.");
            }
     
            rental.CardId = cardId;
            _carRental.Update(rental);
            return new SuccessResult("Card ID updated successfully.");
        }

    }
}
