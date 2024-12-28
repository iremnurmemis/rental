


namespace Business
{
    public class TokenBlackListManager : ITokenBlackListService
    {
        private readonly List<string> _tokenBlackList=new List<string>();
        public void AddTokenToBlacklist(string token)
        {
           _tokenBlackList.Add(token);
        }

        public bool IsTokenBlackListed(string token)
        {
            return _tokenBlackList.Contains(token);
        }
    }
}
