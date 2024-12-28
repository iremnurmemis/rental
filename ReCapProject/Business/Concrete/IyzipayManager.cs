
using Core.Interceptors.Utilities.Results;
using Entities;
using Iyzipay;
using Iyzipay.Request;
using Iyzipay.Model;


namespace Business
{
    public class IyzipayManager : IIyzipayService
    {
        private readonly string _apiKey = "sandbox-xDkzSJUH9yV7cPTx6AHTuGIg0MrOKsrS";
        private readonly string _secretKey = "sandbox-X1rguBQoSZ7AXuob0xwczOq1IyZbjVJE";
        private readonly string _baseUrl = "https://sandbox-api.iyzipay.com";

        private readonly Options _options;
        public IyzipayManager()
        {

            _options = new Options
            {
                ApiKey = _apiKey,
                SecretKey = _secretKey,
                BaseUrl = _baseUrl

            };
        }

        public async Task<string> CreateCardToken(CreatCardTokenDto creatCardTokenDto) 
        {

            CreateCardRequest request = new CreateCardRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = Guid.NewGuid().ToString(),
                Card = new CardInformation
                {
                    CardAlias = "UserCard", // İstediğiniz şekilde belirleyebilirsiniz
                    CardHolderName = creatCardTokenDto.CardHolderName,
                    CardNumber = creatCardTokenDto.CardNumber,
                    ExpireYear = creatCardTokenDto.ExpireYear,
                    ExpireMonth = creatCardTokenDto.ExpireMonth,
                    
                },
                
                ExternalId = "ext_" + Guid.NewGuid().ToString(),
                Email= "test@test.com",


            };
            
            var card=await Iyzipay.Model.Card.Create(request,_options);


            if (card.Status == "success")
            {
                return card.CardToken; // Token başarıyla oluşturulmuşsa döndürülür
            }
            else
            {
                throw new Exception($"Kart token oluşturulamadı: {card.ErrorMessage}");
            }


        }
    }
}
