

namespace Entities
{
    public class Payment:IEntity
    {
        public int Id { get; set; }
        public int CarId {  get; set; }
        public int UserId {  get; set; }
        public int RentalId {  get; set; }
        public int CardId { get; set; }
        public decimal TotalPrice {  get; set; }
        public PaymentStatus Status { get; set; }
    }
}
