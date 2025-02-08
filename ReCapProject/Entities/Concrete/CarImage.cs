

using Microsoft.AspNetCore.Http;

namespace Entities
{
     public class CarImage : IEntity
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int? RentalId { get; set; } //kiralamaya ait fotolar için
        public Car Car { get; set; }
        public string? ImagePath { get; set; }
        public DateTime Date { get; set; }

        public bool IsMain { get; set; }  // Ana resim mi?

    }
}
