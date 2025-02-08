

using Entities;
using Microsoft.AspNetCore.Http;

namespace Entitiesü
{
    public class CarRentalRequest
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }
        public RentalType RentalType { get; set; }
        public int? durationInDays {  get; set; }
        public bool? useBalance { get; set; }
    }

    public class İadeRequest
    {
        public int rentalId { get; set; }
        public List<IFormFile> Images { get; set; }


    }

    public class UpdateCardIdRequest
    {
        public int cardId {  get; set; }
        public int rentalId { get; set; }

    }

    public class RentalImageRequest
    {
        public int RentalId { get; set; }
        public List<IFormFile> Images { get; set; }
    }

}
