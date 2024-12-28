
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace Core
{
    public interface ITokenHelper
    {

        
        string GenerateToken(string username,string password);

    }
}
