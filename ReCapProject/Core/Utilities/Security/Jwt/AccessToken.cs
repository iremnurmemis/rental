

namespace Core
{
    public class AccessToken
    {
        public string Token {  get; set; } //tokenın kendisi
        public DateTime Expiration { get; set; } //tokenın gecerlilik süresi
    }
}
