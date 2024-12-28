

using Core.Interceptors.Utilities.Results;

namespace Business
{
    public interface ICarRentalService
    {
        IResult AddCarRental(int carId, int userId);             // Kiralama ekleme işlemi
        IResult CompleteCarRental(int rentalId);                  // Kiralamayı iade veya tamamlanmış olarak işaretleme
        IResult GetAllCarRentals();                               // Tüm kiralama kayıtlarını alma
        //IResult GetAllCarRentalsWithDetails();                    // Tüm kiralama kayıtlarını araba bilgileriyle alma
        IResult GetActiveCarRentals();                            // Aktif kiralamaları alma
        IResult GetCompletedCarRentals();                         // Tamamlanmış kiralamaları alma
        IResult GetUserCarRentals(int userId);                    // Belirtilen kullanıcının tüm kiralamalarını alma
        IResult GetUserActiveCarRentals(int userId);      // Kullanıcının aktif kiralamaları
        IResult GetUserCompletedCarRentals(int userId);   // Kullanıcının tamamlanmış kiralamaları


    }
}
