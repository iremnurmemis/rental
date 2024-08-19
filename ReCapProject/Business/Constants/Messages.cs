

using Core;

namespace Business
{
    public static class Messages
    {
        public static string CarNameIsvalid = "Araba ismi en az 2 karakter olmalıdır";
        public static string CarPriceIsvalid = "Araba fiyatı 0'dan büyük olmalıdır";
        public static string CarAdded = "Araba başarıyla eklendi";
        public static string CarDeleted = "Araba başarıyla silindi";
        public static string CarUpdated = "Araba başarıyla güncellendi";
        public static string CarListed = "Arabalar listelendi";
        public static string MaintanenceTime = "Sistem bakımda. Lütfen daha sonra tekrar deneyin";

        public static string BrandNameIsvalid = "Marka ismi en az 2 karakter olmalıdır";
        public static string BrandAdded = "Marka başarıyla eklendi";
        public static string BrandDeleted = "Marka başarıyla silindi";
        public static string BrandUpdated = "Marka başarıyla güncellendi";
        public static string BrandListed = "Markalar listelendi";

        public static string ColorNameIsvalid = "Renk ismi en az 2 karakter olmalıdır";
        public static string ColorAdded = "Renk başarıyla eklendi";
        public static string ColorDeleted = "Renk başarıyla silindi";
        public static string ColorUpdated = "Renk başarıyla güncellendi";
        public static string ColorListed = "Renkler listelendi";

        public static string UserNameIsvalid = "Kullanıcı ismi en az 2 karakter olmalıdır";
        public static string UserAdded = "Kullanıcı başarıyla eklendi";
        public static string UserDeleted = "Kullanıcı başarıyla silindi";
        public static string UserUpdated = "Kullanıcı başarıyla güncellendi";
        public static string UserListed = "Kullanıcılar listelendi";

        public static string CustomerNameIsvalid = "Şirket ismi en az 2 karakter olmalıdır";
        public static string CustomerAdded = "Müşteri başarıyla eklendi";
        public static string CustomerDeleted = "Müşteri başarıyla silindi";
        public static string CustomerUpdated = "Müşteri başarıyla güncellendi";
        public static string CustomerListed = "Müşteriler listelendi";

        public static string RentalAdded = "Satış başarıyla eklendi";
        public static string RentalDeleted = "Satış başarıyla silindi";
        public static string RentalUpdated = "Satış başarıyla güncellendi";
        public static string RentalListed = "Satışlar listelendi";

        public static string RentalNotAvailable = "Araba kiralanamaz su an kullanılıyor";
        public static string CarImageAdded = "Resim eklendi";
        public static string CarImageUpdated = "Resim güncellendi";
        public static string CarImageDeleted = "Resim silindi";
        public static string CarImageListed = "Resimler listelendi";
        public static string CarImageListedByCarId = "Arabaya ait resimler listelendi";

        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Şifre hatalı";
        public static string SuccessfulLogin = "Login işlemi başarılı";

        public static string UserAlreadyExists = "Bu kullanıcı zaten mecvut";

        public static string UserRegistered = "Kullanıcı sisteme kayıt edildi";

        public static string AccessTokenCreated = "Access Token basarıyla olusturuldu";
    }
}