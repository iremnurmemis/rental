

using Core;

namespace Entities
{
    public class CarLocationDto:IDto
    {
        public int CarId {  get; set; }
        public double Latitude {  get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public decimal PricePerHour { get; set; }
        public int SeatCount { get; set; }
        public string Transmission { get; set; }
        public string Plate { get; set; }

    }
}
