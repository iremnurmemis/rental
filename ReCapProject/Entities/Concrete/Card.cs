
using Core;
namespace Entities
{
    public class Card:IEntity
    {
        public int Id { get; set; }
        public string CardToken { get; set; }
        public CardType CardType { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }  // Kart numarasının son 4 hanesi

        public ICollection<CarRental>? CarRentals { get; set; }
    }
}
