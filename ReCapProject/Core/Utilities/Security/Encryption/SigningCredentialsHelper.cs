﻿

using Microsoft.IdentityModel.Tokens;

namespace Core
{
    public class SigningCredentialsHelper
    {
        //kullanılan alg tanımlanır
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
