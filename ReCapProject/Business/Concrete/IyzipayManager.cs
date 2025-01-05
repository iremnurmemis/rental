
using Core.Interceptors.Utilities.Results;
using Entities;
using Iyzipay;
using Iyzipay.Request;
using Iyzipay.Model;
using System.Diagnostics;
using System.Globalization;
using Core;


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

        public async Task<Iyzipay.Model.Payment> CreatePayment(Entities.Card card,User user,CarRental rental,decimal totalPrice)
        {
            Options options = _options;
            

            CreatePaymentRequest request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = "123456789",
                Price = totalPrice + ".00",
                PaidPrice = totalPrice + ".00",
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = "B67832",
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                PaymentCard = new PaymentCard
                {
                    CardHolderName = card.CardHolderName,
                    CardNumber = card.CardNumber,
                    ExpireMonth = "12",
                    ExpireYear = "2030",
                    Cvc = "123",
                    RegisterCard = 0
                },
                Buyer = new Buyer
                {
                    Id = user.Id.ToString(),
                    Name = user.FirstName+user.LastName,
                    Surname = user.LastName,
                    GsmNumber = user.PhoneNumber,
                    Email = user.Email,
                    IdentityNumber = "74300864791",
                    LastLoginDate = "2015-10-05 12:43:35",
                    RegistrationDate = "2013-04-21 15:12:09",
                    RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                    Ip = "85.34.78.112",
                    City = "Istanbul",
                    Country = "Turkey",
                    ZipCode = "34732"
                },
                ShippingAddress = new Address
                {
                    ContactName = user.FirstName + user.LastName,
                    City = "Istanbul",
                    Country = "Turkey",
                    Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                    ZipCode = "34742"
                },
                BillingAddress = new Address
                {
                    ContactName = user.FirstName,
                    City = "Istanbul",
                    Country = "Turkey",
                    Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                    ZipCode = "34742"
                },
                BasketItems = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = rental.Id.ToString(),
                        Name = "Car Rental",
                        Category1 = "Rentals",
                        Category2 = "Vehicles",
                        ItemType = BasketItemType.PHYSICAL.ToString(),
                        Price = totalPrice+".00",
                    },
                   
                }
            };

            // Asenkron işlemi await ile çağırıyoruz
            var payment = await Iyzipay.Model.Payment.Create(request, options);

            // Hata kontrolü, başarılıysa ödeme nesnesini döndürürüz
            if (payment.Status == "success")
            {
                return payment;
            }
            else
            {
                throw new Exception($"Ödeme başarısız: {payment.ErrorMessage}");
            }
        }


    }
}



  