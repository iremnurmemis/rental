

namespace Entitiesü
{
    public class CarRentalRequest
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }
    }

    public class İadeRequest
    {
        public int rentalId { get; set; }
       
    }

    public class UpdateCardIdRequest
    {
        public int cardId {  get; set; }
        public int rentalId { get; set; }

    }
}
