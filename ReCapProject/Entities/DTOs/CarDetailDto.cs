
using Core;

namespace Entities
{
    public class CarDetailDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; }
        public bool IsAvailable { get; set; }
        public  string FuelType { get; set; } 
        public  string Transmission { get; set; } 
        public decimal PricePerHour { get; set; }
        public int SeatCount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        
        public List<CarImageDto> CarImages { get; set; }

       
        public List<CarRentalDto> CarRentals { get; set; }

        
        public string MainImage { get; set; }
    }


    public class CarRentalDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public RentalStatus RentalStatus { get; set; }
        public double StartLongitude { get; set; }
        public double StartLatitude { get; set; }
        public double? EndLongitude { get; set; }
        public double? EndLatitude { get; set; }

    }


    public class CarImageDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; } // Ana resim mi?
    }




}
