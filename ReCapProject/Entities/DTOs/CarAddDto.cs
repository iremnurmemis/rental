

namespace Entities
{
    public class CarAddDto
    {
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public int CategoryId { get; set; }
        public int Year { get; set; }
        public string Plate { get; set; }
        public bool IsAvailable { get; set; }
        public FuelType FuelType { get; set; }
        public Transmission Transmission { get; set; }
        public decimal PricePerHour { get; set; }
        public int SeatCount { get; set; }

        // Konum bilgisi
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
