
using Microsoft.Identity.Client;

namespace Core
{
    public interface ITokenHelper
    {

        //user bilgilerini ve ona ait claimlerin de tokenda bulunmasını istiyorum
        AccessToken CreateToken(User user,List<OperationClaim> operationClaims);
    }
}
