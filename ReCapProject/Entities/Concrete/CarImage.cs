

using Microsoft.AspNetCore.Http;

namespace Entities
{
     public class CarImage : IEntity
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string? ImagePath { get; set; }
        public DateTime Date { get; set; }

        //public IFormFile file { get; set; }
    }
}
