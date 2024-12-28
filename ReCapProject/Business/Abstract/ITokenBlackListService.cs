


namespace Business
{
    public interface ITokenBlackListService
    {
        void AddTokenToBlacklist(string token);
        bool IsTokenBlackListed(string token);
    }
}
