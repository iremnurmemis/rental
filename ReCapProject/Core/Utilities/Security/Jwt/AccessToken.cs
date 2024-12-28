

namespace Core
{
    public class AccessToken
    {
        public string Token {  get; set; } //tokenın kendisi
        public DateTime ExpirationDate { get; set; } //tokenın gecerlilik süresi
    }
}
