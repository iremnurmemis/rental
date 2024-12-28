

using Core;

namespace Entities
{
    public class CarListPageDto:IDto
    {
        public int CarId { get; set; }
        public int BrandId { get; set; }
        public string BrandName {  get; set; }
        public string ModelName {  get; set; }
        public string CategoryName {  get; set; }
        public decimal PricePerHour {  get; set; }
        public int SeatCount {  get; set; }
        public string Transmission { get; set; }
    }
}
