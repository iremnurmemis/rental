
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface ICardService
    {
        Task<IDataResult<CreatCardTokenDto>> AddCard(CreatCardTokenDto createCard);

        IDataResult<Card> GetCard( int cardId);
        IResult  DeleteCard(Card card);

        IDataResult<List<Card>> GetUserCards(int userId);
    }
}
