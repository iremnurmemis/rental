
using Core;
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;
using System.Reflection.Metadata.Ecma335;

namespace Business
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;
        ICategoryDal _categoryDal;
        IModelDal _modelDal;
        IBrandDal _brandDal;
        public CarManager(ICarDal carDal, ICategoryDal categoryDal,IBrandDal brandDal,IModelDal modelDal)
        {
            _carDal = carDal;
            _categoryDal = categoryDal;
            _brandDal = brandDal;
            _modelDal = modelDal;
        }

        [ValidationAspect(typeof(CarValidator))]
        public IResult Add(Car car)
        {

            _carDal.Add(car);
            return new SuccessResult(Messages.CarAdded);
        }

        public IResult Delete(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.CarDeleted);
        }

        public IDataResult<List<CarDetailDto>> GetAllDetail()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(),Messages.CarListed);
        }


        public IDataResult<List<Car>> GetAll()
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll()
                , Messages.CarListed);
        }


        public IDataResult<List<Car>> GetByCarId(int carId)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(x => x.Id == carId), Messages.CarListed);
        }

        
        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.CarUpdated);

        }

        public IDataResult<List<Car>> GetByBrandId(int brandId)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c=>c.BrandId==brandId), Messages.CarListed);
        }

        public IDataResult<List<Car>> GetByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.CategoryId == categoryId), Messages.CarListed);

        }

        public IDataResult<List<CarLocationDto>> GetAllCarsLocation()
        {
            var result = (from car in _carDal.GetAll(c => c.IsAvailable == true)
                          join category in _categoryDal.GetAll()
                          on car.CategoryId equals category.Id
                          join brand in _brandDal.GetAll()
                         on car.BrandId equals brand.Id
                          join model in _modelDal.GetAll() on car.ModelId equals model.Id
                          select new CarLocationDto
                          {
                              CarId=car.Id,
                              Latitude = car.Latitude,
                              Longitude = car.Longitude,
                              Category = category.Name,
                              Brand=brand.Name,
                              BrandId=car.BrandId,
                              CategoryId=car.CategoryId,
                              PricePerHour=car.PricePerHour,
                              SeatCount=car.SeatCount,
                              Transmission= car.Transmission.ToString(),
                              Model = model.Name,
                              Plate=car.Plate,
                              PricePerDay=car.PricePerDay,
                              
                          }).ToList();

            return new SuccessDataResult<List<CarLocationDto>>(result, "Müsait araç konumları başarıyla getirildi.");
        }

        public IDataResult<List<CarListPageDto>> GetAllCarsForListPage()
        {
            var result = (from car in _carDal.GetAll(c => c.IsAvailable == true)
                          join category in _categoryDal.GetAll() on car.CategoryId equals category.Id
                          join brand in _brandDal.GetAll() on car.BrandId equals brand.Id
                          join model in _modelDal.GetAll() on car.ModelId equals model.Id

                          select new CarListPageDto
                          {
                             CarId=car.Id,
                             BrandId=brand.Id,
                             BrandName=brand.Name,
                             ModelName=model.Name,
                             CategoryName=category.Name,
                             PricePerHour=car.PricePerHour,
                             PricePerDay=car.PricePerDay,
                             SeatCount=car.SeatCount,
                             Transmission=car.Transmission.ToString(),
                          }).ToList();

            return new SuccessDataResult<List<CarListPageDto>>(result, "Müsait araç bilgileri");

        }

        public IDataResult<List<CarLocationDto>> GetAllCarsLocationByCategoryId(int categoryId)
        {
            var result = (from car in _carDal.GetAll(c => c.IsAvailable == true)
                          join category in _categoryDal.GetAll(c=>c.Id==categoryId)
                          on car.CategoryId equals category.Id
                          select new CarLocationDto
                          {
                              CarId = car.Id,
                              Latitude = car.Latitude,
                              Longitude = car.Longitude,
                              Category = category.Name
                          }).ToList();

            return new SuccessDataResult<List<CarLocationDto>>(result, "categorye göre filtreleme yapıldı haritada");

        }

        public IDataResult<List<CarLocationDto>> GetAllCarsLocationByBrandId(int brandId)
        {
            var result = (from car in _carDal.GetAll(c => c.IsAvailable == true && c.BrandId==brandId)
                          join category in _categoryDal.GetAll()
                          on car.CategoryId equals category.Id
                          select new CarLocationDto
                          {
                              CarId = car.Id,
                              Latitude = car.Latitude,
                              Longitude = car.Longitude,
                              Category = category.Name
                          }).ToList();

            return new SuccessDataResult<List<CarLocationDto>>(result, "markaya göre filtreleme yapıldı haritada");

        }

        public IDataResult<List<Brand>> GetAllAvailableBrand()
        {
            var result = (from car in _carDal.GetAll(c => c.IsAvailable == true)
                          join brand in _brandDal.GetAll() on car.BrandId equals brand.Id
                          select new Brand
                          {
                            Id = car.BrandId,
                            Name=brand.Name,
                          }).ToList();

            return new SuccessDataResult<List<Brand>>(result, "markalar listelendi");

        }

      
    }
}
