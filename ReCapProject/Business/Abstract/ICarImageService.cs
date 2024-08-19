
using Core.Interceptors.Utilities.Results;
using Entities;
using Microsoft.AspNetCore.Http;

namespace Business
{
    public interface ICarImageService
    {
        IResult Add(IFormFile file, CarImage carImage);
        IResult Delete(CarImage carImage);
        IResult Update(IFormFile file, CarImage carImage);

        IDataResult<List<CarImage>> GetAll();  
        IDataResult<CarImage> GetById(int Id);
        IDataResult<List<CarImage>> GetByCarId(int carId);
        

    }
}
