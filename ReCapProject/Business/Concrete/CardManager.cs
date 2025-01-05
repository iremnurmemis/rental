

using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;

namespace Business
{
    public class CardManager : ICardService
    {
        ICardDal _cardDal;
        IIyzipayService _iyzipayService;
        public CardManager(ICardDal cardDal, IIyzipayService iyzipayService)
        {
            _cardDal = cardDal;
            _iyzipayService = iyzipayService;
        }


        public async Task<IDataResult<CreatCardTokenDto>>AddCard(CreatCardTokenDto createCard)
        {
            var cardToken = await _iyzipayService.CreateCardToken(createCard);

            Card newCard = new Card
            {
                CardToken = cardToken,
                CardType = createCard.CardType,
                UserId = createCard.UserId,
                CardHolderName = createCard.CardHolderName,
                CardNumber = createCard.CardNumber,

                 

            };

            _cardDal.Add(newCard);

            CreatCardTokenDto createcard = new CreatCardTokenDto
            {
                Id = newCard.Id,
                CardType = createCard.CardType,
                UserId = createCard.UserId,
                CardHolderName = createCard.CardHolderName,
                CardNumber = createCard.CardNumber,
                ExpireMonth = createCard.ExpireMonth,
                ExpireYear = createCard.ExpireYear,
            };

            return new SuccessDataResult<CreatCardTokenDto>(createcard,"card sisteme başarıyla eklendi");

        }

        public IResult DeleteCard(Card card)
        {
            _cardDal.Delete(card);
            return new SuccessResult("card siliindi");
        }

        public IDataResult<Card> GetCard(int cardId)
        {
            var card = _cardDal.Get(c => c.Id == cardId);
            if (card == null)
            {
                return new ErrorDataResult<Card>("Kart bulunamadı.");
            }

            return new SuccessDataResult<Card>(card);
        }

        public IDataResult<List<Card>> GetUserCards(int userId)
        {
            return new SuccessDataResult<List<Card>>(_cardDal.GetAll(c => c.UserId == userId)); 
        }
    }
}
