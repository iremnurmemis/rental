

using Core;

namespace Entities
{
   public class RentalDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int UserId { get; set; }
        public int? CardId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public RentalStatus RentalStatus { get; set; }
        public double StartLongitude { get; set; }
        public double StartLatitude { get; set; }
        public double? EndLongitude { get; set; }
        public double? EndLatitude { get; set; }

        // Category bilgileri
        public string CategoryName { get; set; }

        // Brand bilgileri
        public string BrandName { get; set; }

        // Model bilgileri
        public string ModelName { get; set; }
        public string FuelTypeName {  get; set; }
        public string TranssmissionName {  get; set; }

    }
}
