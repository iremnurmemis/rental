
namespace Entities
{
    public class UserRentalsDto
    {
        public int RentalId { get; set; }
        public string CarName {  get; set; }
        public  int CarId {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public string rentalStatus {  get; set; }
        public string rentalType {  get; set; }
        public double? overduePrice { get; set; }
        public DateTime? overdueDate { get; set; }
    }
}
