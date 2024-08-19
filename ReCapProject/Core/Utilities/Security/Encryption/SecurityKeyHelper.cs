

using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Core
{
    public class SecurityKeyHelper
    {
        //ben securitykey vericem o da bana symetricsecuritykey döndürecek
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
