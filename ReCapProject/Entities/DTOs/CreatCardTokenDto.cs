
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class CreatCardTokenDto
    {
        public int UserId { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }

        [Required, RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Geçersiz ay formatı.")]
        public string ExpireMonth { get; set; }

        [Required, RegularExpression(@"^\d{4}$", ErrorMessage = "Geçersiz yıl formatı.")]
        public string ExpireYear { get; set; }
        public CardType CardType { get; set; }
    }
}
