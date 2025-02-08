

using Core;

namespace Entities
{
    public class UserPaymentsDto:IDto
    {
        public int Id { get; set; }
        public int CardId {  get; set; }
        public string CardHolderName {  get; set; }
        public string CardNumber { get; set; }
        public int UserId {  get; set; }
        public int? RentalId {  get; set; }
        public decimal RentalAmount { get; set; }

        public int? CarId {  get; set; }
        public string Category {  get; set; }
        public string Brand { get; set; }
        public string Model {  get; set; }
        public string Plate {  get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public string PaymentType {  get; set; }
        public int? balancePackageId {  get; set; }
        public decimal totalPrice { get; set; }



    }
}


