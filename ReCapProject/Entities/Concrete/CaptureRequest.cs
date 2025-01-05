

namespace Entities
{
    public class CaptureRequest
    {
        public string CardToken { get; set; }  // Kart Token
        public decimal Amount { get; set; }  // Ödeme Tutarı
        public int RentalId { get; set; }  // Kiralama ID'si
    }

    public class CaptureResult
    {
        public bool Success { get; set; }  // İşlem başarı durumu
        public string Message { get; set; }  // Başarısızsa hata mesajı
    }


}
