
using Core.Interceptors.Utilities.Results;
using DataAccess;
using DataAccess.Migrations;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Globalization;
using System.Transactions;
using DriverLicence = Entities.DriverLicence;


namespace Business
{
    public class DriverLicenceManager : IDriverLicenceService
    {
        private readonly IDriverLicenceDal _driveLicenceDal;
        private readonly IOCRModelService _ocrModelService;
        private readonly IUserDal _userDal; 
        public DriverLicenceManager(IDriverLicenceDal driverLicenceDal, IOCRModelService ocrModelService, IUserDal userDal)
        {
            _driveLicenceDal = driverLicenceDal;
            _ocrModelService = ocrModelService;
            _userDal = userDal;
        }
        public IDataResult<DriverLicence> Add(DriverLicence driverLicence)
        {
            _driveLicenceDal.Add(driverLicence);
            return new SuccessDataResult<DriverLicence>(driverLicence, "Ehliyet bilgileriniz doğrulanmanın ardından eklendi");    
        }

        public IDataResult<List<DriverLicence>> GetAllDriverLicences()
        {
            return new SuccessDataResult<List<DriverLicence>>(_driveLicenceDal.GetAll(),"listelenndi");
        }

        public IDataResult<DriverLicence> GetUserDriverLicenceInfo(int userId)
        {
            return new SuccessDataResult<DriverLicence>(_driveLicenceDal.Get(d => d.UserId == userId), "userın ehliyet bilgileri geldi");
        }

        public IResult UploadDriverLicence(IFormFile file, int userId)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var user = _userDal.Get(u => u.Id == userId);
                    if (user == null)
                    {
                        return new ErrorResult("Kullanıcı yok");
                    }

                    if (file == null || file.Length == 0)
                    {
                        return new ErrorResult("Geçerli bir dosya yükleyin.");
                    }

                    // Dosyayı byte dizisine çevir
                    byte[] imageBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }

                    // Python modelini çalıştırın ve OCR çıktısını alın
                    var result = _ocrModelService.RunPythonScript(imageBytes);
                    var response = JsonConvert.DeserializeObject<OcrResponse>(result);

                    // OCR sonucunu kontrol et
                    if (response == null || response.Error != null)
                    {
                        if (response?.Error == "Şablona uygun değil, lütfen ehliyet formatında bir belge yükleyin.")
                        {
                            return new ErrorResult("Şablona uygun değil, lütfen ehliyet formatında bir belge yükleyin.");
                        }
                        else
                        {
                            return new ErrorResult(response?.Error ?? "OCR işlemi sırasında bir hata oluştu.");
                        }
                    }

                    // Alanların tam olup olmadığını kontrol et
                    if (string.IsNullOrEmpty(response.Field4A) ||
                        string.IsNullOrEmpty(response.Field4D) ||
                        string.IsNullOrEmpty(response.Field5))
                    {
                        return new ErrorResult("OCR işlemi başarılı ancak bazı alanlar eksik. Lütfen yüklediğiniz belgenin okunabilir olduğundan emin olun.");
                    }

                    // Tarih formatı kontrolü
                    if (!DateTime.TryParseExact(response.Field4A, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var validUntil))
                    {
                        return new ErrorResult("Geçerli bir tarih formatı değil. Lütfen 4A alanını kontrol edin.");
                    }

                    // Ehliyet bilgisi oluştur
                    var driverLicence = new DriverLicence
                    {
                        LicenceNo = response.Field5,
                        UserId = userId,
                        UploadDate = DateTime.UtcNow,
                        ValidUntil = validUntil.ToUniversalTime()
                    };

                    // Ehliyeti ekle
                    var addResult = Add(driverLicence);
                    if (!addResult.Success)
                    {
                        return new ErrorResult(addResult.Message);
                    }

                    // Kullanıcıyı güncelle
                    user.IsDrivingLicenseVerified = true;
                    _userDal.Update(user);

                    // Transaction başarılı, işlemi onayla
                    scope.Complete();

                    return new SuccessResult("Ehliyet başarıyla yüklendi ve işleme alındı.");
                }
                catch (Exception ex)
                {
                    return new ErrorResult($"Bir hata oluştu: {ex.Message}");
                }
            }
        }


        private OcrResponse ParseOcrResult(string ocrResult)
        {
            return JsonConvert.DeserializeObject<OcrResponse>(ocrResult);
        }
    }
}
