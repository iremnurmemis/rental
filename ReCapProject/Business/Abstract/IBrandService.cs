using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IBrandService
    {
        IResult Add(Brand brand);
        IDataResult<List<Brand>> GetAll();
        IDataResult<List<Brand>> GetByBrandId(int Id);
        IResult Update(Brand brand);
        IResult Delete(Brand brand);
    }
}
