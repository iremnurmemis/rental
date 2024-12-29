
namespace Entities
{
    public class CardDetailDto
    {
        public string MaskedCardNumber { get; set; }

        public string CardAlias {  get; set; } //kart adı
        public string CardHolderNmae {  get; set; }
        public CardType CardType { get; set; }
    }
}
