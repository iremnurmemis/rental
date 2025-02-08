using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface ICarService
    {
        IDataResult<List<Car>> GetAll();

        IDataResult<List<CarDetailDto>> GetAllDetail();
        IDataResult<List<Car>> GetByCarId(int carId);
        IDataResult<List<Car>> GetByBrandId(int brandId);
        IDataResult<List<Car>> GetByCategoryId(int categoryId);
        IDataResult<CarDetailDto> GetCarDetail(int carId);

        IDataResult<List<CarLocationDto>> GetAllCarsLocation();//map de müsaiit araclları gösterme ve bunu categoryleri farklı renkte ayırma

        IDataResult<List<Brand>> GetAllAvailableBrand();

        IDataResult<List<CarLocationDto>> GetAllCarsLocationByCategoryId(int categoryId); //mapde categorye göre filtreleme
        IDataResult<List<CarLocationDto>> GetAllCarsLocationByBrandId(int brandId); //mapde categorye göre filtreleme
        IDataResult<List<CarListPageDto>> GetAllCarsForListPage();
        IResult Add(Car car);
        IResult Update(Car car);
    
        IResult Delete(int carId);
        
    }
}
