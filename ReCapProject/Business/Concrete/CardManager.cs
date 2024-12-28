

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


        public async Task<IResult> AddCard(CreatCardTokenDto createCard)
        {
            var cardToken = await _iyzipayService.CreateCardToken(createCard);

            Card newCard = new Card
            {
                CardToken = cardToken,
                CardType = createCard.CardType,
                UserId = createCard.UserId,

            };


            _cardDal.Add(newCard);
            return new SuccessResult("card sisteme başarıyla eklendi");

        }

        public IResult DeleteCard(Card card)
        {
            _cardDal.Delete(card);
            return new SuccessResult("card siliindi");
        }

       


    }
}
