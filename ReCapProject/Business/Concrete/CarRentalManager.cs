
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

        public async Task<IResult> AddCarRental(int carId, int userId, int cardId, RentalType rentalType, int? durationInDays = null)
        {
            var user = _userService.GetById(userId);
            if (user == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı.");
            }

            if (!user.Data.IsDrivingLicenseVerified)
            {
                return new ErrorResult("Kiralamaya başlamadan önce Sürücü belgenizi sisteme yüklemelisiniz.");
            }

            var existingRental = _carRental.Get(cr => cr.UserId == userId && cr.RentalStatus == RentalStatus.Active);
            if (existingRental != null)
            {
                return new ErrorResult("Kullanıcının aktif bir kiralama işlemi zaten var.");
            }

            var carAvailable = _car.Get(c => c.Id == carId && c.IsAvailable);
            if (carAvailable == null)
            {
                return new ErrorResult("Araç şu anda uygun değil.");
            }

            decimal totalPrice = 0;
            DateTime? endDate = null;

            // RentalType'e göre fiyat hesaplama
            if (rentalType == RentalType.Daily)
            {
                if (!durationInDays.HasValue || durationInDays.Value <= 0)
                {
                    return new ErrorResult("Geçerli bir günlük kiralama süresi belirtilmedi.");
                }
                totalPrice = carAvailable.PricePerDay * durationInDays.Value;
                endDate = DateTime.UtcNow.AddDays(durationInDays.Value);
            }
           
            // Kiralama modeline göre eğer saatlikse direkt aktif günnlükse ödeme işlemine bakar
            var pendingRental = new CarRental
            {
                CarId = carId,
                UserId = userId,
                StartDate = DateTime.UtcNow,
                EndDate = rentalType == RentalType.Daily ? endDate : null,
                RentalStatus = rentalType == RentalType.Daily ? RentalStatus.Pending:RentalStatus.Active,
                RentalType = rentalType,
                DurationInDays = rentalType == RentalType.Daily ? durationInDays : null,
                TotalPrice = rentalType == RentalType.Daily ? totalPrice : null,
                CardId = cardId,
                StartLatitude=carAvailable.Latitude,
                StartLongitude=carAvailable.Longitude,
            };

            _carRental.Add(pendingRental);

            if (rentalType == RentalType.Hourly)
            {
               
                // Aracın durumu güncelleniyor
                carAvailable.IsAvailable = false;
                _car.Update(carAvailable);

                // E-posta bildirimi gönder
                var subject = "Araba Kiralama Onayı";
                var body = $"Merhaba {user.Data.FirstName}, araba kiralama işleminiz başarılı.\n\nTeşekkür ederiz!";
                _notificationService.SendNotification(user.Data.Email, subject, body);

                return new SuccessDataResult<CarRental>(pendingRental, "Araç başarıyla kiralandı.");
            }


            // Ödeme işlemi yalnızca günlük için yapılacak
            if (rentalType == RentalType.Daily )
            {
                var card = _cardDal.Get(c => c.Id == cardId);
                if (card == null)
                {
                    return new ErrorResult("Kart bilgisi yanlış.");
                }

                // Ödeme kaydını oluştur
                var paymentRecord = new Payment
                {
                    CardId = cardId,
                    UserId = userId,
                    CarId = carId,
                    RentalId = pendingRental.Id, // Geçici kiralama ID'sini kullanıyoruz
                    TotalPrice = totalPrice,
                    Status = PaymentStatus.Pending, // Başlangıçta 'Pending'
                    CreatedTime = DateTime.UtcNow,
                    Type = PaymentType.Rental,
                };

                _paymentDal.Add(paymentRecord);

                try
                {
                    var paymentResult = await _iyzipayService.CreatePayment(card, user.Data, pendingRental.Id, totalPrice);

                    // Ödeme başarısızsa işlemi sonlandır
                    if (paymentResult.Status != "success")
                    {
                      
                        paymentRecord.Status = PaymentStatus.Failed;
                        _paymentDal.Update(paymentRecord);

                        
                        pendingRental.RentalStatus = RentalStatus.Failed;
                        _carRental.Update(pendingRental);

                        return new ErrorResult($"Ödeme işlemi başarısız: {paymentResult.ErrorMessage}");
                    }
                    else
                    {
                        paymentRecord.Status = PaymentStatus.Success;
                        _paymentDal.Update(paymentRecord);
                    }

                    // Kiralama işlemi başarıyla tamamlanıyorsa
                    using (var transactionScope = new TransactionScope())
                    {
                        try
                        {
                            // Kiralama durumu aktif yap
                            pendingRental.RentalStatus = RentalStatus.Active;
                            pendingRental.StartDate = DateTime.UtcNow;
                            pendingRental.EndDate = rentalType == RentalType.Daily ? endDate : null;
                            _carRental.Update(pendingRental);

                            // Aracın durumu güncelleniyor
                            carAvailable.IsAvailable = false;
                            _car.Update(carAvailable);

                            // E-posta bildirimi gönder
                            var subject = "Araba Kiralama Onayı";
                            var body = $"Merhaba {user.Data.FirstName}, araba kiralama işleminiz başarılı.\n\nTeşekkür ederiz!";
                            _notificationService.SendNotification(user.Data.Email, subject, body);

                            transactionScope.Complete();
                            return new SuccessDataResult<CarRental>(pendingRental, "Araç başarıyla kiralandı.");
                        }
                        catch (Exception ex)
                        {
                            return new ErrorResult($"Kiralama işlemi başarısız: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    paymentRecord.Status = PaymentStatus.Failed;
                    _paymentDal.Update(paymentRecord);

                    pendingRental.RentalStatus = RentalStatus.Failed;
                    _carRental.Update(pendingRental);

                    return new ErrorResult($"Ödeme işlemi sırasında hata oluştu: {ex.Message}");
                }
            }

            // Eğer kiralama tipi geçerli değilse, bir hata mesajı döndürüyoruz
            return new ErrorResult("Geçersiz kiralama tipi.");
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

                rental.EndLatitude = 41.043;
                rental.EndLongitude = 29.0083;

                // Eğer kiralama saatlikse, EndDate ve TotalPrice hesapla
                if (rental.RentalType == RentalType.Hourly)
                {
                    rental.EndDate = DateTime.UtcNow;

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
                    var payment = await _iyzipayService.CreatePayment(card, userBilgi, rental.Id, totalPrice);

                    if (payment.Status != "success")
                    {
                        return new ErrorResult($"Ödeme işlemi başarısız: {payment.ErrorMessage}");
                    }

                    // Ödeme başarılıysa 
                    paymentRecord.Status = PaymentStatus.Success;
                    _paymentDal.Update(paymentRecord);

                    using (var transactionScope = new TransactionScope())
                    {
                        try
                        {
                            rental.RentalStatus = RentalStatus.Completed;
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
                else if (rental.RentalType == RentalType.Daily)
                {
                    if (rental.EndDate.HasValue && rental.EndDate.Value <= DateTime.UtcNow)
                    {
                        // Aşım ücreti hesapla
                        var overdueDuration = DateTime.UtcNow - rental.EndDate.Value;
                        var overdueHours = Math.Ceiling(overdueDuration.TotalHours);

                        // Eğer aşım olmuşsa, sadece aşım ücretini ekle
                        if (overdueHours > 0)
                        {
                            var overdueFee = (decimal)overdueHours * (decimal)carHourlyPrice;

                            // Aşım ücreti ödeme kaydına ekle
                            var totalOverdueFee = Convert.ToInt32(overdueFee);

                            // Ödeme işlemi için veritabanına ödeme kaydını ekleyelim
                            var card = _cardDal.Get(c => c.Id == rental.CardId);
                            var userBilgi = _userService.GetById(rental.UserId).Data;

                            var paymentRecord = new Payment
                            {
                                CardId = card.Id,
                                UserId = rental.UserId,
                                CarId = rental.CarId,
                                RentalId = rental.Id,
                                TotalPrice = totalOverdueFee, // Sadece aşım ücreti
                                Status = PaymentStatus.Failed,
                                CreatedTime = DateTime.UtcNow,
                                Type=PaymentType.Rental,
                            };

                            _paymentDal.Add(paymentRecord);

                            // Ödeme işlemi
                            var payment = await _iyzipayService.CreatePayment(card, userBilgi, rental.Id, totalOverdueFee);

                            if (payment.Status != "success")
                            {
                                return new ErrorResult($"Ödeme işlemi başarısız: {payment.ErrorMessage}");
                            }

                            paymentRecord.Status = PaymentStatus.Success;
                            _paymentDal.Update(paymentRecord);
                            rental.overdueEndDate = DateTime.UtcNow;
                            rental.totalOverdueFee = totalOverdueFee;
                        }

                        using (var transactionScope = new TransactionScope())
                        {
                            try
                            {
                               
                                rental.RentalStatus = RentalStatus.Completed;
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
                    else
                    {
                        return new ErrorResult("Kiralama süresi henüz bitmedi.");
                    }
                }
                else
                {
                    return new ErrorResult("Geçersiz kiralama tipi.");
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
                                  RentalType=cr.RentalType.ToString(),
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
                                   RentalType=cr.RentalType.ToString(),
                                   overdueEndDate=cr.overdueEndDate,
                                   totalOverdueFee=cr.totalOverdueFee,
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
