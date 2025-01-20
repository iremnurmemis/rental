

using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface ICarRentalService
    {
        Task<IResult> AddCarRental(int carId, int userId,int cardId, RentalType rentalType, int? durationInDays = null);             // Kiralama ekleme işlemi
        Task<IResult> CompleteCarRental(int rentalId);                  // Kiralamayı iade veya tamamlanmış olarak işaretleme
        IResult GetAllCarRentals();                               // Tüm kiralama kayıtlarını alma
        //IResult GetAllCarRentalsWithDetails();                    // Tüm kiralama kayıtlarını araba bilgileriyle alma
        IResult GetActiveCarRentals();                            // Aktif kiralamaları alma
        IResult GetCompletedCarRentals();                         // Tamamlanmış kiralamaları alma
        IResult GetUserCarRentals(int userId);                    // Belirtilen kullanıcının tüm kiralamalarını alma
        IResult GetUserActiveCarRentals(int userId);      // Kullanıcının aktif kiralamaları
        IResult GetUserCompletedCarRentals(int userId);   // Kullanıcının tamamlanmış kiralamaları

        IDataResult<RentalDto> GetUserActiveCarRentalsDetails(int userId);     // Kullanıcının aktif kiralamaları frontend için
        IDataResult<List<RentalDto>> GetUserAllCarRentalsDetails(int userId);     // Kullanıcının tüm kiralamaları frontend için

        IResult UpdateCardId(int cardId,int rentalId);

    }
}
