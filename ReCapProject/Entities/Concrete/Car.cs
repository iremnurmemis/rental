

namespace Entities
{
    public class Car:IEntity
    {
       public int Id { get; set; }
       public int BrandId {  get; set; }
       public int ModelId {  get; set; }

       public int ColorId { get; set; }
       public int CategoryId {  get; set; }
       public int Year {  get; set; }
       public string Plate {  get; set; }
       public bool IsAvailable {  get; set; }
       public FuelType FuelType { get; set; }
       public Transmission Transmission { get; set; }
       public decimal PricePerHour {  get; set; } // Saatlik kiralama modeli için aracın saatlik fiyatı 
       public decimal PricePerDay { get; set; } // Günlük kiiralama modeli için aracın günlük fiyatı
       public int SeatCount { get; set; }

       public double Latitude {  get; set; }
       public double Longitude { get; set; }

      // Bir araba birden fazla resme sahip olabilir.
       public ICollection<CarImage>? CarImages { get; set; }

       // Bir araba birçok kez kiralanabilir.
       public ICollection<CarRental>? CarRentals { get; set; }

       public CarImage? MainImage {  get; set; }   
        

    }
}
