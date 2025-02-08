
using Core;
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;
using Microsoft.AspNetCore.Http;

namespace Business
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;
        IFileHelper _fileHelper;
        ICarRentalDal _carRentalDal;
        public CarImageManager(ICarImageDal carImageDal, IFileHelper fileHelper, ICarRentalDal carRentalDal)
        {
            _carImageDal = carImageDal;
            _fileHelper = fileHelper;
            _carRentalDal = carRentalDal;
        }

        public async Task<IResult> AddRentalImages(int rentalId, List<IFormFile> files)
        {
            if (files.Count != 4)
            {
                return new ErrorResult("Kiralama iadesi için 4 adet fotoğraf yüklenmelidir.");
            }

            var rental=_carRentalDal.Get(cr=>cr.Id==rentalId);
            if (rentalId==null)
            {
                return new ErrorResult("Kiralama bulunamadı");
            }

            foreach (var file in files)
            {
                string imagePath = _fileHelper.Add(file);

                var rentalImage = new CarImage
                {
                    RentalId = rentalId,
                    CarId=2,
                    ImagePath = imagePath,
                    Date = DateTime.UtcNow,
                    IsMain = false
                };

                _carImageDal.Add(rentalImage);
            }

            return new SuccessResult("Kiralama iade fotoğrafları başarıyla eklendi.");
        }




        public IResult Add(IFormFile file, CarImage carImage)
        {
            IResult? result = BusinessRules.Run(CheckIfCarImageLimit(carImage.CarId));
            if (result != null)
            {
                return result;
            }

            string guid = _fileHelper.Add(file);
            carImage.ImagePath = guid;
            carImage.Date = DateTime.Now;
            _carImageDal.Add(carImage);
            return new SuccessResult(Messages.CarImageAdded);

        }

        public IResult Delete(CarImage carImage)
        {
            _carImageDal.Delete(carImage);
            _fileHelper.Delete(carImage.ImagePath!);
            return new SuccessResult(Messages.CarImageDeleted);
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(), Messages.CarImageListed);

        }

        public IDataResult<List<CarImage>> GetByCarId(int carId)
        {
            var carImages=_carImageDal.GetAll(cı=>cı.CarId == carId);
            if(carImages.Count == 0)
            {
                carImages.Add(new CarImage() { CarId= carId,ImagePath="default.png"});
                return new SuccessDataResult<List<CarImage>>(carImages); 
            }

            return new SuccessDataResult<List<CarImage>>(carImages);
        }

        public IDataResult<CarImage> GetById(int Id)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(cı => cı.Id == Id), Messages.CarImageListed);
        }

        public IResult Update(IFormFile file, CarImage carImage)
        {
            _fileHelper.Update(file,carImage.ImagePath!);
            carImage.Date= DateTime.Now;
            _carImageDal.Update(carImage);
            return new SuccessResult(Messages.CarImageUpdated);
        }

        private IResult CheckIfCarImageLimit(int carId)
        {
            var result = _carImageDal.GetAll(c => c.CarId == carId).Count;
            if (result >= 5)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }
    }
}
