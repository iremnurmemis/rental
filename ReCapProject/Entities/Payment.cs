

namespace Entities
{
    public class Payment:IEntity
    {
        public int Id { get; set; }
        public int? CarId {  get; set; }
        public int UserId {  get; set; }
        public int? RentalId {  get; set; }
        public int? BalancePackageId { get; set; } // Bakiye paket ID'si
        public int CardId { get; set; }
        public decimal TotalPrice {  get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentType Type { get; set; } // Ödeme türü
        public DateTime CreatedTime { get; set; }
    }
}
