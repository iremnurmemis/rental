

namespace Core
{
    public class TokenOptions
    {
        //jwt tokena ait bilgilerin bulunduğu kısım
        public string Audience { get; set; }
        public string Issuer {  get; set; }
        public int AccessTokenExpiration {  get; set; }
        public string SecurityKey {  get; set; }
    }
}
