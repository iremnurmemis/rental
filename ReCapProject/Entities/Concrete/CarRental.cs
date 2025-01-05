
using Core;

namespace Entities
{
    public class CarRental:IEntity
    {
        public int Id { get; set; }
        public int CarId {  get; set; }
        public Car Car { get; set; }
        public int UserId {  get; set; }
        public User User { get; set; }
        public int? CardId { get; set; }
        public Card Card { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public RentalStatus RentalStatus { get; set; }
        public double StartLongitude {  get; set; }
        public double StartLatitude { get; set; }
        public double? EndLongitude { get; set; }
        public double? EndLatitude { get; set; }

        public ICollection<Payment>? Payments { get; set; }
    }
}

